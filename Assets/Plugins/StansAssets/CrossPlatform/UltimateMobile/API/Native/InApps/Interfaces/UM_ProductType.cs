using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA.CrossPlatform.InApp
{
    public enum UM_ProductType
    {

        /// <summary>
        /// A product that is used once, after which it becomes depleted and must be purchased again.
        /// Example: Fish food for a fishing app.
        /// </summary>
        Consumable,

        /// <summary>
        /// A product that is purchased once and does not expire or decrease with use.
        /// Example: Race track for a game app.
        /// </summary>
        NonConsumable,


        /// <summary>
        /// A product that allows users to purchase a service with a limited duration. 
        /// Example: Gold account upgrade.
        /// </summary>
        Subscription
    }
}