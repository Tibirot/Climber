
using System;
using UnityEngine;
using UnityEditor;

using SA.Foundation.Editor;


namespace SA.CrossPlatform
{
    [Serializable]
    public class UM_PlatfromStateLayout
    {

        [SerializeField] SA_HyperLabel m_header;
        [SerializeField] SA_HyperLabel m_stateLabel;

        [SerializeField] GUIContent m_off;
        [SerializeField] GUIContent m_on;


        public UM_PlatfromStateLayout(GUIContent content) {
           
            m_header = new SA_HyperLabel(content, UM_Skin.PlatfromBlockHeader);
            m_header.SetMouseOverColor(SA_PluginSettingsWindowStyles.SelectedElementColor);
            m_on = new GUIContent("ON");
            m_off = new GUIContent("OFF");
            m_stateLabel = new SA_HyperLabel(m_on, UM_Skin.PlatfromBlockHeader);
            m_stateLabel.SetMouseOverColor(SA_PluginSettingsWindowStyles.SelectedElementColor);

        }

        public void SetActiveState(bool isActive) {
            if(isActive) {
                m_stateLabel.SetContent(m_on);
                m_stateLabel.SetColor(SA_PluginSettingsWindowStyles.SelectedElementColor);
            } else {
                m_stateLabel.SetContent(m_off);
                m_stateLabel.SetColor(SA_PluginSettingsWindowStyles.ProDisabledImageColor);
            }
        }

        public GUIContent Content {
            get {
                return m_header.Content;
            }
        }

        public Color StateColor {
            get {
                return m_stateLabel.Color;
            }
        } 


        public bool OnGUI() {
            GUILayout.Space(5);
            using (new SA_GuiBeginHorizontal()) {
                GUILayout.Space(10);

                float headerWidth = m_header.CalcSize().x;
                bool click = m_header.Draw(GUILayout.Width(headerWidth));
                GUILayout.FlexibleSpace();
                bool labelClick = m_stateLabel.Draw(GUILayout.Width(40));
                if(click || labelClick) {
                    return true;
                } else {
                    return false;
                }
            }
        }

    }
}