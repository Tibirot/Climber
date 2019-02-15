using System;
using System.Collections.Generic;

using SA.iOS;
using SA.Android;

using SA.Foundation.Templates;


namespace SA.CrossPlatform.InApp
{

    internal class UM_EditorInAppClient : UM_AbstractInAppClient, UM_iInAppClient
    {


        //--------------------------------------
        //  UM_AbstractInAppClient
        //--------------------------------------

        protected override void ConnectToService(Action<SA_Result> callback) {
            UM_EditorAPIEmulator.WaitForNetwork(() => {
                callback.Invoke(new SA_Result());
            });
        }

        protected override Dictionary<string, UM_iProduct> GetServerProductsInfo() {

            var products = new Dictionary<string, UM_iProduct>();
#if UNITY_EDITOR

            switch (UnityEditor.EditorUserBuildSettings.activeBuildTarget) {
                case UnityEditor.BuildTarget.Android:
                    foreach (var product in AN_Settings.Instance.InAppProducts) {
                        UM_AndroidProduct p = new UM_AndroidProduct();
                        p.Override(product);
                        products.Add(p.Id, p);
                    }
                    break;
                case UnityEditor.BuildTarget.iOS:
                    foreach (var product in ISN_Settings.Instance.InAppProducts) {
                        UM_IOSProduct p = new UM_IOSProduct();
                        p.Override(product);
                        products.Add(p.Id, p);
                    }
                    break;
            }
#endif

            return products;
        }

        protected override void ObservTransactions() {
            
        }

        //--------------------------------------
        //  UM_iInAppClient
        //--------------------------------------

        public void AddPayment(string productId) {

            UM_EditorAPIEmulator.WaitForNetwork(() => {
                var transaction = new UM_EditorTransaction(productId, UM_TransactionState.Purchased);
                UpdateTransaction(transaction);
            });
        }

        public void FinishTransaction(UM_iTransaction transaction) {
           //Do nothing in editor
        }

        public void RestoreCompletedTransactions() {
           if(UM_Settings.Instance.TestRestoreProducts.Count > 0) {
                foreach(var productsId in UM_Settings.Instance.TestRestoreProducts) {
                    var product = GetProductById(productsId);
                    if(product != null && product.Type == UM_ProductType.NonConsumable) {
                        var transaction = new UM_EditorTransaction(product.Id, UM_TransactionState.Restored);
                        UpdateTransaction(transaction);
                    }
                }
           } else {
                foreach(var product in Products) {
                    if(product.Type == UM_ProductType.NonConsumable) {
                        var transaction = new UM_EditorTransaction(product.Id, UM_TransactionState.Restored);
                        UpdateTransaction(transaction);
                    }
                }
           }
        }

    }
}