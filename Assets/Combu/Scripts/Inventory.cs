using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Combu
{
	[System.Serializable]
	public class Inventory
	{
		long _id = 0;
		public long id { get { return _id; } }
		
		public string name = "";
		public int quantity = 0;
		public Hashtable customData = new Hashtable();
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Inventory"/> class.
		/// </summary>
		public Inventory ()
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Inventory"/> class from a JSON formatted string.
		/// </summary>
		/// <param name='jsonString'>
		/// JSON formatted string.
		/// </param>
		public Inventory (string jsonString)
		{
			FromJson(jsonString);
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Inventory"/> class from a Hashtable.
		/// </summary>
		/// <param name='hash'>
		/// Hash.
		/// </param>
		public Inventory (Hashtable hash)
		{
			FromHashtable(hash);
		}
		
		/// <summary>
		/// Initialize the object from a JSON formatted string.
		/// </summary>
		/// <param name='jsonString'>
		/// Json string.
		/// </param>
		public virtual void FromJson (string jsonString)
		{
			FromHashtable(jsonString.hashtableFromJson());
		}
		
		/// <summary>
		/// Initialize the object from a hashtable.
		/// </summary>
		/// <param name='hash'>
		/// Hash.
		/// </param>
		public virtual void FromHashtable (Hashtable hash)
		{
			if (hash != null)
			{
				if (hash.ContainsKey("Id") && hash["Id"] != null)
					long.TryParse(hash["Id"].ToString(), out _id);
				if (hash.ContainsKey("Name") && hash["Name"] != null)
					name = hash["Name"].ToString();
				if (hash.ContainsKey("Quantity") && hash["Quantity"] != null)
					int.TryParse(hash["Quantity"].ToString(), out quantity);
				if (hash.ContainsKey("CustomData") && hash["CustomData"] != null)
				{
					if (hash["CustomData"] is Hashtable)
						customData = (Hashtable)hash["CustomData"];
					else
						customData = hash["CustomData"].ToString().hashtableFromJson();
					if (customData == null)
						customData = new Hashtable();
				}
			}
		}

		/// <summary>
		/// Load the inventory items of a User.
		/// </summary>
		/// <param name="userId">User identifier.</param>
		/// <param name="callback">Callback.</param>
		public static void Load (string userId, System.Action<Inventory[], string> callback)
		{
			Load<Inventory>(userId, callback);
		}
		/// <summary>
		/// Load the inventory of a User.
		/// </summary>
		/// <param name="userId">User identifier.</param>
		/// <param name="callback">Callback.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void Load<T> (string userId, System.Action<T[], string> callback) where T: Inventory, new()
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "list");
			form.AddField("Id", userId);
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("inventory.php"), form, (string text, string error) => {
				List<T> inventory = new List<T>();
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null && result.ContainsKey("results"))
					{
						ArrayList list = (ArrayList)result["results"];
						if (list != null)
						{
							foreach (Hashtable data in list)
							{
								// Create a new inventory object from the result
								T item = new T();
								item.FromHashtable(data);
								// Add to the list
								inventory.Add(item);
							}
						}
					}
				}
				if (callback != null)
					callback(inventory.ToArray(), error);
			});
		}

		/// <summary>
		/// Update this inventory item to server.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public virtual void Update (System.Action<bool, string> callback)
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "save");
			form.AddField("Id", id.ToString());
			form.AddField("Name", name);
			form.AddField("Quantity", quantity.ToString());
			form.AddField("CustomData", customData.toJson());
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("inventory.php"), form, (string text, string error) => {
				bool success = false;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						if (result.ContainsKey("success"))
							bool.TryParse(result["success"].ToString(), out success);
						if (!success && result.ContainsKey("message"))
							error = result["message"].ToString();
						else if (success)
							FromJson(result["message"].ToString());
					}
				}
				if (callback != null)
					callback(success, error);
			});
		}

		/// <summary>
		/// Delete this inventory item from server.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public virtual void Delete (System.Action<bool, string> callback)
		{
			Delete(id, callback);
		}

		/// <summary>
		/// Delete the specified inventory item from server.
		/// </summary>
		/// <param name="idInventory">Identifier inventory.</param>
		/// <param name="callback">Callback.</param>
		public static void Delete (long idInventory, System.Action<bool, string> callback)
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "delete");
			form.AddField("Id", idInventory.ToString());
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("inventory.php"), form, (string text, string error) => {
				bool success = false;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						if (result.ContainsKey("success"))
							bool.TryParse(result["success"].ToString(), out success);
						if (!success && result.ContainsKey("message"))
							error = result["message"].ToString();
					}
				}
				if (callback != null)
					callback(success, error);
			});
		}
	}
}