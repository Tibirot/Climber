
using System;
using System.Collections.Generic;
using SA.iOS.Contacts;

namespace SA.CrossPlatform.App
{
    internal class UM_IOSContactsService : UM_iContactsService
    {
        public void Retrieve(Action<UM_ContactsResult> callback) {
            ISN_CNContactStore.FetchPhoneContacts((result) => {
                UM_ContactsResult loadResult;
                if (result.IsSucceeded) {

                    List<UM_iContact> contacts = new List<UM_iContact>();

                    foreach (var contact in result.Contacts) {
                        UM_iContact um_contact = new UM_IOSContact(contact);
                        contacts.Add(um_contact);
                    }

                    loadResult = new UM_ContactsResult(contacts);

                } else {
                    loadResult = new UM_ContactsResult(result.Error);
                }
                callback.Invoke(loadResult);
            });
        }
    }
}
