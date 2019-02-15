using System;
using System.Collections.Generic;
using UnityEngine;


using SA.Android.Vending.Billing;
using SA.Android.Vending.Licensing;

using SA.Foundation.Templates;


namespace SA.Android.Vending.Internal
{
    public interface AN_VendingAPI
    {

        void Connect(AN_VendingLib.AN_ConnectionRequest request, Action<AN_BillingConnectionResult> callback);
        void GetPurchases(AN_VendingLib.AN_ConnectionRequest request, Action<AN_InventoryResult> callback);
        void Purchase(AN_VendingLib.AN_PurchaseRequest request, Action<AN_BillingPurchaseResult> callback);
        void Consume(AN_Purchase purchase, Action<SA_Result> callback);

        void CheckAccess(string base64EncodedPublicKey, Action<AN_LicenseResult> callback);

    }
}