using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Checkers
{
    
    public class CellClick : General.CellClick
    {

        public static Action<Vector2Int> OnSelectPosition;

        public Vector2Int position;

        private SpriteRenderer _sr;

        [SerializeField] private bool IsReadyButton;

        private void OnEnable()
        {
            ClickManager.OnClearAll += Clear;
        }

        private void OnDisable()
        {
            ClickManager.OnClearAll -= Clear;
        }

        private void Start()
        {
            _sr = GetComponent<SpriteRenderer>();
        }

        public override void OnClick()
        {
            OnSelectPosition?.Invoke(position);

            if (!IsReadyButton)
                _sr.enabled = true;
        }

        private void Clear()
        {
            if (!IsReadyButton)
                _sr.enabled = false;
        }
    }

    
}