using System;
using System.Collections.Generic;
using UnityEngine;

using SA.Foundation.Time;
using SA.Foundation.Templates;

namespace SA.CrossPlatform.InApp
{
    [Serializable]
    public class UM_AbstractTransaction 
    {
        [SerializeField] protected string m_id;
        [SerializeField] protected string m_productId;
        [SerializeField] protected long m_unitxTimestamp;

        [SerializeField] protected UM_TransactionState m_state;
        [SerializeField] protected SA_Error m_error = null;


        public string Id {
            get {
                return m_id;
            }
        }

        public string ProductId {
            get {
                return m_productId;
            }
        }


        public DateTime Timestamp {
            get {
                return SA_Unix_Time.ToDateTime(m_unitxTimestamp);
            }
        }

        public SA_Error Error {
            get {
                return m_error;
            }

        }

        public UM_TransactionState State {
            get {
                return m_state;
            }
        }
    }
}