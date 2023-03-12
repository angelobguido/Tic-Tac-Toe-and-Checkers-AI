using System.Collections;
using Help;
using UnityEngine;

namespace General
{
    public abstract class Player : MonoBehaviour
    {
        [SerializeField] protected PlayerType playerType;
        protected bool isReady = false;
        protected Move chosenMove;

        public abstract IEnumerator MakeDecision();
        
        protected virtual void OnEnable()
        {
            Manager.OnPlay += GetUnready;
        }

        protected virtual void OnDisable()
        {
            Manager.OnPlay -= GetUnready;
        }

        public Move GetChosenMove()
        {
            return chosenMove;
        }

        public bool IsReady()
        {
            return isReady;
        }

        private void GetUnready()
        {
            isReady = false;
        }
        
    }
    
}
