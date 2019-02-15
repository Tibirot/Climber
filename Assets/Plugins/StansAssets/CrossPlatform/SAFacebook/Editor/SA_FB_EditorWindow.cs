using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SA.Foundation.Editor;
using SA.Foundation.Utility;
using SA.Foundation.UtilitiesEditor;

namespace SA.Facebook
{

    public class SA_FB_EditorWindow : EditorWindow
    {

        private static string newPermition = string.Empty;
        private const string SDK_DOWNLOAD_URL = "https://developers.facebook.com/docs/unity";
        private static Editor m_facebookSettingsEditor = null;



        private void OnEnable() {
            titleContent = new GUIContent("Facebook");
            minSize = new Vector2(350, 100);
        }


        void OnGUI() {

            DrawSettingsUI();

        }

        public static void DrawSettingsUI() {


            

           // FB_Plugin.FacebookSdkVersion.Build

            using (new SA_WindowBlockWithSpace(new GUIContent("Facebook SDK"))) {
                if (SA_FB_InstallationProcessing.IsSDKInstalled) {
                    EditorGUILayout.HelpBox("Facebook SDK Installed!", MessageType.Info);
                } else {

                    EditorGUILayout.HelpBox("Facebook SDK Required!", MessageType.Warning);
                    using (new SA_GuiBeginHorizontal()) {
                        GUILayout.FlexibleSpace();
                        var click = GUILayout.Button("Download SDK", EditorStyles.miniButton, GUILayout.Width(120));
                        if (click) {
                            Application.OpenURL(SDK_DOWNLOAD_URL);
                        }

                        var refreshClick = GUILayout.Button("Refresh", EditorStyles.miniButton, GUILayout.Width(120));
                        if (refreshClick) {
                            SA_FB_InstallationProcessing.ProcessAssets();
                        }
                    }
                }
            }

            if (!SA_FB_InstallationProcessing.IsSDKInstalled) {
                return;
            }

            using (new SA_WindowBlockWithSpace(new GUIContent("SDK Settings"))) {
                if (m_facebookSettingsEditor == null) {
                    var facebookSettings = Resources.Load("FacebookSettings") as ScriptableObject;
                    if(facebookSettings != null) {
                        m_facebookSettingsEditor = Editor.CreateEditor(facebookSettings);
                    }
                }

                if(m_facebookSettingsEditor == null) {
                    EditorGUILayout.HelpBox("Facebook Settings Resources can't be located! " +
                        "Try to use Facebook plugin top menu in order to tirgger Settings Resources creation.", 
                        MessageType.Warning);
                    return;
                }

                EditorGUI.BeginChangeCheck();
                m_facebookSettingsEditor.OnInspectorGUI();
                if(EditorGUI.EndChangeCheck()) {
                    SA_EditorUtility.SetDirty(m_facebookSettingsEditor.target);
                }
                
            }

            GUI.changed = false;
            using (new SA_WindowBlockWithSpace(new GUIContent("Scopes"))) {
                SA_EditorGUILayout.ReorderablList(SA_FB_Settings.Instance.Scopes, (item) => {
                    return item;
                });

                using (new SA_GuiBeginHorizontal()) {
                    EditorGUILayout.LabelField("Add new scope: ");
                    newPermition = EditorGUILayout.TextField(newPermition);
                }


                using (new SA_GuiBeginHorizontal()) {
                    EditorGUILayout.Space();
                    if (GUILayout.Button("Documentation", GUILayout.Width(100))) {
                        Application.OpenURL("https://developers.facebook.com/docs/facebook-login/permissions/v2.0");
                    }


                    if (GUILayout.Button("Add", GUILayout.Width(100))) {
                        if (newPermition != string.Empty) {
                            newPermition = newPermition.Trim();
                            if (!SA_FB_Settings.Instance.Scopes.Contains(newPermition)) {
                                SA_FB_Settings.Instance.Scopes.Add(newPermition);
                            }
                            newPermition = string.Empty;
                        }
                    }
                }


                if (GUI.changed) {
                    SA_FB_Settings.Save();
                }
            }


         
        }
       
    }
}