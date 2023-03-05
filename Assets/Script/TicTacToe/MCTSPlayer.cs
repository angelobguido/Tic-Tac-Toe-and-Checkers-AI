using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
    
    public class MCTSPlayer : Player
    {
        private MCTS mcts;

        private IEnumerator Start()
        {
            yield return null;
            mcts = new MCTS(GameManager.game, playerType);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            GameManager.OnPlay += UpdateMCTS;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GameManager.OnPlay -= UpdateMCTS;
        }

        public override IEnumerator MakeDecision()
        {
            chosenMove = mcts.FindBestMove();
            isReady = true;
            yield return null;
        }

        private void UpdateMCTS(PlayerType player, int row, int column)
        {
            mcts.Update();
        }
    }

    
}