using System;
using System.Collections.Generic;
using UnityEngine;

namespace SA.CrossPlatform.Notifications
{
    [Serializable]
    public class UM_TimeIntervalNotificationTrigger : UM_iNotificationTrigger
    {

        [SerializeField] long m_interval;
        [SerializeField] bool m_repeating = false;


        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:SA.CrossPlatform.Notifications.UM_TimeIntervalNotificationTrigger"/> class.
        /// </summary>
        /// <param name="interval">Time Interval in seconds. </param>
        public UM_TimeIntervalNotificationTrigger(long interval) {
            m_interval = interval;
        }

        /// <summary>
        /// Define it trigger should be repeating.
        /// </summary>
        public void SerRepeating(bool repeating) {
            m_repeating = repeating;
        }


        /// <summary>
        /// The time interval used to create the trigger.
        /// </summary>
        public long Interval {
            get {
                return m_interval;
            }
        }


        /// <summary>
        /// Gets a value indicating whether this
        /// <see cref="T:SA.CrossPlatform.Notifications.UM_TimeIntervalNotificationTrigger"/> is repeating.
        /// </summary>
        public bool Repeating {
            get {
                return m_repeating;
            }
        }
    }
    
}