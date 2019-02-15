using System.Collections.Generic;
using UnityEngine;

using SA.Android;
using SA.iOS;

using SA.Foundation.Editor;

namespace SA.CrossPlatform
{

    public class UM_SocialUI : UM_ServiceSettingsUI
    {

        public class ISNSettings : UM_NativeServiceLayoutBasedSetting
        {
            public override SA_ServiceLayout Layout { get { return CreateInstance<ISN_SocialUI>(); } }
            public override bool IsEnabled {
                get {
                    return ISN_Preprocessor.GetResolver<ISN_SocialResolver>().IsSettingsEnabled;
                }
            }
        }

        public class ANSettings : UM_NativeServiceLayoutBasedSetting
        {
            public override SA_ServiceLayout Layout { get { return CreateInstance<AN_SocialFeaturesUI>(); } }
            public override bool IsEnabled {
                get {
                    return AN_Preprocessor.GetResolver<AN_SocialResolver>().IsSettingsEnabled;
                }
            }
        }

        public override void OnLayoutEnable() {
            base.OnLayoutEnable();
            AddPlatfrom(UM_UIPlatform.IOS, new ISNSettings());
            AddPlatfrom(UM_UIPlatform.Android, new ANSettings());


            AddFeatureUrl("Native Sharing", "https://unionassets.com/ultimate-mobile-pro/native-sharing-740");
            AddFeatureUrl("Facebook", "https://unionassets.com/ultimate-mobile-pro/facebook-741");
            AddFeatureUrl("Twitter", "https://unionassets.com/ultimate-mobile-pro/twitter-743");
            AddFeatureUrl("Instagram", "https://unionassets.com/ultimate-mobile-pro/instagram-742");
            AddFeatureUrl("Whatsapp", "https://unionassets.com/ultimate-mobile-pro/whatsapp-744");
            AddFeatureUrl("E-mail", "https://unionassets.com/ultimate-mobile-pro/e-mail-745");
        }

        public override string Title {
            get {
                return "Social";
            }
        }

        public override string Description {
            get {
                return "Integrate your app with supported social networking services.";
            }
        }

        protected override Texture2D Icon {
            get {
                return UM_Skin.GetServiceIcon("um_social_icon.png");
            }
        }

        protected override void OnServiceUI() {

        }
    }
}