using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SA.CrossPlatform.Notifications
{
    /// <summary>
    /// An object you use to specify a notification’s content and the condition that triggers its delivery.
    /// </summary>
    [Serializable]
    public class UM_NotificationRequest
    {
        [SerializeField] int m_identifier;
        [SerializeField] UM_Notification m_content;
        [SerializeField] UM_iNotificationTrigger m_trigger;

       
        public UM_NotificationRequest(int identifier, UM_Notification content, UM_iNotificationTrigger trigger) {
            m_identifier = identifier;
            m_content = content;
            m_trigger = trigger;
        }


        /// <summary>
        /// The unique identifier for this notification request.
        /// 
        /// Use this string to identify notifications in your app. 
        /// For example, you can pass this string to the <see cref="UM_iNotificationsClient.RemovePendingNotification"/> 
        /// method to cancel a previously scheduled notification.
        /// 
        /// If you use the same identifier when scheduling a new notification, 
        /// the system removes the previously scheduled notification with that identifier and replaces it with the new one.
        /// </summary>
        public int Identifier {
            get {
                return m_identifier;
            }
        }

        /// <summary>
        /// The content associated with the notification.
        /// 
        /// Use this property to access the contents of the notification. 
        /// The content object contains the badge information, sound to be played, 
        /// or alert text to be displayed to the user, in addition to the notification’s thread identifier.
        /// </summary>
        public UM_Notification Content {
            get {
                return m_content;
            }

            set {
                m_content = value;
            }
        }


        /// <summary>
        /// The conditions that trigger the delivery of the notification.
        /// 
        /// For notifications that have already been delivered, use this property 
        /// to determine what caused the delivery to occur.
        /// </summary>
        public UM_iNotificationTrigger Trigger {
            get {
                return m_trigger;
            }
        }
    }
}