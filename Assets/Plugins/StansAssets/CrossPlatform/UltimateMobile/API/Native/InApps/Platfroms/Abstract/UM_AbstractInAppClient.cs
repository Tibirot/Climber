using System;
using System.Collections.Generic;
using UnityEngine;

using SA.Foundation.Templates;
using SA.CrossPlatform.Analytics;

namespace SA.CrossPlatform.InApp
{
    public abstract class UM_AbstractInAppClient 
    {

        private bool m_isConnected = false;
        private bool m_isConnecttionInProgress = false;

        private event Action<SA_Result> m_onConnect = delegate { };

        private Dictionary<string, UM_iProduct> m_products = new Dictionary<string, UM_iProduct>();

        private bool m_isObserverRegisted = false;
        protected UM_iTransactionObserver m_observer;


        //--------------------------------------
        //  Abstract
        //--------------------------------------

        protected abstract void ConnectToService(Action<SA_Result> callback);
        protected abstract void ObservTransactions();

        /// <summary>
        /// Will update products list based on information retrived from server
        /// </summary>
        protected abstract Dictionary<string, UM_iProduct> GetServerProductsInfo();



        //--------------------------------------
        //  Public Methods
        //--------------------------------------

        public void Connect(Action<SA_Result> callback) {
            if (m_isConnected) {
                callback.Invoke(new SA_Result());
                return;
            }

            m_onConnect += callback;
            if (m_isConnecttionInProgress) { return; }
            m_isConnecttionInProgress = true;

            ConnectToService((result) => {
                if(result.IsSucceeded) {
                    m_products = GetServerProductsInfo();
                }

                m_isConnected = true;
                m_isConnecttionInProgress = false;
                m_onConnect.Invoke(result);

                //Checking if we should add an observer
                //In case user added it before service was connected
                if (m_observer != null && !m_isObserverRegisted) {
                    m_isObserverRegisted = true;
                    ObservTransactions();
                }

                m_onConnect = delegate { };
            });
        }

        public void SetTransactionObserver(UM_iTransactionObserver observer) {
            if(m_observer != null) {
                Debug.LogWarning("UM_AbstractInAppClient::SetTransactionObserver you can only set one Transactions Observer");
                return;
            }

            m_observer = observer;

            // Make sure we adding actual observer only when connecte to the service. 
            // Otherwiese we will wait for a successful connection 

            if(IsConnected) {
                m_isObserverRegisted = true;
                ObservTransactions();
            } 
        }


        /// <summary>
        /// Gets the product by identifier.
        /// </summary>
        /// <param name="productIdentifier">Prodcut identifier.</param>
        public UM_iProduct GetProductById(string productIdentifier) {
            return m_products[productIdentifier];
        }




        //--------------------------------------
        //  Get / Set
        //--------------------------------------

        public bool IsConnected {
            get {
                return m_isConnected;
            }
        }

        /// <summary>
        /// A list of products, one product for each valid product identifier provided in the original init request.
        /// only valid to use when <see cref="IsConnected"/> is <c>true</c>
        /// </summary>
        public List<UM_iProduct> Products {
            get {
                return new List<UM_iProduct>(m_products.Values);
            }
        }


        //--------------------------------------
        //  Protected Methods
        //--------------------------------------

        protected void UpdateTransaction(UM_iTransaction transaction) {
            if (m_observer == null) {
                Debug.LogError("UpdateTransaction has been called before m_observer is set");
                return;
            }

            UM_AnalyticsInternal.OnTransactionUpdated(transaction);
            m_observer.OnTransactionUpdated(transaction);
        }

        protected void SetRestoreTransactionsResult(SA_Result result) {
            if (m_observer == null) {
                Debug.LogError("SetRestoreTransactionsResult has been called before m_observer is set");
                return;
            }

            UM_AnalyticsInternal.OnRestoreTransactions();
            m_observer.OnRestoreTransactionsComplete(result);
        }


    }
}