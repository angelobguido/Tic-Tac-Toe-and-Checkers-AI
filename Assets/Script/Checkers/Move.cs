using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Checkers
{
    public class Move: General.Move
    {
        private List<Step> steps;

        public Move(List<Step> steps)
        {
            this.steps = steps;
        }

        public Move(Step step)
        {
            steps = new List<Step> { step };
        }

        protected override bool IsTheSameAs(General.Move other)
        {
            if (other is not Move)
                return false;

            for (var i = 0; i < steps.Count; i++)
            {
                if (!steps[i].Equals(((Move)other).steps[i]))
                    return false;
            }

            return true;

        }
    }
    
}