using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Checkers
{
    
    public class GameMoveAnalyser
    {

        private Piece[,] board;
        private PlayerType nextPlayer;
        private List<Move> captureMoves;
        private List<Move> regularMoves;

        public GameMoveAnalyser(Piece[,] board, PlayerType nextPlayer)
        {

            this.board = board;
            this.nextPlayer = nextPlayer;
            this.captureMoves = new List<Move>();
            this.regularMoves = new List<Move>();

        }

        public bool CanMakeRegularMove(Position position)
        {
            if (captureMoves.Count != 0)
                return false;

        }

        public void AddRegularMoves(Position position)
        {
            
        }
        
        public bool CanMakeCaptureMove(Position position)
        {
            
            
        }
            
        public void AddCaptureMoves(Position position)
        {
            
        }
        
        public bool CanMakeKingRegularMove(Position position)
        {
            if (captureMoves.Count != 0)
                return false;
            
        }
            
        public void AddKingRegularMoves(Position position)
        {
            
            
        }
        
        public bool CanMakeKingCaptureMove(Position position)
        {
            
            
        }
            
        public void AddKingCaptureMoves(Position position)
        {
            
            
        }

        public List<Move> GetAllValidMoves()
        {
            return captureMoves.Count != 0 ? captureMoves : regularMoves;
        }
        
        
    }
    
}
