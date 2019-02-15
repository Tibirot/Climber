using System.Collections.Generic;
using UnityEngine;

using SA.Android;
using SA.iOS;

using SA.Foundation.Editor;


namespace SA.CrossPlatform
{
    public class UM_LocalNotificationsUI : UM_ServiceSettingsUI
    {

        public class ISNSettings : UM_NativeServiceLayoutBasedSetting
        {
            public override SA_ServiceLayout Layout { get { return CreateInstance<ISN_UserNotificationsUI>(); } }
            public override bool IsEnabled {
                get {
                    return ISN_Preprocessor.GetResolver<ISN_UserNotificationsResolver>().IsSettingsEnabled;
                }
            }
        }

        public class ANSettings : UM_NativeServiceLayoutBasedSetting
        {
            public override SA_ServiceLayout Layout { get { return CreateInstance<AN_LocalNotificationsFeaturesUI>(); } }
            public override bool IsEnabled {
                get {
                    return AN_Preprocessor.GetResolver<AN_LocalNotificationsResolver>().IsSettingsEnabled;
                }
            }
        }

        public override void OnLayoutEnable() {
            base.OnLayoutEnable();
            AddPlatfrom(UM_UIPlatform.IOS, new ISNSettings());
            AddPlatfrom(UM_UIPlatform.Android, new ANSettings());

            AddFeatureUrl("Getting Started", "https://unionassets.com/ultimate-mobile-pro/getting-started-735");
            AddFeatureUrl("Scheduling", "https://unionassets.com/ultimate-mobile-pro/scheduling-notifications-737");
            AddFeatureUrl("Canceling", "https://unionassets.com/ultimate-mobile-pro/responding-to-notification-738#canceling-notifications");
            AddFeatureUrl("Responding", "https://unionassets.com/ultimate-mobile-pro/responding-to-notification-738");
        }


        public override string Title {
            get {
                return "Local Notifications";
            }
        }

        public override string Description {
            get {
                return "Supports the delivery and handling of local notifications.";
            }
        }

        protected override Texture2D Icon {
            get {
                return UM_Skin.GetServiceIcon("um_notification_icon.png");
            }
        }


        protected override void OnServiceUI() {

        }
    }
}