using System;
using SA.Foundation.Templates;

namespace SA.CrossPlatform.Advertisement
{

    /// <summary>
    /// A client to interact with advertisements functionality.
    /// </summary>
    public interface UM_iAdsClient {


        /// <summary>
        /// Initialize the Ads client using the id spesifaied in editor settings.
        /// </summary>
        /// <param name="callback">The result callback</param>
        void Initialize(Action<SA_Result> callback);

        /// <summary>
        /// Initialize the Ads client.
        /// </summary>
        /// <param name="appId">Your application / game id as configured inside servise admitistration dashboard</param>
        /// <param name="callback">The result callback</param>
        void Initialize(string appId, Action<SA_Result> callback);

        /// <summary>
        /// Inidicates if ads client was Initialized successfully.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// A client to interact with banner advertisements functionality.
        /// </summary>
        UM_iBannerAds Banner { get; }

        /// <summary>
        /// A client to interact with rewarded advertisements functionality.
        /// </summary>
        UM_iRewardedAds RewardedAds { get; }

        /// <summary>
        /// A client to interact with non-rewarded advertisements functionality.
        /// </summary>
        UM_iNonRewardedAds NonRewardedAds { get; }

    }
}
