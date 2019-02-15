using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SA.Foundation.Templates;

namespace SA.CrossPlatform.App
{

    /// <summary>
    /// Image result object
    /// </summary>
    public class UM_MediaResult : SA_Result
    {

        [SerializeField] UM_Media m_media;


        public UM_MediaResult(UM_Media media) {
            m_media = media;
            if (m_media == null) {
                m_error = new SA_Error(100, "No Media");
            }
        }

        public UM_MediaResult(SA_Error error) : base(error) { }



        /// <summary>
        /// Contains <see cref="UM_Media"/> if result <see cref="IsSucceeded"/>,
        /// Otherwise the object is <c>null</c>.
        /// </summary>
        /// <value>The image.</value>
        public UM_Media Media {
            get {
                return m_media;
            }
        }
    }
}