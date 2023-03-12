using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
    public abstract class Player : MonoBehaviour
    {
        [SerializeField] protected PlayerType playerType;
        protected bool isReady = false;
        protected Move chosenMove;

        public abstract IEnumerator MakeDecision();
        
        protected virtual void OnEnable()
        {
            GameManager.OnPlay += GetUnready;
        }

        protected virtual void OnDisable()
        {
            GameManager.OnPlay -= GetUnready;
        }

        public Move GetChosenMove()
        {
            return chosenMove;
        }

        public bool IsReady()
        {
            return isReady;
        }

        private void GetUnready(PlayerType player, int row, int column)
        {
            isReady = false;
        }
        
    }
    
}
