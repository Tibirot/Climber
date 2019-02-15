using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA.Foundation.Templates;

namespace SA.Facebook
{

    public class SA_FBGraphFirendsListResult : SA_FB_Result
    {

        public List<SA_FB_User> m_users = new List<SA_FB_User>();

        public SA_FBGraphFirendsListResult(IGraphResult graphResult) : base(graphResult) {
            if (m_error == null) {
                try {
                    IDictionary JSON = Json.Deserialize(RawResult) as IDictionary;
                    IDictionary f = JSON[FriendsListKey] as IDictionary;
                    IList flist = f["data"] as IList;


                    for (int i = 0; i < flist.Count; i++) {
                        SA_FB_User user = new SA_FB_User(flist[i] as IDictionary);
                        m_users.Add(user);
                    }

                } catch (System.Exception ex) {
                    m_error = SA_FB_ErrorFactory.GenerateErrorWithCode(SA_FB_ErrorCode.ParsingFailed, ex.Message);
                }
            }
        }

        public virtual string FriendsListKey {
            get {
                return "friends"; // invitable_friends for the invitable firends request
            }
        }

    }
}
