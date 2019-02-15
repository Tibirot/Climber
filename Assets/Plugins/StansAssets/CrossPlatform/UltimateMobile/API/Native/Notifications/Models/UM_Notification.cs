using System;
using System.Collections.Generic;
using UnityEngine;


namespace SA.CrossPlatform.Notifications
{

    [Serializable]
    public class UM_Notification
    {
        [SerializeField] string m_body = string.Empty;
        [SerializeField] string m_title = string.Empty;


        [SerializeField] string m_soundName = string.Empty;
        [SerializeField] string m_iconName = string.Empty;

        [SerializeField] Texture2D m_largeIcon = null;


        /// <summary>
        /// Set the first line of text in the platform notification template.
        /// </summary>
        /// <param name="title">Title.</param>
        public void SetTitle(string title) {
            m_title = title;
        }

        /// <summary>
        /// Set the second line of text in the platform notification template.
        /// </summary>
        /// <param name="body">Content Body.</param>
        public void SetBody(string body) {
            m_body = body;
        }


        /// <summary>
        /// Set the small icon resource, which will be used to represent the notification in the status bar. 
        /// Only use image resource name. 
        /// Example: myIcon.png
        /// </summary>
        /// <param name="iconName">A resource name inside your project.</param>
        public void SetSmallIconName(string iconName) {
            m_iconName = iconName;
        }

        /// <summary>
        /// Add a large icon to the notification content view. 
        /// In the platform template, this image will be shown on the left of the notification view 
        /// in place of the small icon (which will be placed in a small badge atop the large icon).
        /// </summary>
        /// <param name="icon">Icon as Texture2D</param>
        public void SetLargeIcon(Texture2D icon) {
            m_largeIcon = icon;
        }





        /// <summary>
        /// Set the sound to play, when notification received
        /// Only use sound resource name. 
        /// Example: mySound.wav
        /// </summary>
        /// <param name="soundName">A resource name inside your project.</param>
        public void SetSoundName(string soundName) {
            m_soundName = soundName;
        }


        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title {
            get {
                return m_title;
            }
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        public string Body {
            get {
                return m_body;
            }
        }

        /// <summary>
        /// Gets the name of the sound.
        /// </summary>
        public string SoundName {
            get {
                return m_soundName;
            }
        }


        /// <summary>
        /// Gets the name of the icon.
        /// </summary>
        public string IconName {
            get {
                return m_iconName;
            }
        }


        /// <summary>
        /// Gets the large icon.
        /// </summary>
        public Texture2D LargeIcon {
            get {
                return m_largeIcon;
            }
        }
    }
}