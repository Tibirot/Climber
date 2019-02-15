using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA.CrossPlatform.Notifications
{
    /// <summary>
    /// The central object for managing notification-related activities for your app.
    /// </summary>
    public static class UM_NotificationCenter 
    {
        private static UM_iNotificationsClient m_client = null;

        /// <summary>
        /// Returns a new instance of <see cref="UM_iNotificationsClient"/>
        /// </summary>
        public static UM_iNotificationsClient Client {
            get {

                if (m_client == null) {
                    switch (Application.platform) {
                        case RuntimePlatform.Android:
                            m_client = new UM_AndroidNotificationsClient();
                            break;
                        case RuntimePlatform.IPhonePlayer:
                            m_client = new UM_IOSNotificationsClient();
                            break;
                        default:
                            m_client = new UM_EditorNotificationsClient();
                            break;
                    }
                }

                return m_client;
            }
        }

    }
}