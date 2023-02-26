using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
    public abstract class Player : MonoBehaviour
    {
        protected Game game;
        protected BoardDrawer drawer;
        protected PlayerType playerType;
       
        private void Start()
        {
            game = GameManager.GetGame();
            drawer = GameObject.FindWithTag("Board").GetComponent<BoardDrawer>();
            AfterInitialization();
        }

        protected abstract void AfterInitialization();
        
        public void MakeMove(Move move)
        {
            var state = game.MakeMove(move);
            var lastMove = game.GetLastMove();
            
            AfterMove(lastMove, state);
        }
        
        public void MakeRandomMove()
        {
            var state = game.MakeRandomMove();
            var lastMove = game.GetLastMove();
            
            AfterMove(lastMove, state);
        }

        public void MakeDeterminedMove()
        {
            var moves = new List<Move>(game.GetValidMoves());
            moves.Sort((a, b) => (a.row * 3 + a.column).CompareTo( b.row * 3 + b.column ) );
            var move = moves[0];

            MakeMove(move);
        }

        private void AfterMove(Move move, GameState state)
        {
            
            drawer.Draw(playerType, move.row, move.column);
            
            if (state == GameState.InProgress)
            {
                InvokeFinishedMove();    
            }
            else
            {
                switch (state)
                {
                    case GameState.Tie: Debug.Log("It's a tie!");
                        break;
                    case GameState.PlayerOneWins: Debug.Log("Player X won!");
                        break;
                    case GameState.PlayerTwoWins: Debug.Log("Player O won!");
                        break;
                }
            }

        }

        protected abstract void InvokeFinishedMove();
        
    }
    
}
