using System;
using System.Collections.Generic;
using UnityEngine;

using SA.iOS.Contacts;

namespace SA.CrossPlatform.App
{
    [Serializable]
    internal class UM_IOSContact : UM_AbstractContact, UM_iContact
    {

      
        public UM_IOSContact(ISN_CNContact contact) {
            m_name = contact.Nickname;
            
            if(contact.Emails.Count > 0) {
                m_email = contact.Emails[0];
            }

            if (contact.Phones.Count > 0) {
                m_phone = contact.Phones[0].FullNumber;
            }
        }
    }
}