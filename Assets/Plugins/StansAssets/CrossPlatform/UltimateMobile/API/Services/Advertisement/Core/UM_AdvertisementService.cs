using UnityEngine;
using System.Collections.Generic;


namespace SA.CrossPlatform.Advertisement {


    /// <summary>
    /// Main entry point for the Advertisement Services APIs. 
    /// </summary>
    public class UM_AdvertisementService {

        private static Dictionary<UM_AdPlatform, UM_iAdsClient> s_createdClients = new Dictionary<UM_AdPlatform, UM_iAdsClient>(); 


        /// <summary>
        /// Returns ads client based on platfrom.
        /// </summary>
        /// <param name="platfrom">Advertisment platfrom.</param>
        public static UM_iAdsClient GetClient(UM_AdPlatform platfrom) {

            if (s_createdClients.ContainsKey(platfrom)) {
                return s_createdClients[platfrom];
            }

            var client = CreateClient(platfrom);
            s_createdClients.Add(platfrom, client);

            return client;
        }



        private static UM_iAdsClient CreateClient(UM_AdPlatform platfrom) {

            if(Application.isEditor) {
                return new UM_EditorAdsClient();
            }

            switch (platfrom) {
                case UM_AdPlatform.Google:
                    return UM_GoogleAdsClientProxy.CreateAdsClient();
                case UM_AdPlatform.Unity:
                    return new UM_UnityAdsClient();
            }

            return null;
        }


    }
}