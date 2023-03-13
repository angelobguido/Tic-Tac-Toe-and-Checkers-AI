using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Checkers
{

    public class Step
    {
        public Vector2Int from;
        public Vector2Int to;
        
        public Step(int fromRow, int fromColumn, int toRow, int toColumn)
        {
            this.from = new Vector2Int(fromRow, fromColumn);
            this.to = new Vector2Int(toRow, toColumn);
        }
        
        public Step(Vector2Int from, Vector2Int to)
        {
            this.from = from;
            this.to = to;
        }

        public override bool Equals(object obj)
        {
            return obj is Step && ((Step)obj).from.Equals(from) && ((Step)obj).to.Equals(to);
        }
    }

    
}