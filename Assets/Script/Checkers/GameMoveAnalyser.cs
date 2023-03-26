using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using General;
using Help;
using UnityEngine;

namespace Checkers
{

    public enum Direction: byte
    {
        UpRight,
        UpLeft,
        DownRight,
        DownLeft
    }

    public class GameMoveAnalyser
    {

        private Piece[,] board;
        private PlayerType nextPlayer;
        private PlayerType oponent;
        private List<Move > captureMoves;
        private List<Move> regularMoves;

        public GameMoveAnalyser(Piece[,] board, PlayerType nextPlayer)
        {

            this.board = board;
            this.nextPlayer = nextPlayer;
            this.oponent = (nextPlayer == PlayerType.First) ? (PlayerType.Second) : (PlayerType.First);
            this.captureMoves = new List<Move>();
            this.regularMoves = new List<Move>();

        }

        public bool CanMakeRegularMove(Vector2Int position)
        {
            if (captureMoves.Count != 0)
                return false;

            var orientation = (nextPlayer == PlayerType.First) ? (-1) : (1);
            var directions = new Vector2Int[]{new(orientation, -1), new(orientation, 1)};

            foreach (var direction in directions)
            {
                var lookPosition = position + direction;
                if (CanGoTo(lookPosition))
                    return true;
            }
            
            return false;

        }

        public void AddRegularMoves(Vector2Int position)
        {
            
            var orientation = (nextPlayer == PlayerType.First) ? (-1) : (1);
            var directions = new Vector2Int[]{new(orientation, -1), new(orientation, 1)};

            foreach (var direction in directions)
            {
                var lookPosition = position + direction;
                if (CanGoTo(lookPosition))
                {
                    var move = new Move(new Step(position, lookPosition));
                    regularMoves.Add(move);
                }
            }
            
        }
        
        public bool CanMakeCaptureMove(Vector2Int position)
        {
            
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var directionVector = GetVectorWithDirection(direction);
                var lookPosition = position + directionVector;
                
                if (CanCaptureAt(lookPosition, directionVector))
                    return true;
            }
            
            return false;

        }
            
        public void AddCaptureMoves(Vector2Int position)
        {
            var captures = new CaptureStorage();
            var moves = new List<Move>();
           
            _AddCaptureMoves(moves, captures, ref position, position);

            AddGeneralCaptureMoves(moves);

        }
        
        public bool CanMakeKingRegularMove(Vector2Int position)
        {
            if (captureMoves.Count != 0)
                return false;

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var lookPosition = position + GetVectorWithDirection(direction);
                if (CanGoTo(lookPosition))
                    return true;
            }
            
            return false;
            
        }
            
        public void AddKingRegularMoves(Vector2Int position)
        {
            
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var directionVector = GetVectorWithDirection(direction);
                var lookPosition = position + directionVector;
                
                while (CanGoTo(lookPosition))
                {
                    regularMoves.Add(new Move(new Step(position, lookPosition)));
                    lookPosition += directionVector;
                }
                
            }
            
        }
        
        public bool CanMakeKingCaptureMove(Vector2Int position)
        {
            
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var lookPosition = FindFirstPiecePositionAt(direction, position);
                if (CanCaptureAt(lookPosition, GetVectorWithDirection(direction)))
                    return true;
            }
            
            return false;
            
        }
            
        public void AddKingCaptureMoves(Vector2Int position)
        {
            var captures = new CaptureStorage();
            var moves = new List<Move>();
            
            _AddKingCaptureMoves(moves, captures, ref position, position);

            AddGeneralCaptureMoves(moves);

        }

        public List<General.Move> GetAllValidMoves()
        {
            var validMoves = new List<General.Move>();
            if (captureMoves.Count != 0)
            {
                captureMoves.ForEach( (a) => validMoves.Add(a) );
            }
            else
            {
                regularMoves.ForEach( (a)=> validMoves.Add(a));
            }

            return validMoves;
        }

        private bool CanGoTo(Vector2Int position)
        {
            return PieceAnalyser.IsValid(position) && (board[position.x, position.y] == Piece.Nothing);
        }

        private bool CanCaptureAt(Vector2Int position, Vector2Int direction)
        {

            if (!(PieceAnalyser.IsValid(position) &&
                (PieceAnalyser.GetPlayerTypeFromPiece(board[position.x, position.y]) == oponent)))
                return false;

            var nextPosition = position + direction;

            return PieceAnalyser.IsValid(nextPosition) &&
                   PieceAnalyser.GetPlayerTypeFromPiece(board[nextPosition.x, nextPosition.y]) == PlayerType.NullPlayer;
        }

        private Vector2Int FindFirstPiecePositionAt(Direction direction, Vector2Int from)
        {
            var directionVector = GetVectorWithDirection(direction);

            var current = from + directionVector;

            while (CanGoTo(current))
            {
                current += directionVector;
            }

            return PieceAnalyser.IsValid(current) ? current : from;
        }

        private Vector2Int GetVectorWithDirection(Direction direction)
        {
            var directionVector = direction switch
            {
                Direction.UpRight => new Vector2Int(-1, 1),
                Direction.UpLeft => new Vector2Int(-1, -1),
                Direction.DownRight => new Vector2Int(1, 1),
                Direction.DownLeft => new Vector2Int(1, -1),
                _ => new Vector2Int()
            };

            return directionVector;
        }

        private void _AddCaptureMoves(List< Move > moves, CaptureStorage captures, ref Vector2Int firstPosition, Vector2Int lastPosition)
        {
            var capturedSomething = false;
            
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var directionVector = GetVectorWithDirection(direction);
                var lookPosition = lastPosition + directionVector;
                
                if (CanCaptureAt(lookPosition, directionVector) && !captures.IsCaptured(lookPosition))
                {
                    capturedSomething = true;
                    
                    captures.AddCapturedPosition(lookPosition);
                    
                    var nextLookPosition = lookPosition + directionVector;
                    _AddCaptureMoves( moves, new(captures), ref firstPosition, nextLookPosition );
                }
                   
            }

            if (!capturedSomething)
            {
                var bigStep = new Step(firstPosition, lastPosition);
                var move = new Move(bigStep);
                move.captures = captures;
                moves.Add(move);
            }
        }
        
        private void _AddKingCaptureMoves(List< Move > moves, CaptureStorage captures, ref Vector2Int firstPosition, Vector2Int lastPosition)
        {
            var capturedSomething = false;
            
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var directionVector = GetVectorWithDirection(direction);
                var lookPosition = FindFirstPiecePositionAt(direction, lastPosition);
                
                if (CanCaptureAt(lookPosition, directionVector) && !captures.IsCaptured(lookPosition))
                {
                    capturedSomething = true;
                    
                    captures.AddCapturedPosition(lookPosition);
                    
                    var nextLookPosition = lookPosition + directionVector;

                    while (CanGoTo(nextLookPosition))
                    {
                        _AddKingCaptureMoves( moves, new(captures), ref firstPosition, nextLookPosition );
                        
                        nextLookPosition += directionVector;
                    }
                }
                   
            }

            if (!capturedSomething)
            {
                var bigStep = new Step(firstPosition, lastPosition);
                var move = new Move(bigStep);
                move.captures = captures;
                moves.Add(move);
            }
        }

        private void AddGeneralCaptureMoves(List<Move> moves)
        {
            //SORT MOVES. then add the ones that have the most number of captures.
            moves.Sort((a, b) => b.captures.numberOfCaptures.CompareTo(a.captures.numberOfCaptures));

            var maxCaptures = moves[0].captures.numberOfCaptures;

            if (captureMoves.Exists((a) => (a.captures.numberOfCaptures > maxCaptures)))
                return;

            if (captureMoves.Exists((a) => (a.captures.numberOfCaptures == maxCaptures)))
            {
                moves.ForEach( (a) =>
                {
                    if(a.captures.numberOfCaptures == maxCaptures)
                        captureMoves.Add(a);
                });
                
                return;
            }
            
            captureMoves.Clear();
            moves.ForEach( (a) =>
            {
                if(a.captures.numberOfCaptures == maxCaptures)
                    captureMoves.Add(a);
            });
        }
        

    }
    
}
