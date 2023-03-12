using System;
using UnityEngine;

namespace General
{
    
    public abstract class CellClick : MonoBehaviour
    {

        protected Move move;
        public static Action<Move> OnPositionClicked;

        private void Awake()
        {
            CreateMove();
        }

        public void OnClick()
        {
            OnPositionClicked?.Invoke(move);
        }
        
        protected abstract void CreateMove();
    }   
}
