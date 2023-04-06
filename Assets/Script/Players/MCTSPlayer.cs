using System.Collections;
using UnityEngine;

namespace General
{
    
    public class MCTSPlayer : Player
    {
        [SerializeField] public MCTSParameters parameters;
        private MCTS mcts;

        private IEnumerator Start()
        {
            yield return null;
            mcts = new MCTS(Manager.game, playerType, parameters);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Manager.OnPlay += UpdateMCTS;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Manager.OnPlay -= UpdateMCTS;
        }

        public override IEnumerator MakeDecision()
        {
            chosenMove = mcts.FindBestMove();
            isReady = true;
            yield return null;
        }

        private void UpdateMCTS()
        {
            mcts.Update();
        }
    }

    
}