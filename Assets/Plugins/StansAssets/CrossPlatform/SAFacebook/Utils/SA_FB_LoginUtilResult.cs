using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.Facebook
{

    public class SA_FB_LoginUtilResult : IGraphResult
    {

        private string m_error = string.Empty;
        private bool m_isSucceeded = false;

        public SA_FB_LoginUtilResult(bool isSucceeded) {
            m_isSucceeded = isSucceeded;
            if(!m_isSucceeded) {
                m_error = "Operation is requires active session, make sure user is logged in";
            }
        }


        public bool IsSucceeded {
            get {
                return m_isSucceeded;
            }
        }

        public string Error{
            get {
                return m_error;
            }
        }

        public IDictionary<string, object> ResultDictionary {
            get {
                return new Dictionary<string, object>();
            }
        }

        public string RawResult {
            get {
                return string.Empty;
            }
        }

        public bool Cancelled {
            get {
                return false;
            }
        }

        public IList<object> ResultList {
            get {
                return new List<object>();
            }
        }

        public Texture2D Texture {
            get {
                return null;
            }
        }
    }
}