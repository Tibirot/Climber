using System;
using System.Collections.Generic;
using UnityEngine;


namespace SA.CrossPlatform.App
{

    /// <summary>
    /// Picked image from gallery representation
    /// </summary>
    [Serializable]
    public class UM_Media
    {

        [SerializeField] string m_path;
        [SerializeField] UM_MediaType m_type;

        [SerializeField] Texture2D m_thumbnail;


        public UM_Media(Texture2D thumbnail, string path, UM_MediaType type) {
            m_path = path;
            m_type = type;
            m_thumbnail = thumbnail;
        }

        /// <summary>
        /// Device local path to the current media file.
        /// </summary>
        /// <value>The path.</value>
        public string Path {
            get {
                return m_path;
            }
        }

        /// <summary>
        /// Media file thumbnail.
        /// </summary>
        public Texture2D Thumbnail {
            get {
                return m_thumbnail;
            }

        }

        /// <summary>
        /// Type of yhe mdeia
        /// </summary>
        public UM_MediaType Type {
            get {
                return m_type;
            }
        }
    }
}