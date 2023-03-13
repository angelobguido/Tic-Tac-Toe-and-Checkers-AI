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

    public class CaptureStorage
    {

        public byte[,] positionsCaptured;
        public int numberOfCaptures;

        public CaptureStorage()
        {
            positionsCaptured = new byte[8, 8];
            for (var row = 0; row < 8; row++)
            {
                for (var column = 0; column < 8; column++)
                {
                    positionsCaptured[row, column] = 0;
                }
            }

            numberOfCaptures = 0;
        }

        public CaptureStorage(CaptureStorage origin)
        {
            positionsCaptured = new byte[8, 8];
            
            for (var row = 0; row < 8; row++)
            {
                for (var column = 0; column < 8; column++)
                {
                    positionsCaptured[row, column] = origin.positionsCaptured[row, column];
                }
            }

            numberOfCaptures = origin.numberOfCaptures;

        }

        public void AddCapturedPosition(Vector2Int position)
        {
            positionsCaptured[position.x, position.y] = 1;
            numberOfCaptures++;
        }

        public bool IsCaptured(Vector2Int position)
        {
            return positionsCaptured[position.x, position.y] == 1;
        }

    }
    
    public class GameMoveAnalyser
    {

        private Piece[,] board;
        private PlayerType nextPlayer;
        private PlayerType oponent;
        private List<General.Move> captureMoves;
        private List<General.Move> regularMoves;

        public GameMoveAnalyser(Piece[,] board, PlayerType nextPlayer)
        {

            this.board = board;
            this.nextPlayer = nextPlayer;
            this.oponent = (nextPlayer == PlayerType.First) ? (PlayerType.Second) : (PlayerType.First);
            this.captureMoves = new List<General.Move>();
            this.regularMoves = new List<General.Move>();

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
                var lookPosition = position + GetVectorWithDirection(direction);
                if (CanCaptureAt(lookPosition))
                    return true;
            }
            
            return false;

        }
            
        public void AddCaptureMoves(Vector2Int position)
        {
            var captures = new CaptureStorage();
            var movesWithCaptures = new List< Pair<CaptureStorage, Move> >();
            var stepsHistory = new List<Step>();
            
            _AddCaptureMoves(movesWithCaptures, captures, stepsHistory, position);
            
            //SORT MOVES. then add the ones that have the most number of captures.


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
                if (CanCaptureAt(lookPosition))
                    return true;
            }
            
            return false;
            
        }
            
        public void AddKingCaptureMoves(Vector2Int position)
        {
            
            
        }

        public List<General.Move> GetAllValidMoves()
        {
            return captureMoves.Count != 0 ? captureMoves : regularMoves;
        }

        private bool CanGoTo(Vector2Int position)
        {
            return PieceAnalyser.IsValid(position) && (board[position.x, position.y] == Piece.Nothing);
        }

        private bool CanCaptureAt(Vector2Int position)
        {
            return PieceAnalyser.IsValid(position) &&
                   (PieceAnalyser.GetPlayerTypeFromPiece(board[position.x, position.y]) == oponent) &&
                   CanMakeKingRegularMove(position);
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

        private void _AddCaptureMoves(List< Pair<CaptureStorage, Move> > movesWithCaptures, CaptureStorage captures, List<Step> stepsHistory, Vector2Int lastPosition)
        {
            var capturedSomething = false;
            
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var lookPosition = lastPosition + GetVectorWithDirection(direction);
                if (CanCaptureAt(lookPosition) && !captures.IsCaptured(lookPosition))
                {
                    capturedSomething = true;
                    
                    captures.AddCapturedPosition(lookPosition);
                    stepsHistory.Add(new Step(lastPosition, lookPosition));

                    foreach (Direction nextDirection in Enum.GetValues(typeof(Direction)))
                    {
                        var nextLookPosition = lookPosition + GetVectorWithDirection(nextDirection);
                        if (CanGoTo(nextLookPosition))
                        {
                            stepsHistory.Add(new Step(lookPosition, nextLookPosition));
                            _AddCaptureMoves( movesWithCaptures, new(captures), new(stepsHistory), nextLookPosition );
                        }
                    }
                    
                }
                   
            }

            if (!capturedSomething)
            {
                var move = new Move(stepsHistory);
                movesWithCaptures.Add(new Pair<CaptureStorage, Move>( captures, move ));
            }
        }
        

    }
    
}
