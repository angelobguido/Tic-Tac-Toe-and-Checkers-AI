using System.Collections.Generic;
using System.Threading.Tasks;
using Help;
using UnityEngine;

namespace General
{
    
    public class MCTS
    {
        private Game game;
        private PlayerType player;
        private Node root;
        private MCTSParameters parameters;
        
        public MCTS(Game currentGame, PlayerType targetPlayer, MCTSParameters parameters)
        {
            player = targetPlayer;
            game = currentGame;
            root = new Node(game);
            this.parameters = parameters;
        }

        private void GetRolloutAndBackPropagate(Node node)
        {
            var nextPlayerInNodeGame = node.game.GetNextPlayerToPlay();
            var value = node.GetRollout(player, parameters.numberOfParallelSimulationsInLeafNode);

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

        private void GetRolloutAndBackPropagateParallel(Node parentNode)
        {
            var i = 0;
            var tasksList = new List<Task<float>>(); 
            
            while (i < parameters.numberOfParallelTreeProcessing && parentNode.IsExpandable())
            {
                var child = parentNode.Expand();
                tasksList.Add(Task<float>.Factory.StartNew( () => GetRolloutAndApplyToNode(child) ));
                i++;
            }

            var value = 0f;

            var tasks = new Task<float>[tasksList.Count];
            
            for (var j = 0; j < tasksList.Count ; j++)
            {
                tasks[j] = tasksList[j];
            }

            Task.WaitAll(tasks);

            for (var j = 0; j < tasksList.Count ; j++)
            {
                value += tasks[j].Result;
            }

            var currentNode = parentNode;
            
            while (currentNode != root)
            {
                currentNode.visits += i;
                currentNode.value += value;
                value *= -1f;
                currentNode = currentNode.parent;
            }
            
            currentNode.visits += i;
            currentNode.value += value;
            
        }

        private float GetRolloutAndApplyToNode(Node node)
        {
            var nextPlayerInNodeGame = node.game.GetNextPlayerToPlay();
            var value = node.GetRollout(player, parameters.numberOfParallelSimulationsInLeafNode);

            var currentNode = node;
            value = (nextPlayerInNodeGame == player) ? (-value) : (value);
            
            currentNode.Visit();
            currentNode.value += value;
            value *= -1f;

            return value;
        }

        public void Update()
        {
            var lastMove = game.GetLastMove();
            var newRoot = root.children.Find(node => node.move.Equals(lastMove)) ?? new Node(game);

            root = newRoot;
        }

        public Move FindBestMove()
        {
            for (var i = 0; i < parameters.numberOfIterations; i++)
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
            
            //If there are no moves left, the current node will still be a leaf, so just return
            if (currentNode.IsLeaf() && !currentNode.IsExpandable())
            {
                GetRolloutAndBackPropagate(currentNode);    
                return;
            }

            GetRolloutAndBackPropagateParallel(currentNode);

        }
        
    }
    
}