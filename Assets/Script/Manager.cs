using System;
using System.Collections;
using Help;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace General
{
    public class Manager : MonoBehaviour
    {

        [SerializeField] private BoardDrawer drawer;
        [SerializeField] private Player first;
        [SerializeField] private Player second;
        [SerializeField] private int nextScene;
        [SerializeField] private GameType gameType;
        
        public static Game game;

        public static Action OnPlay;

        private void Awake()
        {
            switch (gameType)
            {
                case GameType.TicTacToe: game = new TicTacToe.Game();
                    break;
                case GameType.Checkers: game = new Checkers.Game();
                    break;
            }

            drawer.SetGame(game);

        }

        private IEnumerator Start()
        {

            drawer.UpdateGameDraw();
            yield return new WaitForSeconds(1f);
            Debug.Log("Game is ready!");
            
            while (game.GetCurrentGameState() == GameState.InProgress)
            {
                var player = (game.GetNextPlayerToPlay() == PlayerType.First) ? (first) : (second);
                
                StartCoroutine(player.MakeDecision());
                yield return new WaitUntil( () => player.IsReady() );
                var move = player.GetChosenMove();
                
                game.MakeMove(move);

                if (gameType == GameType.Checkers)
                {

                    var cMove = (Checkers.Move)game.GetLastMove();
                    Debug.Log(cMove.MoveToString());
                }
                
                OnPlay?.Invoke();

                var state = game.GetCurrentGameState();
                
                switch (state)
                {
                    case GameState.Tie: Debug.Log("It's a tie!");
                        break;
                    case GameState.PlayerOneWins: Debug.Log("Player one won!");
                        break;
                    case GameState.PlayerTwoWins: Debug.Log("Player two won!");
                        break;
                }
                
            }

            SceneManager.LoadScene(nextScene);
        }
    }
    
}
