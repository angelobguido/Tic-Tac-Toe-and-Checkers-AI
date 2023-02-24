using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TicTacToe
{
    public class SecondPlayer : Player
    {
        public static Action SecondPlayerFinishedMove;

        protected override void AfterInitialization()
        {
            playerType = PlayerType.Second;
        }
        
        private void OnEnable()
        {
            FirstPlayer.FirstPlayerFinishedMove += MakeDecision;
        }
        
        private void OnDisable()
        {
            FirstPlayer.FirstPlayerFinishedMove -= MakeDecision;
        }

        
        protected override void InvokeFinishedMove()
        {
            SecondPlayerFinishedMove?.Invoke();
        }
        
        private void MakeDecision()
        {
            MakeRandomMove();
        }

    }    
}

