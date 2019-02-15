using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA.CrossPlatform.Social
{

    /// <summary>
    /// Object used to build a dialog for the e-mail sharing.
    /// </summary>
    public class UM_EmailDialogBuilder : UM_ShareDialogBuilder
    {

        private string m_subject = string.Empty;
        private List<string> m_recipients = new List<string>();


        /// <summary>
        /// Set's the e-mail subject line.
        /// </summary>
        public void SetSubject(string subject) {
            m_subject = subject;
        }


        /// <summary>
        /// Add's recepients e-mail.
        /// </summary>
        public void AddRecipient(string recipient) {
            m_recipients.Add(recipient);
        }


        /// <summary>
        /// Builder defined subject
        /// </summary>
        public string Subject {
            get {
                return m_subject;
            }
        }


        /// <summary>
        /// Builder defined recepients.
        /// </summary>
        public List<string> Recipients {
            get {
                return m_recipients;
            }
        }
    }
}