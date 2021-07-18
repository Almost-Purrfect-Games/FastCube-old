using System;
using System.Collections;
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

        [SerializeField]
        private TextMeshProUGUI debugText;

        [SerializeField] private GameObject player;

        private void Start()
        {
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

            debugText.text = IsConnectedToGooglePlayServices ? "Connected to Google Play Games" : "Not connected to Google Play Games";
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
