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

        private void Awake()
        {
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
        }

        private void Start()
        {
            SignInToGooglePlayServices();
        }

        private void SignInToGooglePlayServices()
        {
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

            debugText.text = IsConnectedToGooglePlayServices ? "Connected" : "Not connected";
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
