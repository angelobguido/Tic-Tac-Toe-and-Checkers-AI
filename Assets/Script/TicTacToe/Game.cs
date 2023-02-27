using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace TicTacToe
{
    public class Game
    {
        private char[,] board;
        private List<Move> validMoves;
        private PlayerType nextPlayer = PlayerType.First;
        private Move lastMove;
        private GameState currentGameState = GameState.InProgress;

        public Game()
        {
            board = new char[3, 3] {{' ', ' ', ' '} , {' ', ' ', ' '}, {' ', ' ', ' '}};
            lastMove = new Move(-1, -1);
            InitValidMoves();
        }

        public Game(Game copy)
        {
            board = new char[3, 3];
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    board[i, j] = copy.board[i, j];
                }
            }

            validMoves = new List<Move>(copy.validMoves);
            nextPlayer = copy.nextPlayer;
            lastMove = copy.lastMove;
            currentGameState = copy.currentGameState;

        }

        private void InitValidMoves()
        {
            validMoves = new List<Move>();
            
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    validMoves.Add(new Move(i,j));    
                }
            }
        }

        public GameState MakeMove(Move move)
        {
            if (board[move.row,move.column] == 'X' || board[move.row,move.column] == 'O')
            {
                throw new System.Exception("Player " + ( (nextPlayer == PlayerType.First) ? 'X':'O') + " can't play at that position: " + $"({move.row}, {move.column})" );
            }

            board[move.row,move.column] = GetPlayerRepresentation();
            lastMove = move;
            var moveIndex = validMoves.FindIndex(validMove => validMove.Equals(move));
            validMoves.RemoveAt(moveIndex);

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
        
        private GameState CheckGameState()
        {
            // Check rows for a win
            for (int row = 0; row < 3; row++)
            {
                if (board[row, 0] != ' ' && board[row, 0] == board[row, 1] && board[row, 1] == board[row, 2])
                {
                    if (board[row, 0] == 'X')
                    {
                        return GameState.PlayerOneWins;
                    }
                    else
                    {
                        return GameState.PlayerTwoWins;
                    }
                }
            }

            // Check columns for a win
            for (int col = 0; col < 3; col++)
            {
                if (board[0, col] != ' ' && board[0, col] == board[1, col] && board[1, col] == board[2, col])
                {
                    if (board[0, col] == 'X')
                    {
                        return GameState.PlayerOneWins;
                    }
                    else
                    {
                        return GameState.PlayerTwoWins;
                    }
                }
            }

            // Check diagonals for a win
            if (board[1, 1] != ' ' &&
                ((board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2]) ||
                 (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])))
            {
                if (board[1, 1] == 'X')
                {
                    return GameState.PlayerOneWins;
                }
                else
                {
                    return GameState.PlayerTwoWins;
                }
            }

            // Check if the game is a tie
            bool isTie = true;
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board[row, col] == ' ')
                    {
                        isTie = false;
                        break;
                    }
                }
                if (!isTie)
                {
                    break;
                }
            }
            if (isTie)
            {
                return GameState.Tie;
            }

            // If none of the above conditions are met, the game is still in progress
            return GameState.InProgress;
        }
        
        private void ChangePlayer()
        {
            nextPlayer = (nextPlayer==PlayerType.First) ? (PlayerType.Second):(PlayerType.First);
        }

        private char GetPlayerRepresentation()
        {
            return (nextPlayer == PlayerType.First) ? 'X' : 'O';
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

    }
}


