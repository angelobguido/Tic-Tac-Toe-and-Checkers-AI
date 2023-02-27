using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
    public class FirstPlayer : Player
    {
        
        public static Action FirstPlayerFinishedMove;

        protected override void AfterInitialization()
        {
            playerType = PlayerType.First;
            Invoke("RandomAI", 2f);
        }

        private void OnEnable()
        {
            SecondPlayer.SecondPlayerFinishedMove += RandomAI;
            CellClick.OnPositionClicked += MakeMove;
        }
        
        private void OnDisable()
        {
            SecondPlayer.SecondPlayerFinishedMove -= RandomAI;
            CellClick.OnPositionClicked -= MakeMove;
        }

        protected override void InvokeFinishedMove()
        {
            FirstPlayerFinishedMove?.Invoke();
        }

        private void RandomAI()
        {
            //MakeRandomMove();
            
        }
        
    }
    
}
