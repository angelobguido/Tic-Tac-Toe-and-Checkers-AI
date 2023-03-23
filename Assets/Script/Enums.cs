using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Help
{
    public enum PlayerType: byte
    {
        First, //X
        Second,//O
        NullPlayer
    }

    public enum GameState: byte
    {
        PlayerOneWins,
        PlayerTwoWins,
        Tie,
        InProgress
    }

    public enum Piece : byte
    {
        First,
        Second,
        Black,
        White,
        KingBlack,
        KingWhite
    }

    public enum GameType : byte
    {
        Checkers,
        TicTacToe
    }
}