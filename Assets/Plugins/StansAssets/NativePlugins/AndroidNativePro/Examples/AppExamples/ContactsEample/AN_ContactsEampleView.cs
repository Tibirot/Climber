using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SA.Android.Contacts;

public class AN_ContactsEampleView : MonoBehaviour {

    [SerializeField] RawImage m_image;
    [SerializeField] Text m_name;
    [SerializeField] Text m_info;



    public void SetContactInfo(AN_ContactInfo contact) {
        m_image.texture = contact.Photo;
        m_name.text = contact.Name;
        m_info.text = "Email: " + contact.Email + " Phone: " + contact.Phone;
    }
}
