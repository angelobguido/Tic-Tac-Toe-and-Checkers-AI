using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
    
    public class CellClick : General.CellClick
    {
        
        public static Action<Move> OnPositionClicked;

        [SerializeField] private int row;
        [SerializeField] private int column;
        
        public override void OnClick()
        {
            OnPositionClicked?.Invoke(new Move(row, column));
        }
        
    }   
}
