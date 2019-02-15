using UnityEngine;
using System.Collections.Generic;

#if SA_FB_INSTALLED
using FB_Plugin = Facebook.Unity;
#endif

namespace SA.Facebook
{

    internal class SA_IResult_Proxy : IResult
    {
#if SA_FB_INSTALLED
        private FB_Plugin.IResult m_result;

        public SA_IResult_Proxy(FB_Plugin.IResult result) {
             m_result = result;
        }

#endif
        public string Error {
            get {
#if SA_FB_INSTALLED
                return m_result.Error;
#else
                return string.Empty;
#endif
            }
        }

        public IDictionary<string, object> ResultDictionary {
            get {
#if SA_FB_INSTALLED
                return m_result.ResultDictionary;
#else
                return null;
#endif
                
            }
        }
        public string RawResult {
            get {
#if SA_FB_INSTALLED
                return m_result.RawResult;
#else
                return string.Empty;
#endif
                
            }
        }

        public bool Cancelled {
            get {
#if SA_FB_INSTALLED
                 return m_result.Cancelled;
#else
                return false;
#endif
               
            }
        }
    }
}