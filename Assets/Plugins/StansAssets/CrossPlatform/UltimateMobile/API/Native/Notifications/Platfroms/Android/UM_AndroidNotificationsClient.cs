using System;

using SA.Android;
using SA.Android.App;
using SA.Foundation.Templates;
using SA.Android.Manifest;
using SA.Foundation.UtilitiesEditor;

namespace SA.CrossPlatform.Notifications
{
    public class UM_AndroidNotificationsClient : UM_AbstractNotificationsClient, UM_iNotificationsClient
    {
      
        public UM_AndroidNotificationsClient() {
            AN_NotificationManager.OnNotificationClick.AddSafeListener(this, (android_request) => {
                UM_NotificationRequest request = ToUMRequest(android_request);
                m_onNotificationClick.Invoke(request);
            });

            AN_NotificationManager.OnNotificationReceived.AddSafeListener(this, (android_request) => {
                UM_NotificationRequest request = ToUMRequest(android_request);
                m_onNotificationReceived.Invoke(request);
            });
        }


        public override void RequestAuthorization(Action<SA_Result> callback) {
       
            AN_PermissionsUtility.TryToResolvePermission(AMM_ManifestPermission.WAKE_LOCK, (granted) => {
                if (granted) {
                    callback.Invoke(new SA_Result());
                } else {
                    var error = new SA_Error(100, "User declined");
                    callback.Invoke(new SA_Result(error));
                }   
            });
        }


        public UM_NotificationRequest LastOpenedNotificationt {
            get {
                if(AN_NotificationManager.LastOpenedNotificationRequest == null) {
                    return null;
                }

                return ToUMRequest(AN_NotificationManager.LastOpenedNotificationRequest);
            }
        }

     

        public void RemoveAllPendingNotifications() {
            AN_NotificationManager.UnscheduleAll();
        }

        public void RemoveAllDeliveredNotifications() {
            AN_NotificationManager.CancelAll();
        }

        public void RemovePendingNotification(int identifier) {
            AN_NotificationManager.Unschedule(identifier);
        }

        public void RemoveDeliveredNotification(int identifier) {
            AN_NotificationManager.Cancel(identifier);
        }


        protected override void AddNotificationRequestInternal(UM_NotificationRequest request, Action<SA_Result> callback) {

            try {
                var builder = new AN_NotificationCompat.Builder();
                builder.SetContentTitle(request.Content.Title);
                builder.SetContentText(request.Content.Body);

                if (string.IsNullOrEmpty(request.Content.SoundName)) {
                    builder.SetDefaults(AN_Notification.DEFAULT_LIGHTS | AN_Notification.DEFAULT_SOUND);
                } else {
                    string soundName = SA_AssetDatabase.GetAssetNameWithoutExtension(request.Content.SoundName);
                    builder.SetSound(soundName);
                }


                if (!string.IsNullOrEmpty(request.Content.IconName)) {
                    string iconName = SA_AssetDatabase.GetAssetNameWithoutExtension(request.Content.IconName);
                    builder.SetSmallIcon(iconName);
                }

                if (request.Content.LargeIcon != null) {
                    builder.SetLargeIcon(request.Content.LargeIcon);
                }


                UM_TimeIntervalNotificationTrigger timeIntervalTrigger = (UM_TimeIntervalNotificationTrigger)request.Trigger;

                var trigger = new AN_AlarmNotificationTrigger();
                trigger.SetDate(TimeSpan.FromSeconds(timeIntervalTrigger.Interval));
                trigger.SerRepeating(timeIntervalTrigger.Repeating);

                var android_request = new AN_NotificationRequest(request.Identifier, builder, trigger);

                AN_NotificationManager.Schedule(android_request);

                callback.Invoke(new SA_Result());
            } catch(Exception ex) {
                SA_Error error = new SA_Error(100, ex.Message);
                callback.Invoke(new SA_Result(error));
            }
        }


        private UM_NotificationRequest ToUMRequest(AN_NotificationRequest android_request) {
            UM_Notification content = new UM_Notification();
            content.SetTitle(android_request.Content.Title);
            content.SetBody(android_request.Content.Text);


            long interval = (long)android_request.Trigger.Seconds;
            bool repeating = android_request.Trigger.Repeating;
            UM_TimeIntervalNotificationTrigger timeIntervalTrigger = new UM_TimeIntervalNotificationTrigger(interval);
            timeIntervalTrigger.SerRepeating(repeating);

            UM_NotificationRequest request = new UM_NotificationRequest(android_request.Identifier, content, timeIntervalTrigger);

            return request;
        }
    }
}