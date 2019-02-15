using UnityEngine;
using UnityEditor;

using SA.Foundation.Editor;
using SA.CrossPlatform.Advertisement;


namespace SA.CrossPlatform
{

    public class UM_AdvertisementUI : UM_PluginSettingsUI
    {

        public class UM_AdsResolver : SA_iAPIResolver
        {
            public bool IsSettingsEnabled {
                get {
                    return UM_DefinesResolver.IsAdMobInstalled || UM_DefinesResolver.IsUnityAdsInstalled;
                }

                set { }
            }
        }

        public class UM_GoogleAdsResolver : SA_iAPIResolver
        {
            public bool IsSettingsEnabled {
                get {
                    return UM_DefinesResolver.IsAdMobInstalled;
                }

                set { }
            }
        }


        public class UM_UnityAdsResolver : SA_iAPIResolver
        {
            public bool IsSettingsEnabled {
                get {
                    return UM_DefinesResolver.IsUnityAdsInstalled;
                }

                set { }
            }
        }

        public class UM_ChartBoostResolver : SA_iAPIResolver
        {
            public bool IsSettingsEnabled {
                get {
                    return false;
                }

                set { }
            }
        }


        private const string AD_MOB_SDK_DOWNLOAD_URL = "https://github.com/googleads/googleads-mobile-unity/releases/download/v3.15.1/GoogleMobileAds.unitypackage";
        private const string UNITY_ADS_SDK_DOWNLOAD_URL = "https://assetstore.unity.com/packages/add-ons/services/unity-monetization-3-0-66123";


        private UM_AdsResolver m_serviceResolver;


        private SA_iGUILayoutElement m_admobSettingsUI;

        private UM_AdvertisementPlatfromUI m_adMobBlock;
        private UM_AdvertisementPlatfromUI m_unityAdBlock;
        private UM_AdvertisementPlatfromUI m_chartboostBlock;


        public override void OnAwake() {
            base.OnAwake();

            AddFeatureUrl("Getting Started", "https://unionassets.com/ultimate-mobile-pro/getting-started-778");
            AddFeatureUrl("Initialization", "https://unionassets.com/ultimate-mobile-pro/enabling-the-ads-service-779");
            AddFeatureUrl("Banner Ads", "https://unionassets.com/ultimate-mobile-pro/banner-ads-780");
            AddFeatureUrl("Non-rewarded Ads", "https://unionassets.com/ultimate-mobile-pro/non-rewarded-ads-782");
            AddFeatureUrl("Rewarded Ads", "https://unionassets.com/ultimate-mobile-pro/rewarded-ads-781");

            AddFeatureUrl("Unity Ads", "https://unionassets.com/ultimate-mobile-pro/unity-ads-783");
            AddFeatureUrl("Google AdMob", "https://unionassets.com/ultimate-mobile-pro/google-admob-784");
            AddFeatureUrl("Google EU Consent", "https://unionassets.com/ultimate-mobile-pro/google-admob-784#consent-from-european-users");
            
            AddFeatureUrl("Chartboost", "https://unionassets.com/ultimate-mobile-pro/chartboost-785");
        }

        public override void OnLayoutEnable() {

            base.OnLayoutEnable();

            m_unityAdBlock = new UM_AdvertisementPlatfromUI("Unity Ads", "unity_icon.png", new UM_UnityAdsResolver(), () => {
                DrawUnityAdsUI();
            });

            m_adMobBlock = new UM_AdvertisementPlatfromUI("Google AdMob", "google_icon.png", new UM_GoogleAdsResolver(), () => {
                DrawAdmobUI();
            });

            m_chartboostBlock = new UM_AdvertisementPlatfromUI("Chartboost", "chartboost_icon.png", new UM_ChartBoostResolver(), () => {
                EditorGUILayout.HelpBox("COMING SOON!", MessageType.Info);
            });
        }


        public override string Title {
            get {
                return "Advertisement";
            }
        }

        public override string Description {
            get {
                return "Integrate banner, rewarded and non-rewarded adsfor you game, using the supported ads platfroms.";
            }
        }

        protected override Texture2D Icon {
            get {
                return UM_Skin.GetServiceIcon("um_advertisement_icon.png");
            }
        }

        public override SA_iAPIResolver Resolver {
            get {
                if(m_serviceResolver == null) {
                    m_serviceResolver = new UM_AdsResolver();
                }

                return m_serviceResolver;
            }
        }

        protected override void OnServiceUI() {

            m_unityAdBlock.OnGUI();
            m_adMobBlock.OnGUI();
            m_chartboostBlock.OnGUI();


        }

        private void DrawAdmobUI() {
            if (UM_DefinesResolver.IsAdMobInstalled) {
                EditorGUILayout.HelpBox("Google Mobile Ads SDK Installed!", MessageType.Info);
                DrawAdMobSettings();
            } else {

                EditorGUILayout.HelpBox("Google Mobile Ads SDK Missing!", MessageType.Warning);
                using (new SA_GuiBeginHorizontal()) {
                    GUILayout.FlexibleSpace();
                    var click = GUILayout.Button("Import SDK", EditorStyles.miniButton, GUILayout.Width(120));
                    if (click) {
                        SA_PackageManager.DownloadAndImport("Google Mobiel Ads SDK", AD_MOB_SDK_DOWNLOAD_URL, interactive:false);
                    }

                    var refreshClick = GUILayout.Button("Refresh", EditorStyles.miniButton, GUILayout.Width(120));
                    if (refreshClick) {
                        UM_DefinesResolver.ProcessAssets();
                    }
                }
            }

        }


       

       private void DrawAdMobSettings() {
            if(m_admobSettingsUI == null) {
                var settingsUI = UM_GoogleAdsClientProxy.CreateSettingsLayout();
                if (settingsUI != null) {
                    m_admobSettingsUI = (settingsUI as SA_iGUILayoutElement);
                    m_admobSettingsUI.OnLayoutEnable();
                }
            } 
            if(m_admobSettingsUI == null) {
                UM_SettingsUtil.DrawAddonRequestUI(UM_Addon.AdMob);
            } else {
                m_admobSettingsUI.OnGUI();
            }
       }




        private static void DrawUnityAdsUI() {
            if (UM_DefinesResolver.IsUnityAdsInstalled) {
                DrawUnityAdsSettins();
            } else {
                EditorGUILayout.HelpBox("Unity Monetization SDK Missing!", MessageType.Warning);
                using (new SA_GuiBeginHorizontal()) {
                    GUILayout.FlexibleSpace();
                    var click = GUILayout.Button("Download SDK", EditorStyles.miniButton, GUILayout.Width(120));
                    if (click) {
                        Application.OpenURL(UNITY_ADS_SDK_DOWNLOAD_URL);
                    }

                    var refreshClick = GUILayout.Button("Refresh", EditorStyles.miniButton, GUILayout.Width(120));
                    if (refreshClick) {
                        UM_DefinesResolver.ProcessAssets();
                    }
                }
            }
        }
        private static void DrawUnityAdsSettins() {
            UM_AdvertisementUnityAdsUI.OnGUI();
        }



        public static void DrawPlatfromIds(UM_PlatfromAdIds platfrom) {
            platfrom.AppId = EditorGUILayout.TextField("App Id: ", platfrom.AppId);
            platfrom.BannerId = EditorGUILayout.TextField("Banner Id: ", platfrom.BannerId);
            platfrom.RewardedId = EditorGUILayout.TextField("Rewarded Id: ", platfrom.RewardedId);
            platfrom.NonRewardedId = EditorGUILayout.TextField("Non-Rewarded Id: ", platfrom.NonRewardedId);
        }  

    }
}