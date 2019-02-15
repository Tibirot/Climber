using System;
using System.Collections.Generic;
using UnityEngine;

namespace SA.iOS.UserNotifications.Internal
{

    [Serializable]
    public class ISN_UNNotificationRequests 
    {

        [SerializeField] List<ISN_UNNotificationRequest> m_requests;

        public List<ISN_UNNotificationRequest> Requests {
            get {
                return m_requests;
            }
        }
    }
}