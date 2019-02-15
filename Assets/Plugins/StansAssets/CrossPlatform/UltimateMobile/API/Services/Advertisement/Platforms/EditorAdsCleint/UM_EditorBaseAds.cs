using System;
using SA.Foundation.Templates;

namespace SA.CrossPlatform.Advertisement
{

    internal class UM_EditorBaseAds
    {
        protected bool m_isReady = false;

        public void Load(Action<SA_Result> callback) {
            Load("editor_ads_id", callback);
        }

        public void Load(string id, Action<SA_Result> callback) {
            UM_EditorAPIEmulator.WaitForNetwork(() => {
                m_isReady = true;
                callback.Invoke(new SA_Result());
            });
        }


        public virtual bool IsReady {
            get {
                return m_isReady;
            }
        }
    }
}