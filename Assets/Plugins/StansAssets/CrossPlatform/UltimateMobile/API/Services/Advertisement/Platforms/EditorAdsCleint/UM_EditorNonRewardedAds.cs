using System;
using UnityEngine;


namespace SA.CrossPlatform.Advertisement
{
    internal class UM_EditorNonRewardedAds : UM_EditorBaseAds, UM_iNonRewardedAds
    {

        public void Show(Action callabck) {
            if (!IsReady) {
                string message = "Failed to show non-rewarded, contnet is not ready yet!";
                Debug.LogError(message);
                throw new InvalidOperationException(message);
            } else {
                UM_EditorAPIEmulator.WaitForNetwork(() => {
                    callabck.Invoke();
                    m_isReady = false;
                });
            }
        }
    }
}