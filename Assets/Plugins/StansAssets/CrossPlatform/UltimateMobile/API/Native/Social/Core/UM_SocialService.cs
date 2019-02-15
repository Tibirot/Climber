using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA.CrossPlatform.Social
{

    /// <summary>
    /// Main entry point for the Social  APIs. 
    /// This class provides APIs and interfaces to access the game social sharing functionality.
    /// </summary>
    public static class UM_SocialService
    {
        
        private static UM_iNativeSharingClient m_sharingClient = null;


        /// <summary>
        /// Returns a new instance of <see cref="UM_iNativeSharingClient"/>
        /// </summary>
        public static UM_iNativeSharingClient SharingClient {
            get {

                if (m_sharingClient == null) {
                    switch (Application.platform) {
                        case RuntimePlatform.Android:
                            m_sharingClient = new UM_AndroidNativeSharingClient();
                            break;
                        case RuntimePlatform.IPhonePlayer:
                            m_sharingClient = new UM_IOSNativeSharingClient();
                            break;
                        default:
                            m_sharingClient = new UM_EditorSharingClient();
                            break;
                    }
                }

                return m_sharingClient;
            }
        }

    }
}