using UnityEngine;
using System;


using SA.Foundation.Config;
using SA.Foundation.Patterns;


namespace SA.CrossPlatform.Advertisement
{
    public class UM_UnityAdsSettings : SA_ScriptableSingleton<UM_UnityAdsSettings>
    {
        public UM_PlatfromAdIds AndroidIds;
        public UM_PlatfromAdIds IOSIds;

        public bool TestMode = false;

        public UM_PlatfromAdIds Platform {
            get {
                switch (Application.platform) {
                    case RuntimePlatform.Android:
                        return AndroidIds;
                    case RuntimePlatform.IPhonePlayer:
                        return IOSIds;
                    default:
                        return new UM_PlatfromAdIds();
                }
            }
        }

        //--------------------------------------
        // SA_ScriptableSettings
        //--------------------------------------

        public override string PluginName {
            get {
                return "UM Unity Ads";
            }
        }

        public override string DocumentationURL {
            get {
                return "https://unionassets.com/ultimate-mobile-pro/unity-ads-783";
            }
        }

        public override string SettingsUIMenuItem {
            get {
                return SA_Config.EDITOR_MENU_ROOT + "Cross-Platform/3rd-Party";
            }
        }

        protected override string BasePath {
            get {
                return string.Empty;
            }
        }


      
    }
}