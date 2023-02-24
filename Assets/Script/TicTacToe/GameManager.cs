using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
    public class GameManager
    {
        private static Game game = new Game();

        public static Game GetGame()
        {
            return game;
        }
        
    }
    
}
