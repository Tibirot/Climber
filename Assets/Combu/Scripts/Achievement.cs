using UnityEngine;
using UnityEngine.SocialPlatforms;
using System.Collections;
using System.Collections.Generic;

namespace Combu
{
	/// <summary>
	/// Achievement class implementing the Unity built-in Social interfaces (IAchievement, IAchievementDescription).
	/// </summary>
	[System.Serializable]
	public class Achievement : IAchievement, IAchievementDescription, IComparer
	{
		string _title = "";
		string _description = "";
		int _finished = 0;
		int _progress = 0;
		
		public Achievement () {
		}
		public Achievement (string jsonString) {
			FromJson(jsonString);
		}
		
		public virtual void FromJson (string jsonString) {
			Hashtable hash = jsonString.hashtableFromJson();
			if (hash != null) {
				if (hash.ContainsKey("Id") && hash["Id"] != null)
					id = hash["Id"].ToString();
				if (hash.ContainsKey("Title") && hash["Title"] != null)
					_title = hash["Title"].ToString();
				if (hash.ContainsKey("Description") && hash["Description"] != null)
					_description = hash["Description"].ToString();
				if (hash.ContainsKey("Progress") && hash["Progress"] != null) {
					int.TryParse(hash["Progress"].ToString(), out _progress);
				}
				if (hash.ContainsKey("Finished") && hash["Finished"] != null) {
					int.TryParse(hash["Finished"].ToString(), out _finished);
				}
			}
		}

		#region IAchievement implementation

		public void ReportProgress (System.Action<bool> callback)
		{
			if (!CombuManager.isInitialized)
				throw new System.Exception("Combu Manager not initialized");
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "progress");
			form.AddField("IdAchievement", id);
			form.AddField("Progress", _progress.ToString());
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("achievements.php"), form, (string text, string error) => {
				bool success = false;
				if (string.IsNullOrEmpty(error)) {
					Hashtable result = text.hashtableFromJson();
					if (result != null) {
						if (result.ContainsKey("success"))
							bool.TryParse(result["success"].ToString(), out success);
						if (!success && result.ContainsKey("message"))
							error = result["message"].ToString();
					}
				}
				if (callback != null)
					callback(success);
			});
		}

		public string id { get; set; }

		public double percentCompleted {
			get { return _progress * 0.01; }
			set { _progress = Mathf.RoundToInt((float)value * 100f); }
		}

		public bool completed {
			get { return (_progress >= 100); }
		}

		public bool hidden {
			get { return false; }
		}
		public System.DateTime lastReportedDate {
			get { return System.DateTime.MinValue; }
		}
		public string title { get { return _title; } }
		public string description { get { return _description; } }
		public int finished { get { return _finished; } }

		#endregion

		#region IAchievementDescription implementation

		[System.NonSerialized]
		protected Texture2D _image = null;

		public Texture2D image {
			get { return _image; }
		}
		
		public string achievedDescription {
			get { return _title; }
		}
		
		public string unachievedDescription {
			get { return _title; }
		}
		
		public int points {
			get { return (int)_progress; }
		}
		
		#endregion

		#region IComparer implementation

		public int Compare (object x, object y)
		{
			IAchievement a = (IAchievement)x;
			IAchievement b = (IAchievement)y;
			long idA = long.Parse(a.id);
			long idB = long.Parse(b.id);
			if (idA == idB)
				return a.percentCompleted.CompareTo(b.percentCompleted);
			return idA.CompareTo(idB);
		}

		#endregion
	}
}
