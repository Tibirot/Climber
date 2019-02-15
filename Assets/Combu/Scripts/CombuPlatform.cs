using UnityEngine;
using UnityEngine.SocialPlatforms;
using System.Collections;
using System.Collections.Generic;

namespace Combu
{
	/// <summary>
	/// Combu Platform implementation of Unity built-in Social interfaces (ISocialPlatform).
	/// </summary>
	public class CombuPlatform : ISocialPlatform
	{
		User _localUser = new User();
		public ILocalUser localUser { get { return _localUser; } }

		#region ISocialPlatform implementation

		/// <summary>
		/// Authenticates the user.
		/// </summary>
		/// <param name="user">User.</param>
		/// <param name="callback">Callback.</param>
		/// <example>
		/// Example of usage:
		/// <code>
		/// CombuManager.platform.Authenticate (user, (bool success) => {
		///    Debug.Log("Authenticate: " + success);
		/// });
		/// </code>
		/// </example>
		public virtual void Authenticate (ILocalUser user, System.Action<bool> callback)
		{
			if (!CombuManager.isInitialized)
				throw new System.Exception("Combu Manager not initialized");
			user.Authenticate( (bool success) => {
				if (success)
					_localUser = (User)user;
				if (callback != null)
					callback(success);
			});
		}

		/// <summary>
		/// Authenticates the user with specified username and password.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		/// <param name="callback">Callback.</param>
		/// <example>
		/// Example of usage:
		/// <code>
		/// CombuManager.platform.Authenticate ("username", "password", (bool success, string error) => {
		///    Debug.Log("Authenticate: " + success + " -- " + error);
		/// });
		/// </code>
		/// </example>
		public virtual void Authenticate (string username, string password, System.Action<bool, string> callback)
		{
			Authenticate<User>(username, password, callback);
		}
		/// <summary>
		/// Authenticates the user with specified username and password using the specified profile class.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		/// <param name="callback">Callback.</param>
		/// <typeparam name="T">Type of the returned profiles.</typeparam>
		/// <example>
		/// Example of usage:
		/// <code>
		/// CombuManager.platform.Authenticate<MyUserClass> ("username", "password", (bool success, string error) => {
		///    Debug.Log("Authenticate: " + success + " -- " + error);
		/// });
		/// </code>
		/// </example>
		public virtual void Authenticate<T> (string username, string password, System.Action<bool, string> callback) where T: User, new()
		{
			if (!CombuManager.isInitialized)
				throw new System.Exception("Combu Manager not initialized");
			_localUser = new T();
			_localUser.userName = username;
			_localUser.Authenticate(password, callback);
		}
		/// <summary>
		/// Authenticate the specified user.
		/// </summary>
		/// <param name="user">User.</param>
		/// <param name="callback">Callback.</param>
		/// <example>
		/// Example of usage:
		/// <code>
		/// CombuManager.platform.Authenticate (user, (bool success, string error) => {
		///    Debug.Log("Authenticate: " + success + " -- " + error);
		/// });
		/// </code>
		/// </example>
		public virtual void Authenticate (ILocalUser user, System.Action<bool, string> callback)
		{
			string password = (user is User ? (user as User).password : "");
			Authenticate<User> (user.userName, password, callback);
		}

		/// <summary>
		/// Loads the users by Id.
		/// </summary>
		/// <param name="userIDs">User I ds.</param>
		/// <param name="callback">Callback.</param>
		/// <example>
		/// Example of usage:
		/// <code>
		/// CombuManager.platform.LoadUsers (new string[] {1234, 5678, 9012}, (IUserProfile[] users) => {
		///    Debug.Log("Users: " + users.Length);
		/// });
		/// </code>
		/// </example>
		public virtual void LoadUsers (string[] userIDs, System.Action<IUserProfile[]> callback)
		{
			User.Load(userIDs, (User[] profiles) => {
				if (callback != null)
					callback((IUserProfile[])profiles);
			});
		}

		/// <summary>
		/// Reports the progress of an Achievement expressed as percentage. The progress will be multiplied by 100.0 and finally rounded to int.
		/// </summary>
		/// <param name="achievementID">Achievement identifier.</param>
		/// <param name="progress">Progress.</param>
		/// <param name="callback">Callback.</param>
		/// <example>
		/// Example of usage:
		/// <code>
		/// CombuManager.platform.ReportProgress ("1234", 1.0, (bool success) => {
		///    Debug.Log("ReportProgress: " + success);
		/// });
		/// </code>
		/// </example>
		public virtual void ReportProgress (string achievementId, double progress, System.Action<bool> callback)
		{
			ReportProgress(achievementId, Mathf.RoundToInt((float)progress * 100f), callback);
		}
		/// <summary>
		/// Reports the progress of an Achievement.
		/// </summary>
		/// <param name="achievementId">Achievement identifier.</param>
		/// <param name="progress">Progress.</param>
		/// <param name="callback">Callback.</param>
		/// <example>
		/// Example of usage:
		/// <code>
		/// CombuManager.platform.ReportProgress ("1234", 100, (bool success) => {
		///    Debug.Log("ReportProgress: " + success);
		/// });
		/// </code>
		/// </example>
		public virtual void ReportProgress (string achievementId, int progress, System.Action<bool> callback)
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "progress");
			form.AddField("IdAchievement", achievementId);
			form.AddField("Progress", progress.ToString());
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("achievements.php"), form, (string text, string error) => {
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

		/// <summary>
		/// Resets all achievements of localUser.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public static void ResetAllAchievements (System.Action<bool> callback)
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "reset");
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("achievements.php"), form, (string text, string error) => {
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

		/// <summary>
		/// Loads the achievement descriptions.
		/// </summary>
		/// <param name="callback">Callback.</param>
		/// <example>
		/// Example of usage:
		/// <code>
		/// CombuManager.platform.LoadAchievementDescriptions ((IAchievementDescription[] descriptions) => {
		///    Debug.Log("Achievement descriptions: " + descriptions.Length);
		/// });
		/// </code>
		/// </example>
		public virtual void LoadAchievementDescriptions (System.Action<IAchievementDescription[]> callback)
		{
			if (!CombuManager.isInitialized)
				throw new System.Exception("Combu Manager not initialized");
			if (callback == null)
				throw new System.ArgumentNullException("callback");
			LoadAchievements((IAchievement[] achievements) => {
				List<IAchievementDescription> descriptions = new List<IAchievementDescription>();
				descriptions.AddRange((Achievement[])achievements);
				if (callback != null)
					callback(descriptions.ToArray());
			});
		}

		/// <summary>
		/// Loads the achievements.
		/// </summary>
		/// <param name="callback">Callback.</param>
		/// <example>
		/// Example of usage:
		/// <code>
		/// CombuManager.platform.LoadAchievements ((IAchievement[] achievements) => {
		///    Debug.Log("Achievements: " + achievements.Length);
		/// });
		/// </code>
		/// </example>
		public virtual void LoadAchievements (System.Action<IAchievement[]> callback)
		{
			LoadAchievements<Achievement>((Achievement[] achievements) => {
				if (callback != null)
					callback((IAchievement[])achievements);
			});
		}
		/// <summary>
		/// Loads the achievements.
		/// </summary>
		/// <param name="callback">Callback.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		/// <example>
		/// Example of usage:
		/// <code>
		/// CombuManager.platform.LoadAchievements<MyAchievementClass> ((MyAchievementClass[] achievements) => {
		///    Debug.Log("Achievements: " + achievements.Length);
		/// });
		/// </code>
		/// </example>
		public virtual void LoadAchievements<T> (System.Action<T[]> callback) where T: Achievement, new()
		{
			if (!CombuManager.isInitialized)
				throw new System.Exception("Combu Manager not initialized");
			if (callback == null)
				throw new System.ArgumentNullException("callback");
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "load");
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("achievements.php"), form, (string text, string error) => {
				List<T> achievements = new List<T>();
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						if (result.ContainsKey("results"))
						{
							ArrayList records = (ArrayList)result["results"];
							if (records != null)
							{
								foreach (Hashtable record in records)
								{
									T achievement = new T();
									achievement.FromJson(record.toJson());
									achievements.Add(achievement);
								}
							}
						}
					}
				}
				achievements.Sort(CompareAchievements);
				for (int i = achievements.Count - 1; i > 0; --i)
				{
					if (achievements[i - 1].id.Equals(achievements[i].id))
						achievements.RemoveAt(i);
				}
				if (callback != null)
					callback(achievements.ToArray());
			});
		}

		int CompareAchievements (Achievement a, Achievement b)
		{
			long idA = long.Parse(a.id);
			long idB = long.Parse(b.id);
			if (idA == idB)
				return a.percentCompleted.CompareTo(b.percentCompleted);
			return idA.CompareTo(idB);
		}

		/// <summary>
		/// Creates the achievement.
		/// </summary>
		/// <returns>The achievement.</returns>
		/// <example>
		/// Example of usage:
		/// <code>
		/// IAchievement achievement = CombuManager.platform.CreateAchievement();
		/// </code>
		/// </example>
		public virtual IAchievement CreateAchievement ()
		{
			return new Achievement();
		}

		/// <summary>
		/// Reports the score of a Leaderboard.
		/// </summary>
		/// <param name="score">Score.</param>
		/// <param name="board">Board.</param>
		/// <param name="callback">Callback.</param>
		/// <example>
		/// Example of usage:
		/// <code>
		/// CombuManager.platform.ReportScore (1000, "1234", (bool success) => {
		///    Debug.Log("ReportScore: " + success);
		/// });
		/// </code>
		/// </example>
		public virtual void ReportScore (long score, string board, System.Action<bool> callback)
		{
			ReportScore(score.ToString(), board, string.Empty, callback);
		}
		/// <summary>
		/// Reports the score of a Leaderboard.
		/// </summary>
		/// <param name="score">Score.</param>
		/// <param name="board">Board.</param>
		/// <param name="callback">Callback.</param>
		/// <example>
		/// Example of usage:
		/// <code>
		/// CombuManager.platform.ReportScore ("1000", "1234", (bool success) => {
		///    Debug.Log("ReportScore: " + success);
		/// });
		/// </code>
		/// </example>
		public virtual void ReportScore (string score, string board, System.Action<bool> callback)
		{
			ReportScore(score, board, string.Empty, callback);
		}
		/// <summary>
		/// Reports the score of a Leaderboard.
		/// </summary>
		/// <param name="score">Score.</param>
		/// <param name="board">Board.</param>
		/// <param name="username">Username.</param>
		/// <param name="callback">Callback.</param>
		/// <example>
		/// Example of usage:
		/// <code>
		/// CombuManager.platform.ReportScore ("1000", "1234", "username", (bool success) => {
		///    Debug.Log("ReportScore: " + success);
		/// });
		/// </code>
		/// </example>
		public virtual void ReportScore (string score, string board, string username, System.Action<bool> callback)
		{
			if (!CombuManager.isInitialized)
				throw new System.Exception("Combu Manager not initialized");
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "score");
			if (!string.IsNullOrEmpty(username))
				form.AddField("Username", username);
			form.AddField("IdLeaderboard", board);
			form.AddField("Score", score);
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

		/// <summary>
		/// Loads the scores of a Leaderboard.
		/// </summary>
		/// <param name="leaderboardID">Leaderboard I.</param>
		/// <param name="callback">Callback.</param>
		/// <example>
		/// Example of usage:
		/// <code>
		/// CombuManager.platform.LoadScores ("1234", (IScore[] scores) => {
		///    Debug.Log("Scores: " + scores.Length);
		/// });
		/// </code>
		/// </example>
		public virtual void LoadScores (string leaderboardID, System.Action<IScore[]> callback)
		{
			LoadScores(leaderboardID, 1, 10, callback);
		}
		/// <summary>
		/// Loads the scores of a Leaderboard.
		/// </summary>
		/// <param name="leaderboardID">Leaderboard I.</param>
		/// <param name="page">Page.</param>
		/// <param name="countPerPage">Count per page.</param>
		/// <param name="callback">Callback.</param>
		/// <example>
		/// Example of usage:
		/// <code>
		/// CombuManager.platform.LoadScores ("1234", 1, 10, (IScore[] scores) => {
		///    Debug.Log("Scores: " + scores.Length);
		/// });
		/// </code>
		/// </example>
		public virtual void LoadScores (string leaderboardID, int page, int countPerPage, System.Action<IScore[]> callback)
        {
            LoadScores(leaderboardID, TimeScope.Week, page, countPerPage, callback);
        }
        /// <summary>
        /// Loads the scores of a Leaderboard.
        /// </summary>
        /// <param name="leaderboardID">Leaderboard identifier.</param>
        /// <param name="timeScope">Time scope.</param>
        /// <param name="page">Page.</param>
        /// <param name="countPerPage">Count per page.</param>
        /// <param name="callback">Callback.</param>
        public void LoadScores(string leaderboardID, TimeScope timeScope, int page, int countPerPage, System.Action<IScore[]> callback)
		{
			if (!CombuManager.isInitialized)
				throw new System.Exception("Combu Manager not initialized");
			Leaderboard leaderboard = new Leaderboard();
			leaderboard.id = leaderboardID;
            leaderboard.timeScope = timeScope;
			leaderboard.range = new Range(page, countPerPage);
			leaderboard.LoadScores((bool success) => {
				if (callback != null)
					callback(leaderboard.scores);
			});
		}

		/// <summary>
		/// Creates the leaderboard.
		/// </summary>
		/// <returns>The leaderboard.</returns>
		/// <example>
		/// Example of usage:
		/// <code>
		/// ILeaderboard leaderboard = CombuManager.platform.CreateLeaderboard();
		/// </code>
		/// </example>
		public virtual ILeaderboard CreateLeaderboard ()
		{
			if (!CombuManager.isInitialized)
				throw new System.Exception("Combu Manager not initialized");
			return new Leaderboard();
		}

		/// <summary>
		/// Shows the achievements UI. Requires achievementUIObject and eventually achievementUIFunction set in order to work.
		/// </summary>
		/// <example>
		/// Example of usage:
		/// <code>
		/// CombuManager.platform.ShowAchievementsUI();
		/// </code>
		/// </example>
		public virtual void ShowAchievementsUI ()
		{
			if (!CombuManager.isInitialized)
				throw new System.Exception("Combu Manager not initialized");
			if (CombuManager.instance.achievementUIObject != null)
			{
				if (!CombuManager.instance.achievementUIObject.activeInHierarchy)
					CombuManager.instance.achievementUIObject.SetActive(true);
				if (!string.IsNullOrEmpty(CombuManager.instance.achievementUIFunction))
					CombuManager.instance.achievementUIObject.SendMessage(CombuManager.instance.achievementUIFunction);
			}
		}

		/// <summary>
		/// Shows the leaderboard UI. Requires leaderboardUIObject and eventually leaderboardUIFunction set in order to work.
		/// </summary>
		/// <example>
		/// Example of usage:
		/// <code>
		/// CombuManager.platform.ShowAchievementsUI();
		/// </code>
		/// </example>
		public virtual void ShowLeaderboardUI ()
		{
			if (!CombuManager.isInitialized)
				throw new System.Exception("Combu Manager not initialized");
			if (CombuManager.instance.leaderboardUIObject != null)
			{
				if (!CombuManager.instance.leaderboardUIObject.activeInHierarchy)
					CombuManager.instance.leaderboardUIObject.SetActive(true);
				if (!string.IsNullOrEmpty(CombuManager.instance.leaderboardUIFunction))
					CombuManager.instance.leaderboardUIObject.SendMessage(CombuManager.instance.leaderboardUIFunction);
			}
		}

		/// <summary>
		/// Loads the friends of localUser.
		/// </summary>
		/// <param name="user">User.</param>
		/// <param name="callback">Callback.</param>
		/// <example>
		/// Example of usage:
		/// <code>
		/// CombuManager.platform.LoadFriends (CombuManager.localUser, (bool success) => {
		///    Debug.Log("LoadFriends: " + success);
		/// });
		/// </code>
		/// </example>
		public virtual void LoadFriends (ILocalUser user, System.Action<bool> callback)
		{
			if (!CombuManager.isInitialized)
				throw new System.Exception("Combu Manager not initialized");
			user.LoadFriends(callback);
		}

		/// <summary>
		/// Loads the scores of a Leaderboard.
		/// </summary>
		/// <param name="board">Board.</param>
		/// <param name="callback">Callback.</param>
		/// <example>
		/// Example of usage:
		/// <code>
		/// Leaderboard myLeaderboard = CombuManager.platform.CreateLeaderboard();
		/// myLeaderboard.id = "123";
		/// myLeaderboard.timeScope = UnityEngine.SocialPlatforms.TimeScope.AllTime;
		/// myLeaderboard.range = new UnityEngine.SocialPlatforms.Range(1, 10);
		/// CombuManager.platform.LoadScores (myLeaderboard, (bool success) => {
		///    Debug.Log("LoadScores: " + success);
		/// });
		/// </code>
		/// </example>
		public virtual void LoadScores (ILeaderboard board, System.Action<bool> callback)
		{
			if (!CombuManager.isInitialized)
				throw new System.Exception("Combu Manager not initialized");
			board.LoadScores(callback);
		}

		/// <summary>
		/// Gets the loading state of a Leaderboard.
		/// </summary>
		/// <returns><c>true</c>, if loading was gotten, <c>false</c> otherwise.</returns>
		/// <param name="board">Board.</param>
		/// <example>
		/// Example of usage:
		/// <code>
		/// Debug.Log("LoadFriends: " + CombuManager.platform.GetLoading(myLeaderboard));
		/// </code>
		/// </example>
		public virtual bool GetLoading (ILeaderboard board)
		{
			if (!CombuManager.isInitialized)
				throw new System.Exception("Combu Manager not initialized");
			return board.loading;
		}

		#endregion

		/// <summary>
		/// Sets the local user. For internal use only (e.g. User.Authenticate), it's not recommended to call this method directly.
		/// </summary>
		/// <param name="user">User.</param>
		public virtual void SetLocalUser (User user)
		{
			_localUser = user;
		}

		/// <summary>
		/// Logout localUser.
		/// </summary>
		/// <param name="callback">Callback.</param>
		/// <example>
		/// Example of usage:
		/// <code>
		/// CombuManager.platform.Logout();
		/// </code>
		/// </example>
		public virtual void Logout (System.Action callback)
		{
			if (!CombuManager.isInitialized)
				throw new System.Exception("Combu Manager not initialized");
			_localUser = new User();
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "logout");
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("users.php"), form, (string text, string error) => {
				if (callback != null)
					callback();
			});
		}

		/// <summary>
		/// Loads the scores of a Leaderboard by user.
		/// </summary>
		/// <param name="leaderboardId">Leaderboard identifier.</param>
		/// <param name="user">User.</param>
		/// <param name="interval">Interval.</param>
		/// <param name="callback">Callback.</param>
		/// <example>
		/// Example of usage:
		/// <code>
		/// CombuManager.platform.LoadScores ("1234", CombuManager.localUser, eLeaderboardInterval.Week, 10, (Score score, int page, string error) => {
		///    Debug.Log("Score: " + score.rank);
		/// });
		/// </code>
		/// </example>
		public virtual void LoadScoresByUser (string leaderboardId, User user, eLeaderboardInterval interval, int limit, System.Action<Score, int, string> callback)
		{
			if (!CombuManager.isInitialized)
				throw new System.Exception("Combu Manager not initialized");
			Leaderboard board = new Leaderboard();
			board.id = leaderboardId;
			board.LoadScoresByUser(user, interval, limit, callback);
		}
	}
}
