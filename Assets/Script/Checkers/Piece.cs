using System.Collections;
using System.Collections.Generic;
using General;
using Help;
using UnityEngine;
using UnityEngine.Animations;

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

    public class PieceAnalyser
    {
        public static PlayerType GetPlayerTypeFromPiece(Piece piece)
        {
            if (piece is Piece.Nothing)
                return PlayerType.NullPlayer;
            return piece is Piece.BasicBlack or Piece.KingBlack ? PlayerType.Second : PlayerType.First;
        }

        public static bool IsKing(Piece piece)
        {
            return piece is Piece.KingBlack or Piece.KingWhite;
        }

        public static bool IsValid(Vector2Int position)
        {
            return position.x is >= 0 and <= 7 && position.y is >= 0 and <= 7;
        }
    
    }
    
    
}
