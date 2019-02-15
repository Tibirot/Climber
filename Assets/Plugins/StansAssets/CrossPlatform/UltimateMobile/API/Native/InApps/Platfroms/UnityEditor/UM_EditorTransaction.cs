using System;
using SA.Foundation.Utility;
using SA.Foundation.Time;

namespace SA.CrossPlatform.InApp
{
    [Serializable]
    public class UM_EditorTransaction : UM_AbstractTransaction, UM_iTransaction
    {
        public UM_EditorTransaction(string productId, UM_TransactionState state) {
            m_id = SA_IdFactory.RandomString;
            m_productId = productId;
            m_unitxTimestamp = SA_Unix_Time.ToUnixTime(DateTime.Now);
            m_state = state;
        }
    }
}