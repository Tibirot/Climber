using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA.CrossPlatform.InApp
{
    public interface UM_iProduct 
    {

        /// <summary>
        /// The string that identifies the product to the payment service.
        /// </summary>
        string Id { get; }


        /// <summary>
        /// The locale used to format the price of the product.
        /// </summary>
        string Price { get; }

        /// <summary>
        /// Gets the price in micros.
        /// </summary>
        long PriceInMicros { get; }

        /// <summary>
        /// The name of the product.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// A description of the product.
        /// </summary>
        string Description { get; }


        /// <summary>
        /// The currency code for the locale.
        /// Example currency codes include "USD", "EUR", and "JPY".
        /// </summary>
        string PriceCurrencyCode { get; }

        /// <summary>
        /// Gets icon of the product.
        /// Please not, that this is an icon that was set using the Editor UI for the product.
        /// </summary>
        Texture2D Icon { get; }

        /// <summary>
        /// Type of the product.
        /// </summary>
        UM_ProductType Type { get; }

        /// <summary>
        /// Inidcates if products if product was recognized by the payment service.
        /// Value is always <c>false</c> before you connect to the service.
        /// If after connection to the payment service, property value remains <c>false</c>,
        /// it means that this product wasn't recognized by a payment service and not avaliabe for purchase.
        /// </summary>
        bool IsActive { get; }
    }
}