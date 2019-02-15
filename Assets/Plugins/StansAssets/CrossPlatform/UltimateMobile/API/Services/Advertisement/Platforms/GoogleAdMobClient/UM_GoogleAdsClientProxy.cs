using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SA.Foundation.Utility;

namespace SA.CrossPlatform.Advertisement
{
    public static class UM_GoogleAdsClientProxy {

        private const string CLIENT_CLASS_NAME = "SA.CrossPlatform.Advertisement.UM_GoogleAdsClient";
        private const string SETTINGS_UI_CLASS_NAME = "SA.CrossPlatform.Advertisement.UM_GoogleMobileAdsUI";


        public static object CreateSettingsLayout() {
            return SA_Reflection.CreateInstance(SETTINGS_UI_CLASS_NAME);
        }


        public static UM_iAdsClient CreateAdsClient() {
            return SA_Reflection.CreateInstance(CLIENT_CLASS_NAME) as UM_iAdsClient;
        }

    }
}