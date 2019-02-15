using System;
using System.Collections;

using UnityEngine;


using SA.Foundation.Templates;
using SA.Foundation.Async;

namespace SA.CrossPlatform.Advertisement
{
    internal class UM_EditorAdsClient : UM_AbstractAdsClient, UM_iAdsClient
    {

        public void Initialize(Action<SA_Result> callback) {
           Initialize("editor_client_id", callback);
        }


        protected override void ConnectToService(string appId, Action<SA_Result> callback) {
            UM_EditorAPIEmulator.WaitForNetwork(() => {
                callback.Invoke(new SA_Result());
            });

        }

        public UM_iBannerAds Banner {
            get {
                if(m_banner == null) {
                    m_banner = new UM_EditorBannerAds();
                }
                return m_banner;
            }
        }

        public UM_iRewardedAds RewardedAds {
            get {
                if (m_rewardedAds == null) {
                    m_rewardedAds = new UM_EditorRewardedAds();
                }

                return m_rewardedAds;
            }
        }

        public UM_iNonRewardedAds NonRewardedAds {
            get {
                if (m_nonRewardedAds == null) {
                    m_nonRewardedAds = new UM_EditorNonRewardedAds();
                }

                return m_nonRewardedAds;
            }
        }


    }
}
