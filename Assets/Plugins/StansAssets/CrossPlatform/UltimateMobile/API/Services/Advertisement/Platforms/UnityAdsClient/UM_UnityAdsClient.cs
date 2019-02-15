using System;
using System.Collections;

using UnityEngine;

#if SA_UNITY_ADS_INSTALLED
using UADS = UnityEngine.Advertisements;
using UnityEngine.Monetization;
#endif

using SA.Foundation.Templates;
using SA.Foundation.Async;

namespace SA.CrossPlatform.Advertisement
{
    internal class UM_UnityAdsClient : UM_AbstractAdsClient, UM_iAdsClient
    {

        public void Initialize(Action<SA_Result> callback) {
           Initialize(UM_UnityAdsSettings.Instance.Platform.AppId, callback);
        }

#if SA_UNITY_ADS_INSTALLED
        private Action<SA_Result> m_initCallback;
#endif

        protected override void ConnectToService(string appId, Action<SA_Result> callback) {
#if SA_UNITY_ADS_INSTALLED
            m_initCallback = callback;
            Monetization.Initialize(appId, UM_UnityAdsSettings.Instance.TestMode);

            SA_Coroutine.Start(WaitForInitialization());
#else
            var error = new SA_Error(1, "Unity Ads SDK is missing");
            var result = new SA_Result(error);
            callback.Invoke(result);
#endif
        }

#if SA_UNITY_ADS_INSTALLED

        private IEnumerator WaitForInitialization() {
            while (!Monetization.isInitialized) {
                yield return new WaitForSeconds(0.25f);
            }

            Debug.Log("Unity Ads INITIED");
            m_initCallback.Invoke(new SA_Result());
        }
#endif

        public UM_iBannerAds Banner {
            get {
                if(m_banner == null) {
                    m_banner = new UM_UnityBannerAds();
                }
                return m_banner;
            }
        }

        public UM_iRewardedAds RewardedAds {
            get {
                if (m_rewardedAds == null) {
                    m_rewardedAds = new UM_UnityRewardedAds();
                }

                return m_rewardedAds;
            }
        }

        public UM_iNonRewardedAds NonRewardedAds {
            get {
                if (m_nonRewardedAds == null) {
                    m_nonRewardedAds = new UM_UnityNonRewardedAds();
                }

                return m_nonRewardedAds;
            }
        }


    }
}
