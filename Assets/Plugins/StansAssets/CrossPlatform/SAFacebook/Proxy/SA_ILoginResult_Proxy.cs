
using UnityEngine;
using System.Collections;

#if SA_FB_INSTALLED
using FB_Plugin = Facebook.Unity;
#endif

namespace SA.Facebook
{

    internal class SA_ILoginResult_Proxy : SA_IResult_Proxy, ILoginResult
    {
#if SA_FB_INSTALLED
       // private FB_Plugin.ILoginResult m_result;
        private SA_AccessToken m_accessToken = null;

        public SA_ILoginResult_Proxy(FB_Plugin.ILoginResult result):base(result) {
            //m_result = result;

            if(result.AccessToken != null) {
                m_accessToken = new SA_AccessToken(result.AccessToken);
            }
        }

#endif

        public SA_AccessToken AccessToken {
            get {
#if SA_FB_INSTALLED
                return m_accessToken;
#else
                return null;
#endif
            }
        }
    }
}