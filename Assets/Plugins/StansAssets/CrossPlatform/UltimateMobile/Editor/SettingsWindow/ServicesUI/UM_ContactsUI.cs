using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using SA.Android;
using SA.iOS;

using SA.Foundation.Editor;
using SA.CrossPlatform.App;


namespace SA.CrossPlatform
{

    public class UM_ContactsUI : UM_ServiceSettingsUI
    {

        public class ISNSettings : UM_NativeServiceLayoutBasedSetting
        {
            public override SA_ServiceLayout Layout { get { return CreateInstance<ISN_ContactsUI>(); } }

            public override bool IsEnabled {
                get {
                    return ISN_Preprocessor.GetResolver<ISN_ContactsResolver>().IsSettingsEnabled;
                }
            }
        }

        public class ANSettings : UM_NativeServiceLayoutBasedSetting
        {
            public override SA_ServiceLayout Layout { get { return CreateInstance<AN_ContactsFeaturesUI>(); } }

            public override bool IsEnabled {
                get {
                    return AN_Preprocessor.GetResolver<AN_ContactsResolver>().IsSettingsEnabled;
                }
            }
        }

        public override void OnLayoutEnable() {
            base.OnLayoutEnable();
            AddPlatfrom(UM_UIPlatform.IOS, new ISNSettings());
            AddPlatfrom(UM_UIPlatform.Android, new ANSettings());

            AddFeatureUrl("Getting Started", "https://unionassets.com/ultimate-mobile-pro/getting-started-733");
            AddFeatureUrl("Phone Contacts", "https://unionassets.com/ultimate-mobile-pro/retrieving-phone-contacts-746");
        }

        public override string Title {
            get {
                return "Contacts";
            }
        }

        public override string Description {
            get {
                return "Access the user's contacts and format and localize contact information.";
            }
        }

        protected override Texture2D Icon {
            get {
                return UM_Skin.GetServiceIcon("um_contact_icon.png");
            }
        }



        protected override void OnServiceUI() {
            using (new SA_WindowBlockWithSpace(new GUIContent("Editor Testing"))) {
                EditorGUILayout.HelpBox("Spesifiy contacts book entries that will be used " +
                    "while emulating API inside the editor.", MessageType.Info);

                SA_EditorGUILayout.ReorderablList(UM_Settings.Instance.EditorTestingContacts, GetContactDisplayName, DrawProductContent, () => {


                    string name = "John Smith";
                    string phone = "1–800–854–3680";
                    string email = "johnsmith@gmail.com";

                    var contact = new UM_EditorContact(name, phone, email);
                    UM_Settings.Instance.EditorTestingContacts.Add(contact);
                });
            }
        }





        private string GetContactDisplayName(UM_EditorContact contact) {
            return contact.Name + " (" + contact.Email + ")";
        }

        private void DrawProductContent(UM_EditorContact contact) {

            contact.Name = SA_EditorGUILayout.TextField("Name", contact.Name);
            contact.Email = SA_EditorGUILayout.TextField("Email", contact.Email);
            contact.Phone = SA_EditorGUILayout.TextField("Phone", contact.Phone);

        }
    }
}