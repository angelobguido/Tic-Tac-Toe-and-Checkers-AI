using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
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

        public Node(Game game)
        {
            value = 0;
            visits = 0;

            this.game = game;

        }
        
        public Node(Node parent, Move move, Game game)
        {
            value = 0;
            visits = 0;

            this.parent = parent;
            
            this.move = move;
            
            this.game = new Game(game);
            game.MakeMove(move);
            
            children = new List<Node>();
        }

        public float GetUCB()
        {
            if (visits == 0)
                return 1000 + Random.value * 0.1f;

            return GetAverageValue() + Mathf.Sqrt(Mathf.Log(parent.visits))/visits + Random.value * 0.1f;
        }

        private float GetAverageValue()
        {
            return value / visits;
        }
    }
    
}
