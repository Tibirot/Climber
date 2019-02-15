using System;
using System.Collections.Generic;
using UnityEngine;


namespace SA.Android.Contacts
{
    /// <summary>
    /// The representation of a contact inside the device contact book
    /// </summary>
    [Serializable]
    public class AN_ContactInfo 
    {

        [SerializeField] string m_id;
        [SerializeField] string m_name;

        [SerializeField] string m_phone;
        [SerializeField] string m_note;
        [SerializeField] string m_photoData;
        [SerializeField] string m_email;



        [SerializeField] AN_ContactOrganization m_organization;
        [SerializeField] AN_ContactPostalAddress m_address;


        private Texture2D m_photo;

        /// <summary>
        /// Contact Id
        /// </summary>
        public string Id {
            get {
                return m_id;
            }
        }

        /// <summary>
        /// The name of the contact
        /// </summary>
        public string Name {
            get {
                return m_name;
            }
        }

        /// <summary>
        /// Phone number
        /// </summary>
        public string Phone {
            get {
                return m_phone;
            }
        }

        /// <summary>
        /// User notes regarding this contact
        /// </summary>
        public string Note {
            get {
                return m_note;
            }
        }

        /// <summary>
        /// Photo image
        /// </summary>
        public Texture2D Photo {
            get {

                if (m_photo == null) {
                    if(string.IsNullOrEmpty(m_photoData)) {
                        return null;
                    }

                    m_photo = new Texture2D(1, 1);
                    m_photo.LoadImageFromBase64(m_photoData);
                }

                return m_photo;
            }
        }


        /// <summary>
        /// Contact email
        /// </summary>
        public string Email {
            get {
                return m_email;
            }
        }

        /// <summary>
        /// Contact Organization
        /// </summary>
        public AN_ContactOrganization Organization {
            get {
                return m_organization;
            }

        }

        /// <summary>
        /// Contact Postal Address
        /// </summary>
        public AN_ContactPostalAddress Address {
            get {
                return m_address;
            }
        }
    }
}