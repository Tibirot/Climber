using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SA.Android.Utilities;
using System;

using SA.Android.Vending.Billing;
using SA.Foundation.Templates;
using SA.Android.Vending.Licensing;

namespace SA.Android.Vending.Internal
{

    public class AN_VendingNativeAPI : AN_VendingAPI
    {


        const string AN_BILLING_CLASS = "com.stansassets.billing.core.AN_Billing";
        const string AN_LICENSE_CLASS = "com.stansassets.licensing.AN_LicenseChecker";
        public const string AN_BILLING_PROXY_ACTIVITY_CLASS = "com.stansassets.billing.core.AN_BillingProxyActivity";

        public void Connect(AN_VendingLib.AN_ConnectionRequest request, Action<AN_BillingConnectionResult> callback) {
            AN_Java.Bridge.CallStaticWithCallback<AN_BillingConnectionResult>(
                AN_BILLING_CLASS, "Connect", callback, request);

        }

        public void Purchase(AN_VendingLib.AN_PurchaseRequest request, Action<AN_BillingPurchaseResult> callback) {
            AN_Java.Bridge.CallStaticWithCallback<AN_BillingPurchaseResult>(
                AN_BILLING_CLASS, "Purchase", callback, request);
        }

        public void Consume(AN_Purchase purchase, Action<SA_Result> callback) {
            AN_Java.Bridge.CallStaticWithCallback<SA_Result>(
               AN_BILLING_CLASS, "Consume", callback, purchase);
        }

        public void GetPurchases(AN_VendingLib.AN_ConnectionRequest request, Action<AN_InventoryResult> callback) {
            AN_Java.Bridge.CallStaticWithCallback<AN_InventoryResult>(
               AN_BILLING_CLASS, "GetPurchases", callback, request);
        }

        public void CheckAccess(string base64EncodedPublicKey, Action<AN_LicenseResult> callback) {
            AN_Java.Bridge.CallStaticWithCallback<AN_LicenseResult>(
                AN_LICENSE_CLASS, "CheckAccess", callback, base64EncodedPublicKey);
        }
    }
}