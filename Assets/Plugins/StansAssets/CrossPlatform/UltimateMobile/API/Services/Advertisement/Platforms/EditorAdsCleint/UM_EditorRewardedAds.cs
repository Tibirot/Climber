using System;
using UnityEngine;

namespace SA.CrossPlatform.Advertisement
{

    internal class UM_EditorRewardedAds : UM_EditorBaseAds, UM_iRewardedAds
    {

        public void Show(Action<UM_RewardedAdsResult> callabck) {

            if (!IsReady) {
                string message = "Failed to show rewarded, contnet is not ready yet!";
                Debug.LogError(message);
                throw new InvalidOperationException(message);
            } else {
                UM_EditorAPIEmulator.WaitForNetwork(() => {
                    callabck.Invoke(UM_RewardedAdsResult.Finished);
                    m_isReady = false;
                });
               
            }
        }

    }
}