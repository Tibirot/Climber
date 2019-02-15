using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SA.Foundation.Editor;

namespace SA.CrossPlatform
{
    public class UM_ServicesTab : SA_ServicesTab
    {
        protected override void OnCreateServices() {

           
            RegisterService(CreateInstance<UM_FoundationUI>());
            RegisterService(CreateInstance<UM_InAppsUI>());
            RegisterService(CreateInstance<UM_GameServicesUI>());
            RegisterService(CreateInstance<UM_SocialUI>());
            RegisterService(CreateInstance<UM_CameraUI>());
            RegisterService(CreateInstance<UM_GalleryUI>());

            RegisterService(CreateInstance<UM_ContactsUI>());
            RegisterService(CreateInstance<UM_MediaUI>());
            

            RegisterService(CreateInstance<UM_LocalNotificationsUI>());
            RegisterService(CreateInstance<UM_RemoteNotificationsUI>());

        }

    }

}

