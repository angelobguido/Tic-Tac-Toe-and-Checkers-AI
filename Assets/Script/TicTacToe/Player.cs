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
            
            game.MakeMove(move);
            drawer.Draw(playerType, move.row, move.column);
            InvokeFinishedMove();
          
        }
        
        public void MakeRandomMove()
        {

            var state = game.MakeRandomMove();
            var lastMove = game.GetLastMove();
            
            drawer.Draw(playerType, lastMove.row, lastMove.column);
            
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
