using UnityEngine;
using UnityEditor;

using SA.Foundation.Editor;

namespace SA.CrossPlatform
{
    public static class UM_SettingsUtil 
    {

        private const string PLAYMAKER_ADDON = "https://dl.dropboxusercontent.com/s/k2ivxt6j8dhhb9h/PlayMakerAddon.unitypackage";
        private const string ADMOB_ADDON = "https://dl.dropboxusercontent.com/s/p17e7xi858l4qw7/GoogleMobileAdsClient.unitypackage";

        public static void DrawAddonRequestUI(UM_Addon addon) {
            EditorGUILayout.HelpBox("Ultimate Mobile " + addon + " Addon required", MessageType.Warning);
            using (new SA_GuiBeginHorizontal()) {
                GUILayout.FlexibleSpace();
                var content = new GUIContent(" " + addon + " Addon", UM_Skin.GetPlatformIcon("unity_icon.png"));
                var click = GUILayout.Button(content, EditorStyles.miniButton, GUILayout.Width(120), GUILayout.Height(18));
                if (click) {
                    string url = null;
                    switch (addon) {
                        case UM_Addon.AdMob:
                            url = ADMOB_ADDON;
                            break;
                        case UM_Addon.Playmaker:
                            url = PLAYMAKER_ADDON;
                            break;
                    }
                    SA_PackageManager.DownloadAndImport(addon + " Addon", url, interactive: true);
                }
            }
        }
    }
}