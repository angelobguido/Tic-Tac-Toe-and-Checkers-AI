using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
    public struct Move
    {
        public int row;
        public int column;

        public Move(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
    }
    
}