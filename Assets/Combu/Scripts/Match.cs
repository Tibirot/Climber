using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Combu
{
	[Serializable]
	public class Match
	{
		[Serializable]
		public class MatchRoundData
		{
			public List<MatchAccount> users = new List<MatchAccount>();
			public List<MatchRound> scores = new List<MatchRound>();
		}

		public long id = 0;
		public long idTournament = 0;
		public string title = "";
		public int roundsCount = 1;
		public DateTime dateCreation = DateTime.Now;
		public DateTime? dateExpire = null;
		public Hashtable customData = new Hashtable();

		List<long> _deletedUsers = new List<long>();
		List<MatchAccount> _users = new List<MatchAccount>();
		public List<MatchAccount> users { get { return _users; } }

		List<MatchRoundData> _rounds = new List<MatchRoundData>();
		public List<MatchRoundData> rounds { get { return _rounds; } }

		bool _finished = false;
		public bool finished { get { return _finished; } }

		bool _quickMatch = false;
		public bool searchingQuickMatch { get { return _quickMatch; } }

		public Match ()
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Combu.Match"/> class.
		/// </summary>
		/// <param name="jsonString">JSON string to initialize the instance.</param>
		public Match (string jsonString)
		{
			FromJson(jsonString);
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Combu.Match"/> class.
		/// </summary>
		/// <param name="data">Data to initialize the instance.</param>
		public Match (Hashtable data)
		{
			FromHashtable(data);
		}

		/// <summary>
		/// Initialize the object from a JSON formatted string.
		/// </summary>
		/// <param name="jsonString">Json string.</param>
		public virtual void FromJson (string jsonString)
		{
			FromHashtable(jsonString.hashtableFromJson());
		}

		/// <summary>
		/// Initialize the object from a hashtable.
		/// </summary>
		/// <param name="hash">Hash.</param>
		public virtual void FromHashtable (Hashtable hash)
		{
			if (hash == null)
				return;
			if (hash.ContainsKey("Id") && hash["Id"] != null)
			{
				long.TryParse(hash["Id"].ToString(), out id);
			}
			if (hash.ContainsKey("IdTournament") && hash["IdTournament"] != null)
			{
				long.TryParse(hash["IdTournament"].ToString(), out idTournament);
			}
			if (hash.ContainsKey("Title") && hash["Title"] != null)
			{
				title = hash["Title"].ToString();
			}
			if (hash.ContainsKey("Rounds") && hash["Rounds"] != null)
			{
				int.TryParse(hash["Rounds"].ToString(), out roundsCount);
			}
			if (hash.ContainsKey("DateCreation") && hash["DateCreation"] != null && !string.IsNullOrEmpty(hash["DateCreation"].ToString()))
			{
				dateCreation = hash ["DateCreation"].ToString ().ToDatetimeOrDefault ();
			}
			if (hash.ContainsKey("DateExpire") && hash["DateExpire"] != null && !string.IsNullOrEmpty(hash["DateExpire"].ToString()))
			{
				dateExpire = hash ["DateExpire"].ToString ().ToDatetimeOrDefault ();
			}
			if (hash.ContainsKey("Finished") && hash["Finished"] != null)
			{
				int f = 0;
				if (int.TryParse(hash["Finished"].ToString(), out f))
					_finished = f.Equals(1);
			}
			if (hash.ContainsKey("CustomData") && hash["CustomData"] != null && hash["CustomData"] is Hashtable)
			{
				customData = (Hashtable)hash["CustomData"];
			}
			if (hash.ContainsKey("Users") && hash["Users"] != null)
			{
				_users.Clear();
				_deletedUsers.Clear();
				ArrayList listUsers = (ArrayList)hash["Users"];
				foreach (Hashtable userData in listUsers)
				{
					MatchAccount user = new MatchAccount(userData);
					_users.Add(user);
				}
				FillRounds();
			}
		}

		/// <summary>
		/// Fills the rounds from Match Accounts.
		/// </summary>
		void FillRounds ()
		{
			_rounds.Clear();
			for (int i = 0; i < roundsCount; ++i)
			{
				_rounds.Add(new MatchRoundData());
			}
			// Fill the rounds for each user
			foreach (MatchAccount user in _users)
			{
				for (int i = 0; i < roundsCount; ++i)
				{
					_rounds[i].users.Add(user);
					
					MatchRound round;
					if (i < user.rounds.Count)
					{
						round = user.rounds[i];
					}
					else
					{
						round = new MatchRound();
						round.idMatchAccount = user.id;
					}
					_rounds[i].scores.Add(round);
				}
			}
		}

		/// <summary>
		/// Adds the user to this match.
		/// </summary>
		/// <param name="user">User.</param>
		public virtual void AddUser (Profile user)
		{
			MatchAccount matchAccount = new MatchAccount();
			matchAccount.idMatch = id;
			matchAccount.user = user;
			matchAccount.idAccount = matchAccount.user.idLong;
			_users.Add(matchAccount);
		}

		/// <summary>
		/// Removes the user.
		/// </summary>
		/// <param name="user">User.</param>
		public virtual void RemoveUser (Profile user)
		{
			RemoveUser(user.idLong);
		}
		/// <summary>
		/// Removes the user.
		/// </summary>
		/// <param name="idUser">Identifier user.</param>
		public virtual void RemoveUser (long idUser)
		{
			RemoveUser(idUser, string.Empty);
		}
		/// <summary>
		/// Removes the user.
		/// </summary>
		/// <param name="username">Username.</param>
		public virtual void RemoveUser (string username)
		{
			RemoveUser(0, username);
		}
		/// <summary>
		/// Removes the user.
		/// </summary>
		/// <param name="idUser">Identifier user.</param>
		/// <param name="username">Username.</param>
		protected virtual void RemoveUser (long idUser, string username)
		{
			if (idUser < 1 && string.IsNullOrEmpty(username))
				return;
			int i;
			if (idUser > 0)
				i = _users.FindIndex( u => u.idAccount.Equals(idUser) );
			else
				i = _users.FindIndex( u => u.user != null && u.user.userName.Equals(username) );
			if (i != -1)
			{
				if (_users[i].id > 0)
					_deletedUsers.Add(_users[i].id);
				_users.RemoveAt(i);
			}
		}

		/// <summary>
		/// Send the specified score.
		/// </summary>
		/// <param name="score">Score.</param>
		/// <param name="callback">Callback.</param>
		public void Score (float score, Action<bool, string> callback)
		{
			if (_finished)
			{
				if (callback != null)
					callback(false, "This Match is already finished");
				return;
			}

			long myId = CombuManager.localUser.idLong;
			MatchAccount matchAccount = _users.Find( u => u.idAccount.Equals(myId) );
			MatchRound round = null;
			if (matchAccount != null)
				round = matchAccount.rounds.Find( r => !r.hasScore );
			if (round == null)
			{
				if (callback != null)
					callback(false, "Cannot send score to this Match or it has been already sent");
				return;
			}

			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "match_score");
			form.AddField("Id", round.id.ToString());
			form.AddField("Score", score.ToString());
			form.AddField("CustomData", customData.toJson());
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("match.php"), form, (string text, string error) => {
				bool success = false;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						bool.TryParse(result["success"].ToString(), out success);
						if (success)
						{
							matchAccount.FromJson(result["message"].ToString());
							FillRounds();
						}
						else
							error = result["message"].ToString();
					}
				}
				if (callback != null)
					callback(success, error);
			});
		}

		/// <summary>
		/// Save the this instance in the server.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public void Save (Action<bool, string> callback)
		{
			if (_users.Count < 2)
			{
				if (callback != null)
					callback(false, "A match requires at least 2 users");
				return;
			}

			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "match_save");
			form.AddField("Id", id.ToString());
			form.AddField("IdTournament", idTournament.ToString());
			form.AddField("Title", title);
			form.AddField("Rounds", roundsCount.ToString());
			if (dateExpire.HasValue && dateExpire != null)
			{
				form.AddField("DateExpire", ((System.DateTime)dateExpire).ToString("yyyy-MM-dd HH:mm:ss"));
			}
			form.AddField("CustomData", customData.toJson());

			// Add new users to the request
			ArrayList listUsers = new ArrayList();
			foreach (MatchAccount user in _users)
			{
				if (user.id < 1)
					listUsers.Add(user.idAccount);
			}
			form.AddField("Users", listUsers.toJson());

			// Add the deleted users to the request
			listUsers.Clear();
			foreach (long userId in _deletedUsers)
			{
				listUsers.Add(userId);
			}
			form.AddField("DeleteUsers", listUsers.toJson());

			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("match.php"), form, (string text, string error) => {
				bool success = false;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						bool.TryParse(result["success"].ToString(), out success);
						if (success)
							FromJson(result["message"].ToString());
						else
							error = result["message"].ToString();
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
		/// Delete the specified Match.
		/// </summary>
		/// <param name="idMatch">Identifier match.</param>
		/// <param name="callback">Callback.</param>
		public static void Delete (long idMatch, Action<bool, string> callback)
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "match_delete");
			form.AddField("Id", idMatch.ToString());
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
		/// Creates a quick match.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public static void QuickMatch (bool friendsOnly, SearchCustomData[] customData, int rounds, Action<Match> callback)
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "match_quick");
			form.AddField("Friends", friendsOnly ? "1" : "0");
			
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
				Match match = null;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						bool success = false;
						bool.TryParse(result["success"].ToString(), out success);
						if (success)
							match = new Match(result["message"].ToString());
						else
							error = result["message"].ToString();
					}
				}
				if (callback != null)
					callback(match);
			});
		}

		/// <summary>
		/// Load the list of Matchs by specified filters.
		/// </summary>
		/// <param name="idTournament">Identifier tournament.</param>
		/// <param name="activeOnly">If set to <c>true</c> then displays active matches only, else archived matches.</param>
		/// <param name="title">Title.</param>
		/// <param name="callback">Callback.</param>
		public static void Load (long idTournament, bool activeOnly, string title, Action<Match[]> callback)
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "match_list");
			if (!activeOnly)
				form.AddField("Active", "0");
			if (idTournament > 0)
				form.AddField("IdTournament", idTournament.ToString());
			if (!string.IsNullOrEmpty(title))
				form.AddField("Title", title);
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("match.php"), form, (string text, string error) => {
				List<Match> listMatches = new List<Match>();
				Hashtable result = text.hashtableFromJson();
				if (result != null && result.ContainsKey("results"))
				{
					ArrayList list = (ArrayList)result["results"];
					if (list != null)
					{
						foreach (Hashtable data in list)
						{
							// Create a new object from the result
							Match match = new Match(data);
							// Add to the list
							listMatches.Add(match);
						}
					}	
				}
				if (callback != null)
					callback(listMatches.ToArray());
			});
		}

		/// <summary>
		/// Load the specified Match.
		/// </summary>
		/// <param name="idMatch">Identifier match.</param>
		/// <param name="callback">Callback.</param>
		public static void Load (long idMatch, Action<Match> callback)
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "match_load");
			if (idMatch > 0)
				form.AddField("Id", idMatch.ToString());
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("match.php"), form, (string text, string error) => {
				Match match = null;
				Hashtable result = text.hashtableFromJson();
				if (result != null && result.ContainsKey("success"))
				{
					bool success = false;
					bool.TryParse(result["success"].ToString(), out success);
					if (success)
						match = new Match(result["message"].ToString());
					else
						error = result["message"].ToString();
				}
				if (callback != null)
					callback(match);
			});
		}
	}

}
