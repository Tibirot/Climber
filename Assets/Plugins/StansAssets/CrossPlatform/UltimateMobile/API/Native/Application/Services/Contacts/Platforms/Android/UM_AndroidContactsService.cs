using System;
using System.Collections.Generic;
using SA.Android.Contacts;

namespace SA.CrossPlatform.App
{
    internal class UM_AndroidContactsService : UM_iContactsService
    {
        public void Retrieve(Action<UM_ContactsResult> callback) {

            AN_ContactsContract.Retrieve((result) => {

                UM_ContactsResult loadResult;
                if (result.IsSucceeded) {

                    List<UM_iContact> contacts = new List<UM_iContact>();

                    foreach (var contact in result.Contacts) {
                        UM_iContact um_contact = new UM_AndroidContact(contact);
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
