using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using SA.Android;
using SA.iOS;
using SA.Foundation.Editor;

using SA.iOS.XCode;

namespace SA.CrossPlatform
{
    public abstract class UM_ServiceSettingsUI : SA_ServiceLayout
    {

        private List<UM_ServicePlatfromInfo> m_platforms = new List<UM_ServicePlatfromInfo>();

        public override void OnAwake() {
            base.OnAwake();
        }

        public override void OnLayoutEnable() {

            m_features.Clear();
            m_platforms.Clear();
        }

        protected override IEnumerable<string> SupportedPlatforms {
            get {
                return new List<string>() { "iOS", "Android", "Unity Editor" };
            }
        }

        public override SA_iAPIResolver Resolver {
            get {
                return null;
            }
        }

        protected void AddPlatfrom(UM_UIPlatform platform, UM_NativeServiceSettings settings) {
            var info = new UM_ServicePlatfromInfo(platform, settings);
            m_platforms.Add(info);
        }



        protected override void DrawServiceRequirements() {

        }

        protected virtual void DrawDefaultBlocks() {
            if (m_platforms.Count > 0) {
                using (new SA_WindowBlockWithSpace(new GUIContent("Plugins"), 5)) {

                    foreach (var platfrom in m_platforms) {
                        bool clicked = platfrom.Layout.OnGUI();
                        if (clicked) {
                            OpenPlatfromUI(platfrom);
                        }
                    }
                }
            } else {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("COMING SOON!!\n" +
                    "Feel free to get in touch if you need to get this working NOW.", MessageType.Info);
            }

        }


        protected override void DrawGettingStartedBlock() {
            base.DrawGettingStartedBlock();
            DrawDefaultBlocks();
        }

        private void OpenPlatfromUI(UM_ServicePlatfromInfo platfromInfo) {
            var info = new UM_SettingsWindow.SelectedBlockInfo();
            info.SettingsBlockTypeName = platfromInfo.Settings.ServiceUIType.Name;
            info.Platform = platfromInfo.Platform;

            UM_SettingsWindow.SelectBlock(info);
        }

      

      
        protected override void DrawServiceStateInteractive() {
           
        }


        protected override bool DrawServiceStateInfo() {

            foreach (var platfrom in m_platforms) {

                using (new SA_GuiChangeContentColor(platfrom.Layout.StateColor)) {
                    var content = new GUIContent(platfrom.Layout.Content.image);
                    EditorGUILayout.LabelField(content, GUILayout.Height(22), GUILayout.Width(22));
                }
                
                GUILayout.Space(-6);
            }
            return false;
        }

        protected override void CheckServiceAvailability() {
            foreach (var platfrom in m_platforms) {
                platfrom.Layout.SetActiveState(platfrom.Settings.IsEnabled);
            }
        }
    }
}