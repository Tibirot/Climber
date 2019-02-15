using System.Collections;
using System.Collections.Generic;
using SA.Android.Vending.Billing;
using SA.CrossPlatform.InApp;
using SA.CrossPlatform.UI;
using SA.Foundation.Templates;
using SA.iOS.StoreKit;
using UnityEngine;


 public class UM_TransactionObserverExample : UM_iTransactionObserver {
        public void OnRestoreTransactionsComplete(SA_Result result) {
            //Restore transaction flow was completed.
            //If you've set any flags before starting the Restoration flow, this is the perfect spot to drop it.
        }

        public void OnTransactionUpdated(UM_iTransaction transaction) {
            //Transactions have been updated.
            //Let's act accordingly
            
            PrintTransactionInfo(transaction);

            switch (transaction.State) {
                case UM_TransactionState.Purchased:
                case UM_TransactionState.Restored:
                    //Our product has been successfully purchased or restored
                    //So we need to provide content to our user depends on productIdentifier

                    //Before we will provide content to the user we might want 
                    //to make sure that purchase is valid, using server-side validation

                    //let's see check the current platform
                    switch(Application.platform) {
                        case RuntimePlatform.Android:
                            //On android all the info we need is inside the ordinal android transaction
                            //So let's get one.

                            string productId = transaction.ProductId;
                            AN_Inventory inventory = AN_Billing.Inventory;
                            AN_Purchase an_purchase = inventory.GetPurchaseByProductId(productId);

                            //Now you have asses to the all data from the original goal transaction object
                            //That you can send to your server side and validate the purchase.
                            Debug.Log(an_purchase.OriginalJson);
             
                            break;
                        case RuntimePlatform.IPhonePlayer:
                            //For iOS we need to validate the AppStoreReceipt. 
                            //So we need to get it using iOS Native
                            var receipt = ISN_SKPaymentQueue.AppStoreReceipt;

                            //You can now send receipt to your server side
                            Debug.Log("Receipt loaded, byte array length: " + receipt.Data.Length);
                            Debug.Log("Receipt As Base64 String" + receipt.AsBase64String);

                            break;
                    }

                    UnlockProduct(transaction);
                    //Once product is successfully unlocked we can finish the transaction
                    UM_InAppService.Client.FinishTransaction(transaction);
                    break;
                case UM_TransactionState.Deferred:
                    //Only fir iOS
                    //iOS 8 introduces Ask to Buy, which lets parents approve any 
                    //purchases initiated by children
                    //You should update your UI to reflect this deferred state, 
                    //and expect another Transaction Complete to be called again 
                    //with a new transaction state 
                    //reflecting the parent’s decision or after the transaction times out. 
                    //Avoid blocking your UI or gameplay while waiting 
                    //for the transaction to be updated.
                    break;
                case UM_TransactionState.Failed:
                    //Our purchase flow is failed.
                    //We can unlock interface and tell user that the purchase is failed. 

                    UM_InAppService.Client.FinishTransaction(transaction);
                    break;
            }
        }


        private void PrintTransactionInfo(UM_iTransaction transaction) {
            Debug.Log("transaction.Id: " + transaction.Id);
            Debug.Log("transaction.State: " + transaction.State);
            Debug.Log("transaction.ProductId: " + transaction.ProductId);
            Debug.Log("transaction.Timestamp: " + transaction.Timestamp);

            var title = "Transaction Updated.";
            var message = string.Format("ProductId: {0}. TransactionId: {1}. State: {2}", 
                transaction.ProductId,
                transaction.Id,
                transaction.State);
            
            UM_DialogsUtility.ShowMessage(title, message);
        }

        private void UnlockProduct(UM_iTransaction transaction) {
            //Reward your used based on transaction.Id
        }
    }