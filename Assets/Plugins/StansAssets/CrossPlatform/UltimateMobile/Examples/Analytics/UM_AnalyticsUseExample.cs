using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SA.CrossPlatform;
using SA.CrossPlatform.Analytics;


using SA.Facebook;

public class UM_AnalyticsUseExample : MonoBehaviour
{
    
    void Start()
    {

        UM_AnalyticsService.Client.Init();

        //Unity settings via code example
        UM_Settings.Instance.Analytics.UnityClient.LimitUserTracking = true;
        UM_Settings.Instance.Analytics.UnityClient.DeviceStatsEnabled = true;


        //Firebase settings via code
        UM_Settings.Instance.Analytics.FirebaseClient.MinimumSessionDuration = 10;
        UM_Settings.Instance.Analytics.FirebaseClient.SessionTimeoutDuration = 30;

        SA_FB_Analytics.LogAppEvent("my event");

    }

  
}
