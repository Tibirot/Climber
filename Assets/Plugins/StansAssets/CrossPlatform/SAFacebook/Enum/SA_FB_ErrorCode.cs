using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA.Facebook
{
    public enum SA_FB_ErrorCode
    {
        NullResult = 0,
        UserCanceled = 1,
        APIError = 2,
        EmptyRawResult= 3,
        ParsingFailed = 4
    }
}