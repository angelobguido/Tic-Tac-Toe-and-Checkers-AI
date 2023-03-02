using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Checkers
{
    public class Move
    {
        private List<Step> steps;

        public Move(List<Step> steps)
        {
            this.steps = steps;
        }

        public Move(Step step)
        {
            steps = new List<Step>();
            steps.Add(step);
        }
    }
    
}