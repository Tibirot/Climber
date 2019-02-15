using System;
using System.Collections.Generic;
using UnityEngine;

using SA.Foundation.Templates;
using SA.iOS.StoreKit;


namespace SA.CrossPlatform.InApp
{

    internal class UM_IOSInAppClient : UM_AbstractInAppClient, UM_iInAppClient, ISN_iSKPaymentTransactionObserver
    {


        //--------------------------------------
        //  UM_AbstractInAppClient
        //--------------------------------------

        protected override void ConnectToService(Action<SA_Result> callback) {

            ISN_SKPaymentQueue.Init((result) => {
                callback.Invoke(result);
            });
        }

        protected override Dictionary<string, UM_iProduct> GetServerProductsInfo() {
            var products = new Dictionary<string, UM_iProduct>();
            foreach (var product in ISN_SKPaymentQueue.Products) {
                UM_IOSProduct p = new UM_IOSProduct();
                p.Override(product);

                products.Add(p.Id, p);
            }
            return products;
        }

        protected override void ObservTransactions() {
            ISN_SKPaymentQueue.AddTransactionObserver(this);
        }

        //--------------------------------------
        //  UM_iInAppClient
        //--------------------------------------

        public void AddPayment(string productId) {
            ISN_SKPaymentQueue.AddPayment(productId);
        }

        public void FinishTransaction(UM_iTransaction transaction) {
            UM_IOSTransaction t = (UM_IOSTransaction) transaction;

            ISN_SKPaymentTransaction skPaymentTransaction = t.IosTransaction;
            ISN_SKPaymentQueue.FinishTransaction(skPaymentTransaction);
        }

        public void RestoreCompletedTransactions() {
            ISN_SKPaymentQueue.RestoreCompletedTransactions();
        }


        //--------------------------------------
        //  ISN_TransactionObserver implementation
        //--------------------------------------

        public void OnTransactionUpdated(ISN_SKPaymentTransaction transaction) {
            var um_transaction = new UM_IOSTransaction(transaction);
            switch (transaction.State) {
                case ISN_SKPaymentTransactionState.Purchasing:
                    break;
                case ISN_SKPaymentTransactionState.Purchased:
                case ISN_SKPaymentTransactionState.Restored:
                    UpdateTransaction(um_transaction);
                    break;
                case ISN_SKPaymentTransactionState.Deferred:
                    UpdateTransaction(um_transaction);
                    break;
                case ISN_SKPaymentTransactionState.Failed:
                    UpdateTransaction(um_transaction);
                    break;
            }
        }

        public void OnTransactionRemoved(ISN_SKPaymentTransaction result) {
            //Your application does not typically need to anything on this event,  
            //but it may be used to update user interface to reflect that a transaction has been completed.
        }

        public bool OnShouldAddStorePayment(ISN_SKProduct product) {
            AddPayment(product.ProductIdentifier);
            return true;
        }


        public void OnRestoreTransactionsComplete(SA_Result result) {
            SetRestoreTransactionsResult(result);
        }


    }
}