using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    [CreateAssetMenu(fileName = "MCTSParameters", menuName = "ScriptableObjects/MCTSParameters", order = 1)]
    public class MCTSParameters : ScriptableObject
    {
        public int numberOfIterations;
        public int numberOfParallelSimulationsInLeafNode;
        public int numberOfParallelTreeProcessing;
    }
}