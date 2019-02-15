using UnityEngine;

using SA.Facebook;
using SA.Foundation.Editor;

namespace SA.CrossPlatform
{

    public class UM_Facebook : UM_PluginSettingsUI {


        private UM_FacebookResolver m_resolver;

        public override void OnAwake() {
            base.OnAwake();

            AddFeatureUrl("Getting Started", "https://unionassets.com/ultimate-mobile-pro/getting-started-759");
            AddFeatureUrl("Sing In", "https://unionassets.com/ultimate-mobile-pro/sing-in-760");
            AddFeatureUrl("User Info", "https://unionassets.com/ultimate-mobile-pro/user-info-761");
            AddFeatureUrl("Analytics", "https://unionassets.com/ultimate-mobile-pro/analytics-816");
            
        }



        public override string Title {
            get {
                return "Facebook";
            } 
        }

        public override string Description {
            get {
                return "Build cross-platform games with Facebook rapidly and easily.";
            }
        }

        protected override Texture2D Icon {
            get {
                return UM_Skin.GetServiceIcon("un_facebook_icon.png");
            }
        }

        public override SA_iAPIResolver Resolver {
            get {

                if(m_resolver == null) {
                    m_resolver = new UM_FacebookResolver();
                }

                return m_resolver;
            }
        }

        protected override void OnServiceUI() {
            SA_FB_EditorWindow.DrawSettingsUI();
        }
    }
}