using System;
using System.Collections.Generic;
using UnityEngine;

using SA.Foundation.Templates;
using SA.Android.Vending.Billing;


namespace SA.CrossPlatform.InApp
{

    internal class UM_AndroidInAppClient : UM_AbstractInAppClient, UM_iInAppClient
    {

        //--------------------------------------
        //  UM_AbstractInAppClient
        //--------------------------------------

        protected override void ConnectToService(Action<SA_Result> callback) {
            AN_Billing.Connect((result) => {

                if(!result.IsInAppsAPIAvalible && !result.IsSubsAPIAvalible) {
                    var error = new SA_Error(100, "Current device does not support InApps");
                    var connectionResult = new SA_Result(error);
                    callback.Invoke(connectionResult);
                    return;
                }

                callback.Invoke(result);
            });
        }

        protected override Dictionary<string, UM_iProduct> GetServerProductsInfo() {
            var products = new Dictionary<string, UM_iProduct>();
            foreach (var product in AN_Billing.Inventory.Products) {
                UM_AndroidProduct p = new UM_AndroidProduct();
                p.Override(product);

                products.Add(p.Id, p);
            }
            return products;
        }


        protected override void ObservTransactions() {
            foreach (var purchase in AN_Billing.Inventory.Purchases) {
                var transaction = new UM_AndroidTransaction(purchase, isRestored: false);

                if (!UM_AndroidInAppTransactions.IsTransactionCompleted(transaction.Id)) {
                    UpdateTransaction(transaction);
                }
            }
        }


        //--------------------------------------
        //  UM_iInAppClient
        //--------------------------------------

        public void AddPayment(string productId) {
            var product = AN_Billing.Inventory.GetProductById(productId);
            AN_Billing.Purchase(product, (result) => {
                var transaction = new UM_AndroidTransaction(result);
                UpdateTransaction(transaction);
            });
        }

        public void FinishTransaction(UM_iTransaction transaction) {

            if(transaction.State == UM_TransactionState.Failed) {
                //noting to fninish since it's failed
                //it will not have product or transaction id
                return;
            }

            var product = AN_Billing.Inventory.GetProductById(transaction.ProductId);

            if (product != null) {
                if (product.Type == AN_ProductType.inapp && product.IsConsumable) {
                    var purchase = (transaction as UM_AndroidTransaction).Purchase;
                    if (purchase != null) {
                        AN_Billing.Consume(purchase, (result) => { });
                    }
                }
            } else {
                Debug.LogError("Transaction is finished, but no product found with such id");
            }
                

            UM_AndroidInAppTransactions.RegisterCompleteTransaction(transaction.Id);
        }


        public void RestoreCompletedTransactions() {
           foreach(var purchase in AN_Billing.Inventory.Purchases) {
                var transaction = new UM_AndroidTransaction(purchase, isRestored: true);
                UpdateTransaction(transaction);
           }
        }

      
    }
}