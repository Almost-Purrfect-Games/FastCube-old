using System;
using System.Collections;
using Firebase.Analytics;
using TMPro;
using UnityEngine;

namespace games.almost_purrfect.fastcube.behaviours
{
    public class InfiniteRunController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI scoreText;

        [SerializeField]
        private GameObject player;

        [SerializeField] private GameObject[] deactivateOnPause;
        [SerializeField] private GameObject[] deactivateOnUnpause;
        [SerializeField] private GameObject[] activateOnPause;
        [SerializeField] private GameObject[] activateOnUnpause;

        private void OnEnable()
        {
            GameStateManager.OnGamePaused += OnGamePaused;
            GameStateManager.OnGameUnpaused += OnGameUnpaused;
        }

        private void OnDisable()
        {
            GameStateManager.OnGamePaused -= OnGamePaused;
            GameStateManager.OnGameUnpaused -= OnGameUnpaused;
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                GameStateManager.PauseGame();
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                GameStateManager.PauseGame();
            }
        }

        private void Update()
        {
            if (player != null)
            {
                player.GetComponent<Player>().enabled = GameStateManager.LevelReady;
            }

            if (player != null && player.transform.position.y < -10f)
            {
                if (FirebaseManager.IsAnalyticsAvailable)
                {
                    FirebaseAnalytics.LogEvent("died", new Parameter("how", "fall"));
                }
                GameOver();
            }

            scoreText.text = GameStateManager.CurrentScore.ToString();
        }

        private void GameOver()
        {
            Destroy(player);

            GameServicesManager.ReportScore(GameStateManager.RestartLevel);
        }

        private void OnGamePaused()
        {
            foreach (var o in deactivateOnPause)
            {
                o.SetActive(false);
            }

            foreach (var o in activateOnPause)
            {
                o.SetActive(true);
            }
        }

        private void OnGameUnpaused()
        {
            foreach (var o in deactivateOnUnpause)
            {
                o.SetActive(false);
            }

            foreach (var o in activateOnUnpause)
            {
                o.SetActive(true);
            }
        }
    }
}
