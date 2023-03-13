using System;
using System.Collections;
using System.Collections.Generic;
using General;
using Help;
using UnityEngine;

namespace Checkers
{
    public class BoardDrawer : General.BoardDrawer
    {
        [SerializeField] private GameObject whitePiece;
        [SerializeField] private GameObject blackPiece;
        [SerializeField] private GameObject kingWhitePiece;
        [SerializeField] private GameObject kingBlackPiece;

        [SerializeField] private Transform pieces;
        [SerializeField] private Transform positions;

        private Transform[,] positionMap;
        
        private void Awake()
        {
            positionMap = new Transform[8, 8];
            var child = 0;
            
            for (var row = 0; row < 8; row++)
            {
                for (var column = 0; column < 8; column++)
                {
                    positionMap[row, column] = positions.transform.GetChild(child++);
                }
            }
        }
        
        public override void UpdateGameDraw()
        {
            ClearAll();
            var board = ((Game)_game).board;
            
            for (var row = 0; row < 8; row++)
            {
                for (var column = 0; column < 8; column++)
                {
                    switch (board[row,column])
                    {
                        case Piece.BasicBlack:
                            var obj = Instantiate(blackPiece, positionMap[row, column].position, Quaternion.identity).transform;
                            Debug.Log(obj);
                            obj.SetParent(pieces);
                            
                            break;
                        
                        case Piece.KingBlack:
                            Instantiate(kingBlackPiece, positionMap[row, column].position, Quaternion.identity).transform.SetParent(pieces);
                            break;
                        
                        case Piece.BasicWhite:
                            Instantiate(whitePiece, positionMap[row, column].position, Quaternion.identity).transform.SetParent(pieces);
                            break;
                        
                        case Piece.KingWhite:
                            Instantiate(kingWhitePiece, positionMap[row, column].position, Quaternion.identity).transform.SetParent(pieces);
                            break;
                    }
                }
            }
            
        }

        private void ClearAll()
        {
            foreach (var piece in pieces)
            {
                Destroy(((Transform)piece).gameObject);
            }
        }

    }    
}

