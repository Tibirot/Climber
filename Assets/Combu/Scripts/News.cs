using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Combu
{
	[System.Serializable]
	public class News
	{
		public long id = 0;
		public DateTime date = DateTime.MinValue;
		public string subject = "";
		public string message = "";
		public string url = "";
		
		public News()
		{
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
				if (hash.ContainsKey("Subject") && hash["Subject"] != null)
				{
					subject = hash["Subject"].ToString();
				}
				if (hash.ContainsKey("Message") && hash["Message"] != null)
				{
					message = hash["Message"].ToString();
				}
				if (hash.ContainsKey("PublishDate") && hash["PublishDate"] != null)
				{
					date = hash ["PublishDate"].ToString ().ToDatetimeOrDefault ();
				}
				if (hash.ContainsKey("Url") && hash["Url"] != null)
				{
					url = hash["Url"].ToString();
				}
			}
		}

		/// <summary>
		/// Load the specified pageNumber and limit of news.
		/// </summary>
		/// <param name="pageNumber">Page number.</param>
		/// <param name="limit">Limit.</param>
		/// <param name="callback">Callback.</param>
		public static void Load (int pageNumber, int limit, Action<News[], int, int, string> callback)
		{
			Load<News>(pageNumber, limit, callback);
		}
		/// <summary>
		/// Load the specified pageNumber and limit of news.
		/// </summary>
		/// <param name="pageNumber">Page number.</param>
		/// <param name="limit">Limit.</param>
		/// <param name="callback">Callback.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void Load<T> (int pageNumber, int limit, Action<News[], int, int, string> callback) where T: News, new()
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "list");
			form.AddField("Limit", limit.ToString());
			form.AddField("Page", pageNumber.ToString());
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("news.php"), form, (string text, string error) => {
				List<T> news = new List<T>();
				int count = 0, pagesCount = 0;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null && result.ContainsKey("total"))
					{
						count = int.Parse(result["total"].ToString());
						pagesCount = int.Parse(result["pages"].ToString());
						ArrayList list = (ArrayList)result["results"];
						if (list != null)
						{
							foreach (Hashtable data in list)
							{
								// Create a new object from the result
								T article = new T();
								article.FromHashtable(data);
								// Add to the list
								news.Add(article);
							}
						}	
					}
				}
				if (callback != null)
					callback(news.ToArray(), count, pagesCount, error);
			});
		}
	}
}