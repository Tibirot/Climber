using UnityEngine;
using System.Collections;
using SA.Foundation.Templates;
using System;

using SA.Foundation.Async;

namespace SA.CrossPlatform.Notifications
{
    public class UM_EditorNotificationsClient : UM_AbstractNotificationsClient, UM_iNotificationsClient
    {
        public UM_NotificationRequest LastOpenedNotificationt {
            get {
                return null;
            }
        }

        public void RemoveAllDeliveredNotifications() {
            
        }

        public void RemoveAllPendingNotifications() {
           
        }

        public void RemoveDeliveredNotification(int identifier) {
            
        }

        public void RemovePendingNotification(int identifier) {
           
        }

        public override void RequestAuthorization(Action<SA_Result> callback) {
            callback.Invoke(new SA_Result());
        }

        protected override void AddNotificationRequestInternal(UM_NotificationRequest request, Action<SA_Result> callback) {
            SA_Coroutine.WaitForSeconds(1f, () => {
                callback.Invoke(new SA_Result());
            });
        }
    }
}