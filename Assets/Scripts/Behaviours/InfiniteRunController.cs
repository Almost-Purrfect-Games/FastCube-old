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
    }
}
