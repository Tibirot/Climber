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
    internal class UM_UnityNonRewardedAds : UM_UnityBaseAds, UM_iNonRewardedAds
    {
#if SA_UNITY_ADS_INSTALLED
        private Action<SA_Result> m_loadCallback;
#endif

        public void Load(Action<SA_Result> callback) {
            Load(UM_UnityAdsSettings.Instance.Platform.NonRewardedId, callback);
        }

        public void Load(string id, Action<SA_Result> callback) {
#if SA_UNITY_ADS_INSTALLED
            m_advertisementId = id;
            m_loadCallback = callback;
            SA_Coroutine.Start(WaitForLoad());
#endif

        }

#if SA_UNITY_ADS_INSTALLED
        private IEnumerator WaitForLoad() {
            while (!IsReady) {
                yield return new WaitForSeconds(0.25f);
            }

            m_loadCallback.Invoke(new SA_Result());
        }
#endif


        public void Show(Action callback) {
#if SA_UNITY_ADS_INSTALLED

            ShowAdPlacementContent ad = Monetization.GetPlacementContent(m_advertisementId) as ShowAdPlacementContent;

            if (ad != null) {
                ad.Show((ShowResult finishState) => {
                    callback.Invoke();
                });
            } else {
                callback.Invoke();
            }
#endif
        }
    }
}