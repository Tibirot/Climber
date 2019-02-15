using System;
using System.Collections.Generic;
using UnityEngine;
using SA.iOS.StoreKit;
using SA.Foundation.Time;


namespace SA.CrossPlatform.InApp
{
    [Serializable]
    public class UM_IOSTransaction : UM_AbstractTransaction, UM_iTransaction
    {

        private ISN_SKPaymentTransaction m_iosTransaction;

        public UM_IOSTransaction(ISN_SKPaymentTransaction transaction) {
            m_id = transaction.TransactionIdentifier;
            m_productId = transaction.ProductIdentifier;
            m_unitxTimestamp = SA_Unix_Time.ToUnixTime(transaction.Date);

            switch (transaction.State) {
                case ISN_SKPaymentTransactionState.Deferred:
                    m_state = UM_TransactionState.Deferred;
                    break;
                case ISN_SKPaymentTransactionState.Failed:
                    m_state = UM_TransactionState.Failed;
                    break;
                case ISN_SKPaymentTransactionState.Restored:
                    m_state = UM_TransactionState.Restored;
                    break;
                case ISN_SKPaymentTransactionState.Purchased:
                    m_state = UM_TransactionState.Purchased;
                    break;
            }

            m_error = transaction.Error;

            m_iosTransaction = transaction;
        }

        public ISN_SKPaymentTransaction IosTransaction {
            get {
                return m_iosTransaction;
            }
        }
    }
}