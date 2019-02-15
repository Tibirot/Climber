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

    internal class UM_UnityRewardedAds : UM_UnityBaseAds, UM_iRewardedAds
    {


#if SA_UNITY_ADS_INSTALLED
        private Action<SA_Result> m_loadCallback;
        private Action<UM_RewardedAdsResult> m_showCallback;
#endif


        public void Load(Action<SA_Result> callback) {
            Load(UM_UnityAdsSettings.Instance.Platform.RewardedId, callback);
        }

        public void Load(string id, Action<SA_Result> callback) {
#if SA_UNITY_ADS_INSTALLED
            m_advertisementId = id;
            m_loadCallback = callback;

            Debug.Log("Unity Ads Load R Video");
            SA_Coroutine.Start(WaitForLoad());
#endif

        }




        public void Show(Action<UM_RewardedAdsResult> callabck) {
#if SA_UNITY_ADS_INSTALLED
            m_showCallback = callabck;

            ShowAdCallbacks options = new ShowAdCallbacks();
            options.finishCallback = FinishCallback;
            ShowAdPlacementContent ad = Monetization.GetPlacementContent(m_advertisementId) as ShowAdPlacementContent;

            if (ad != null) {
                ad.Show(options);
            } else {
                m_showCallback.Invoke(UM_RewardedAdsResult.Failed);
                m_showCallback = null;
            }
#endif
        }


#if SA_UNITY_ADS_INSTALLED

        private IEnumerator WaitForLoad() {
            while (!IsReady) {
                Debug.Log("Video Not ready... " + m_advertisementId);
                yield return new WaitForSeconds(0.25f);
            }
            Debug.Log("Video Ready!!!!");
            m_loadCallback.Invoke(new SA_Result());
        }

        private void FinishCallback(ShowResult finishState) {
            switch (finishState) {
                case ShowResult.Failed:
                    m_showCallback.Invoke(UM_RewardedAdsResult.Failed);
                    break;
                case ShowResult.Finished:
                    m_showCallback.Invoke(UM_RewardedAdsResult.Finished);
                    break;
                case ShowResult.Skipped:
                    m_showCallback.Invoke(UM_RewardedAdsResult.Skipped);
                    break;
            }

            m_showCallback = null;
        }
#endif


    }
}