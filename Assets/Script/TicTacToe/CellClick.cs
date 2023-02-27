using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
    
    public class CellClick : MonoBehaviour
    {

        [SerializeField] private int row;
        [SerializeField] private int column;

        public static Action<Move> OnPositionClicked;
        
        public void OnClick()
        {
            OnPositionClicked.Invoke(new Move(row, column));
        }
    }   
}
