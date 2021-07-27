using System;
using System.Collections;
using Firebase.Analytics;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace games.almost_purrfect.fastcube.behaviours
{
    public class GameManager : MonoBehaviour
    {
        public static bool LevelReady;

        public static bool IsConnectedToGooglePlayServices;

        public static bool IsAnalyticsAvailable;

        public static bool IsGamePlaying;

        public static int CurrentScore = 0;

        public static event Action OnGamePaused;

        public static event Action OnGameUnpaused;

        [SerializeField]
        private TextMeshProUGUI debugText;

        [SerializeField]
        private TextMeshProUGUI scoreText;

        [SerializeField] private GameObject player;

        public void PauseGame()
        {
            Debug.Log("GAME PAUSED");
            IsGamePlaying = false;
            OnGamePaused?.Invoke();
        }

        public void UnpauseGame()
        {
            Debug.Log("GAME UNPAUSED");
            IsGamePlaying = true;
            OnGameUnpaused?.Invoke();
        }

        public void RestartLevel()
        {
            CurrentScore = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void ExitGame()
        {
            Application.Quit(0);
        }

        private void Start()
        {
            IsGamePlaying = true;
            SignInToGooglePlayServices();
        }

        private void SignInToGooglePlayServices()
        {
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();

            PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (result) =>
            {
                Debug.Log("Google Play sign in result");
                Debug.Log(result);

                IsConnectedToGooglePlayServices = result switch
                {
                    SignInStatus.Success => true,
                    _ => false
                };

                if (IsConnectedToGooglePlayServices && IsAnalyticsAvailable)
                {
                    FirebaseAnalytics.SetUserProperty(
                        FirebaseAnalytics.UserPropertySignUpMethod, "Google Play Game Services");
                    FirebaseAnalytics.SetUserId(PlayGamesPlatform.Instance.GetUserDisplayName());
                    FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
                }
            });
        }

        private void Update()
        {
            if (player != null)
            {
                player.GetComponent<Player>().enabled = LevelReady;
            }

            if (player != null && player.transform.position.y < -10f)
            {
                if (IsAnalyticsAvailable)
                {
                    FirebaseAnalytics.LogEvent("died", new Parameter("how", "fall"));
                }
                StartCoroutine(GameOver());
            }

            scoreText.text = CurrentScore.ToString();
        }

        private IEnumerator GameOver()
        {
            Destroy(player);
            yield return new WaitForSeconds(1f);
            RestartLevel();

            yield return null;
        }
    }
}
