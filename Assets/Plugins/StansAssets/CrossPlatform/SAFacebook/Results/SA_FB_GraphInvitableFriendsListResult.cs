using System.Collections;
using System.Collections.Generic;
using System;
using SA.Foundation.Templates;

namespace SA.Facebook
{

    public class SA_FB_GraphInvitableFriendsListResult : SA_FB_GraphResult
    {

        private List<SA_FB_User> m_users = new List<SA_FB_User>();

        public SA_FB_GraphInvitableFriendsListResult(IGraphResult graphResult) : base(graphResult) {
            if (m_error == null) {
                try {
                    IDictionary RAW = Json.Deserialize(RawResult) as IDictionary;
                    IDictionary friends = RAW[FriendsListKey] as IDictionary;
                    IList flist = friends["data"] as IList;

                    for (int i = 0; i < flist.Count; i++) {
                        SA_FB_User user = new SA_FB_User(flist[i] as IDictionary);
                        m_users.Add(user);
                    }

                    ParseResultId(RAW);
                    ParsePaginatedResult(friends);

                } catch (Exception ex) {
                    m_error = new SA_Error(5, "Failed to parse friends data " + ex.Message);
                }
            }
        }


        /// <summary>
        /// List of loaded user model's
        /// </summary>
        public List<SA_FB_User> Users {
            get {
                return m_users;
            }
        }


        protected virtual string FriendsListKey {
            get {
                return "invitable_friends"; 
            }
        }
    }
}