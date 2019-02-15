using System.Collections.Generic;

using SA.Facebook;


namespace SA.CrossPlatform.Analytics
{
    internal class UM_FacebookAnalyticsClient : UM_iAnalyticsInternalClient
    {

        public void Event(string eventName) {
            SA_FB_Analytics.LogAppEvent(eventName);
        }

        public void Event(string eventName, IDictionary<string, object> data) {

            var fbData = (Dictionary<string, object>) data;
            SA_FB_Analytics.LogAppEvent(eventName, null, fbData);
        }


        public void Transaction(string productId, float amount, string currency) {

            var data = new Dictionary<string, object>();
            data.Add("productId", productId);

            SA_FB_Analytics.LogPurchase(amount, currency, data);
        }


        public void SetUserId(string userId) {
           //Not supported
        }


        public void SetUserBirthYear(int birthYear) {
            //Not supported
        }


        public void SetUserGender(UM_Gender gender) {
            //Not supported
        }

    }
}
