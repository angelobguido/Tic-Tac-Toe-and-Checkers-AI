using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType: byte
{
    First, //X
    Second //O
}

public enum GameState: byte
{
    PlayerOneWins,
    PlayerTwoWins,
    Tie,
    InProgress
}

namespace Checkers
{
    public enum Piece: byte
    {
        BasicBlack,
        KingBlack,
        BasicWhite,
        KingWhite,
        Nothing
    }
    
}
