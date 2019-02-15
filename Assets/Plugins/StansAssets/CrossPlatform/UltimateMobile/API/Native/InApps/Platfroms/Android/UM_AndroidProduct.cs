using System;
using System.Collections.Generic;
using UnityEngine;
using SA.Android.Vending.Billing;

namespace SA.CrossPlatform.InApp
{

    [Serializable]
    public class UM_AndroidProduct : UM_AbstractProduct<AN_Product>, UM_iProduct
    {

        public override void OnOverride(AN_Product productTemplate) {
            m_id = productTemplate.ProductId;
            m_price = productTemplate.Price;
            m_priceInMicros = productTemplate.PriceAmountMicros;
            m_priceCurrencyCode = productTemplate.PriceCurrencyCode;

            m_title = productTemplate.Title;
            m_description = productTemplate.Description;
        }
    }
}