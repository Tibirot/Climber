using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SA.Foundation.Templates;


namespace SA.Facebook
{
    public static class SA_FB_ErrorFactory
    {

        public static SA_Error GenerateErrorWithCode(SA_FB_ErrorCode code, string message = null) {

            if(!string.IsNullOrEmpty(message)) {
                message = code.ToString() + " | " + message;
            }

            return new SA_Error((int)code, message);
        }


    }
}