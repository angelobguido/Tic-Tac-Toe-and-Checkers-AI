using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Checkers
{

    public class Position
    {
        public int row;
        public int column;

        public Position(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
        
    }
    
    public class Step
    {
        public Position from;
        public Position to;
        
        public Step(int fromRow, int fromColumn, int toRow, int toColumn)
        {
            this.from = new Position(fromRow, fromColumn);
            this.to = new Position(toRow, toColumn);
        }
        
        public Step(Position from, Position to)
        {
            this.from = from;
            this.to = to;
        }
        
    }

    
}