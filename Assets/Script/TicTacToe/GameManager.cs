using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TicTacToe
{
    public class GameManager : MonoBehaviour
    {

        [SerializeField] private Player first;
        [SerializeField] private Player second;
        [SerializeField] private int nextScene;
        
        public static Game game;

        public static Action<PlayerType, int, int> OnPlay;

        private void Awake()
        {
            game = new Game();
        }

        private IEnumerator Start()
        {

            yield return new WaitForSeconds(1f);
            Debug.Log("Game is ready!");
            
            while (game.GetCurrentGameState() == GameState.InProgress)
            {
                var player = (game.GetNextPlayerToPlay() == PlayerType.First) ? (first) : (second);
                
                StartCoroutine(player.MakeDecision());
                yield return new WaitUntil( () => player.IsReady() );
                var move = player.GetChosenMove();
                
                game.MakeMove(move);
                
                OnPlay?.Invoke((player == first)?(PlayerType.First):(PlayerType.Second), move.row, move.column);

                var state = game.GetCurrentGameState();
                
                switch (state)
                {
                    case GameState.Tie: Debug.Log("It's a tie!");
                        break;
                    case GameState.PlayerOneWins: Debug.Log("Player X won!");
                        break;
                    case GameState.PlayerTwoWins: Debug.Log("Player O won!");
                        break;
                }
                
            }

            SceneManager.LoadScene(nextScene);
        }
    }
    
}
