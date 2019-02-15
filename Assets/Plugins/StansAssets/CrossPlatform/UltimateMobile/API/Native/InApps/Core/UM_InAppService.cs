using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA.CrossPlatform.InApp
{

    /// <summary>
    /// Main entry point for the In-App Purchaces API. 
    /// This class provides API and interfaces to access the in-game billing functionality.
    /// </summary>
    public static class UM_InAppService 
    {
        private static UM_iInAppClient m_client = null;


        /// <summary>
        /// Returns a new instance of <see cref="UM_iInAppClient"/>
        /// </summary>
        public static UM_iInAppClient Client {
            get {

                if(m_client == null) {
                    switch(Application.platform) {
                        case RuntimePlatform.Android:
                            m_client = new UM_AndroidInAppClient();
                            break;
                        case RuntimePlatform.IPhonePlayer:
                            m_client = new UM_IOSInAppClient();
                            break;
                        default:
                            m_client = new UM_EditorInAppClient();
                            break;
                    }
                }

                return m_client;
            }
        }

    }
}