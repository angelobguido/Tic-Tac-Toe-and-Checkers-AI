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
        }
        
        private void OnDisable()
        {
            SecondPlayer.SecondPlayerFinishedMove -= RandomAI;
        }

        protected override void InvokeFinishedMove()
        {
            Debug.Log("Invoked player one finished");
            FirstPlayerFinishedMove?.Invoke();
        }

        private void RandomAI()
        {
            //MakeRandomMove();
            MakeDeterminedMove();
        }
        
    }
    
}
