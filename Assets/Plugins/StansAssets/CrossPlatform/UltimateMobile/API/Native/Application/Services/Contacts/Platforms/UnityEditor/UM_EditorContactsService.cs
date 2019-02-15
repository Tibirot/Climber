
using System;
using System.Collections.Generic;
using SA.iOS.Contacts;

using SA.Foundation.Async;

namespace SA.CrossPlatform.App
{
    internal class UM_EditorContactsService : UM_iContactsService
    {
        public void Retrieve(Action<UM_ContactsResult> callback) {


            SA_Coroutine.WaitForSeconds(2f, () => {

                List<UM_iContact> contacts = new List<UM_iContact>();
                foreach (var contact in UM_Settings.Instance.EditorTestingContacts) {
                    contacts.Add(contact.Clone());
                }

                var loadResult = new UM_ContactsResult(contacts);
                callback.Invoke(loadResult);
            });

        }
    }
}
