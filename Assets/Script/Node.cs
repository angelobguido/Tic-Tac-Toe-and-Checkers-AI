using System.Collections.Generic;
using Help;
using UnityEngine;
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

        public GameState GetRollout()
        {
            var simulationGame = game.CreateClone();

            if (simulationGame.GetCurrentGameState() != GameState.InProgress)
                return simulationGame.GetCurrentGameState();

            var state = simulationGame.MakeRandomMove();

            while (state == GameState.InProgress)
            {
                state = simulationGame.MakeRandomMove();
            }

            simulationGame = null;

            return state;
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
