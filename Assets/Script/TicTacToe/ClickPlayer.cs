using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
    
    public class ClickPlayer : Player
    {
        private bool choosing = true;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            CellClick.OnPositionClicked += Choose;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            CellClick.OnPositionClicked -= Choose;
        }

        public override IEnumerator MakeDecision()
        {
            choosing = true;
            yield return new WaitUntil(() => !choosing);
            isReady = true;
        }

        private void Choose(Move move)
        {
            if (GameManager.game.GetNextPlayerToPlay() == playerType)
            {
                choosing = false;
                chosenMove = move;
            }
        }

    }
    
}
