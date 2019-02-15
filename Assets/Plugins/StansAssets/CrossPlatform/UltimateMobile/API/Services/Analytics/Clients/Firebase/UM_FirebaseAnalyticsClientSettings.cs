using System;

namespace SA.CrossPlatform.Analytics
{
    [Serializable]
    public class UM_FirebaseAnalyticsClientSettings
    {

        /// <summary>
        /// Sets the minimum engagement time required before starting a session. 
        /// The default value 10 seconds.
        /// </summary>
        public int MinimumSessionDuration = 10;


        /// <summary>
        /// Sets the duration of inactivity that terminates the current session. 
        /// The default value is (30 minutes).
        /// </summary>
        public int SessionTimeoutDuration = 30;
    }
}