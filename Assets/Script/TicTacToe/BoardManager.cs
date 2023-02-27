using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
    
    public class BoardManager : MonoBehaviour
    {
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 worldPoint = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Collider2D collider = Physics2D.OverlapPoint(worldPoint);
                if (collider != null)
                {
                    var cell = collider.GetComponent<CellClick>();
                    if (cell != null)
                    {
                        cell.OnClick();
                    }
                }
            }
        }
    }
    
}
