using System.Collections;
using UnityEngine;

namespace General
{
    
    public class ClickPlayer : Player
    {
        private bool choosing = true;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            ClickManager.OnMoveMade += Choose;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ClickManager.OnMoveMade -= Choose;
        }

        public override IEnumerator MakeDecision()
        {
            choosing = true;
            yield return new WaitUntil(() => !choosing);
            isReady = true;
        }

        private void Choose(Move move)
        {
            if (Manager.game.GetNextPlayerToPlay() == playerType)
            {
                choosing = false;
                chosenMove = move;
            }
        }

    }
    
}
