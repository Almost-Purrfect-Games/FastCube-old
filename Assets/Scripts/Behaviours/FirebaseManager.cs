using System;
using Firebase.Analytics;
using UnityEngine;

namespace games.almost_purrfect.fastcube.behaviours
{
    public class FirebaseManager : MonoBehaviour
    {

        public static bool IsAnalyticsAvailable;

        public static Action OnAnalyticsAvailable;
        private void Start()
        {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                IsAnalyticsAvailable = true;
                OnAnalyticsAvailable?.Invoke();
            });
        }
    }
}
