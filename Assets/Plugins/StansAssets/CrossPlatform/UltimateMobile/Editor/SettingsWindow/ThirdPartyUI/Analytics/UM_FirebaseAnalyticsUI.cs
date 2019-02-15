using UnityEngine;
using UnityEditor;

using SA.Android;
using SA.Android.Firebase;
using SA.Foundation.Editor;

namespace SA.CrossPlatform
{
    public static class UM_FirebaseAnalyticsUI
    {
        static GUIContent s_minimumSessionDuration = new GUIContent("Minimum Session Duration");
        static GUIContent s_sessionTimeoutDuration = new GUIContent("Session Timeout Duration");


        public static void OnGUI() {
            if(AN_FirebaseDefinesResolver.IsAnalyticsSDKInstalled) {
                var firebaseClient = UM_Settings.Instance.Analytics.FirebaseClient;

                EditorGUILayout.HelpBox("The minimum engagement time required before starting a session. (seconds)", MessageType.Info);
                firebaseClient.MinimumSessionDuration = SA_EditorGUILayout.IntField(s_minimumSessionDuration, firebaseClient.MinimumSessionDuration);
                EditorGUILayout.Space();


                EditorGUILayout.HelpBox("Controls whether the sending of device stats at runtime is enabled. (seconds)", MessageType.Info);
                firebaseClient.SessionTimeoutDuration = SA_EditorGUILayout.IntField(s_sessionTimeoutDuration, firebaseClient.SessionTimeoutDuration);

            } else {
                AN_FirebaseFeaturesUI.DrawAnalyticsInstalRequired();
            }
        }
    }
}