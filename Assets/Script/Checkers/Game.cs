using System.Collections;
using System.Collections.Generic;
using General;
using Help;
using UnityEngine;
using Random = System.Random;

namespace Checkers
{
    public class Game: General.Game
    {
        public Piece[,] board;

        public override General.Game CreateClone()
        {
            return new Game(this);
        }
        
        public Game()
        {
            InitGame();
        }

        public Game(General.Game copy)
        {
            MakeCopyFrom(copy);
        }

        protected override void InitValidMoves()
        {
            //
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
            //
        }

        protected override void UpdateBoard(General.Move move)
        {
            //
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
            //check checkers state
            return GameState.InProgress;
        }

    }
}


