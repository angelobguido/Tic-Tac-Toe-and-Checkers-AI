using System.Collections.Generic;
using Help;
using Random = System.Random;

namespace General
{
    public abstract class Game
    {
        protected List<Move> validMoves;
        protected PlayerType nextPlayer = PlayerType.First;
        protected Move lastMove;
        protected GameState currentGameState = GameState.InProgress;
        
        public abstract Game CreateClone();

        protected void InitGame()
        {
            InitBoard();
            InitValidMoves();
        }

        protected void MakeCopyFrom(Game game)
        {
            CopyBoard(game);
            
            validMoves = new List<Move>(game.validMoves);
            nextPlayer = game.nextPlayer;
            lastMove = game.lastMove;
            currentGameState = game.currentGameState;

        }
        
        public GameState MakeMove(Move move)
        {
            
            VerifyMove(move);
            UpdateBoard(move);
            
            lastMove = move;
            
            UpdateValidMoves();
            
            ChangePlayer();
            
            currentGameState = CheckGameState();

            return currentGameState;

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
        
        public GameState GetCurrentGameState()
        {
            return currentGameState;
        }

        public PlayerType GetNextPlayerToPlay()
        {
            return nextPlayer;
        }
        
        private void ChangePlayer()
        {
            nextPlayer = (nextPlayer==PlayerType.First) ? (PlayerType.Second):(PlayerType.First);
        }
        
        protected abstract void InitValidMoves();
        protected abstract void InitBoard();
        protected abstract void CopyBoard(Game game);

        protected abstract void VerifyMove(Move move);
        protected abstract void UpdateBoard(Move move);

        protected abstract void UpdateValidMoves();

        protected abstract GameState CheckGameState();

    }
}


