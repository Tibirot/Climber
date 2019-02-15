using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Combu
{
	/// <summary>
	/// Tournaments class.
	/// </summary>
	[Serializable]
	public class Tournament
	{
		public long id = 0;
		public long idOwner = 0;
		public string title = "";
		public DateTime dateCreation = DateTime.Now;
		public DateTime? dateFinished = null;
		public Hashtable customData = new Hashtable();

		Profile _owner = null;
		public Profile owner { get { return _owner; } }

		List<Match> _matches = new List<Match>();
		public List<Match> matches { get { return _matches; } }

		bool _finished = false;
		public bool finished { get { return _finished; } }
		
		public Tournament ()
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Combu.Tournament"/> class.
		/// </summary>
		/// <param name="jsonString">JSON string to initialize the instance.</param>
		public Tournament (string jsonString)
		{
			FromJson(jsonString);
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Combu.Tournament"/> class.
		/// </summary>
		/// <param name="data">Data to initialize the instance.</param>
		public Tournament (Hashtable data)
		{
			FromHashtable(data);
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
				if (hash.ContainsKey("IdOwner") && hash["IdOwner"] != null)
				{
					long.TryParse(hash["IdOwner"].ToString(), out idOwner);
				}
				if (hash.ContainsKey("Title") && hash["Title"] != null)
				{
					title = hash["Title"].ToString();
				}
				if (hash.ContainsKey("DateCreation") && hash["DateCreation"] != null && !string.IsNullOrEmpty(hash["DateCreation"].ToString()))
				{
					dateCreation = hash ["DateCreation"].ToString ().ToDatetimeOrDefault ();
				}
				if (hash.ContainsKey("Finished") && hash["Finished"] != null)
				{
					int f = 0;
					if (int.TryParse(hash["Finished"].ToString(), out f))
						_finished = f.Equals(1);
				}
				if (hash.ContainsKey("DateFinished") && hash["DateFinished"] != null && !string.IsNullOrEmpty(hash["DateFinished"].ToString()))
				{
					dateFinished = hash ["DateFinished"].ToString ().ToDatetime ();
				}
				if (hash.ContainsKey("CustomData") && hash["CustomData"] != null && hash["CustomData"] is Hashtable)
				{
					customData = (Hashtable)hash["CustomData"];
				}
				if (hash.ContainsKey("Owner") && hash["Owner"] != null)
				{
					Profile myUser = new Profile(hash["Owner"].ToString());
					if (myUser.idLong > 0)
						_owner = myUser;
				}
				if (hash.ContainsKey("Matches") && hash["Matches"] != null)
				{
					_matches.Clear();
					ArrayList listMatches = (ArrayList)hash["Matches"];
					foreach (Hashtable matchData in listMatches)
					{
						_matches.Add(new Match(matchData));
					}
				}
			}
		}

		/// <summary>
		/// Save this instance in the database.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public virtual void Save (Action<bool, string> callback)
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "tournament_save");
			form.AddField("Id", id.ToString());
			form.AddField("Title", title);
			form.AddField("CustomData", customData.toJson());
			
			ArrayList listMatches = new ArrayList();
			foreach (Match match in _matches)
			{
				if (match.id < 1)
				{
					Hashtable matchData = new Hashtable();
					matchData["Title"] = match.title;
					matchData["Rounds"] = match.roundsCount;
					if (match.dateExpire.HasValue && match.dateExpire != null)
					{
						matchData["DateExpire"] = ((System.DateTime)match.dateExpire).ToString("yyyy-MM-dd HH:mm:ss");
					}
					matchData["CustomData"] = match.customData.toJson();
					ArrayList listUsers = new ArrayList();
					foreach (MatchAccount user in match.users)
					{
						listUsers.Add(user.idAccount);
					}
					matchData["Users"] = listUsers;
					listMatches.Add(matchData);
				}
			}
			form.AddField("Matches", listMatches.toJson());
			
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("match.php"), form, (string text, string error) => {
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
							if (success)
								FromJson(result["message"].ToString());
							else
								error = result["message"].ToString();
						}
					}
				}
				if (callback != null)
					callback(success, error);
			});
		}
		
		/// <summary>
		/// Delete this instance from the database.
		/// </summary>
		public virtual void Delete (Action<bool, string> callback)
		{
			Delete(id, callback);
		}

		/// <summary>
		/// Delete the specified Tournament.
		/// </summary>
		/// <param name="idTournament">Identifier tournament.</param>
		/// <param name="callback">Callback.</param>
		public static void Delete (long idTournament, Action<bool, string> callback)
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "tournament_delete");
			form.AddField("Id", idTournament.ToString());
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("match.php"), form, (string text, string error) => {
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
		/// Load the list of tournaments with the specified filters.
		/// </summary>
		/// <param name="finished">If set to <c>true</c> then it returns also the finished tournaments.</param>
		/// <param name="customData">Custom data.</param>
		/// <param name="callback">Callback.</param>
		public static void Load (bool finished, SearchCustomData[] customData, System.Action<Tournament[]> callback)
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "tournament_list");
			form.AddField("Finished", finished ? "1" : "0");
			
			// Build the JSON string for CustomData filtering
			string searchCustomData = "";
			if (customData != null)
			{
				ArrayList list = new ArrayList();
				foreach (SearchCustomData data in customData)
				{
					if (string.IsNullOrEmpty(data.key))
						continue;
					Hashtable search = new Hashtable();
					search.Add("key", data.key);
					search.Add("value", data.value);
					switch (data.op) {
					case eSearchOperator.Equals:
						search.Add("op", "=");
						break;
					case eSearchOperator.Disequals:
						search.Add("op", "!");
						break;
					case eSearchOperator.Like:
						search.Add("op", "%");
						break;
					case eSearchOperator.Greater:
						search.Add("op", ">");
						break;
					case eSearchOperator.GreaterOrEquals:
						search.Add("op", ">=");
						break;
					case eSearchOperator.Lower:
						search.Add("op", "<");
						break;
					case eSearchOperator.LowerOrEquals:
						search.Add("op", "<=");
						break;
					}
					list.Add(search);
				}
				if (list.Count > 0)
					searchCustomData = list.toJson();
			}
			form.AddField("CustomData", searchCustomData);

			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("match.php"), form, (string text, string error) => {
				if (callback != null)
				{
					List<Tournament> listTournaments = new List<Tournament>();
					Hashtable result = text.hashtableFromJson();
					if (result != null && result.ContainsKey("results"))
					{
						ArrayList list = (ArrayList)result["results"];
						if (list != null)
						{
							foreach (Hashtable data in list)
							{
								// Create a new object from the result
								Tournament tournament = new Tournament(data);
								// Add to the list
								listTournaments.Add(tournament);
							}
						}	
					}
					callback(listTournaments.ToArray());
				}
			});
		}

		/// <summary>
		/// Load the tournament with the specified id.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="callback">Callback.</param>
		public static void Load (long id, System.Action<Tournament> callback)
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "tournament_load");
			form.AddField("Id", id.ToString());
			
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("match.php"), form, (string text, string error) => {
				if (callback != null)
				{
					Tournament t = null;
					Hashtable result = text.hashtableFromJson();
					if (result != null && result.ContainsKey("success") && bool.Parse(result["success"].ToString()))
					{
						t = new Tournament(result["message"].ToString());
					}
					callback(t);
				}
			});
		}

		public void Leave (Action<bool, string> callback)
		{
			Leave(id, CombuManager.localUser.idLong, callback);
		}

		public static void Leave (long idTournament, long idUser, Action<bool, string> callback)
		{
		}
		static void Leave (Tournament tournament, long idTournament, long idUser, Action<bool, string> callback)
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "tournament_leave");
			if (tournament != null)
				form.AddField("Id", tournament.id.ToString());
			else
				form.AddField("Id", idTournament.ToString());
			form.AddField("User", idUser.ToString());
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("match.php"), form, (string text, string error) => {
				bool success = false;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null && result.ContainsKey("success"))
					{
						bool.TryParse(result["success"].ToString(), out success);
						if (success)
						{
							if (tournament != null)
								tournament.FromJson(result["message"].ToString());
						}
						else
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
		/// Creates a quick tournament. If the number of other users is 1 then it creates 3 rounds, else 2 rounds for each enemy.
		/// </summary>
		/// <returns>The tournament.</returns>
		/// <param name="users">The list of other users (excluding localUser).</param>
		public static Tournament QuickTournament (Profile[] users)
		{
			Tournament t = new Tournament();

			// Create the matches
			Match match;
			if (users.Length == 1)
			{
				// It's 1v1
				match = new Match();
				match.AddUser(CombuManager.localUser);
				match.AddUser(users[0]);
				t.matches.Add(match);

				match = new Match();
				match.AddUser(users[0]);
				match.AddUser(CombuManager.localUser);
				t.matches.Add(match);

				match = new Match();
				match.AddUser(CombuManager.localUser);
				match.AddUser(users[0]);
				t.matches.Add(match);
			}
			else
			{
				Dictionary<string, List<Match>> allMatches = new Dictionary<string, List<Match>>();
				List<Profile> allUsers = new List<Profile>();
				allUsers.Add(CombuManager.localUser);
				allUsers.AddRange(users);

				foreach (Profile p in allUsers)
				{
					List<Match> listUserMatches = new List<Match>();
					foreach (Profile other in allUsers)
					{
						if (other == p)
							continue;

						match = new Match();
						match.AddUser(p);
						match.AddUser(other);
						listUserMatches.Add(match);
					}

					allMatches.Add(p.id, listUserMatches);
				}

				for (int i = 0; i < allUsers.Count; ++i)
				{
					foreach (Profile p in allUsers)
					{
						List<Match> listUserMatches = allMatches[p.id];
						if (i < listUserMatches.Count)
							t.matches.Add(listUserMatches[i]);
					}
				}
			}

			return t;
		}
	}
}
