using System.Collections.Generic;
using UnityEngine;

using SA.Android;
using SA.iOS;
using SA.iOS.XCode;

using SA.Foundation.Editor;


namespace SA.CrossPlatform
{
    public class UM_RemoteNotificationsUI : UM_ServiceSettingsUI
    {

        public class ISNSettings : UM_NativeServiceLayoutBasedSetting
        {
            public override SA_ServiceLayout Layout { get { return CreateInstance<ISN_UserNotificationsUI>(); } }

            public override bool IsEnabled {
                get {
                    return ISD_API.Capability.PushNotifications.Enabled;
                }
            }
        }

        public class ANSettings : UM_NativeServiceLayoutBasedSetting
        {
            public override SA_ServiceLayout Layout { get { return CreateInstance<AN_FirebaseFeaturesUI>(); } }

            public override bool IsEnabled {
                get {
                    return AN_FirebaseDefinesResolver.IsMessagingSDKInstalled;
                }
            }
        }

        public override void OnLayoutEnable() {
            base.OnLayoutEnable();
            AddPlatfrom(UM_UIPlatform.IOS, new ISNSettings());
            AddPlatfrom(UM_UIPlatform.Android, new ANSettings());
        }


        public override string Title {
            get {
                return "Remote Notifications";
            }
        }

        public override string Description {
            get {
                return "Supports the delivery and handling of remote notifications.";
            }
        }

        protected override Texture2D Icon {
            get {
                return UM_Skin.GetServiceIcon("um_remote_notifications_icon.png");
            }
        }


        protected override void OnServiceUI() {

        }
    }
}