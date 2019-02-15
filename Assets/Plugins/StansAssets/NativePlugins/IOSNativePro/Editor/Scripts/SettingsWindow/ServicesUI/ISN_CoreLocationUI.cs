using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using SA.Foundation.Editor;

namespace SA.iOS
{
    public class ISN_CoreLocationUI : ISN_ServiceSettingsUI
    {

        public override void OnAwake() {
            base.OnAwake();

            AddFeatureUrl("Getting Started", "https://unionassets.com/ios-native-pro/manual#core-location");

        }

        public override string Title {
            get {
                return "Core Location";
            }
        }

        public override string Description {
            get {
                return "Provides services for determining a device’s geographic location.";
            }
        }

        protected override Texture2D Icon {
            get {
                return SA_EditorAssets.GetTextureAtPath(ISN_Skin.ICONS_PATH + "CoreLocation_icon.png");
            }
        }

        public override SA_iAPIResolver Resolver {
            get {
                return ISN_Preprocessor.GetResolver<ISN_CoreLocationResolver>();
            }
        }


        protected override void OnServiceUI() {
           
        }


    }

}