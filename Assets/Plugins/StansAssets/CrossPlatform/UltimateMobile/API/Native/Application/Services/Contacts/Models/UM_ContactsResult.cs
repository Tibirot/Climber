using System;
using System.Collections.Generic;
using UnityEngine;

using SA.Foundation.Templates;


namespace SA.CrossPlatform.App
{

    /// <summary>
    /// Contacts retrive result
    /// </summary>
    [Serializable]
    public class UM_ContactsResult : SA_Result
    {
        [SerializeField] List<UM_iContact> m_contacts = new List<UM_iContact>();


        public UM_ContactsResult(SA_Error error) : base(error) { }
        public UM_ContactsResult(List<UM_iContact> contacts) : base() {
            m_contacts = contacts;
        }

       
        /// <summary>
        /// The list of loaded contacts.
        /// </summary>
        public List<UM_iContact> Contacts {
            get {
                return m_contacts;
            }
        }

    }
}