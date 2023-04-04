using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TicTacToe
{
    
    public class ClickManager : General.ClickManager
    {
        private void OnEnable()
        {
            CellClick.OnPositionClicked += TransmitMovement;
        }

        private void OnDisable()
        {
            CellClick.OnPositionClicked -= TransmitMovement;
        }

        
    }

    
}