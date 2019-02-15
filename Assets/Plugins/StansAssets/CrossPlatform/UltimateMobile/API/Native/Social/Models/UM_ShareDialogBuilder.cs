using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA.CrossPlatform.Social
{
    /// <summary>
    /// Dialog builder is used to pre-definee sharing dialog options.
    /// </summary>
    public class UM_ShareDialogBuilder
    {

        private string m_text = string.Empty;
        private string m_url = string.Empty;
        private List<Texture2D> m_images = new List<Texture2D>();


        /// <summary>
        /// Set's the sharing Text.
        /// </summary>
        /// <param name="text">Text.</param>
        public void SetText(string text) {
            m_text = text;
        }


        /// <summary>
        /// Set's the sharing URL
        /// </summary>
        /// <param name="url">URL.</param>
        public void SetUrl(string url) {
            m_url = url;
        }


        /// <summary>
        /// Adds an image that will be shared via the native dialog.
        /// Please note that image has to be redeable.
        /// </summary>
        /// <param name="image">Image.</param>
        public void AddImage(Texture2D image) {
            m_images.Add(image);
        }


        /// <summary>
        /// Defined sharing text.
        /// </summary>
        public string Text {
            get {
                return m_text;
            }
        }


        /// <summary>
        /// Images that was defined for the curreent builder.
        /// </summary>
        /// <value>The images.</value>
        public List<Texture2D> Images {
            get {
                return m_images;
            }
        }

        /// <summary>
        /// Sharind dialog builder defined URL.
        /// </summary>
        public string Url {
            get {
                return m_url;
            }
        }


        /// <summary>
        /// Buildeer <see cref="Text"/> propeerty combined with the builder <see cref="Url"/> property
        /// </summary>
        /// <value>The text with URL.</value>
        public string TextWithUrl {
            get {
                if (!string.IsNullOrEmpty(m_url)) {
                    return m_text  + "  " + m_url;
                } else {
                    return m_text;
                }
            }
        }
    }
}