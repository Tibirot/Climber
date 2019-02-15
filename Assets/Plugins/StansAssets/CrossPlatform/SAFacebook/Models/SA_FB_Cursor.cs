using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA.Facebook
{
    /// <summary>
    /// The representation of Graph API paginated cursor
    /// </summary>
    public class SA_FB_Cursor 
    {
        private SA_FB_CursorType m_type;
        private string m_value;


        public SA_FB_Cursor(SA_FB_CursorType type, string value) {
            m_type = type;
            m_value = value;
        }


        /// <summary>
        /// Cursor type
        /// </summary>
        public SA_FB_CursorType Type {
            get {
                return m_type;
            }
        }

        /// <summary>
        /// Cursor value
        /// </summary>
        public string Value {
            get {
                return m_value;
            }
        }
    }
}