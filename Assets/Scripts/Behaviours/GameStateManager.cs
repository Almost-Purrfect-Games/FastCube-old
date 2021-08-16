using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace games.almost_purrfect.fastcube.behaviours
{
    /**
     * <summary>All statics that control the state of the game</summary>
     */
    public class GameStateManager : MonoBehaviour
    {
        public static bool LevelReady;

        public static bool IsGamePlaying;

        public static int CurrentScore = 0;

        public static event Action OnGamePaused;

        public static event Action OnGameUnpaused;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public static void PauseGame()
        {
            if (!IsGamePlaying) return;

            IsGamePlaying = false;
            OnGamePaused?.Invoke();
        }

        public static void UnpauseGame()
        {
            IsGamePlaying = true;
            OnGameUnpaused?.Invoke();
        }

        public void StartGame()
        {
            CurrentScore = 0;
            SceneManager.LoadScene("InfiniteRun");
        }

        public void ExitToMainMenu()
        {
            CurrentScore = 0;
            SceneManager.LoadScene("Main");
        }

        public static void RestartLevel()
        {
            CurrentScore = 0;
            SceneManager.LoadScene("InfiniteRun");
        }

        public void ExitGame()
        {
            Application.Quit(0);
        }

    }
}
