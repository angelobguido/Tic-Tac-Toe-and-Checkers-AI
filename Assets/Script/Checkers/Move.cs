using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Checkers
{
    
    public class CaptureStorage
    {

        public byte[,] capturesMap;
        public List<Vector2Int> capturesPositions;
        public int numberOfCaptures;

        public CaptureStorage()
        {
            capturesMap = new byte[8, 8];
            capturesPositions = new List<Vector2Int>();
            for (var row = 0; row < 8; row++)
            {
                for (var column = 0; column < 8; column++)
                {
                    capturesMap[row, column] = 0;
                }
            }

            numberOfCaptures = 0;
        }

        public CaptureStorage(CaptureStorage origin)
        {
            capturesMap = new byte[8, 8];
            capturesPositions = new List<Vector2Int>(origin.capturesPositions);
            for (var row = 0; row < 8; row++)
            {
                for (var column = 0; column < 8; column++)
                {
                    capturesMap[row, column] = origin.capturesMap[row, column];
                }
            }

            numberOfCaptures = origin.numberOfCaptures;

        }

        public void AddCapturedPosition(Vector2Int position)
        {
            capturesMap[position.x, position.y] = 1;
            capturesPositions.Add(position);
            numberOfCaptures++;
        }

        public bool IsCaptured(Vector2Int position)
        {
            return capturesMap[position.x, position.y] == 1;
        }
        
        public override bool Equals(object obj)
        {
            return obj is CaptureStorage && ((CaptureStorage)obj).capturesPositions.SequenceEqual(capturesPositions);
        }

    }
    
    
    public class Move: General.Move
    {
        public Step step;
        public CaptureStorage captures;

        public Move(Step step)
        {
            this.step = step;
        }

        protected override bool IsTheSameAs(General.Move other)
        {
            if (other is not Move)
                return false;

            if (!step.Equals(((Move)other).step)) return false;
            
            if (captures == null && ((Move)other).captures == null)
            {
                return true;
            }

            return captures.Equals(((Move)other).captures);

        }
    }
    
}