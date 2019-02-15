

using System;
using System.Collections.Generic;
using UnityEngine;

using SA.Foundation.Config;
using SA.Foundation.Editor;

using UnityEditor;


namespace SA.CrossPlatform
{
    
    public class UM_AdvertisementPlatfromUI : SA_CollapsableWindowBlockLayout
    {
       
      
        private SA_HyperLabel m_stateLabel;
      

        private GUIContent m_off;
        private GUIContent m_on;

        private SA_iAPIResolver m_resolver;


        public UM_AdvertisementPlatfromUI(string name, string image, SA_iAPIResolver resolver, Action onGUI) : base(new GUIContent(name, UM_Skin.GetPlatformIcon(image)), onGUI) {

            m_on = new GUIContent("ON");
            m_off = new GUIContent("OFF");
            m_stateLabel = new SA_HyperLabel(m_on, EditorStyles.boldLabel);
            m_stateLabel.SetMouseOverColor(SA_PluginSettingsWindowStyles.SelectedElementColor);

            m_resolver = resolver;
        }


        protected override void OnAfterHeaderGUI() {


            GUILayout.FlexibleSpace();
            if (m_resolver.IsSettingsEnabled) {

                using (new SA_GuiChangeColor(SA_PluginSettingsWindowStyles.SelectedElementColor)) {
                    EditorGUILayout.LabelField(m_on, EditorStyles.boldLabel, GUILayout.Width(35));
                }
                   
            } else {
                EditorGUILayout.LabelField(m_off, EditorStyles.boldLabel, GUILayout.Width(35));
            }
            
        }
        

    }
}

