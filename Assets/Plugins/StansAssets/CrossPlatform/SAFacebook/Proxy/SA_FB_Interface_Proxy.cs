
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if SA_FB_INSTALLED
using FB_Plugin = Facebook.Unity;
#endif

namespace SA.Facebook
{

    public interface ILoginResult : IResult
    {
        SA_AccessToken AccessToken { get; }
    }


    public interface IResult
    {
        string Error { get; }
        IDictionary<string, object> ResultDictionary { get; }
        string RawResult { get; }
        bool Cancelled { get; }
    }

    public interface IGraphResult : IResult
    {
        IList<object> ResultList { get; }
        Texture2D Texture { get; }
    }

}

