using System;
using System.Collections;
using System.Collections.Generic;
using General;
using Unity.VisualScripting;
using UnityEngine;

namespace Checkers
{
    public class ClickManager : General.ClickManager
    {

        public static Action OnClearAll;

        [SerializeField] private Transform positions;
        private Queue<Vector2Int> selectedPositions;


        private void OnEnable()
        {
            CellClick.OnSelectPosition += ReceivePosition;
        }

        private void OnDisable()
        {
            CellClick.OnSelectPosition -= ReceivePosition;            
        }

        private void Awake()
        {
            selectedPositions = new Queue<Vector2Int>();
        }

        private void Start()
        {
            var child = 0;
            
            for (var row = 0; row < 8; row++)
            {
                for (var column = 0; column < 8; column++)
                {
                    positions.GetChild(child).GetComponent<CellClick>().position = new (row, column);
                    child++;
                }
            }
        }

        private void ReceivePosition(Vector2Int position)
        {
            if (position.Equals(new(-1, -1)))
            {
                BuildMoveAndTransmit();
            }
            else
            {
                selectedPositions.Enqueue(position);
            }
        }

        private void BuildMoveAndTransmit()
        {
            
            if (selectedPositions.Count == 0)
                return;

            var step = new Step( new (0,0), new (0,0) );
            var captures = new CaptureStorage();

            var firstPosition = selectedPositions.Dequeue();
            step.from = firstPosition;
            
            var game = (Manager.game);
            
            while (selectedPositions.Count != 0)
            {
                var nextPosition = selectedPositions.Dequeue();

                if (((Game)game).board[nextPosition.x, nextPosition.y] != Piece.Nothing)
                {
                    captures.AddCapturedPosition(nextPosition);
                }

                step.to = nextPosition;
            }
            
            var move = new Move(step);

            if (captures.numberOfCaptures != 0)
            {
                move.captures = captures;
            }
            
            if (IsValid(move))
            {
                TransmitMovement(move);
            }
            else
            {
                selectedPositions.Clear();
            }
            
            OnClearAll?.Invoke();

        }

        private bool IsValid(Move move)
        {
            return Manager.game.GetValidMoves().Exists( (a)=> a.Equals(move) );
        }

    }
    
}
