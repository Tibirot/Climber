using System;
using System.Collections.Generic;
using UnityEngine;

namespace SA.CrossPlatform.Analytics
{
    [Serializable]
    public class UM_AnalyticsSettings
    {
        public UM_UnityAnalyticsClientSettings UnityClient = new UM_UnityAnalyticsClientSettings();
        public UM_FirebaseAnalyticsClientSettings FirebaseClient = new UM_FirebaseAnalyticsClientSettings();
        public UM_AnalyticsAutomationSettings Automation = new UM_AnalyticsAutomationSettings();
    }
}
