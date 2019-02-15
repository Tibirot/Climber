using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA.Foundation.Templates;


namespace SA.Android.App
{

    /// <summary>
    /// Object representing android activity onActivityResult callback
    /// https://developer.android.com/reference/android/app/Activity.html#onActivityResult(int,%20int,%20android.content.Intent)
    /// </summary>
    public class AN_ActivityResult : SA_Result
    {
        [SerializeField] int m_resultCode;
        [SerializeField] string m_data;

        /// <summary>
        /// The integer result code returned by the child activity through its setResult().
        /// </summary>
        public AN_Activity.Result ResultCode {
            get {
                return (AN_Activity.Result) m_resultCode;
            }
        }

        /// <summary>
        /// An Intent, which can return result data to the caller (various data can be attached to Intent "extras").
        /// </summary>
        public string Data {
            get {
                return m_data;
            }
        }
    }

}
