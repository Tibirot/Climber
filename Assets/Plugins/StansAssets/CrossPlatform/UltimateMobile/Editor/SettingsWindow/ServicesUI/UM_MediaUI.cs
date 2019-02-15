using System.Collections.Generic;
using UnityEngine;

using SA.Android;
using SA.iOS;

using SA.Foundation.Editor;


namespace SA.CrossPlatform
{

    public class UM_MediaUI : UM_ServiceSettingsUI
    {

        public class ISNSettings : UM_NativeServiceLayoutBasedSetting
        {
            public override SA_ServiceLayout Layout { get { return CreateInstance<ISN_AVKitUI>(); } }
            public override bool IsEnabled {
                get {
                    return ISN_Preprocessor.GetResolver<ISN_AVKitResolver>().IsSettingsEnabled;
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

            AddFeatureUrl("Getting Started", "https://unionassets.com/ultimate-mobile-pro/getting-started-794");
            AddFeatureUrl("Play Remove Video", "https://unionassets.com/ultimate-mobile-pro/show-remote-video-795");
        }

        public override string Title {
            get {
                return "Media Player";
            }
        }

        public override string Description {
            get {
                return "MediaPlayer class can be used to control playback of audio/video files and streams.";
            }
        }

        protected override Texture2D Icon {
            get {
                return UM_Skin.GetServiceIcon("um_media_icon.png");
            }
        }



        protected override void OnServiceUI() {

        }
    }
}