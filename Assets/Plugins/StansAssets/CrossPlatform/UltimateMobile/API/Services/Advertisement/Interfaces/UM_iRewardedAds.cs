using System;
using SA.Foundation.Templates;


namespace SA.CrossPlatform.Advertisement
{
    public interface UM_iRewardedAds
    {

        void Load(Action<SA_Result> callback);
        void Load(string id, Action<SA_Result> callback);
        void Show(Action<UM_RewardedAdsResult> callabck);

        /// <summary>
        /// Indicates if rewarded ads is ready to be shown
        /// </summary>
        bool IsReady { get; }
    }
}