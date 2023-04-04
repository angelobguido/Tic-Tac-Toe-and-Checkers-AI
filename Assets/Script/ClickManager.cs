using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    
    public class ClickManager : MonoBehaviour
    {
        
        public static Action<Move> OnMoveMade;
        
        protected void TransmitMovement(Move move)
        {
            OnMoveMade?.Invoke(move);
        }
    }

    
}