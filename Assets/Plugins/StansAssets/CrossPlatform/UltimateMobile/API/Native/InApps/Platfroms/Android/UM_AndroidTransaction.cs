using System;
using System.Collections.Generic;
using UnityEngine;

using SA.Android.Vending.Billing;


namespace SA.CrossPlatform.InApp
{
    [Serializable]
    public class UM_AndroidTransaction : UM_AbstractTransaction, UM_iTransaction
    {
        private AN_Purchase m_purchase;

        public UM_AndroidTransaction(AN_BillingPurchaseResult transactionResult) {

            if(transactionResult.IsSucceeded) {
                SetPurchase(transactionResult.Purchase, false);
            } else {
                m_state = UM_TransactionState.Failed;
                m_error = transactionResult.Error;
            }
        }

        public UM_AndroidTransaction(AN_Purchase purchase, bool isRestored) {
            SetPurchase(purchase, isRestored);
        }

        private void SetPurchase(AN_Purchase purchase, bool isRestored) {
            m_purchase = purchase;
            m_id = m_purchase.OrderId;
            m_productId = m_purchase.ProductId;
            m_unitxTimestamp = m_purchase.PurchaseTime;

            if(isRestored) {
                m_state = UM_TransactionState.Restored;
            } else {
                m_state = UM_TransactionState.Purchased;
            }
           
        }

        public AN_Purchase Purchase {
            get {
                return m_purchase;
            }
        }
    }
}