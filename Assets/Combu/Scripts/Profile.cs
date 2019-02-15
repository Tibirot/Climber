using UnityEngine;
using UnityEngine.SocialPlatforms;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Combu
{
	[System.Serializable]
	public class Profile : IUserProfile
	{
		protected long _id = 0;
		protected string _userName = "";
		[System.NonSerialized]
		protected Texture2D _image;
		protected string _sessionToken = "";
		protected System.DateTime? _lastSeen;

		List<ProfilePlatform> _platforms = new List<ProfilePlatform>();
		public List<ProfilePlatform> platforms { get { return _platforms; } }

		/// <summary>
		/// Gets the identifier value as string.
		/// </summary>
		/// <value>The identifier.</value>
		public string id { get { return _id.ToString(); } }
		/// <summary>
		/// Gets the identifier value as long. id is just a ToString() of idLong, since Ids are stored as long in the database.
		/// </summary>
		/// <value>The identifier long.</value>
		public long idLong { get { return _id; } }
		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>The name of the user.</value>
		public string userName
		{
			get { return _userName; }
			set { _userName = value; }
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="Combu.Profile"/> is a friend of the local user.
		/// </summary>
		/// <value><c>true</c> if is friend; otherwise, <c>false</c>.</value>
		public bool isFriend
		{
			get
			{
				if (CombuManager.isInitialized && CombuManager.instance.isAuthenticated)
				{
					foreach (var friend in CombuManager.localUser.friends)
					{
						if (friend.id.Equals(id))
							return true;
					}
				}
				return false;
			}
		}
		/// <summary>
		/// Gets the online state.
		/// </summary>
		/// <value>The state.</value>
		public virtual UserState state
		{
			get
			{
				if (_lastSeen.HasValue && _lastSeen != null)
				{
					int seconds = (int)(System.DateTime.Now.ToUniversalTime() - _lastSeen.Value).TotalSeconds;
					if (seconds <= CombuManager.instance.onlineSeconds)
					{
						if (CombuManager.instance.playingSeconds > 0 && seconds <= CombuManager.instance.playingSeconds)
							return UserState.Playing;
						return UserState.Online;
					}
				}
				return UserState.Offline;
			}
		}
		/// <summary>
		/// Gets or sets the image.
		/// </summary>
		/// <value>The image.</value>
		public Texture2D image
		{
			get { return _image; }
			set { _image = value; }
		}
		/// <summary>
		/// Gets the session token.
		/// </summary>
		/// <value>The session token.</value>
		public string sessionToken { get { return _sessionToken; } }
		/// <summary>
		/// Gets the last seen date/time.
		/// </summary>
		/// <value>The last seen.</value>
		public System.DateTime? lastSeen { get { return _lastSeen; } }
		/// <summary>
		/// The email address.
		/// </summary>
		public string email;
		/// <summary>
		/// The global custom data shared between apps.
		/// </summary>
        public Hashtable customData = new Hashtable();
        /// <summary>
        /// The custom data for the current app scope.
        /// </summary>
        public Hashtable appCustomData = new Hashtable();

		public Profile() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CBUser"/> class from a JSON formatted string.
		/// </summary>
		/// <param name='jsonString'>
		/// JSON formatted string.
		/// </param>
		public Profile (string jsonString)
		{
			FromJson(jsonString);
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="CBUser"/> class from a Hashtable.
		/// </summary>
		/// <param name='hash'>
		/// Hash.
		/// </param>
		public Profile (Hashtable hash)
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
					long.TryParse(hash["Id"].ToString(), out _id);
				}
				if (hash.ContainsKey("GUID") && hash["GUID"] != null)
				{
					_sessionToken = hash["GUID"].ToString();
				}
				if (hash.ContainsKey("LastSeen") && hash["LastSeen"] != null)
				{
					_lastSeen = null;
					if (!string.IsNullOrEmpty(hash["LastSeen"].ToString()))
					{
						_lastSeen = hash ["LastSeen"].ToString ().ToDatetime ();
					}
				}
				if (hash.ContainsKey("Username") && hash["Username"] != null)
				{
					_userName = hash["Username"].ToString();
				}
				if (hash.ContainsKey("Email") && hash["Email"] != null)
				{
					email = hash["Email"].ToString();
				}
				if (hash.ContainsKey("CustomData") && hash["CustomData"] != null && hash["CustomData"] is Hashtable)
				{
					customData = (Hashtable)hash["CustomData"];
                }
                if (hash.ContainsKey("AppCustomData") && hash["AppCustomData"] != null && hash["AppCustomData"] is Hashtable)
                {
                    appCustomData = (Hashtable)hash["AppCustomData"];
                }
				if (hash.ContainsKey("Platforms") && hash["Platforms"] != null && hash["Platforms"] is ArrayList)
				{
					_platforms.Clear();
					ArrayList listPlatforms = (ArrayList)hash["Platforms"];
					if (listPlatforms != null)
					{
						foreach (Hashtable dataPlatform in listPlatforms)
						{
							_platforms.Add(new ProfilePlatform(dataPlatform));
						}
					}
				}
			}
		}
	}
}