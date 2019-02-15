using System;
using System.Collections;

using SA.Foundation.Templates;


namespace SA.Facebook {
    public class SA_FB_GetProfileImageUrlResult : SA_FB_Result {
        private string m_url;

        public SA_FB_GetProfileImageUrlResult(IResult graphResult) : base(graphResult) {
            if (m_error == null) {
                try {
                    IDictionary JSON = Json.Deserialize(RawResult) as IDictionary;
                    var user = new SA_FB_User(JSON);
                    m_url = user.PictureUrl;
                }
                catch (Exception ex) {
                    m_error = new SA_Error(5, "Failed to parse user data " + ex.Message);
                }
            }
        }

        public string URL {
            get { return m_url; }
        }
    }
}
