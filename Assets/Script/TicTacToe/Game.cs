using System.Collections;
using System.Collections.Generic;

namespace TicTacToe
{
    public class Game
    {
        private char[,] board;
        private PlayerType currentPlayer = PlayerType.First;

        public void MakeMove(int row, int column)
        {
            if (board[row,column] == 'X' || board[row,column] == 'O')
            {
                throw new System.Exception("Player " + ( (currentPlayer == PlayerType.First) ? 'X':'O') + " can't play at that position" );
            }

            board[row,column] = GetPlayerRepresentation();

            switch (CheckGameState())
            {
                case GameState.PlayerOneWins:
                    throw new System.Exception("Player " + ( (currentPlayer == PlayerType.First) ? 'X':'O') + " won!" );
                
                case GameState.PlayerTwoWins:
                    throw new System.Exception("Player " + ( (currentPlayer == PlayerType.First) ? 'X':'O') + " won!" );
                
                case GameState.Tie:
                    throw new System.Exception("It's a tie!");
            }
            
            ChangePlayer();
        }

        private GameState CheckGameState()
        {
            // Check rows for a win
            for (int row = 0; row < 3; row++)
            {
                if (board[row, 0] != '\0' && board[row, 0] == board[row, 1] && board[row, 1] == board[row, 2])
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
                if (board[0, col] != '\0' && board[0, col] == board[1, col] && board[1, col] == board[2, col])
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
            if (board[1, 1] != '\0' &&
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
                    if (board[row, col] == '\0')
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
            currentPlayer = (currentPlayer==PlayerType.First) ? (PlayerType.Second):(PlayerType.First);
        }

        private char GetPlayerRepresentation()
        {
            return (currentPlayer == PlayerType.First) ? 'X' : 'O';
        }

    }
}


