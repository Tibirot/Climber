using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA.Facebook
{

    /// <summary>
    /// Clases used to simplifay Facebook Graph API request building
    /// </summary>
    public class SA_FB_RequestBuilder 
    {

        private StringBuilder m_request;

        public SA_FB_RequestBuilder(string request) {
            m_request = new StringBuilder(request);
        }


        /// <summary>
        /// Add's facebook Graph API command to a current request
        /// </summary>
        public void AddCommand(string command, params object[] args) {
            m_request.Append(".");
            m_request.Append(command);
            m_request.Append("(");

            for(int i = 0; i < args.Length; i++) {
                m_request.Append(args[i]);
                if(i != args.Length-1) {
                    m_request.Append(",");
                }
            }
            foreach(var arg in args) {
                
            }

            m_request.Append(")");
        }

        /// <summary>
        /// Add's a limit command
        /// </summary>
        public void AddLimit(int limit) {
            AddCommand("limit", limit);
        }

        /// <summary>
        /// Add pagination cursor
        /// </summary>
        public void AddCursor(SA_FB_Cursor cursor) {
            if(cursor != null) {
                AddCommand(cursor.Type.ToString(), cursor.Value);
            }
        }


        /// <summary>
        /// Return's builded request string
        /// </summary>
        public string RequestString {
            get {
                return m_request.ToString();
            }
        }

       
    }
}