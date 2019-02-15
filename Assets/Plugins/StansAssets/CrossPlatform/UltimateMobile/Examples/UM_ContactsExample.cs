using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SA.CrossPlatform.App;

public class UM_ContactsExample : MonoBehaviour {

    [SerializeField] Button m_loadButton;

    private void Start() {
        m_loadButton.onClick.AddListener(() => {
            LoadContacts();
        });
    }

    void LoadContacts() {
        var client = UM_Application.ContactsService;
        client.Retrieve((result) => {
            if(result.IsSucceeded) {
                foreach(UM_iContact contact in result.Contacts) {
                    Debug.Log("contact.Name:" + contact.Name);
                    Debug.Log("contact.Phone:" + contact.Phone);
                    Debug.Log("contact.Email:" + contact.Email);
                }
            } else {
                Debug.Log("Failed to load contacts: " + result.Error.FullMessage);
            }

        });
    }
	
}
