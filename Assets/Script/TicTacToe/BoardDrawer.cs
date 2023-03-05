using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
    public class BoardDrawer : MonoBehaviour
    {
        [SerializeField] private Transform[] positions;
        [SerializeField] private GameObject firstPlayerPiece;
        [SerializeField] private GameObject secondPlayerPiece;

        private void OnEnable()
        {
            GameManager.OnPlay += Draw;
        }

        private void OnDisable()
        {
            GameManager.OnPlay -= Draw;
        }

        public void Draw(PlayerType player, int row, int column)
        {
            var playerPiece = (player == PlayerType.First) ? firstPlayerPiece : secondPlayerPiece;
            Instantiate(playerPiece, positions[row*3 + column].position, Quaternion.identity);
        }
    }    
}

