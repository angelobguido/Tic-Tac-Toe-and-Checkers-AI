using System.Collections;
using System.Collections.Generic;
using General;
using Help;
using UnityEngine;
using Random = System.Random;

namespace Checkers
{
    public class Game
    {
        private Piece[,] board;
        private List<Move> validMoves;
        private PlayerType nextPlayer = PlayerType.First;
        private Move lastMove;
        private GameState currentGameState = GameState.InProgress;

        public Game()
        {
            InitBoard();
            InitValidMoves();
        }

        public Game(Game copy)
        {
            
            board = new Piece[8, 8];
            for (var row = 0; row < 8; row++)
            {
                for (var column = 0; column < 8; column++)
                {
                    board[row, column] = copy.board[row, column];
                }
            }

            validMoves = new List<Move>(copy.validMoves);
            nextPlayer = copy.nextPlayer;
            lastMove = copy.lastMove;
            currentGameState = copy.currentGameState;

        }

        public GameState MakeMove(Move move)
        {
            //Make move
            
            //Update valid moves

            ChangePlayer();
            
            currentGameState = CheckGameState();

            return currentGameState;

        }

        public GameState GetCurrentGameState()
        {
            return currentGameState;
        }

        public PlayerType GetNextPlayerToPlay()
        {
            return nextPlayer;
        }
        
        private void InitBoard()
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

        private void InitValidMoves()
        {
            //
        }

        
        private GameState CheckGameState()
        {
            //check checkers state
            return currentGameState;
        }
        
        private void ChangePlayer()
        {
            nextPlayer = (nextPlayer==PlayerType.First) ? (PlayerType.Second):(PlayerType.First);
        }

        public GameState MakeRandomMove()
        {
            var r = new Random();
            var randomMove = validMoves[r.Next(0, validMoves.Count)];

            return MakeMove(randomMove);
        }

        public Move GetLastMove()
        {
            return lastMove;
        }

        public List<Move> GetValidMoves()
        {
            return validMoves;
        }

        private void UpdateValidMoves()
        {
            // Create an analyser object
            var moveAnalyser = new GameMoveAnalyser(board, nextPlayer);
            
            // Determine the opponent's piece color
            var opponentPiece = (nextPlayer == PlayerType.Second) ? Piece.BasicWhite : Piece.BasicBlack;
            var opponentKing = (nextPlayer == PlayerType.Second) ? Piece.KingWhite : Piece.KingBlack;

            // Loop through all pieces on the board
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    // Check if this piece belongs to the current player
                    var piece = board[row, col];

                    var position = new Vector2Int(row, col);

                    if ((nextPlayer == PlayerType.Second && piece == Piece.BasicBlack) ||
                        (nextPlayer == PlayerType.First && piece == Piece.BasicWhite))
                    {
                        // Check if this piece can make a regular move
                        if (moveAnalyser.CanMakeRegularMove(position))
                        {
                            moveAnalyser.AddRegularMoves(position);
                        }

                        // Check if this piece can make a capture move
                        if (moveAnalyser.CanMakeCaptureMove(position))
                        {
                            moveAnalyser.AddCaptureMoves(position);
                        }
                    }
                    // Check if this piece belongs to the current player and is a king
                    else if ((nextPlayer == PlayerType.Second && piece == Piece.BasicBlack) ||
                             (nextPlayer == PlayerType.First && piece == Piece.BasicWhite))
                    {
                        // Check if this king can make a regular move
                        if (moveAnalyser.CanMakeKingRegularMove(position))
                        {
                            moveAnalyser.AddKingRegularMoves(position);
                        }

                        // Check if this king can make a capture move
                        if (moveAnalyser.CanMakeKingCaptureMove(position))
                        {
                            moveAnalyser.AddKingCaptureMoves(position);
                        }
                    }
                }
            }
        }

    }
}


