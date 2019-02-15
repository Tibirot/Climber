using UnityEngine;

using System.Collections;
using System;
using SA.Foundation.Templates;

namespace SA.Facebook {
    public class SA_FB_GetUserResult : SA_FB_Result {
        private SA_FB_User m_user;
        public SA_FB_GetUserResult(IResult graphResult) : base(graphResult) {
            if (m_error == null) {
                try {
                    IDictionary JSON = Json.Deserialize(RawResult) as IDictionary;
                    m_user = new SA_FB_User(JSON);
                }
                catch (Exception ex) {
                    m_error = new SA_Error(5, "Failed to parse user data " + ex.Message);
                }
            }
        }

        public SA_FB_User User {
            get {
                return m_user;
            }
        }
    }
}
