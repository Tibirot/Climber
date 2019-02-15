using System;
using SA.Foundation.Templates;


namespace SA.CrossPlatform.Advertisement
{
    public interface UM_iNonRewardedAds
    {
        void Load(Action<SA_Result> callback);
        void Load(string id, Action<SA_Result> callback);
        void Show(Action callback);

        /// <summary>
        /// Indicates if rewarded ads is ready to be shown
        /// </summary>
        bool IsReady { get; }
    }
}