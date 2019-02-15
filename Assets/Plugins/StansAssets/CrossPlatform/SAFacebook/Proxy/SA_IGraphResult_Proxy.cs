
using UnityEngine;
using System.Collections.Generic;

#if SA_FB_INSTALLED
using FB_Plugin = Facebook.Unity;
#endif


namespace SA.Facebook
{

    internal class SA_IGraphResult_Proxy : SA_IResult_Proxy, IGraphResult
    {
#if SA_FB_INSTALLED
        private FB_Plugin.IGraphResult m_result;

        public SA_IGraphResult_Proxy(FB_Plugin.IGraphResult result):base(result) {
            m_result = result;
        }

#endif


        public IList<object> ResultList {
            get {
#if SA_FB_INSTALLED
                return m_result.ResultList;
#else
                return null;
#endif
            }
        }

        public Texture2D Texture {
            get {
#if SA_FB_INSTALLED
                return m_result.Texture;
#else
                return null;
#endif
            }
        }

    }
}