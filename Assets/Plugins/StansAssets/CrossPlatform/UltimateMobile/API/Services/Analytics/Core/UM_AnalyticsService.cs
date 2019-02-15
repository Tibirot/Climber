using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.CrossPlatform.Analytics
{
    /// <summary>
    /// Main entry point for the Advertisement Services APIs. 
    /// </summary>
    public static class UM_AnalyticsService
    {

        private static UM_iAnalyticsClient s_client = null;


        /// <summary>
        /// Returns analytics client.
        /// </summary>
        public static UM_iAnalyticsClient Client {
            get {
                if(s_client == null) {
                    s_client = new UM_MasterAnalyticsClient();
                    UM_AnalyticsInternal.Init();
                }
                return s_client;
            }
        }

    }
}