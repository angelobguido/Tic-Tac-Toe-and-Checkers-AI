using System;

namespace General
{
    public abstract class Move
    {
        public override bool Equals(Object obj)
        {
            return obj is Move && IsTheSameAs((Move)obj);
        }

        protected abstract bool IsTheSameAs(Move other);
    }
    
}