using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
    
    public class MCTS
    {
        private Game game;
        private PlayerType player;
        private Node root;
        private static readonly int NumberOfIterations = 100;
        
        public MCTS(Game currentGame, PlayerType targetPlayer)
        {
            player = targetPlayer;
            game = currentGame;
            root = new Node(game);
        }

        private Node SelectNextNodeFrom(Node node)
        {
            node.children.Sort((a,b) => b.GetUCB().CompareTo(a.GetUCB()) );
            return node.children[0];
        }

        private void Expand(Node node)
        {
            var validMoves = node.game.GetValidMoves();
            validMoves.ForEach( validMove => node.children.Add(new Node(node, validMove, node.game)) );
        }

        public void Update()
        {
            var lastMove = game.GetLastMove();
            var newRoot = root.children.Find(node => node.move.Equals(lastMove));

            root = newRoot;
        }

        public Move FindBestMove()
        {
            
        }

        private Move _FindBestMove(Node current)
        {
            current
        }
        
    }
    
}