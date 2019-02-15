using UnityEditor;
using UnityEngine;

using SA.Android;
using SA.iOS;

using SA.Foundation.Editor;
using Rotorz.ReorderableList;

namespace SA.CrossPlatform
{

    public class UM_InAppsUI : UM_ServiceSettingsUI
    {

        private SA_PluginActiveTextLink m_learnMoreLink;

        public class ISNSettings : UM_NativeServiceLayoutBasedSetting
        {
            public override SA_ServiceLayout Layout { get { return CreateInstance<ISN_StoreKitUI>(); } }

            public override bool IsEnabled {
                get {
                    return ISN_Preprocessor.GetResolver<ISN_StoreKitResolver>().IsSettingsEnabled;
                }
            }
        }

        public class ANSettings : UM_NativeServiceLayoutBasedSetting
        {
            public override SA_ServiceLayout Layout { get { return CreateInstance<AN_VendingFeaturesUI>(); } }
            public override bool IsEnabled {
                get {
                    return AN_Preprocessor.GetResolver<AN_VendingResolver>().IsSettingsEnabled;
                }
            }
        }





        public override void OnLayoutEnable() {
            base.OnLayoutEnable();
            AddPlatfrom(UM_UIPlatform.IOS, new ISNSettings());
            AddPlatfrom(UM_UIPlatform.Android, new ANSettings());

            AddFeatureUrl("Getting Started", "https://unionassets.com/ultimate-mobile-pro/getting-started-724");
            AddFeatureUrl("Connecting to Service", "https://unionassets.com/ultimate-mobile-pro/connecting-to-the-service-726");
            AddFeatureUrl("Purchase flow", "https://unionassets.com/ultimate-mobile-pro/purchase-flow-727");
            AddFeatureUrl("Transactions Validation", "https://unionassets.com/ultimate-mobile-pro/transactions-validation-728");
            AddFeatureUrl("Editor Testing", "https://unionassets.com/ultimate-mobile-pro/test-inside-the-editor-793");

            m_learnMoreLink = new SA_PluginActiveTextLink("[?] Learn More");
        }

        public override string Title {
            get {
                return "In-App Purchasing";
            }
        }

        public override string Description {
            get {
                return "Offer customers extra content and features using in-app purchases — including premium content, " +
                    "digital goods, and subscriptions — directly within your app. ";
            }
        }

        protected override Texture2D Icon {
            get {
                return UM_Skin.GetServiceIcon("um_market_icon.png");
            }
        }


        protected override void OnServiceUI() {
            using (new SA_WindowBlockWithSpace(new GUIContent("Editor Testing"))) {


                using (new SA_GuiBeginHorizontal()) {
                    GUILayout.FlexibleSpace();
                    bool click = m_learnMoreLink.DrawWithCalcSize();
                    if (click) {
                        Application.OpenURL("https://unionassets.com/ultimate-mobile-pro/test-inside-the-editor-793#restore-purchases");
                    }
                }

                ReorderableListGUI.Title("Products Restore Emulation");
                ReorderableListGUI.ListField(UM_Settings.Instance.TestRestoreProducts,
                    (Rect position, string text) => {
                        return EditorGUI.TextField(position, text);
                    },

                    () => {
                        EditorGUILayout.LabelField("All configured products will be restored by default.");

                    }
                 );

               

            }
        }
    }
}