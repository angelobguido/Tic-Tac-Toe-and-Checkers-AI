using System.Collections.Generic;
using System.Threading.Tasks;
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

        public float GetRollout(PlayerType mctsPlayer, int numberOfParallelSimulations)
        {
            var simulationGame = game.CreateClone();
            
            if (simulationGame.GetCurrentGameState() != GameState.InProgress)
                return MCTSLeafTask.GetRewardFromState(simulationGame.GetCurrentGameState(), mctsPlayer);
            
            var tasks = new Task<float>[numberOfParallelSimulations];
            
            for (var i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task<float>.Factory.StartNew( () => MCTSLeafTask.MakeSimulation(game, mctsPlayer) );
            }

            Task.WaitAll(tasks);
            
            var rewardAverage = 0f;
            
            foreach (var task in tasks)
            {
                rewardAverage += task.Result;
            }

            rewardAverage /= numberOfParallelSimulations;
            
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

        public Node Expand()
        {
            if (IsExpandable())
            {
                children.Add(new Node(this, remainingMoves.Pop(), game));
                return children[^1];
            }

            return null;
        }

        private void InitialiseNode()
        {
            children = new List<Node>();
            remainingMoves = game.GetCurrentGameState() == GameState.InProgress ? new Stack<Move>(game.GetValidMoves()) : new Stack<Move>();
        }
    }
    
}
