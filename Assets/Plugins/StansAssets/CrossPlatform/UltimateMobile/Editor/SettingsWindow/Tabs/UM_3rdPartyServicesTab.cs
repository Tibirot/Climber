using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SA.Foundation.Editor;

namespace SA.CrossPlatform
{
    public class UM_3rdPartyServicesTab : SA_ServicesTab
    {
        protected override void OnCreateServices() {

            RegisterService(CreateInstance<UM_Facebook>());
            RegisterService(CreateInstance<UM_AdvertisementUI>());
            RegisterService(CreateInstance<UM_AnalyticsUI>());
            RegisterService(CreateInstance<UM_PlaymakerUI>());

            RegisterService(CreateInstance<UM_FirebaseUI>());
        }

    }

}

