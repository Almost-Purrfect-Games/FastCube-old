using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace games.almost_purrfect.fastcube.behaviours
{
    public class GameManager : MonoBehaviour
    {

        [SerializeField]
        private GameObject player;

        private void Update()
        {
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
