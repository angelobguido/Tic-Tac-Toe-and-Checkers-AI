using System.Collections;
using System.Collections.Generic;
using Help;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

namespace General
{
    [BurstCompile]
    public struct MCTSLeafJob : IJobParallelFor
    {
        public NativeArray<float> result;
        
        [ReadOnly]
        public NativeArray<Checkers.Piece> checkersBoard;
        [ReadOnly]
        public NativeArray<char> tictactoeBoard;
        
        public PlayerType player;
        public GameType gameType;

        public void Execute(int i)
        {
            Game game = new TicTacToe.Game();
            switch (gameType)
            {
                case GameType.Checkers:
                    game = new Checkers.Game(checkersBoard, player);
                    break;
                
                case GameType.TicTacToe:
                    game = new TicTacToe.Game(tictactoeBoard, player);
                    break;
            }
            
            var state = game.MakeRandomMove();

            while (state == GameState.InProgress)
            {
                state = game.MakeRandomMove();
            }
            
            result[i] = GetRewardFromState(state, player);
        }
        
        public static float GetRewardFromState(GameState state, PlayerType mctsPlayer)
        {
            var reward = 0f;
            switch (state)
            {
                case GameState.Tie: 
                    reward = 0f;
                    break;
                
                case GameState.PlayerOneWins: 
                    reward = (mctsPlayer == PlayerType.First) ? (1f) : (-1f);
                    break;
                
                case GameState.PlayerTwoWins: 
                    reward = (mctsPlayer == PlayerType.First) ? (-1f) : (1f);
                    break;
                
            }

            return reward;
        }
    }

    
}