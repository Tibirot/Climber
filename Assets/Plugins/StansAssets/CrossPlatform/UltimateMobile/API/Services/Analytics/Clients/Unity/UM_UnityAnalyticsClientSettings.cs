using System;
using System.Collections.Generic;
using UnityEngine;


namespace SA.CrossPlatform.Analytics
{
    [Serializable]
    public class UM_UnityAnalyticsClientSettings
    {

        /// <summary>
        ///  Controls whether to limit user tracking at runtime.
        /// </summary>
        public bool LimitUserTracking = false;


        /// <summary>
        /// Controls whether the sending of device stats at runtime is enabled.
        /// </summary>
        public bool DeviceStatsEnabled = true;
    }
}