using UnityEngine;

namespace General
{
    public abstract class BoardDrawer : MonoBehaviour
    {

        protected Game _game;

        public void SetGame(Game game)
        {
            this._game = game;
        }
        
        private void OnEnable()
        {
            Manager.OnPlay += UpdateGameDraw;
        }

        private void OnDisable()
        {
            Manager.OnPlay -= UpdateGameDraw;
        }

        public abstract void UpdateGameDraw();
    }    
}

