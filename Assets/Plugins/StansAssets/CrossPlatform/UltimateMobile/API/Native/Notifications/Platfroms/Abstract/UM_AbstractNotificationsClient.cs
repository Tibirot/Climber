using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using SA.Foundation.Events;
using SA.Foundation.Templates;
using System;

namespace SA.CrossPlatform.Notifications
{
    public abstract class UM_AbstractNotificationsClient
    {
        protected SA_Event<UM_NotificationRequest> m_onNotificationClick = new SA_Event<UM_NotificationRequest>();
        protected SA_Event<UM_NotificationRequest> m_onNotificationReceived = new SA_Event<UM_NotificationRequest>();

        

        public abstract void RequestAuthorization(Action<SA_Result> callback);
        protected abstract void AddNotificationRequestInternal(UM_NotificationRequest request, Action<SA_Result> callback);



        public void AddNotificationRequest(UM_NotificationRequest request, Action<SA_Result> callback) {
            RequestAuthorization((result) => {
                if (result.IsSucceeded) {
                    AddNotificationRequestInternal(request, callback);
                } else {
                    callback.Invoke(result);
                }
            });
        }




        public SA_iEvent<UM_NotificationRequest> OnNotificationClick {
            get {
                return m_onNotificationClick;
            }
        }


        public SA_iEvent<UM_NotificationRequest> OnNotificationReceived {
            get {
                return m_onNotificationReceived;
            }
        }



    }
}