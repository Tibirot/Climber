using System;
using System.Collections.Generic;
using UnityEngine;

namespace SA.iOS.UIKit
{

    [Serializable]
    public class ISN_UIAvailableMediaTypes {
        [SerializeField] List<string> m_types;


        /// <summary>
        /// Gets the types.
        /// </summary>
        /// <value>The types.</value>
        public List<string> Types {
            get {
                return m_types;
            }
        }
    }
}
