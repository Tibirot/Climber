using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA.CrossPlatform.App
{

    /// <summary>
    /// This class provides APIs for interacting with current build properties.
    /// </summary>
    public static class UM_Build
    {

        /// <summary>
        /// Returns a shared instance of <see cref="UM_iGalleryService"/>
        /// </summary>
        private static UM_iBuildInfo m_info = null;
        public static UM_iBuildInfo Info {
            get {
                if (m_info == null) {
                    switch (Application.platform) {
                        case RuntimePlatform.Android:
                            m_info = new UM_AndroidBuildInfo();
                            break;
                        case RuntimePlatform.IPhonePlayer:
                            m_info = new UM_IOSBuildInfo();
                            break;
                        default:
                            m_info = new UM_EditorBuildInfo();
                            break;
                    }
                }
                return m_info;
            }
        }

    }
}