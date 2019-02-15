using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Combu;

/*
 * This script include sample code to demonstrate how to implement a simple item shop workflow
 * by using Unity UI components.
 * 
 * The examples (both scenes and scripts) are provided as-are and can be eventually modified over time to reflect major changes to API or just to be improved.
 * So we discourage the approach of editing any demo file, but you could create your own scripts and inherit their classes from these examples if you want,
 * and eventually override the methods that you want to change or add more input fields and stuff to fit your needs.
 */
public class CombuDemoShop : CombuDemoScene
{
	[System.Serializable]
	public class ShopItem
	{
		public string name;
		public int price;
	}
	
	public List<ShopItem> purchasableItems = new List<ShopItem>();
	public Text textCoins;
	public RectTransform tfmItems;
	public GameObject prefabItem;

	List<Inventory> inventoryItems = new List<Inventory>();

	protected override void OnStart ()
	{
		UpdateCoins();
		LoadItems();
	}

	protected override void OnUserLogin (bool success, string error)
	{
		base.OnUserLogin (success, error);
		if (success)
		{
			UpdateCoins();
			UpdateInventory( () => LoadItems() );
		}
	}

	void UpdateCoins ()
	{
		if (CombuManager.localUser.authenticated)
			textCoins.text = "Coins: " + (CombuManager.localUser as CombuDemoUser).coins.ToString();
		else
			textCoins.text = "";
	}

	void UpdateInventory (System.Action callback = null)
	{
		Inventory.Load(CombuManager.localUser.id, (Inventory[] items, string error) => {
			inventoryItems.Clear();
			inventoryItems.AddRange(items);
			if (callback != null)
				callback();
		});
	}

	void LoadItems ()
	{
		if (purchasableItems.Count == 0)
			return;
		if (tfmItems.childCount == 0)
		{
			float y = 0, height = prefabItem.GetComponent<RectTransform>().sizeDelta.y;
			for (int i = 0; i < purchasableItems.Count; ++i)
			{
				ShopItem item = purchasableItems[i];

				GameObject go = (GameObject)Instantiate(prefabItem);
				go.transform.SetParent(tfmItems, false);
				
				RectTransform rectTfm = go.GetComponent<RectTransform>();
				rectTfm.localPosition = Vector3.zero;
				if (tfmItems.childCount > 1)
					rectTfm.localPosition = new Vector3(0, y, 0);
				y += height;

				Text newButtonText = go.transform.GetChild(0).GetComponent<Text>();
				newButtonText.text = i + ") " + item.name + " (" + item.price + ")";

				UnityAction<BaseEventData> callback;
				callback = new UnityAction<BaseEventData>( (BaseEventData baseEvent) => {
					Text clickedText = baseEvent.selectedObject.transform.GetChild(0).GetComponent<Text>();
					int itemIndex = int.Parse(clickedText.text.Substring(0, clickedText.text.IndexOf(")")));
					BuyItem(purchasableItems[itemIndex]);
				});
				// Add an EventTrigger to load the messages of a conversation
				EventTrigger eventTrigger = go.AddComponent<EventTrigger>();
				EventTrigger.Entry entry = new EventTrigger.Entry();
				entry.eventID = EventTriggerType.PointerClick;
				entry.callback = new EventTrigger.TriggerEvent();
				entry.callback.AddListener(callback);
				eventTrigger.triggers = new List<EventTrigger.Entry>();
				eventTrigger.triggers.Add(entry);
			}

			GameObject buttonAddCoins = (GameObject)Instantiate(prefabItem);
			buttonAddCoins.transform.SetParent(tfmItems, false);
			
			RectTransform rectTfmAddCoins = buttonAddCoins.GetComponent<RectTransform>();
			rectTfmAddCoins.localPosition = Vector3.zero;
			if (tfmItems.childCount > 1)
				rectTfmAddCoins.localPosition = new Vector3(0, y, 0);
			y += height;
			
			buttonAddCoins.transform.GetChild(0).GetComponent<Text>().text = "Add 100 Coins";
			
			UnityAction<BaseEventData> callbackAddCoins = new UnityAction<BaseEventData>( (BaseEventData baseEvent) => {
				CombuDemoUser player = (CombuManager.localUser as CombuDemoUser);
				player.coins += 100;
				player.Update( (bool success, string error) => {
					UpdateCoins();
				});
			});
			// Add an EventTrigger to load the messages of a conversation
			EventTrigger eventTriggerAddCoins = buttonAddCoins.AddComponent<EventTrigger>();
			EventTrigger.Entry entryAddCoins = new EventTrigger.Entry();
			entryAddCoins.eventID = EventTriggerType.PointerClick;
			entryAddCoins.callback = new EventTrigger.TriggerEvent();
			entryAddCoins.callback.AddListener(callbackAddCoins);
			eventTriggerAddCoins.triggers = new List<EventTrigger.Entry>();
			eventTriggerAddCoins.triggers.Add(entryAddCoins);

		}
		for (int i = 0; i < purchasableItems.Count; ++i)
		{
			List<Inventory> allItems = inventoryItems.FindAll( item => item.name.Equals(purchasableItems[i].name) );
			if (allItems.Count == 0)
				continue;
			int quantity = 0;
			allItems.ForEach( item => quantity += item.quantity );
			Text newButtonText = tfmItems.GetChild(i).GetChild(0).GetComponent<Text>();
			newButtonText.text = i + ") " + purchasableItems[i].name + " (" + purchasableItems[i].price + ") x" + quantity;
		}
	}

	void BuyItem (ShopItem purchaseItem)
	{
		if (purchaseItem == null)
			return;
		if ((CombuManager.localUser as CombuDemoUser).coins < purchaseItem.price)
		{
			Debug.Log("You haven't enough coins to purchase this item");
		}
		else
		{
			// Search the item in the current inventory
			Inventory newItem = inventoryItems.Find( i => i.name.Equals(purchaseItem.name) );
			// If it doesn't exist then create it now
			if (newItem == null) {
				newItem = new Inventory();
				newItem.name = purchaseItem.name;
			}
			// Increase the quantity and save
			newItem.quantity++;
			newItem.Update( (bool success, string error) => {
				if (success)
				{
					CombuDemoUser player = (CombuManager.localUser as CombuDemoUser);
					player.coins -= purchaseItem.price;
					player.Update( (bool successUpdate, string errorUpdate) => {
						UpdateCoins();
					});
					UpdateInventory( () => {
						LoadItems();
					});
				}
				else
				{
					Debug.LogError("ERROR: " + error);
				}
			});
		}
	}
}
