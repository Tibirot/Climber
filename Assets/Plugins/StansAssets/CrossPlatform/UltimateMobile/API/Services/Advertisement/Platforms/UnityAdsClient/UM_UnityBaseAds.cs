#if SA_UNITY_ADS_INSTALLED
using UnityEngine.Monetization;
#endif

namespace SA.CrossPlatform.Advertisement
{

    internal class UM_UnityBaseAds
    {
        protected string m_advertisementId = string.Empty;


        public virtual bool IsReady {
            get {
#if SA_UNITY_ADS_INSTALLED
                if (!Monetization.isInitialized) {
                    return false;
                }
                return Monetization.IsReady(m_advertisementId);
#else
                return false;
#endif
            }
        }
    }
}