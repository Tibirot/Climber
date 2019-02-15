using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Combu
{
	[System.Serializable]
	public class UserGroup
	{
		public long id = 0;
		public string name;
		public long idOwner;
		public User owner;
		public Hashtable customData = new Hashtable();
		public User[] users = new User[0];
		
		/// <summary>
		/// Initializes a new instance of the <see cref="UserGroup"/> class.
		/// </summary>
		public UserGroup()
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="UserGroup"/> class.
		/// </summary>
		/// <param name="jsonString">Json string.</param>
		public UserGroup (string jsonString)
		{
			FromJson(jsonString);
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="UserGroup"/> class.
		/// </summary>
		/// <param name="hash">Hash.</param>
		public UserGroup (Hashtable hash)
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
				{
					long.TryParse(hash["Id"].ToString(), out id);
				}
				if (hash.ContainsKey("Name") && hash["Name"] != null)
				{
					name = hash["Name"].ToString();
				}
				if (hash.ContainsKey("IdOwner") && hash["IdOwner"] != null)
				{
					long.TryParse(hash["IdOwner"].ToString(), out idOwner);
				}
				if (hash.ContainsKey("Owner") && hash["Owner"] != null)
				{
					owner = new User(hash["Owner"].ToString());
				}
				else
				{
					owner = null;
				}
				if (hash.ContainsKey("CustomData") && hash["CustomData"] != null)
				{
					if (hash["CustomData"] is Hashtable)
						customData = (Hashtable)hash["CustomData"];
					else
						customData = hash["CustomData"].ToString().hashtableFromJson();
					if (customData == null)
						customData = new Hashtable();
				}
				if (hash.ContainsKey("Users") && hash["Users"] != null)
				{
					List<User> listUsers = new List<User>();
					ArrayList list = (hash["Users"] is ArrayList ? (ArrayList)hash["Users"] : hash["Users"].ToString().arrayListFromJson());
					foreach (Hashtable userData in list)
					{
						listUsers.Add(new User(userData));
					}
					users = listUsers.ToArray();
				}
			}
		}

		public static void Load (long idOwner, System.Action<UserGroup[], string> callback)
		{
			_Load (idOwner: idOwner, callback: callback);
		}
		public static void Load (string usernameOwner, System.Action<UserGroup[], string> callback)
		{
			_Load (usernameOwner: usernameOwner, callback: callback);
		}
		public static void LoadMembership (long idMember, System.Action<UserGroup[], string> callback)
		{
			_Load (idMember: idMember, callback: callback);
		}
		public static void LoadMembership (string usernameMember, System.Action<UserGroup[], string> callback)
		{
			_Load (usernameMember: usernameMember, callback: callback);
		}
		public static void Load (string groupName, int pageNumber, int limit, System.Action<UserGroup[], int, int, string> callback)
		{
			_Load (groupName:groupName, pageNumber: pageNumber, limit: limit, callbackPaginated: callback);
		}
		static void _Load (
			string groupName = "",
			long idOwner = 0, string usernameOwner = "",
			long idMember = 0, string usernameMember = "",
			System.Action<UserGroup[], string> callback = null,
			int pageNumber = -1, int limit = -1,
			System.Action<UserGroup[], int, int, string> callbackPaginated = null)
		{
			// Call the webservice
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "list");
			if (idOwner > 0)
				form.AddField("IdOwner", idOwner.ToString());
			else
				form.AddField("UsernameOwner", usernameOwner);
			if (idMember > 0)
				form.AddField("IdMember", idMember.ToString());
			else
				form.AddField("UsernameMember", usernameMember);
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("groups.php"), form, (string text, string error) => {
				List<UserGroup> groups = new List<UserGroup>();
				int count = 0, pagesCount = 0;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						if (result.ContainsKey("results"))
						{
							count = int.Parse(result["total"].ToString());
							pagesCount = int.Parse(result["pages"].ToString());
							ArrayList list = (ArrayList)result["results"];
							foreach (Hashtable hash in list)
							{
								groups.Add(new UserGroup(hash));
							}
						}
					}
				}
				if (callback != null)
					callback(groups.ToArray(), error);
				if (callbackPaginated != null)
					callbackPaginated(groups.ToArray(), count, pagesCount, error);
			});
		}

		/// <summary>
		/// Save this instance.
		/// </summary>
		public virtual void Save (System.Action<bool, string> callback)
		{
			// Build the list of members
			string userIds = "", userNames = "";
			foreach (User user in users) {
				if (long.Parse(user.id) > 0)
					userIds += (userIds == "" ? "" : ",") + user.id;
				else
					userNames += (userNames == "" ? "" : ",") + user.userName;
			}
			// Call the webservice
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "save");
			form.AddField("Id", id.ToString());
			form.AddField("Name", name);
			form.AddField("CustomData", customData.toJson());
			if (!string.IsNullOrEmpty(userIds))
				form.AddField("IdUser", userIds);
			else
				form.AddField("Username", userNames);
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("groups.php"), form, (string text, string error) => {
				bool success = false;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						if (result.ContainsKey("success"))
							bool.TryParse(result["success"].ToString(), out success);
						if (success)
							FromJson(result["message"].ToString());
						else if (result.ContainsKey("message"))
							error = result["message"].ToString();
					}
				}
				if (callback != null)
					callback(success, error);
			});
		}
		
		/// <summary>
		/// Delete this instance.
		/// </summary>
		public virtual void Delete (System.Action<bool, string> callback)
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "delete");
			form.AddField("Id", id.ToString());
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("groups.php"), form, (string text, string error) => {
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

		/// <summary>
		/// Join the local user to this group.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public virtual void Join (System.Action<bool, string> callback)
		{
			if (!CombuManager.localUser.authenticated)
			{
				if (callback != null)
					callback(false, "User is not authenticated");
				return;
			}
			Join<UserGroup>(new long[] {CombuManager.localUser.idLong}, null, callback);
		}
		/// <summary>
		/// Join the specified idUsers to this group.
		/// </summary>
		/// <param name="idUsers">Identifier users.</param>
		/// <param name="callback">Callback.</param>
		public virtual void Join (long[] idUsers, System.Action<bool, string> callback)
		{
			Join<UserGroup>(idUsers, null, callback);
		}
		/// <summary>
		/// Join the specified usernames to this group.
		/// </summary>
		/// <param name="usernames">Usernames.</param>
		/// <param name="callback">Callback.</param>
		public virtual void Join (string[] usernames, System.Action<bool, string> callback)
		{
			Join<UserGroup>(null, usernames, callback);
		}
		/// <summary>
		/// Join the specified idUsers/usernames to this group.
		/// </summary>
		/// <param name="idUsers">Identifier users.</param>
		/// <param name="usernames">Usernames.</param>
		/// <param name="callback">Callback.</param>
		/// <typeparam name="T">Type for UserGroup records.</typeparam>
		protected virtual void Join<T> (long[] idUsers, string[] usernames, System.Action<bool, string> callback) where T: UserGroup, new()
		{
			// Build the list of members
			string userIds = "", userNames = "";
			if (idUsers != null)
			{
				foreach (long userId in idUsers)
				{
					if (userId > 0)
						userIds += (userIds == "" ? "" : ",") + userId;
				}
			}
			if (usernames != null)
			{
				foreach (string username in usernames)
				{
					if (!string.IsNullOrEmpty(username))
						userNames += (userNames == "" ? "" : ",") + username;
				}
			}
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "join");
			form.AddField("Id", id.ToString());
			if (!string.IsNullOrEmpty(userIds))
				form.AddField("IdUser", userIds);
			else
				form.AddField("Username", userNames);
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("groups.php"), form, (string text, string error) => {
				bool success = false;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						if (result.ContainsKey("success"))
							bool.TryParse(result["success"].ToString(), out success);
						if (success)
						{
							//fromJson(result["message"].ToString());
						}
						else if (result.ContainsKey("message"))
						{
							error = result["message"].ToString();
						}
					}
				}
				if (callback != null)
					callback(success, error);
			});
		}

		/// <summary>
		/// Leave the local user from this group.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public virtual void Leave (System.Action<bool, string> callback)
		{
			if (!CombuManager.localUser.authenticated)
			{
				if (callback != null)
					callback(false, "User is not authenticated");
				return;
			}
			Leave<UserGroup>(new long[] {CombuManager.localUser.idLong}, null, callback);
		}

		/// <summary>
		/// Leave the specified idUsers from this group.
		/// </summary>
		/// <param name="idUsers">Identifier users.</param>
		/// <param name="callback">Callback.</param>
		public virtual void Leave (long[] idUsers, System.Action<bool, string> callback)
		{
			Leave<UserGroup>(idUsers, null, callback);
		}
		/// <summary>
		/// Leave the specified usernames from this group.
		/// </summary>
		/// <param name="usernames">Usernames.</param>
		/// <param name="callback">Callback.</param>
		public virtual void Leave (string[] usernames, System.Action<bool, string> callback)
		{
			Leave<UserGroup>(null, usernames, callback);
		}
		/// <summary>
		/// Leave the specified idUsers/usernames from this group.
		/// </summary>
		/// <param name="idUsers">Identifier users.</param>
		/// <param name="usernames">Usernames.</param>
		/// <param name="callback">Callback.</param>
		/// <typeparam name="T">Type for UserGroup records.</typeparam>
		protected virtual void Leave<T> (long[] idUsers, string[] usernames, System.Action<bool, string> callback) where T: UserGroup, new()
		{
			// Build the list of members
			string userIds = "", userNames = "";
			if (idUsers != null)
			{
				foreach (long userId in idUsers)
				{
					if (userId > 0)
						userIds += (userIds == "" ? "" : ",") + userId;
				}
			}
			if (usernames != null)
			{
				foreach (string username in usernames)
				{
					if (!string.IsNullOrEmpty(username))
						userNames += (userNames == "" ? "" : ",") + username;
				}
			}
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "leave");
			form.AddField("Id", id.ToString());
			if (!string.IsNullOrEmpty(userIds))
				form.AddField("IdUser", userIds);
			else
				form.AddField("Username", userNames);
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("groups.php"), form, (string text, string error) => {
				bool success = false;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						if (result.ContainsKey("success"))
							bool.TryParse(result["success"].ToString(), out success);
						if (success)
						{
							//fromJson(result["message"].ToString());
						}
						else if (result.ContainsKey("message"))
						{
							error = result["message"].ToString();
						}
					}
				}
				if (callback != null)
					callback(success, error);
			});
		}
	}
}