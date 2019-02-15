using System.Collections.Generic;
using UnityEngine;

using SA.Android;
using SA.iOS;

using SA.Foundation.Editor;

namespace SA.CrossPlatform
{
    public class UM_FoundationUI : UM_ServiceSettingsUI
    {

        public class ISNSettings : UM_NativeServiceLayoutBasedSetting
        {
            public override SA_ServiceLayout Layout { get { return CreateInstance<ISN_FoundationUI>(); } }
            public override bool IsEnabled {
                get {
                    return ISN_Preprocessor.GetResolver<ISN_FoundationResolver>().IsSettingsEnabled;
                }
            }
        }

        public class ANSettings : UM_NativeServiceLayoutBasedSetting
        {
            public override SA_ServiceLayout Layout { get { return CreateInstance<AN_AppFeaturesUI>(); } }

            public override bool IsEnabled {
                get {
                    return AN_Preprocessor.GetResolver<AN_CoreResolver>().IsSettingsEnabled;
                }
            }
        }

        public override void OnLayoutEnable() {
            base.OnLayoutEnable();
            AddPlatfrom(UM_UIPlatform.IOS, new ISNSettings());
            AddPlatfrom(UM_UIPlatform.Android, new ANSettings());


            AddFeatureUrl("Introduction", "https://unionassets.com/ultimate-mobile-pro/introduction-725");

            AddFeatureUrl("Plugin Editor UI", "https://unionassets.com/ultimate-mobile-pro/plugin-editor-ui-736");
            AddFeatureUrl("3rd-Party Tab", "https://unionassets.com/ultimate-mobile-pro/services-ui-762");
            AddFeatureUrl("Summary Tab", "https://unionassets.com/ultimate-mobile-pro/summary-tab-768");


            AddFeatureUrl("Native Dialogs", "https://unionassets.com/ultimate-mobile-pro/native-dialogs-722");
            AddFeatureUrl("Native Preloader", "https://unionassets.com/ultimate-mobile-pro/native-preloader-766");
            AddFeatureUrl("Rate Us Dialog", "https://unionassets.com/ultimate-mobile-pro/rate-us-dialog-767");
            AddFeatureUrl("Date Picker Dialog", "https://unionassets.com/ultimate-mobile-pro/date-picker-dialog-777");


            AddFeatureUrl("Build Info", "https://unionassets.com/ultimate-mobile-pro/build-info-723");
        }

        public override string Title {
            get {
                return "Foundation";
            }
        }

        public override string Description {
            get {
                return "Access operating-system services to define the base layer of functionality for your app.";
            }
        }

        protected override Texture2D Icon {
            get {
                return UM_Skin.GetServiceIcon("um_system_icon.png");
            }
        }


        protected override void OnServiceUI() {

        }
    }
}