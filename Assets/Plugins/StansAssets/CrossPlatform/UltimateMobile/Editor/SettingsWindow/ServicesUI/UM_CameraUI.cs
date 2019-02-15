using System.Collections.Generic;
using UnityEngine;

using SA.Android;
using SA.iOS;

using SA.Foundation.Editor;


namespace SA.CrossPlatform
{

    public class UM_CameraUI : UM_ServiceSettingsUI
    {

     
        public override void OnLayoutEnable() {
            base.OnLayoutEnable();
            AddPlatfrom(UM_UIPlatform.IOS, new UM_GalleryUI.ISNSettings());
            AddPlatfrom(UM_UIPlatform.Android, new UM_GalleryUI.ANSettings());

            AddFeatureUrl("Capture an Image", "https://unionassets.com/ultimate-mobile-pro/camera-api-747#capture-image-from-camera-1");
            AddFeatureUrl("Capture a Video", "https://unionassets.com/ultimate-mobile-pro/camera-api-747#capture-image-from-camera");
        }


        public override string Title {
            get {
                return "Camera";
            }
        }

        public override string Description {
            get {
                return "Capture image or video with device camera";
            }
        }

        protected override Texture2D Icon {
            get {
                return UM_Skin.GetServiceIcon("um_camera_icon.png");
            }
        }



        protected override void OnServiceUI() {
           
        }

       
    }
}