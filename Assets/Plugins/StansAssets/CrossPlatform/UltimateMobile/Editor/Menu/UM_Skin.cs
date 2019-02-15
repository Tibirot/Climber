using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SA.Foundation.Editor;

namespace SA.CrossPlatform
{
    public static class UM_Skin
    {

        private const string ICONS_PATH = UM_Settings.PLUGIN_FOLDER + "Editor/Art/Icons/";

        public static Texture2D SettingsWindowIcon {
            get {
                if (EditorGUIUtility.isProSkin) {
                    return GetDefaultIcon("ultimate_icon_pro.png");
                } else {
                    return GetDefaultIcon("ultimate_icon.png");
                }
            }
        }

        public static Texture2D GetServiceIcon(string iconName) {
            return SA_EditorAssets.GetTextureAtPath(ICONS_PATH + "Services/" + iconName);
        }


        public static Texture2D GetPlatformIcon(string iconName) {
            return SA_EditorAssets.GetTextureAtPath(ICONS_PATH + "Platfroms/" + iconName);
        }

        public static Texture2D GetDefaultIcon(string iconName) {
            return SA_EditorAssets.GetTextureAtPath(ICONS_PATH + "Default/" + iconName);
        }

        private static GUIStyle m_platfromBlockHeader = null;
        public static GUIStyle PlatfromBlockHeader {
            get {
                if (m_platfromBlockHeader == null) {
                    m_platfromBlockHeader = new GUIStyle(SA_PluginSettingsWindowStyles.ServiceBlockHeader);
                    m_platfromBlockHeader.fontSize = 11;
                }

                return m_platfromBlockHeader;
            }
        }
    }
}