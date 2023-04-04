using System.Collections.Generic;
using Help;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Piece = Checkers.Piece;
using Random = UnityEngine.Random;

namespace General
{
    public class Node
    {
        public Move move;
        public float value;
        public int visits;
        public Game game;
        public Node parent;

        public List<Node> children;
        private Stack<Node> expandableChildren;
        private Stack<Move> remainingMoves;

        private static readonly float ExplorationConstant = 0.7f;
        
        private static readonly int NumberOfParallelSimulations = 2;

        public Node(Game game)
        {
            value = 0;
            visits = 0;

            this.game = game.CreateClone();
            
            InitialiseNode();
        }
        
        public Node(Node parent, Move move, Game game)
        {
            
            value = 0;
            visits = 0;

            this.parent = parent;
            
            this.move = move;

            this.game = game.CreateClone();
            this.game.MakeMove(move);
            
            InitialiseNode();
            
        }

        public float GetRollout(PlayerType mctsPlayer)
        {
            var simulationGame = game.CreateClone();
            GameState state;

            if (simulationGame.GetCurrentGameState() != GameState.InProgress)
                return MCTSLeafJob.GetRewardFromState(simulationGame.GetCurrentGameState(), mctsPlayer);


            var result = new NativeArray<float>(NumberOfParallelSimulations, Allocator.TempJob);
            var leafJob = new MCTSLeafJob();
            leafJob.player = mctsPlayer;
            leafJob.result = result;
            leafJob.gameType = Manager.gameType;

            var i = 0;
            switch (Manager.gameType)
            {
                case GameType.Checkers:
                    
                    leafJob.checkersBoard = new NativeArray<Checkers.Piece>(8 * 8, Allocator.TempJob);
                    foreach (var piece in ((Checkers.Game)game).board)
                    {
                        leafJob.checkersBoard[i] = piece;
                        i++;
                    }

                    leafJob.tictactoeBoard = new NativeArray<char>(0, Allocator.TempJob);
                    break;
                
                case GameType.TicTacToe:
                    leafJob.tictactoeBoard = new NativeArray<char>(3 * 3, Allocator.TempJob);
                    foreach (var piece in ((TicTacToe.Game)game).board)
                    {
                        leafJob.tictactoeBoard[i] = piece;
                        i++;
                    }

                    leafJob.checkersBoard = new NativeArray<Piece>(0, Allocator.TempJob);
                    break;
            }

            var handle = leafJob.Schedule(result.Length, 1);
            handle.Complete();

            var rewardAverage = 0f;
            
            foreach (var reward in result)
            {
                rewardAverage += reward;
            }

            rewardAverage /= NumberOfParallelSimulations;
            
            result.Dispose();
            leafJob.checkersBoard.Dispose();
            leafJob.tictactoeBoard.Dispose();

            return rewardAverage;
        

        }

        public float GetUCB()
        {
            if (visits == 0)
                return 1000;
            
            return GetAverageValue() + ExplorationConstant*Mathf.Sqrt(Mathf.Log(parent.visits)/visits);
        }

        public float GetAverageValue()
        {
            return value / visits;
        }

        public void Visit()
        {
            visits++;
        }

        public bool IsLeaf()
        {
            return children.Count == 0;
        }

        public bool IsExpandable()
        {
            return remainingMoves.Count != 0;
        }
        
        public Node SelectNextNode()
        {
            children.Sort((a,b) => b.GetUCB().CompareTo(a.GetUCB()) );
            return children[0];
        }

        public void Expand()
        {
            if (IsExpandable())
            {
                children.Add(new Node(this, remainingMoves.Pop(), game));
            }
        }

        private void InitialiseNode()
        {
            children = new List<Node>();
            remainingMoves = game.GetCurrentGameState() == GameState.InProgress ? new Stack<Move>(game.GetValidMoves()) : new Stack<Move>();
        }
    }
    
}
