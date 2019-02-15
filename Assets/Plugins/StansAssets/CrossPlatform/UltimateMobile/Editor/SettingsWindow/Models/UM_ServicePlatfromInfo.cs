using System;
using System.Collections.Generic;
using UnityEngine;

namespace SA.CrossPlatform
{
    [Serializable]
    public class UM_ServicePlatfromInfo 
    {

        [SerializeField] UM_UIPlatform platform;
        [SerializeField] UM_NativeServiceSettings m_settings;
       

      
        [SerializeField] GUIContent m_content;
        [SerializeField] UM_PlatfromStateLayout m_layout;


        public UM_ServicePlatfromInfo(UM_UIPlatform platform, UM_NativeServiceSettings settings) {
            this.platform = platform;
            m_settings = settings;

            switch (platform) {
                case UM_UIPlatform.IOS:
                    m_content = new GUIContent(" iOS (" + m_settings.ServiceName + ")", UM_Skin.GetPlatformIcon("ios_icon.png"));
                    break;
                case UM_UIPlatform.Android:
                    m_content = new GUIContent(" Android (" + m_settings.ServiceName + ")", UM_Skin.GetPlatformIcon("android_icon.png"));
                    break;
            }
            m_layout = new UM_PlatfromStateLayout(m_content);
        }

        public UM_PlatfromStateLayout Layout {
            get {
                return m_layout;
            }
        }

      

        public UM_UIPlatform Platform {
            get {
                return platform;
            }
        }

        public UM_NativeServiceSettings Settings {
            get {
                return m_settings;
            }
        }
    }
}