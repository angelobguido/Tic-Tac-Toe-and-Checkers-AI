using System.Collections;
using System.Collections.Generic;
using Help;
using Unity.Collections;
using UnityEngine;
using Random = System.Random;

namespace TicTacToe
{
    public class Game: General.Game
    {
        public char[,] board;

        public Game()
        {
            InitGame();
        }

        public Game(General.Game copy)
        {
            MakeCopyFrom(copy);
        }
        
        public override General.Game CreateClone()
        {
            return new Game(this);
        }

        protected override void InitValidMoves()
        {
            validMoves = new List<General.Move>();
            
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    validMoves.Add(new Move(i,j));    
                }
            }
        }

        protected override void InitBoard()
        {
            board = new char[3, 3] {{' ', ' ', ' '} , {' ', ' ', ' '}, {' ', ' ', ' '}};
        }

        protected override void CopyBoard(General.Game game)
        {
            var otherBoard = ((TicTacToe.Game)game).board;
            board = new char[3, 3];
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    board[i, j] = otherBoard[i, j];
                }
            }

        }

        protected override void VerifyMove(General.Move move)
        {
            var tttMove = (Move)move;
            
            if (board[tttMove.row,tttMove.column] == 'X' || board[tttMove.row,tttMove.column] == 'O')
            {
                throw new System.Exception("Player " + ( (nextPlayer == PlayerType.First) ? 'X':'O') + " can't play at that position: " + $"({tttMove.row}, {tttMove.column})" );
            }
        }

        protected override void UpdateBoard(General.Move move)
        {
            var tttMove = (Move)move;
            board[tttMove.row,tttMove.column] = GetPlayerRepresentation();
        }
        
        private char GetPlayerRepresentation()
        {
            return (nextPlayer == PlayerType.First) ? 'X' : 'O';
        }

        protected override void UpdateValidMoves()
        {
            var moveIndex = validMoves.FindIndex(validMove => validMove.Equals(lastMove));
            
            validMoves.RemoveAt(moveIndex);
        }
        
        protected override GameState CheckGameState()
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

    }
}


