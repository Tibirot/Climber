using System;
using System.Collections.Generic;

using SA.Android.Firebase.Analytics;


namespace SA.CrossPlatform.Analytics
{
    internal class UM_FirebaseAnalyticsClient : UM_iAnalyticsInternalClient
    {

        public UM_FirebaseAnalyticsClient() {

            var firebaseClient = UM_Settings.Instance.Analytics.FirebaseClient;


            AN_FirebaseAnalytics.SetMinimumSessionDuration(TimeSpan.FromSeconds(firebaseClient.MinimumSessionDuration));
            AN_FirebaseAnalytics.SetSessionTimeoutDuration(TimeSpan.FromSeconds(firebaseClient.SessionTimeoutDuration));
        }

        public void Event(string eventName) {
            AN_FirebaseAnalytics.LogEvent(eventName);
        }

        public void Event(string eventName, IDictionary<string, object> data) {
            AN_FirebaseAnalytics.LogEvent(eventName, data);
        }


        public void Transaction(string productId, float amount, string currency) {
            AN_FirebaseAnalytics.Transaction(productId, amount, currency);
        }


        public void SetUserId(string userId) {
            AN_FirebaseAnalytics.SetUserId(userId);
        }


        public void SetUserBirthYear(int birthYear) {
            AN_FirebaseAnalytics.SetUserProperty("birthYear", birthYear.ToString());
        }


        public void SetUserGender(UM_Gender gender) {
            AN_FirebaseAnalytics.SetUserProperty("gender", gender.ToString());
        }

        public void Init() {
            throw new NotImplementedException();
        }
    }
}
