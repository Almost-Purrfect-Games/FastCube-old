using System;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace games.almost_purrfect.fastcube.behaviours
{
    public class GameManager : MonoBehaviour
    {
        public static bool LevelReady;

        [SerializeField] private GameObject player;

        private void InitializeGooglePlayServices()
        {
            var config = new PlayGamesClientConfiguration.Builder()
                .RequestServerAuthCode(false)
                .Build();

            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            Debug.Log("Google Play Games initialized");
        }

        private void Start()
        {
            InitializeGooglePlayServices();

            PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (result) =>
            {
                Debug.Log("Google Play sign in result");
                Debug.Log(result);

                if (result == SignInStatus.Success)
                {
                    Social.ReportProgress("CgkI5_2m85oQEAIQAg", 100.0, (r) =>
                    {
                        Debug.Log("Achievement reported");
                        Debug.Log(r);
                    });
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
                StartCoroutine(GameOver());
            }
        }

        private IEnumerator GameOver()
        {
            Destroy(player);
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            yield return null;
        }
    }
}
