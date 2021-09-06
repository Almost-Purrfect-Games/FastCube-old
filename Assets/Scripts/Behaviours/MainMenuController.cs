using TMPro;
using UnityEngine;

namespace games.almost_purrfect.fastcube.behaviours
{
    public class MainMenuController : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI versionLabel;

        private void Start()
        {
            versionLabel.text = $"v{Application.version}";
        }
    }
}
