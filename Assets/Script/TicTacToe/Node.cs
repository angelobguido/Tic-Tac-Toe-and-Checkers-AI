using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace TicTacToe
{
    public class Node
    {
        public Move move;
        public float value;
        public int visits;
        public Game game;
        public Node parent;

        public List<Node> children;

        private static readonly float RandomConstantMultiplier = 0.01f;

        public Node(Game game)
        {
            value = 0;
            visits = 0;

            this.game = new Game(game);
            
            children = new List<Node>();

        }
        
        public Node(Node parent, Move move, Game game)
        {
            
            value = 0;
            visits = 0;

            this.parent = parent;
            
            this.move = move;
            
            this.game = new Game(game);
            this.game.MakeMove(move);
            
            children = new List<Node>();
        }

        public GameState GetRollout()
        {
            var simulationGame = new Game(game);

            if (simulationGame.GetValidMoves().Count == 0)
            {
                return simulationGame.GetCurrentGameState();
            }
            
            var state = simulationGame.MakeRandomMove();

            while (state == GameState.InProgress)
            {
                state = simulationGame.MakeRandomMove();
            }

            simulationGame = null;

            return state;
        }

        private float GetUCB()
        {
            if (visits == 0)
                return 1000 + Random.value * RandomConstantMultiplier;

            return GetAverageValue() + Mathf.Sqrt(Mathf.Log(parent.visits))/visits + Random.value * RandomConstantMultiplier;
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
        
        public Node SelectNextNode()
        {
            children.Sort((a,b) => b.GetUCB().CompareTo(a.GetUCB()) );
            return children[0];
        }

        public void Expand()
        {
            var validMoves = game.GetValidMoves();
            validMoves.ForEach( validMove => children.Add(new Node(this, validMove, game)) );
        }
    }
    
}
