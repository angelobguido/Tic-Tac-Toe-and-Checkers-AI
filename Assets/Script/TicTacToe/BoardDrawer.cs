using System;
using System.Collections;
using System.Collections.Generic;
using General;
using Help;
using UnityEngine;

namespace TicTacToe
{
    public class BoardDrawer : General.BoardDrawer
    {
        [SerializeField] private Transform[] positions;
        [SerializeField] private GameObject firstPlayerPiece;
        [SerializeField] private GameObject secondPlayerPiece;

        public override void UpdateGameDraw()
        {

            if (_game.GetLastMove() == null)
                return;
            
            var playerPiece = (_game.GetNextPlayerToPlay() == PlayerType.First)?(secondPlayerPiece):(firstPlayerPiece);
            var position = (Move)_game.GetLastMove();
            Instantiate(playerPiece, positions[position.row*3 + position.column].position, Quaternion.identity);
        }

    }    
}

