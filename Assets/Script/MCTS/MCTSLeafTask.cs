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
    public struct MCTSLeafTask
    {
        public static float MakeSimulation(Game game, PlayerType player)
        {
            var copy = game.CreateClone();
            
            var state = copy.MakeRandomMove();

            while (state == GameState.InProgress)
            {
                state = copy.MakeRandomMove();
            }
            
            return GetRewardFromState(state, player);
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