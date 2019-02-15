using System;
using System.Collections.Generic;
using UnityEngine;

using SA.Foundation.Templates;


namespace SA.CrossPlatform.InApp
{

    /// <summary>
    /// Communicates with the Billing service
    /// and presents a user interface so that the user can authorize payment. 
    /// </summary>
    public interface UM_iInAppClient 
    {

        /// <summary>
        /// Initializes the billing service with the set of previously defined products.
        /// Once the connection is successfully established, 
        /// <see cref="Products"/> list will be update with the information received from a payment service.
        /// </summary>
        /// <param name="callback">Operstion callback.</param>
        void Connect(Action<SA_Result> callback);


        /// <summary>
        /// Adds a payment request to the queue.
        /// 
        /// An application should always have at least one observer of the payment queue before adding payment requests.
        /// The payment request must have a product identifier registered with the platform payment service.
        /// 
        /// When a payment request is added to the queue, 
        /// the payment queue processes that request with the Apple App Store 
        /// and arranges for payment from the user. When that transaction is complete or if a failure occurs, 
        /// the payment queue sends the <see cref="UM_iTransactionObserver"/> object that encapsulates the request 
        /// to all transaction observers.
        /// </summary>
        /// <param name="productId">Product identifier.</param>
        void AddPayment(string productId);


        /// <summary>
        /// Asks the payment queue to restore previously completed purchases.
        /// 
        /// our application calls this method to restore transactions that were previously finished 
        /// so that you can process them again. 
        /// For example, your application would use this to allow a user to unlock previously purchased content 
        /// onto a new device.
        /// </summary>
        void RestoreCompletedTransactions();


        /// <summary>
        /// Completes a pending transaction.
        /// 
        /// Your application should call this method from a transaction observer 
        /// that received a notification from the payment queue. 
        /// Calling <see cref="FinishTransaction"/> on a transaction removes it from the queue. 
        /// Your application should call <see cref="FinishTransaction"/> only after 
        /// it has successfully processed the transaction and unlocked the functionality purchased by the user.
        /// </summary>
        /// <param name="transaction">transaction to finish</param>
        void FinishTransaction(UM_iTransaction transaction);

        /// <summary>
        /// Sets an observer to the payment queue.
        /// 
        /// Your application should add an observer to the payment queue during application initialization. 
        /// If there are no observers attached to the queue, the payment queue does not synchronize its list 
        /// of pending transactions with the payment servicee, 
        /// because there is no observer to respond to updated transactions.
        /// 
        /// If an application quits when transactions are still being processed, 
        /// those transactions are not lost. The next time the application launches, 
        /// the payment queue will resume processing the transactions. 
        /// Your application should always expect to be notified of completed transactions.
        /// </summary>
        /// <param name="observer">The observer to add to the queue.</param>
        void SetTransactionObserver(UM_iTransactionObserver observer);

        /// <summary>
        /// Gets the product by identifier.
        /// </summary>
        /// <param name="productIdentifier">Prodcut identifier.</param>
        UM_iProduct GetProductById(string productIdentifier);


        /// <summary>
        /// An avalobale product list for yor app. When you not connected to the payment seervice, 
        /// see the <see cref="IsConnected"/> property, product list will contain the products info you set before
        /// the connection request (using the plugin Editor UI).
        /// 
        /// Once you conneted to the service, products info will be updated according to the data received from 
        /// a payment service.
        /// </summary>
        List<UM_iProduct> Products { get; }

        bool IsConnected { get; }
    }
}