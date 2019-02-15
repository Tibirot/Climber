using UnityEngine;
using UnityEngine.SocialPlatforms;
using System.Collections;
using System.Collections.Generic;

namespace Combu
{
	[System.Serializable]
	public class Leaderboard : ILeaderboard
	{
		string _title = "";
		string _description = "";
		Score _localUserScore;
		Score[] _scores = new Score[0];
		eLeaderboardTimeScope _customTimeScope = eLeaderboardTimeScope.None;
		uint _maxRange = 0;
		bool _loading = false;
		List<string> _userIDs = new List<string>();
		public bool highestScorePerPlayer;
		public bool sumScoresPerPlayer;

		public bool loading { get { return _loading; } }
		public string id { get; set; }
		public string code { get; set; }
		public UserScope userScope { get; set; }
		public Range range { get; set; }
		public TimeScope timeScope { get; set; }
		public eLeaderboardTimeScope customTimescope { get { return _customTimeScope; } set { _customTimeScope = value; } }
		public IScore localUserScore { get { return _localUserScore; } }
		public uint maxRange { get { return _maxRange; } }
		public IScore[] scores { get { return _scores; } }
		public string title { get { return _title; } }
		public string description { get { return _description; } }
		
		public Leaderboard()
		{
			range = new Range(1, 10);
			timeScope = TimeScope.Today;
			userScope = UserScope.Global;
		}
		public Leaderboard (string jsonString)
		{
			FromJson(jsonString);
		}

		/// <summary>
		/// Initialize the object from a JSON formatted string.
		/// </summary>
		/// <param name="jsonString">Json string.</param>
		public virtual void FromJson (string jsonString)
		{
			Hashtable hash = jsonString.hashtableFromJson();
			if (hash != null)
			{
				if (hash.ContainsKey("Id") && hash["Id"] != null)
					id = hash["Id"].ToString();
				if (hash.ContainsKey("Code") && hash["Code"] != null)
					code = hash["Code"].ToString();
				if (hash.ContainsKey("Title") && hash["Title"] != null)
					_title = hash["Title"].ToString();
				if (hash.ContainsKey("Description") && hash["Description"] != null)
					_description = hash["Description"].ToString();
			}
			range = new Range();
			timeScope = TimeScope.AllTime;
		}

		#region ILeaderboard implementation

		/// <summary>
		/// Sets the user filter.
		/// </summary>
		/// <param name="userIDs">User I ds.</param>
		public void SetUserFilter (string[] userIDs)
		{
			_userIDs.Clear();
			_userIDs.AddRange(userIDs);
		}

		/// <summary>
		/// Loads the scores.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public void LoadScores (System.Action<bool> callback)
		{
			// Leaderboard's id or code must be specified
			if (string.IsNullOrEmpty (id) && string.IsNullOrEmpty (code))
				throw new System.Exception ("You must specify the leaderboard's id or code to load the scores");

			if (_loading) {
				if (callback != null)
					callback (false);
				return;
			}

			int timeInterval = 0;
			timeInterval = (int)eLeaderboardInterval.Today;
			if (_customTimeScope == eLeaderboardTimeScope.None)
			{
				switch (timeScope)
				{
				case TimeScope.AllTime:
					timeInterval = (int)eLeaderboardInterval.Total;
					break;
				case TimeScope.Week:
					timeInterval = (int)eLeaderboardInterval.Week;
					break;
				}
			}
			else
			{
				switch (_customTimeScope)
				{
				case eLeaderboardTimeScope.Month:
					timeInterval = (int)eLeaderboardInterval.Month;
					break;
				}
			}

			_maxRange = 0;
			_scores = new Score[0];
			_loading = true;
			
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "highscore");
			if (!string.IsNullOrEmpty(id))
				form.AddField("IdLeaderboard", id);
			else if (!string.IsNullOrEmpty(code))
				form.AddField("IdLeaderboard", code);
			form.AddField("Interval", timeInterval.ToString());
			form.AddField("UserScope", ((int)userScope).ToString());
			if (highestScorePerPlayer)
				form.AddField("GroupPlayer", "1");
			if (sumScoresPerPlayer)
				form.AddField("SumPlayer", "1");
			if (_userIDs.Count > 0)
				form.AddField("IdUsers", string.Join(",", _userIDs.ToArray()));
			form.AddField("Page", range.from.ToString());
			if (range.count > 0)
				form.AddField("Limit", range.count.ToString());

			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("leaderboards.php"), form, (string text, string error) => {
				List<Score> listScores = new List<Score>();
				int pagesCount = 0;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null && result.ContainsKey("total"))
					{
						pagesCount = int.Parse(result["pages"].ToString());
						ArrayList list = (ArrayList)result["results"];
						if (list != null)
						{
							foreach (Hashtable data in list)
							{
								// Create a new score object from the result
								Score score = new Score(
									data["IdLeaderboard"].ToString(),
									int.Parse(data["Rank"].ToString()),
									new User(data["User"].ToString()),
									double.Parse(data["Score"].ToString())
									);
								// Add to the list
								listScores.Add(score);
							}
						}
						if (!result.ContainsKey("localScore") || result["localScore"] == null)
						{
							_localUserScore = null;
						}
						else
						{
							Hashtable localScoreData = (Hashtable)result["localScore"];
							_localUserScore = new Score(id,
							                            int.Parse(localScoreData["Rank"].ToString()),
							                            CombuManager.localUser,
							                            double.Parse(localScoreData["Score"].ToString())
							                            );
						}
					}
				}
				_maxRange = (uint)pagesCount;
				_scores = listScores.ToArray();
				_loading = false;
				if (callback != null)
					callback(string.IsNullOrEmpty(error));
			});
		}

		#endregion

		/// <summary>
		/// Load the specified leaderboardId.
		/// </summary>
		/// <param name="leaderboardId">Leaderboard identifier.</param>
		/// <param name="callback">Callback.</param>
		public static void Load (string leaderboardId, System.Action<Leaderboard, string> callback)
		{
			Load<Leaderboard>(leaderboardId, callback);
		}
		/// <summary>
		/// Load the specified leaderboardId.
		/// </summary>
		/// <param name="leaderboardId">Leaderboard identifier.</param>
		/// <param name="callback">Callback.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void Load<T> (string leaderboardId, System.Action<Leaderboard, string> callback) where T: Leaderboard, new()
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "load");
			form.AddField("IdLeaderboard", leaderboardId);
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("leaderboards.php"), form, (string text, string error) => {
				T leaderboard = null;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						bool success = false;
						if (result.ContainsKey("success"))
							bool.TryParse(result["success"].ToString(), out success);
						if (success && result.ContainsKey("message"))
						{
							leaderboard = new T();
							leaderboard.FromJson(result["message"].ToString());
						}
						else if (!success && result.ContainsKey("message"))
							error = result["message"].ToString();
					}
				}
				if (callback != null)
					callback(leaderboard, error);
			});
		}

		/// <summary>
		/// Loads the scores of a user on this leaderboard.
		/// </summary>
		/// <param name="user">User.</param>
		/// <param name="interval">Interval.</param>
		/// <param name="callback">Callback.</param>
		public virtual void LoadScoresByUser (User user, eLeaderboardInterval interval, int limit, System.Action<Score, int, string> callback)
		{
            LoadScoresByUser(user.idLong > 0 ? user.idLong : 0, user.idLong > 0 ? string.Empty : user.userName, interval, limit, callback);
		}
        /// <summary>
        /// Loads the scores by user id.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="interval">Interval.</param>
        /// <param name="limit">Limit.</param>
        /// <param name="callback">Callback.</param>
        public virtual void LoadScoresByUser(long userId, eLeaderboardInterval interval, int limit, System.Action<Score, int, string> callback)
        {
            LoadScoresByUser(userId, string.Empty, interval, limit, callback);
        }
        /// <summary>
        /// Loads the scores by userName.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="interval">Interval.</param>
        /// <param name="limit">Limit.</param>
        /// <param name="callback">Callback.</param>
        public virtual void LoadScoresByUser(string userName, eLeaderboardInterval interval, int limit, System.Action<Score, int, string> callback)
        {
            LoadScoresByUser(0, userName, interval, limit, callback);
        }
        /// <summary>
        /// Loads the scores by user.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="userName">User name.</param>
        /// <param name="interval">Interval.</param>
        /// <param name="limit">Limit.</param>
        /// <param name="callback">Callback.</param>
        protected void LoadScoresByUser(long userId, string userName, eLeaderboardInterval interval, int limit, System.Action<Score, int, string> callback)
        {
            var form = CombuManager.instance.CreateForm();
            form.AddField("action", "highscore_account");
            if (userId > 0)
            {
				form.AddField("Id", userId.ToString());
            }
            else if (!string.IsNullOrEmpty(userName))
            {
                form.AddField("Username", userName);
            }
            form.AddField("IdLeaderboard", id);
            form.AddField("Interval", ((int)interval).ToString());
            form.AddField("Limit", limit.ToString());
            CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("leaderboards.php"), form, (string text, string error) => {
                Score score = null;
                int page = 1;
                if (string.IsNullOrEmpty(error))
                {
                    Hashtable result = text.hashtableFromJson();
                    if (result != null && result.ContainsKey("Score"))
                    {
                        score = new Score(
                            result["IdLeaderboard"] + "",
                            result["CodeLeaderboard"] + "",
                            int.Parse(result["Rank"].ToString()),
                            new User(result["User"].ToString()),
                            double.Parse(result["Score"].ToString())
                            );
                        page = int.Parse(result["Page"].ToString());
                    }
                }
                if (callback != null)
                    callback(score, page, error);
            });
        }

        /// <summary>
        /// Loads the scores of a user.
        /// </summary>
        /// <param name="user">User.</param>
        /// <param name="interval">Interval.</param>
        /// <param name="callback">Callback.</param>
		public static void LoadScoresByUser (User user, eLeaderboardInterval interval, System.Action<Score[], string> callback)
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "highscore_account");
			form.AddField("Id", user.id);
			form.AddField("Interval", ((int)interval).ToString());
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("leaderboards.php"), form, (string text, string error) => {
				List<Score> scores = new List<Score>();
				if (string.IsNullOrEmpty(error))
				{
					ArrayList result = text.arrayListFromJson();
					if (result != null)
					{
						foreach (Hashtable data in result)
						{
							scores.Add(new Score(
								data["IdLeaderboard"] + "",
								data["CodeLeaderboard"] + "",
								int.Parse(data["Rank"].ToString()),
								user,
								double.Parse(data["Score"].ToString())
							));
						}
					}
				}
				if (callback != null)
					callback(scores.ToArray(), error);
			});
		}
	}

	public class Score : IScore
	{
		public string leaderboardID { get; set; }
		public string leaderboardCode { get; set; }
		int _rank = 0;
		double _value = 0;
		System.DateTime _date;
		Profile _user;
		
		public System.DateTime date { get { return _date; } }
		public string formattedValue { get { return _value.ToString(); } }
		public string userID { get { return (_user == null ? "" : _user.id); } }
		public Profile user { get { return _user; } }
		public int rank { get { return _rank; } }
		public long value
		{
			get { return (long)_value; }
			set { _value = (double)value; }
		}
		public float valueFloat
		{
			get { return (float)_value; }
			set { _value = (float)value; }
		}
		public double valueDouble
		{
			get { return _value; }
			set { _value = value; }
		}

		public Score()
		{
		}
		public Score (string idLeaderboard, int rank, User user, double score)
			: this(idLeaderboard, "", rank, user, score)
		{
		}
		public Score (string idLeaderboard, string codeLeaderboard, int rank, User user, double score)
		{
			Initialize(idLeaderboard, codeLeaderboard, rank, user, score);
		}

		/// <summary>
		/// Initialize this Score with the specified idLeaderboard, rank, user and score.
		/// </summary>
		/// <param name="idLeaderboard">Identifier leaderboard.</param>
		/// <param name="rank">Rank.</param>
		/// <param name="user">User.</param>
		/// <param name="score">Score.</param>
		[System.Obsolete("Use the overload which accepts both idLeaderboard and codeLeaderboard, specify an empty string as codeLeaderboard if you don't need", true)]
		public void Initialize (string idLeaderboard, int rank, Profile user, double score)
		{
			Initialize (idLeaderboard, "", rank, user, score);
		}

		/// <summary>
		/// Initialize this Score with the specified idLeaderboard, rank, user and score.
		/// </summary>
		/// <param name="idLeaderboard">Identifier of the leaderboard.</param>
		/// <param name="codeLeaderboard">Code of the leaderboard.</param>
		/// <param name="rank">Rank.</param>
		/// <param name="user">User.</param>
		/// <param name="score">Score.</param>
		public void Initialize (string idLeaderboard, string codeLeaderboard, int rank, Profile user, double score)
		{
			leaderboardID = idLeaderboard;
			leaderboardCode = string.Empty;
			_date = System.DateTime.MinValue;
			_rank = rank;
			_user = user;
			_value = score;
		}

		#region IScore implementation

		/// <summary>
		/// Reports the score.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public void ReportScore (System.Action<bool> callback)
		{
			if (!CombuManager.isInitialized)
				throw new System.Exception("Combu Manager not initialized");

			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "score");
			if (_user != null && !string.IsNullOrEmpty(_user.userName))
				form.AddField("Username", _user.userName);
			form.AddField("IdLeaderboard", leaderboardID.ToString());
			form.AddField("Score", _value.ToString());
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("leaderboards.php"), form, (string text, string error) => {
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
					callback(success);
			});
		}

		#endregion
	}
}