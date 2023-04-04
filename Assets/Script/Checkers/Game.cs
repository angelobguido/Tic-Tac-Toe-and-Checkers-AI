using System.Collections;
using System.Collections.Generic;
using General;
using Help;
using Unity.Collections;
using UnityEngine;
using Random = System.Random;

namespace Checkers
{
    public class Game: General.Game
    {
        public Piece[,] board;
        private int numberOfNoCaptures = 0;

        public override General.Game CreateClone()
        {
            return new Game(this);
        }
        
        public Game(NativeArray<Piece> boardArray, PlayerType player)
        {
            board = new Piece[8, 8];
            var i = 0;
            for (var row = 0; row < 8; row++)
            {
                for (var column = 0; column < 8; column++)
                {
                    board[row, column] = boardArray[i];
                    i++;
                }
            }

            this.nextPlayer = player;
            UpdateValidMoves();
        }
        
        public Game()
        {
            InitGame();
        }

        public Game(General.Game copy)
        {
            numberOfNoCaptures = ((Checkers.Game)copy).numberOfNoCaptures;
            MakeCopyFrom(copy);
        }

        protected override void InitValidMoves()
        {
            UpdateValidMoves();
        }

        protected override void InitBoard()
        {
            board = new Piece[8, 8];
            
            for (var row = 0; row < 8; row++)
            {
                for (var column = 0; column < 8; column++)
                {
                    //Check if the row position is valid for the initial board, then check if it is inside the black square
                    if (  (row is <= 2 or >= 5 ) && (row % 2 == (column + 1) % 2))
                    {
                        var piece = (row >= 5) ? Piece.BasicWhite : Piece.BasicBlack;
                        board[row, column] = piece;
                    }
                    else
                    {
                        board[row, column] = Piece.Nothing;
                    }
                    
                }
            }
        }

        protected override void CopyBoard(General.Game game)
        {
            board = new Piece[8, 8];
            var otherBoard = ((Game)game).board;
            
            for (var row = 0; row < 8; row++)
            {
                for (var column = 0; column < 8; column++)
                {
                    board[row, column] = otherBoard[row, column];
                }
            }
        }

        protected override void VerifyMove(General.Move move)
        {
            var cMove = (Move)move;
            
            if (!validMoves.Exists( (a) => a.Equals(cMove) ))
            {
                throw new System.Exception("Player " + ( (nextPlayer == PlayerType.First) ? "White":"Black") + " can't play at that position" );
            }
        }

        protected override void UpdateBoard(General.Move move)
        {
            var cMove = (Move)move;
            var piece = board[cMove.step.from.x, cMove.step.from.y];
            board[cMove.step.from.x, cMove.step.from.y] = Piece.Nothing;

            cMove.captures?.capturesPositions.ForEach( (a) => board[a.x,a.y] = Piece.Nothing );

            if (cMove.captures == null)
            {
                numberOfNoCaptures++;
            }
            else
            {
                numberOfNoCaptures = 0;
            }

            if (IsPromotionPosition(cMove.step.to, piece))
            {
                board[cMove.step.to.x, cMove.step.to.y] = (PieceAnalyser.GetPlayerTypeFromPiece(piece) == PlayerType.First) ? (Piece.KingWhite) : (Piece.KingBlack);    
            }
            else
            {
                board[cMove.step.to.x, cMove.step.to.y] = piece;
            }
            
        }

        private bool IsPromotionPosition(Vector2Int position, Piece piece)
        {
            var finalRow = (PieceAnalyser.GetPlayerTypeFromPiece(piece) == PlayerType.First) ? 0 : 7;

            return finalRow == position.x;
        }

        protected override void UpdateValidMoves()
        {
            // Create an analyser object
            var moveAnalyser = new GameMoveAnalyser(board, nextPlayer);
            
            // Loop through all pieces on the board
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var piece = board[row, col];

                    var position = new Vector2Int(row, col);

                    // Check if this piece belongs to the current player
                    if (nextPlayer == PieceAnalyser.GetPlayerTypeFromPiece(piece))
                    {
                        if (PieceAnalyser.IsKing(piece))
                        {
                            if (moveAnalyser.CanMakeKingCaptureMove(position))
                            {
                                moveAnalyser.AddKingCaptureMoves(position);
                            }
                            
                            else if (moveAnalyser.CanMakeKingRegularMove(position))
                            {
                                moveAnalyser.AddKingRegularMoves(position);
                            }
                        }

                        else
                        {
                            if (moveAnalyser.CanMakeCaptureMove(position))
                            {
                                moveAnalyser.AddCaptureMoves(position);
                            }
                            
                            else if (moveAnalyser.CanMakeRegularMove(position))
                            {
                                moveAnalyser.AddRegularMoves(position);
                            }
                        }
                    }
                }
            }

            validMoves = moveAnalyser.GetAllValidMoves();
        }
        
        protected override GameState CheckGameState()
        {
            if(validMoves.Count == 0)
                return (nextPlayer==PlayerType.First)?(GameState.PlayerTwoWins):(GameState.PlayerOneWins);

            if (numberOfNoCaptures == 40)
                return GameState.Tie;
            
            return GameState.InProgress;
        }

    }
}


