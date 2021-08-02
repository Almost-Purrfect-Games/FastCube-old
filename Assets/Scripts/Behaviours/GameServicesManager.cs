using System;
using Firebase.Analytics;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.UI;

namespace games.almost_purrfect.fastcube.behaviours
{
    public class GameServicesManager : MonoBehaviour
    {

        public static bool IsConnectedToGooglePlayServices;

        [SerializeField]
        private Button leaderboardButton;


        public void ShowLeaderboard()
        {
            Social.ShowLeaderboardUI();
        }

        public static void ReportScore(Action callback)
        {
            if (!IsConnectedToGooglePlayServices)
            {
                Debug.Log("Not reporting score - not connected to GPGS");
                callback();
                return;
            }

            Social.ReportScore(GameStateManager.CurrentScore, GPGSIds.leaderboard_infinite_run,
                (success) =>
                {
                    if (success)
                    {
                        Debug.Log("Score reported");
                    }
                    else
                    {
                        Debug.Log("Score was not reported");
                    }

                    callback();
                });
        }

        private void Start()
        {
            SignInToGooglePlayServices(() =>
            {
                FirebaseManager.OnAnalyticsAvailable += OnFirebaseAnalyticsAvailable;
            });
        }

        private void Update()
        {
            leaderboardButton.interactable = IsConnectedToGooglePlayServices;
        }

        public void OnFirebaseAnalyticsAvailable()
        {
            Debug.Log("ON FIREBASE ANALYTICS AVAILABLE CALLED");
            Debug.Log($"Connected to GPGS: {IsConnectedToGooglePlayServices}");
            if (IsConnectedToGooglePlayServices)
            {
                FirebaseAnalytics.SetUserProperty(
                    FirebaseAnalytics.UserPropertySignUpMethod, "Google Play Game Services");
                FirebaseAnalytics.SetUserId(PlayGamesPlatform.Instance.GetUserDisplayName());
                FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
            }

            FirebaseManager.OnAnalyticsAvailable -= OnFirebaseAnalyticsAvailable;
        }

        private void SignInToGooglePlayServices(Action callback)
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

                callback();
            });
        }
    }

}
