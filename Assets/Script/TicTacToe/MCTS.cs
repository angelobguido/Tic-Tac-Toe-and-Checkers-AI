using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace TicTacToe
{
    
    public class MCTS
    {
        private Game game;
        private PlayerType player;
        private Node root;
        private static readonly int NumberOfIterations = 1000;
        
        public MCTS(Game currentGame, PlayerType targetPlayer)
        {
            player = targetPlayer;
            game = currentGame;
            root = new Node(game);
            FindBestMove();
        }

        private void GetRolloutAndBackPropagate(Node node)
        {
            var nextPlayerInNodeGame = node.game.GetNextPlayerToPlay();
            var finalState = node.GetRollout();
            float value = 0f;

            switch (finalState)
            {
                case GameState.Tie: 
                    value = 0f;
                    break;
                
                case GameState.PlayerOneWins: 
                    value = (player == PlayerType.First) ? (1f) : (-1f);
                    break;
                
                case GameState.PlayerTwoWins: 
                    value = (player == PlayerType.First) ? (-1f) : (1f);
                    break;
                
            }

            var currentNode = node;
            value = (nextPlayerInNodeGame == player) ? (-value) : (value);
            
            while (currentNode != root)
            {
                currentNode.Visit();
                currentNode.value += value;
                value *= -1f;
                currentNode = currentNode.parent;
            }
            
            currentNode.Visit();
            currentNode.value += value;
            
        }

        public void Update()
        {
            var lastMove = game.GetLastMove();
            var newRoot = root.children.Find(node => node.move.Equals(lastMove));
            
            root = newRoot;
        }

        public Move FindBestMove()
        {
            for (var i = 0; i < NumberOfIterations; i++)
                ProcessTree();
            
            root.children.Sort( (a,b) => b.GetAverageValue().CompareTo(a.GetAverageValue()) );
            
            var bestMove = root.children[0].move;

            return bestMove;
        }

        private void ProcessTree()
        {

            var currentNode = root;
            
            while (!(currentNode.IsExpandable() || currentNode.IsLeaf()))
            {
                currentNode = currentNode.SelectNextNode();
            }
            
            currentNode.Expand();

            //If there are no moves left, the current node will still be a leaf, so just return
            if (currentNode.IsLeaf())
            {
                GetRolloutAndBackPropagate(currentNode);    
                return;
            }
            
            var nextNode = currentNode.SelectNextNode();
            GetRolloutAndBackPropagate(nextNode);

            
        }
        
    }
    
}