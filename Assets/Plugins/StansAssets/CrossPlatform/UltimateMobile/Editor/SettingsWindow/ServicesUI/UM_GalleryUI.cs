using System.Collections.Generic;
using UnityEngine;

using SA.Android;
using SA.iOS;

using SA.Foundation.Editor;


namespace SA.CrossPlatform
{

    public class UM_GalleryUI : UM_ServiceSettingsUI
    {

        public class ISNSettings : UM_NativeServiceLayoutBasedSetting
        {
            public override SA_ServiceLayout Layout { get { return CreateInstance<ISN_UIKitUI>(); } }
            public override bool IsEnabled {
                get {
                    return ISN_Preprocessor.GetResolver<ISN_UIKitResolver>().IsSettingsEnabled;
                }
            }
        }

        public class ANSettings : UM_NativeServiceLayoutBasedSetting
        {
            public override SA_ServiceLayout Layout { get { return CreateInstance<AN_CameraAndGalleryFeaturesUI>(); } }
            public override bool IsEnabled {
                get {
                    return AN_Preprocessor.GetResolver<AN_CameraAndGalleryResolver>().IsSettingsEnabled;
                }
            }
        }

        public override void OnLayoutEnable() {
            base.OnLayoutEnable();
            AddPlatfrom(UM_UIPlatform.IOS, new ISNSettings());
            AddPlatfrom(UM_UIPlatform.Android, new ANSettings());

            AddFeatureUrl("Save to Gallery", "https://unionassets.com/ultimate-mobile-pro/save-to-gallery-748");
            AddFeatureUrl("Save Screenshot", "https://unionassets.com/ultimate-mobile-pro/save-to-gallery-748#save-screenshot");
            AddFeatureUrl("Pick an Image", "https://unionassets.com/ultimate-mobile-pro/pick-from-gallery-749#capture-image-from-camera-1");
            AddFeatureUrl("Pick a Video", "https://unionassets.com/ultimate-mobile-pro/pick-from-gallery-749#pick-a-video");
        }

        public override string Title {
            get {
                return "Gallery";
            }
        }

        public override string Description {
            get {
                return "Pick image or video from the device local storage";
            }
        }

        protected override Texture2D Icon {
            get {
                return UM_Skin.GetServiceIcon("um_gallery_icon.png");
            }
        }



        protected override void OnServiceUI() {

        }
    }
}