using System.Collections;
using System.Collections.Generic;
using System;

using SA.Foundation.Templates;


namespace SA.Facebook
{

    public class SA_FB_GraphFriendsListResult : SA_FB_GraphInvitableFriendsListResult
    {

        private int m_totalFriendsCount = 0;

        public SA_FB_GraphFriendsListResult(IGraphResult graphResult) : base(graphResult) {
            if (m_error == null) {
                try {
                    IDictionary JSON = Json.Deserialize(RawResult) as IDictionary;
                    IDictionary body = JSON[FriendsListKey] as IDictionary;


                    if(body.Contains("summary")) {
                        IDictionary summary = body["summary"] as IDictionary;
                        if(summary.Contains("total_count")) {
                            m_totalFriendsCount = Convert.ToInt32(summary["total_count"]);
                        }
                    }

                } catch (Exception ex) {
                    m_error = new SA_Error(5, "Failed to parse friends data " + ex.Message);
                }
            }
        }



        public int TotalFriendsCount {
            get {
                return m_totalFriendsCount;
            }
        }

        protected override string FriendsListKey {
            get {
                return "friends";
            }
        }
    }
}
