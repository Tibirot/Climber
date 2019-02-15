using System;


#if SA_FB_INSTALLED
using FB_Plugin = Facebook.Unity;
#endif

namespace SA.Facebook
{
    public class SA_AccessToken {
#if SA_FB_INSTALLED

        private FB_Plugin.AccessToken m_accessToken;

        public SA_AccessToken(FB_Plugin.AccessToken accessToken) {
            m_accessToken = accessToken;
        }
#endif

        public string TokenString {
            get {
#if SA_FB_INSTALLED
                return m_accessToken.TokenString;
#else
                return string.Empty;;
#endif
            }
        }

        public DateTime ExpirationTime {
            get {

#if SA_FB_INSTALLED
                return m_accessToken.ExpirationTime;
#else
                  return DateTime.Now;
#endif
            }
        }


        public string UserId {
            get {
#if SA_FB_INSTALLED
                return m_accessToken.UserId;
#else
                return string.Empty;;
#endif
            }
        }

        public DateTime? LastRefresh {
            get {
#if SA_FB_INSTALLED
                return m_accessToken.LastRefresh;
#else
                return null;
#endif
            }
        }


#if SA_FB_INSTALLED
        public override string ToString() {
            return m_accessToken.ToString();
        }
#endif



        public static SA_AccessToken CurrentAccessToken {
            get {
            #if SA_FB_INSTALLED
                return new SA_AccessToken(FB_Plugin.AccessToken.CurrentAccessToken);
            #else
                return null;
            #endif
            }
        }

        
    }
}