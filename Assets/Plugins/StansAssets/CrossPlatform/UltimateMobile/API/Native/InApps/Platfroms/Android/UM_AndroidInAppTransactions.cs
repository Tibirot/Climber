using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA.CrossPlatform.InApp
{

    internal static class UM_AndroidInAppTransactions 
    {

       [Serializable]
        public class TransactionsList
        {
            public List<string> Completed = new List<string>();
        }

        const string COMPLETED_TRANSACTIONS_LIST_KEY = "COMPLETED_TRANSACTIONS_LIST_KEY";
        private static TransactionsList m_transactionsList = null;



        public static void RegisterCompleteTransaction(string id) {
            Transactions.Completed.Add(id);
            Save();
            
        }

        public static bool IsTransactionCompleted(string id) {
            return Transactions.Completed.Contains(id);
        }



        private static void Save() {
            string json = JsonUtility.ToJson(Transactions);
            PlayerPrefs.SetString(COMPLETED_TRANSACTIONS_LIST_KEY, json);
        }


        private static TransactionsList Transactions {
            get {

                if(m_transactionsList == null) {
                    if (PlayerPrefs.HasKey(COMPLETED_TRANSACTIONS_LIST_KEY)) {
                        string json = PlayerPrefs.GetString(COMPLETED_TRANSACTIONS_LIST_KEY);
                        m_transactionsList = JsonUtility.FromJson<TransactionsList>(json);
                    } else {
                        m_transactionsList = new TransactionsList();
                    }
                }
  
                return m_transactionsList;
            }
        }





    }
}