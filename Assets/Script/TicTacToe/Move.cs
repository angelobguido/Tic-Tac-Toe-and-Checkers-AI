using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
    public class Move : General.Move
    {
        public int row;
        public int column;

        public Move(int row, int column)
        {
            this.row = row;
            this.column = column;
        }

        protected override bool IsTheSameAs(General.Move other)
        {
            return other is Move && ((Move)other).row == row && ((Move)other).column == column;
        }
    }
    
}