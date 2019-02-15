using System;
using System.Collections.Generic;
using UnityEngine;

using SA.iOS.UserNotifications;
using SA.Foundation.Templates;

namespace SA.CrossPlatform.Notifications
{
    public class UM_IOSNotificationsClient : UM_AbstractNotificationsClient, UM_iNotificationsClient {


        public override void RequestAuthorization(Action<SA_Result> callback) {
            int options = ISN_UNAuthorizationOptions.Alert | ISN_UNAuthorizationOptions.Sound;
            ISN_UNUserNotificationCenter.RequestAuthorization(options, callback);
        }


        public UM_IOSNotificationsClient() {
            ISN_UNUserNotificationCenterDelegate.WillPresentNotification.AddListener((ISN_UNNotification notification) => {
                UM_NotificationRequest request = ToUMRequest(notification.Request);
                m_onNotificationReceived.Invoke(request);
            });

            ISN_UNUserNotificationCenterDelegate.DidReceiveNotificationResponse.AddListener((ISN_UNNotificationResponse responce) => {
                if(responce.ActionIdentifier.Equals(ISN_UNNotificationAction.DefaultActionIdentifier)) {
                    UM_NotificationRequest request = ToUMRequest(responce.Notification.Request);
                    m_onNotificationClick.Invoke(request);
                }
            }); 
        }


        public UM_NotificationRequest LastOpenedNotificationt {
            get {
                var responce = ISN_UNUserNotificationCenterDelegate.LastReceivedResponse;
                if (responce == null) {
                    return null;
                }

                return ToUMRequest(responce.Notification.Request);
            }
        }


        public void RemoveAllPendingNotifications() {
            ISN_UNUserNotificationCenter.RemoveAllPendingNotificationRequests();
        }

        public void RemovePendingNotification(int identifier) {
            ISN_UNUserNotificationCenter.RemovePendingNotificationRequests(identifier.ToString());
        }

        public void RemoveAllDeliveredNotifications() {
            ISN_UNUserNotificationCenter.RemoveAllDeliveredNotifications();
        }

        public void RemoveDeliveredNotification(int identifier) {
            ISN_UNUserNotificationCenter.RemoveDeliveredNotifications(identifier.ToString());
        }



        protected override void AddNotificationRequestInternal(UM_NotificationRequest request, Action<SA_Result> callback) {
            var content = new ISN_UNNotificationContent();
            content.Title = request.Content.Title;
            content.Body = request.Content.Body;

            if (string.IsNullOrEmpty(request.Content.SoundName)) {
                content.Sound = ISN_UNNotificationSound.DefaultSound;
            } else {
                content.Sound = ISN_UNNotificationSound.SoundNamed(request.Content.SoundName);
            }
           


            ISN_UNNotificationTrigger trigger = null;

            if (request.Trigger is UM_TimeIntervalNotificationTrigger) {
                UM_TimeIntervalNotificationTrigger timeIntervalTrigger = (UM_TimeIntervalNotificationTrigger)request.Trigger;
                trigger = new ISN_UNTimeIntervalNotificationTrigger(timeIntervalTrigger.Interval, timeIntervalTrigger.Repeating);
            }

            var ios_request = new ISN_UNNotificationRequest(request.Identifier.ToString(), content, trigger);
            ISN_UNUserNotificationCenter.AddNotificationRequest(ios_request, callback);
        }

        public void RemovePendingNotificationRequest(int Identifier) {
            ISN_UNUserNotificationCenter.RemovePendingNotificationRequests(new string[] { Identifier.ToString() });
        }


        private UM_NotificationRequest ToUMRequest(ISN_UNNotificationRequest ios_request) {
            
            UM_Notification content = new UM_Notification();
            content.SetTitle(ios_request.Content.Title);
            content.SetBody(ios_request.Content.Body);

            ISN_UNTimeIntervalNotificationTrigger timeIntervalTrigger = (ISN_UNTimeIntervalNotificationTrigger)ios_request.Trigger;



            long interval = timeIntervalTrigger.TimeInterval;
            bool repeating = timeIntervalTrigger.Repeats;
            UM_TimeIntervalNotificationTrigger trigger = new UM_TimeIntervalNotificationTrigger(interval);
            trigger.SerRepeating(repeating);

            int Identifier = Convert.ToInt32(ios_request.Identifier);
            UM_NotificationRequest request = new UM_NotificationRequest(Identifier, content, trigger);

            return request;
        }

      
    }
}