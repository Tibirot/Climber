/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEngine;

#if UNITY_PURCHASING
using UnityEngine.Purchasing;
#endif

namespace SIS
{
    /// <summary>
    /// Script that listens to purchases and other IAP events:
    /// here we tell our game what to do when these events happen.
    /// <summary>
    public class IAPListener : MonoBehaviour
    {
        //subscribe to the most important IAP events
        private void OnEnable()
        {
            IAPManager.purchaseSucceededEvent += HandleSuccessfulPurchase;
            IAPManager.purchaseFailedEvent += HandleFailedPurchase;
            ShopManager.itemSelectedEvent += HandleSelectedItem;
            ShopManager.itemDeselectedEvent += HandleDeselectedItem;
        }


        private void OnDisable()
        {
			IAPManager.purchaseSucceededEvent -= HandleSuccessfulPurchase;
			IAPManager.purchaseFailedEvent -= HandleFailedPurchase;
			ShopManager.itemSelectedEvent -= HandleSelectedItem;
			ShopManager.itemDeselectedEvent -= HandleDeselectedItem;
        }


        /// <summary>
        /// Handle the completion of purchases, be it for products or virtual currency.
        /// Most of the IAP logic is handled internally already, such as adding products or currency to the inventory.
        /// However, this is the spot for you to implement your custom game logic for instantiating in-game products etc.
        /// </summary>
        public void HandleSuccessfulPurchase(string id)
        {
            if (IAPManager.isDebug) Debug.Log("IAPListener reports: HandleSuccessfulPurchase: " + id);

            //differ between ids set in the IAP Settings editor
            switch (id)
            {
                //section for in app purchases
                case "coins":
                    //the user bought the item "coins", show appropriate feedback
                    ShowMessage("1000 coins were added to your balance!");
                    break;
                case "coin_pack":
                    ShowMessage("2500 coins were added to your balance!");
                    break;
                case "big_coin_pack":
                    ShowMessage("6000 coins were added to your balance!");
                    break;
                case "huge_coin_pack":
                    ShowMessage("12000 coins were added to your balance!");
                    break;
                case "no_ads":
                    //no_ads purchased. You can now check DBManager.isPurchased("no_ads")
                    //before showing ads and block them
                    ShowMessage("Ads disabled!");
                    break;
                case "abo_monthly":
                    //same here - your code to unlock subscription content
                    ShowMessage("Subscribed to monthly abo!");
                    break;
                case "restore":
                    //nothing else to call here,
                    //the actual restore is handled by IAPManager
                    ShowMessage("Restored transactions!");
                    break;

                //section for in game content
                case "bullets":
                    //if you define a usage count in the IAP Settings editor, then the amount
                    //has been added to your inventory already. No need to call something like
                    //DBManager.IncreasePlayerData("bullets", new SimpleJSON.JSONData(bullets + 100));
                    ShowMessage("Bullets were added to your inventory!");
                    break;
                case "health":
                    ShowMessage("Medikits were added to your inventory!");
                    break;
                case "energy":
                    ShowMessage("Energy was added to your inventory!");
                    break;
                case "speed":
                    ShowMessage("Speed boost unlocked!");
                    break;
                case "speed_1":
                case "speed_2":
                case "speed_3":
                    ShowMessage("Speed boost upgraded!");
                    break;
                case "bonus":
                    ShowMessage("Bonus level unlocked!");
                    break;

                case "uzi":
                    ShowMessage("Uzi unlocked!");
                    break;
                case "ak47":
                    ShowMessage("AK47 unlocked!");
                    break;
                case "m4":
                    ShowMessage("M4 unlocked!");
                    break;

                case "hat":
                    ShowMessage("Hat unlocked!");
                    break;
                case "backpack":
                    ShowMessage("Backpack unlocked!");
                    break;
                case "belt":
                    ShowMessage("Ammo belt unlocked!");
                    break;
                case "jetpack":
                    ShowMessage("Jetpack unlocked!");
                    break;
                case "booster":
                    ShowMessage("Double XP unlocked!");
                    break;
            }
        }

        //just shows a message via our ShopManager component,
        //but checks for an instance of it first
        void ShowMessage(string text)
        {
            if (ShopManager.GetInstance())
                ShopManager.ShowMessage(text);
        }

        //called when an purchaseFailedEvent happens,
        //we do the same here
        void HandleFailedPurchase(string error)
        {
            if (ShopManager.GetInstance())
                ShopManager.ShowMessage(error);
        }


        //called when a purchased shop item gets selected
        void HandleSelectedItem(string id)
        {
            if (IAPManager.isDebug) Debug.Log("Selected: " + id);
        }


        //called when a selected shop item gets deselected
        void HandleDeselectedItem(string id)
        {
            if (IAPManager.isDebug) Debug.Log("Deselected: " + id);
        }
    }
}