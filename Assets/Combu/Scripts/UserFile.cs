using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Combu
{
	[Serializable]
	public class UserFile
	{
		public enum eShareType
		{
			Everybody,
			Nobody,
			Friends
		}
		
		long _id = 0;
		public long id { get { return _id; } }
		
		long _idAccount = 0;
		public long idAccount { get { return _idAccount; } }
		
		public string name = "";
		public string url = "";
		public eShareType sharing = eShareType.Nobody;
		
		int _views = 0;
		public int views { get { return _views; } }
		
		int _likes = 0;
		public int likes { get { return _likes; } }

		public Hashtable customData = new Hashtable();
		
		/// <summary>
		/// Initializes a new instance of the <see cref="UserFile"/> class.
		/// </summary>
		public UserFile()
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="UserFile"/> class from a JSON formatted string.
		/// </summary>
		/// <param name='jsonString'>
		/// JSON formatted string.
		/// </param>
		public UserFile (string jsonString)
		{
			FromJson(jsonString);
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="UserFile"/> class from a Hashtable.
		/// </summary>
		/// <param name='hash'>
		/// Hash.
		/// </param>
		public UserFile (Hashtable hash)
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
				if (hash.ContainsKey("IdAccount") && hash["IdAccount"] != null)
					long.TryParse(hash["IdAccount"].ToString(), out _idAccount);
				if (hash.ContainsKey("Name") && hash["Name"] != null)
					name = hash["Name"].ToString();
				if (hash.ContainsKey("Url") && hash["Url"] != null)
					url = hash["Url"].ToString();
				if (hash.ContainsKey("ShareType") && hash["ShareType"] != null)
					sharing = (eShareType)int.Parse(hash["ShareType"].ToString());
				if (hash.ContainsKey("Views") && hash["Views"] != null)
					int.TryParse(hash["Views"].ToString(), out _views);
				if (hash.ContainsKey("Likes") && hash["Likes"] != null)
					int.TryParse(hash["Likes"].ToString(), out _likes);
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
        /// Load the file file with the specified Id and callback.
        /// </summary>
        /// <param name="fileId">File identifier.</param>
        /// <param name="callback">Callback.</param>
        public static void Load(long fileId, Action<UserFile, string> callback)
        {
            var form = CombuManager.instance.CreateForm();
            form.AddField("action", "load");
            form.AddField("Id", fileId.ToString());
            CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("user_files.php"), form, (string text, string error) =>
            {
                UserFile file = null;
                if (string.IsNullOrEmpty(error))
                {
                    Hashtable result = text.hashtableFromJson();
                    if (result != null)
                    {
                        bool success = false;
                        if (result.ContainsKey("success"))
                        {
                            bool.TryParse(result["success"].ToString(), out success);
                        }
                        if (result.ContainsKey("message") && result["message"] != null)
                        {
                            if (success)
                            {
                                file = new UserFile(result["message"].ToString());
                            }
                            else
                            {
                                error = result["message"].ToString();
                            }
                        }
                    }
                }
                if (callback != null)
                    callback(file, error);
            });
        }

		/// <summary>
		/// Load the UserFiles with the specified userId, includeShared, pageNumber, countPerPage and callback.
		/// </summary>
		/// <param name="userId">User identifier.</param>
		/// <param name="includeShared">If set to <c>true</c> include shared.</param>
		/// <param name="pageNumber">Page number.</param>
		/// <param name="countPerPage">Count per page.</param>
		/// <param name="callback">Callback.</param>
		public static void Load (string userId, bool includeShared, int pageNumber, int countPerPage, Action<UserFile[], int, int, string> callback)
		{
			Load<UserFile>(userId, includeShared, pageNumber, countPerPage, callback);
		}
		/// <summary>
		/// Load the UserFiles with the specified userId, includeShared, pageNumber, countPerPage and callback.
		/// </summary>
		/// <param name="userId">User identifier.</param>
		/// <param name="includeShared">If set to <c>true</c> include shared.</param>
		/// <param name="pageNumber">Page number.</param>
		/// <param name="countPerPage">Count per page.</param>
		/// <param name="callback">Callback.</param>
		/// <typeparam name="T">Type for UserFile.</typeparam>
		public static void Load<T> (string userId, bool includeShared, int pageNumber, int countPerPage, Action<UserFile[], int, int, string> callback) where T: UserFile, new()
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "list");
			form.AddField("Id", userId.ToString());
			if (CombuManager.localUser.authenticated && CombuManager.localUser.id.Equals(userId))
				form.AddField("Shared", includeShared ? "1" : "0");
			form.AddField("Limit", countPerPage.ToString());
			form.AddField("Page", pageNumber.ToString());
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("user_files.php"), form, (string text, string error) => {
				List<T> files = new List<T>();
				int count = 0, pagesCount = 0;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null && result.ContainsKey("results"))
					{
						ArrayList list = (ArrayList)result["results"];
						count = int.Parse(result["total"].ToString());
						pagesCount = int.Parse(result["pages"].ToString());
						if (list != null)
						{
							foreach (Hashtable data in list)
							{
								// Create a new user file object from the result
								T file = new T();
								file.FromHashtable(data);
								// Add to the list
								files.Add(file);
							}
						}
					}
				}
				if (callback != null)
					callback(files.ToArray(), count, pagesCount, error);
			});
		}

		/// <summary>
		/// Update this file to server.
		/// </summary>
		/// <param name="contents">Contents of the file to send.</param>
		/// <param name="callback">Callback.</param>
		public virtual void Update (byte[] contents, Action<bool, string> callback)
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "save");
			form.AddField("Id", id.ToString());
			form.AddField("Name", name);
			if (contents != null)
				form.AddBinaryData("File", contents);
			form.AddField("ShareType", ((int)sharing).ToString());
			form.AddField("CustomData", customData.toJson());
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("user_files.php"), form, (string text, string error) => {
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
						if (!success && result.ContainsKey("message"))
							error = result["message"].ToString();
					}
				}
				if (callback != null)
					callback(success, error);
			});

		}

		/// <summary>
		/// Deletes this UserFile from server.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public virtual void Delete (Action<bool, string> callback)
		{
			Delete(id, callback);
		}

		/// <summary>
		/// Deletes the specified File from server.
		/// </summary>
		/// <param name="idFile">Identifier file.</param>
		/// <param name="callback">Callback.</param>
		public static void Delete (long idFile, Action<bool, string> callback)
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "delete");
			form.AddField("Id", idFile.ToString());
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("user_files.php"), form, (string text, string error) => {
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
		/// Increase the View count of this UserFile.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public virtual void View (Action<bool, string> callback)
		{
			View(this, 0, callback);
		}

		/// <summary>
		/// Increase the View count of a UserFile.
		/// </summary>
		/// <param name="idFile">Identifier file.</param>
		/// <param name="callback">Callback.</param>
		public static void View (long idFile, Action<bool, string> callback)
		{
			View(null, idFile, callback);
		}

		/// <summary>
		/// Increase the View count of a UserFile.
		/// </summary>
		/// <param name="file">File.</param>
		/// <param name="idFile">Identifier file (if File is null).</param>
		/// <param name="callback">Callback.</param>
		protected static void View (UserFile file, long idFile, Action<bool, string> callback)
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "view");
			if (file != null)
				form.AddField("Id", file.id.ToString());
			else
				form.AddField("Id", idFile.ToString());
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("user_files.php"), form, (string text, string error) => {
				bool success = false;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						if (result.ContainsKey("success"))
							bool.TryParse(result["success"].ToString(), out success);
						if (result.ContainsKey("message"))
						{
							if (!success)
								error = result["message"].ToString();
							else if (file != null)
								file.FromJson(result["message"].ToString());
						}
					}
				}
				if (callback != null)
					callback(success, error);
			});
		}

		/// <summary>
		/// Increase the Like count of this UserFile.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public virtual void Like (Action<bool, string> callback)
		{
			Like(this, 0, callback);
		}
		
		/// <summary>
		/// Increase the Like count of a UserFile.
		/// </summary>
		/// <param name="idFile">Identifier file.</param>
		/// <param name="callback">Callback.</param>
		public static void Like (long idFile, Action<bool, string> callback)
		{
			Like(null, idFile, callback);
		}
		
		/// <summary>
		/// Increase the Like count of a UserFile.
		/// </summary>
		/// <param name="file">File.</param>
		/// <param name="idFile">Identifier file (if File is null).</param>
		/// <param name="callback">Callback.</param>
		protected static void Like (UserFile file, long idFile, Action<bool, string> callback)
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "like");
			if (file != null)
				form.AddField("Id", file.id.ToString());
			else
				form.AddField("Id", idFile.ToString());
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("user_files.php"), form, (string text, string error) => {
				bool success = false;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						if (result.ContainsKey("success"))
							bool.TryParse(result["success"].ToString(), out success);
						if (result.ContainsKey("message"))
						{
							if (!success)
								error = result["message"].ToString();
							else if (file != null)
								file.FromJson(result["message"].ToString());
						}
					}
				}
				if (callback != null)
					callback(success, error);
			});
		}

		/// <summary>
		/// Download the bytes from the url.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public void Download (Action<byte[]> callback)
		{
			CombuManager.instance.StartCoroutine(DownloadUrl(callback));
		}

		/// <summary>
		/// Co-routine that downloads the bytes from URL.
		/// </summary>
		/// <param name="callback">Callback.</param>
		IEnumerator DownloadUrl (Action<byte[]> callback)
		{
			byte[] bytes = new byte[0];
			if (!string.IsNullOrEmpty(url))
			{
				WWW www = new WWW(url);
				yield return www;
				if (string.IsNullOrEmpty(www.error))
					bytes = www.bytes;
			}
			if (callback != null)
				callback(bytes);
		}
	}
}
