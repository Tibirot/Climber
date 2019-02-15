using System;
using System.Collections.Generic;
using UnityEngine;

using SA.Foundation.Templates;

namespace SA.CrossPlatform.Advertisement
{

    public abstract class UM_AbstractAdsClient
    {

        private bool m_isConnected = false;
        private bool m_isConnecttionInProgress = false;

        protected UM_iBannerAds m_banner;
        protected UM_iRewardedAds m_rewardedAds;
        protected UM_iNonRewardedAds m_nonRewardedAds;

        private event Action<SA_Result> m_onConnect = delegate { };


        //--------------------------------------
        //  Abstract
        //--------------------------------------

        protected abstract void ConnectToService(string appId, Action<SA_Result> callback);


        //--------------------------------------
        //  Public Methods
        //--------------------------------------


        public void Initialize(string appId, Action<SA_Result> callback) {
            if (m_isConnected) {
                callback.Invoke(new SA_Result());
                return;
            }

            m_onConnect += callback;
            if (m_isConnecttionInProgress) { return; }
            m_isConnecttionInProgress = true;

            ConnectToService(appId, (result) => {
                m_isConnected = true;
                m_isConnecttionInProgress = false;
                m_onConnect.Invoke(result);
                m_onConnect = delegate { };
            });
        }


        //--------------------------------------
        //  Get / Set
        //--------------------------------------

        public bool IsInitialized {
            get {
                return m_isConnected;
            }
        }
    }
}