
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using SA.Android;
using SA.iOS;

using SA.Foundation.Editor;

using SA.CrossPlatform.Advertisement;

namespace SA.CrossPlatform
{

    public class UM_FirebaseUI : UM_PluginSettingsUI
    {
        private AN_FirebaseFeaturesUI m_firebaseUI;


        public override void OnAwake() {
            base.OnAwake();
            AndroidUI.OnAwake();

            AddFeatureUrl("Getting Started", "https://unionassets.com/ultimate-mobile-pro/getting-started-813");
            AddFeatureUrl("Cloud Messaging", "https://unionassets.com/ultimate-mobile-pro/cloud-messaging-814");
            AddFeatureUrl("Analytics", "https://unionassets.com/ultimate-mobile-pro/analytics-815");
        }

        public override void OnLayoutEnable() {
            AndroidUI.OnLayoutEnable();
            base.OnLayoutEnable();
        }


        public override string Title {
            get {
                return AndroidUI.Title;
            }
        }

        public override string Description {
            get {
                return AndroidUI.Description;
            }
        }

        protected override Texture2D Icon {
            get {
                return UM_Skin.GetServiceIcon("um_firebase_icon.png");
            }
        }

        public override SA_iAPIResolver Resolver {
            get {

                return AndroidUI.Resolver;
            }
        }


        public override void DrawServiceUI() {
            AndroidUI.DrawServiceUI();
        }

        protected override void OnServiceUI() {
          
        }




        private AN_FirebaseFeaturesUI AndroidUI {
            get {
                if(m_firebaseUI == null) {
                    m_firebaseUI = CreateInstance<AN_FirebaseFeaturesUI>();
                }

                return m_firebaseUI;
            }
        }



    }
}