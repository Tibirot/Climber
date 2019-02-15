using System;

#if SA_UNITY_ADS_INSTALLED
using UADS = UnityEngine.Advertisements;
#endif

using SA.Foundation.Templates;



namespace SA.CrossPlatform.Advertisement
{

    internal class UM_UnityBannerAds : UM_UnityBaseAds, UM_iBannerAds
    {

        private Action m_showCallback;
        private Action<SA_Result> m_loadCallback;
      


        public void Load(Action<SA_Result> callback) {
            Load(UM_UnityAdsSettings.Instance.Platform.BannerId, callback);
        }

        public void Load(string id, Action<SA_Result> callback) {

#if SA_UNITY_ADS_INSTALLED
            m_advertisementId = id;
            if (UADS.Advertisement.IsReady(id)) {
                callback.Invoke(new SA_Result());
                return;
            }

            m_loadCallback = callback;
            UADS.BannerLoadOptions loadOptions = new UADS.BannerLoadOptions();
            loadOptions.errorCallback += LoadErrorCallback;
            loadOptions.loadCallback += LoadCallback;
            UADS.Advertisement.Banner.Load(id, loadOptions);
#endif
        }

        public void Show(Action callback) {
#if SA_UNITY_ADS_INSTALLED
            m_showCallback = callback;
            UADS.BannerOptions showOptions = new UADS.BannerOptions();
            showOptions.showCallback += ShowCallback;

            UADS.Advertisement.Banner.Show(m_advertisementId, showOptions);
#endif
        }

        public void Hide() {
#if SA_UNITY_ADS_INSTALLED
            UADS.Advertisement.Banner.Hide();
#endif
        }

        public void Destroy() {
#if SA_UNITY_ADS_INSTALLED
            UADS.Advertisement.Banner.Hide(true);
#endif
        }


        public override bool IsReady {
            get {
#if SA_UNITY_ADS_INSTALLED
                return UADS.Advertisement.IsReady(m_advertisementId);
#else
                return false;
#endif
            }
        }


#if SA_UNITY_ADS_INSTALLED

        private void LoadCallback() {
            m_loadCallback.Invoke(new SA_Result());
            m_loadCallback = null;
        }

        private void LoadErrorCallback(string message) {
            var error = new SA_Error(1, message);
            m_loadCallback.Invoke(new SA_Result(error));
            m_loadCallback = null;
        }

        private void ShowCallback() {
            m_showCallback.Invoke();
            m_showCallback = null;
        }
#endif

    }
}