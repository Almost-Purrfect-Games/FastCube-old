using System;
using System.Collections;
using Firebase.Analytics;
using TMPro;
using UnityEngine;

namespace games.almost_purrfect.fastcube.behaviours
{
    public class InfiniteRunController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI gameOverScoreText;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject helpButton;

        [SerializeField] private GameObject[] deactivateOnPause;
        [SerializeField] private GameObject[] deactivateOnUnpause;
        [SerializeField] private GameObject[] activateOnPause;
        [SerializeField] private GameObject[] activateOnUnpause;
        [SerializeField] private GameObject[] activateOnGameOver;
        [SerializeField] private GameObject[] deactivateOnGameOver;
        [SerializeField] private GameObject[] deactivateOnHelpOpen;
        [SerializeField] private GameObject[] deactivateOnHelpClose;
        [SerializeField] private GameObject[] activateOnHelpOpen;
        [SerializeField] private GameObject[] activateOnHelpClose;

        private void OnEnable()
        {
            GameStateManager.OnGamePaused += OnGamePaused;
            GameStateManager.OnGameUnpaused += OnGameUnpaused;
            GameStateManager.OnHelpInvoked += OnHelpInvoked;
            GameStateManager.OnHelpClosed += OnHelpClosed;
        }

        private void OnDisable()
        {
            GameStateManager.OnGamePaused -= OnGamePaused;
            GameStateManager.OnGameUnpaused -= OnGameUnpaused;
            GameStateManager.OnHelpInvoked -= OnHelpInvoked;
            GameStateManager.OnHelpClosed -= OnHelpClosed;
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

        private void Start()
        {
            StartCoroutine(nameof(DelayedHelpButtonDisplay));
        }

        private IEnumerator DelayedHelpButtonDisplay()
        {
            yield return new WaitForSeconds(5);

            if (!GameStateManager.PlayerMoved)
            {
                helpButton.SetActive(true);
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

            if (GameStateManager.PlayerMoved)
            {
                helpButton.SetActive(false);
            }

            scoreText.text = GameStateManager.CurrentScore.ToString();
            gameOverScoreText.text = GameStateManager.CurrentScore.ToString();
        }

        private void GameOver()
        {
            GameStateManager.IsGamePlaying = false;
            Destroy(player);
            GameServicesManager.ReportScore(() =>
            {
                foreach (var o in deactivateOnGameOver)
                {
                    o.SetActive(false);
                }

                foreach (var o in activateOnGameOver)
                {
                    o.SetActive(true);
                }
            });
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

        private void OnHelpInvoked()
        {
            foreach (var o in deactivateOnHelpOpen)
            {
                o.SetActive(false);
            }

            foreach (var o in activateOnHelpOpen)
            {
                o.SetActive(true);
            }
        }

        private void OnHelpClosed()
        {
            foreach (var o in deactivateOnHelpClose)
            {
                o.SetActive(false);
            }

            foreach (var o in activateOnHelpClose)
            {
                o.SetActive(true);
            }
        }
    }
}
