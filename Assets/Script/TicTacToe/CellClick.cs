using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
    
    public class CellClick : General.CellClick
    {

        [SerializeField] private int row;
        [SerializeField] private int column;

        protected override void CreateMove()
        {
            move = new Move(row, column);
        }
    }   
}
