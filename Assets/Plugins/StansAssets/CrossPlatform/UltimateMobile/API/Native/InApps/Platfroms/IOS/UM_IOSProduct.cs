using System;
using System.Collections.Generic;
using UnityEngine;

using SA.iOS.StoreKit;

namespace SA.CrossPlatform.InApp
{
    [Serializable]
    public class UM_IOSProduct : UM_AbstractProduct<ISN_SKProduct>, UM_iProduct
    {

        public override void OnOverride(ISN_SKProduct productTemplate) {
            m_id = productTemplate.ProductIdentifier;
            m_price = productTemplate.LocalizedPrice;
            m_priceInMicros = productTemplate.PriceInMicros;
            m_priceCurrencyCode = productTemplate.PriceLocale.CurrencyCode;
            m_title = productTemplate.LocalizedTitle;
            m_description = productTemplate.LocalizedDescription;
        }

    }
}