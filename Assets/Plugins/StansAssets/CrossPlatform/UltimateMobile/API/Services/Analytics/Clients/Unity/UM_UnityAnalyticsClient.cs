using System;
using System.Collections.Generic;
using UnityAnalytics = UnityEngine.Analytics;




namespace SA.CrossPlatform.Analytics
{
    internal class UM_UnityAnalyticsClient : UM_iAnalyticsInternalClient
    {

        public UM_UnityAnalyticsClient() {
            var unityClient = UM_Settings.Instance.Analytics.UnityClient;

            UnityAnalytics.Analytics.limitUserTracking = unityClient.LimitUserTracking;
            UnityAnalytics.Analytics.deviceStatsEnabled = unityClient.DeviceStatsEnabled;
        }       


        public void Event(string eventName) {
            UnityAnalytics.Analytics.CustomEvent(eventName);
        }

        public void Event(string eventName, IDictionary<string, object> data) {
            UnityAnalytics.Analytics.CustomEvent(eventName, data);
        }


        public void Transaction(string productId, float amount, string currency) {

            //float(32 bit) to decimal(128bit) is safe
            //decimal(128bit) to float(32 bit) is not
            UnityAnalytics.Analytics.Transaction(productId, Convert.ToDecimal(amount), currency);
        }


        public void SetUserId(string userId) {
            UnityAnalytics.Analytics.SetUserId(userId);
        }


        public void SetUserBirthYear(int birthYear) {
            UnityAnalytics.Analytics.SetUserBirthYear(birthYear);
        }


        public void SetUserGender(UM_Gender gender) {

            var unityGender = UnityAnalytics.Gender.Unknown;
            switch (gender) {
                case UM_Gender.Male:
                    unityGender = UnityAnalytics.Gender.Male;
                    break;
                case UM_Gender.Female:
                    unityGender = UnityAnalytics.Gender.Female;
                    break;
            }
            UnityAnalytics.Analytics.SetUserGender(unityGender);
        }

    }
}
