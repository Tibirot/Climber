using System;
using UnityEngine;


namespace SA.CrossPlatform.Advertisement
{

    internal class UM_EditorBannerAds : UM_EditorBaseAds, UM_iBannerAds
    {

        public void Show(Action callback) {

            if(!IsReady) {
                string message = "Failed to show banner, banner is not ready!";
                Debug.LogError(message);
                throw new InvalidOperationException(message);
            }

            UM_EditorAPIEmulator.WaitForNetwork(() => {
                callback.Invoke();
            });


        }

        public void Hide() {
            //Just do nothing
        }

        public void Destroy() {
            m_isReady = false;
        }

    }
}