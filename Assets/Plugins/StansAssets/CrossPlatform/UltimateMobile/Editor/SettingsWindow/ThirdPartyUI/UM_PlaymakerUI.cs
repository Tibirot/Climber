using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using SA.Foundation.Editor;
using SA.Foundation.Utility;

namespace SA.CrossPlatform
{

    public class UM_PlaymakerUI : UM_PluginSettingsUI
    {

        private class UM_PlaymakerResolver : SA_iAPIResolver
        {
            public bool IsSettingsEnabled {
                get {
                    return UM_DefinesResolver.IsPlayMakerInstalled;
                }

                set { }
            }
        }


        private const string PLAYMAKER_UI_CLASS_NAME = "SA.CrossPlatform.Addons.PlayMaker.UM_PlaymakerActionsUI";
        private const string PLAYMAKER_STORE_URL = "https://assetstore.unity.com/packages/tools/visual-scripting/playmaker-368";

        private SA_iGUILayoutElement m_playmakerSettingsUI;
        private UM_PlaymakerResolver m_playmakerResolver;



        public override void OnAwake() {
            base.OnAwake();

            AddFeatureUrl("Getting Started", "https://unionassets.com/ultimate-mobile-pro/getting-started-797");
            AddFeatureUrl("In App Purchases", "https://unionassets.com/ultimate-mobile-pro/in-app-purchases-798");
            AddFeatureUrl("Game Services", "https://unionassets.com/ultimate-mobile-pro/game-services-799");
            AddFeatureUrl("Social", "https://unionassets.com/ultimate-mobile-pro/social-800");
            AddFeatureUrl("Camera & Gallery", "https://unionassets.com/ultimate-mobile-pro/camera-gallery-801");
            AddFeatureUrl("Local Notifications", "https://unionassets.com/ultimate-mobile-pro/local-notifications-802");
            AddFeatureUrl("Native UI", "https://unionassets.com/ultimate-mobile-pro/native-ui-803");
            AddFeatureUrl("Advertisement", "https://unionassets.com/ultimate-mobile-pro/advertisement-804");
            AddFeatureUrl("Analytics", "https://unionassets.com/ultimate-mobile-pro/analytics-821");
        }


        public override string Title {
            get {
                return "Playmaker";
            }
        }

        public override string Description {
            get {
                return "Use Ultimate Mobile API with Playmaker visual scripting solution.";
            }
        }

        protected override Texture2D Icon {
            get {
                return UM_Skin.GetServiceIcon("um_playmaker.png");
            }
        }

        public override SA_iAPIResolver Resolver {
            get {
                if (m_playmakerResolver == null) {
                    m_playmakerResolver = new UM_PlaymakerResolver();
                }

                return m_playmakerResolver;
            }
        }

        protected override void OnServiceUI() {

            using (new SA_WindowBlockWithSpace(new GUIContent("Playmaker"))) {

                if (UM_DefinesResolver.IsPlayMakerInstalled) {
                    EditorGUILayout.HelpBox("PlayMaker Plugin Installed!", MessageType.Info);
                    DrawPlayMakerSettings();
                } else {

                    EditorGUILayout.HelpBox("PlayMaker Plugin is Missing!", MessageType.Warning);
                    using (new SA_GuiBeginHorizontal()) {
                        GUILayout.FlexibleSpace();
                        var click = GUILayout.Button("Get Playmaker", EditorStyles.miniButton, GUILayout.Width(120));
                        if (click) {
                            Application.OpenURL(PLAYMAKER_STORE_URL);
                        }

                        var refreshClick = GUILayout.Button("Refresh", EditorStyles.miniButton, GUILayout.Width(120));
                        if (refreshClick) {
                            UM_DefinesResolver.ProcessAssets();
                        }
                    }
                }

            }
        }

        private void DrawPlayMakerSettings () {
            if (m_playmakerSettingsUI == null) {
                var settingsUI = SA_Reflection.CreateInstance(PLAYMAKER_UI_CLASS_NAME);
                if (settingsUI != null) {
                    m_playmakerSettingsUI = (settingsUI as SA_iGUILayoutElement);
                    m_playmakerSettingsUI.OnLayoutEnable();
                }
            }
            if (m_playmakerSettingsUI == null) {
                UM_SettingsUtil.DrawAddonRequestUI(UM_Addon.Playmaker);
            } else {
                m_playmakerSettingsUI.OnGUI();
            }
        }
    }
}