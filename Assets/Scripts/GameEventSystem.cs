using UnityEngine;

namespace DefaultNamespace
{
    public class GameEventSystem : MonoBehaviour
    {
        public static GameState currentGameState = GameState.StartMenu;
        
        public void StartGame()
        {
            GameEventSystem.currentGameState = GameState.Playing;
        }

        public void Update()
        {
            if (currentGameState != GameState.StartMenu) return;
            
            if(Input.GetKeyDown (KeyCode.Return))
            {
                StartGame();
            }
        }
    }

    public enum GameState
    {
        StartMenu,
        Playing,
        GameOver
    }
}