////////////////////////////////////////////////////////////////////////////////
//  
// @module Common Android Native Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SA.Foundation.Network.Web;



namespace SA.Facebook
{

	/// <summary>
	/// Facebook user model
	/// Contains parsed user fields from a Facebook API responce,
	/// and additional methods to retirive mode data based on current model state
	/// like for example generate user avatar url, etc
	/// </summary>
	/// <param name="uri">The URI of the resource to retrieve via HTTP GET.</param>
	/// <param name="callback">Callback will be fired once request is completed</param>
	public class SA_FB_User
	{

		private string m_id = string.Empty;
		private string m_name = string.Empty;
		private string m_first_name = string.Empty;
		private string m_last_name = string.Empty;
		private string m_username = string.Empty;

		private string m_profile_url = string.Empty;
		private string m_email = string.Empty;

		private string m_location = string.Empty;
		private string m_locale = string.Empty;

		private string m_rawJSON = string.Empty;

		private DateTime m_birthday = new DateTime();
		private SA_FB_Gender m_gender = SA_FB_Gender.Male;

		private string m_ageRange = string.Empty;
		private string m_picUrl = string.Empty;

		private Dictionary<SA_FB_ProfileImageSize, string> m_picturesUrls = new Dictionary<SA_FB_ProfileImageSize, string>();
		
		//--------------------------------------
		// INITIALIZE
		//--------------------------------------

		public SA_FB_User(IDictionary JSON)
		{
			ParseData(JSON);

		}


		//--------------------------------------
		//  PUBLIC METHODS
		//--------------------------------------


		/// <summary>
		/// Generates user profile image URL
		/// </summary>
		/// <param name="size">Requested profile image size</param>
		
		public void GetProfileURL(SA_FB_ProfileImageSize size, Action<string> callback)
		{
			if (m_picturesUrls.ContainsKey(size)) {
				callback.Invoke(m_picturesUrls[size]);
				return;
			}

			SA.Facebook.SA_FB.GraphAPI.ResolveProfileImageUrl(Id, size, (url) => {
				m_picturesUrls.Add(size, url);
				callback.Invoke(url);
			});
		}

		/// <summary>
		/// Download user profile image of a given size
		/// </summary>
		/// <param name="size">Requested profile image size</param>
		/// <param name="callback">Callback with user Texture2D profile image</param>
		public void GetProfileImage(SA_FB_ProfileImageSize size, Action<Texture2D> callback) {
			GetProfileURL(size, url => { SA_CachedRequestsFactory.GetTexture2D(url, callback); });
		}

		//--------------------------------------
		//  GET/SET
		//--------------------------------------

		public string RawJSON
		{
			get
			{
				return m_rawJSON;
			}
		}


		public string Id
		{
			get
			{
				return m_id;
			}
		}

		public DateTime Birthday
		{
			get
			{
				return m_birthday;
			}
		}

		public string Name
		{
			get
			{
				return m_name;
			}
		}

		public string FirstName
		{
			get
			{
				return m_first_name;
			}
		}

		public string LastName
		{
			get
			{
				return m_last_name;
			}
		}


		public string UserName
		{
			get
			{
				return m_username;
			}
		}


		public string ProfileUrl
		{
			get
			{
				return m_profile_url;
			}
		}

		public string Email
		{
			get
			{
				return m_email;
			}
		}


		public string Locale
		{
			get
			{
				return m_locale;
			}
		}

		public string Location
		{
			get
			{
				return m_location;
			}
		}


		public SA_FB_Gender Gender
		{
			get
			{
				return m_gender;
			}
		}

		public string AgeRange
		{
			get
			{
				return m_ageRange;
			}
		}

		public string PictureUrl
		{
			get
			{
				return m_picUrl;
			}
		}


		//--------------------------------------
		//  PRIVATE METHODS
		//--------------------------------------


		private void ParseData(IDictionary JSON)
		{



			if(JSON.Contains("id"))
			{
				m_id = Convert.ToString(JSON["id"]);
			}


			if(JSON.Contains("birthday"))
			{
				m_birthday = DateTime.Parse(Convert.ToString(JSON["birthday"]));
			}


			if(JSON.Contains("name"))
			{
				m_name = Convert.ToString(JSON["name"]);
			}

			if(JSON.Contains("first_name"))
			{
				m_first_name = Convert.ToString(JSON["first_name"]);
			}

			if(JSON.Contains("last_name"))
			{
				m_last_name = Convert.ToString(JSON["last_name"]);
			}

			if(JSON.Contains("username"))
			{
				m_username = Convert.ToString(JSON["username"]);
			}

			if(JSON.Contains("link"))
			{
				m_profile_url = Convert.ToString(JSON["link"]);
			}

			if(JSON.Contains("email"))
			{
				m_email = Convert.ToString(JSON["email"]);
			}

			if(JSON.Contains("locale"))
			{
				m_locale = Convert.ToString(JSON["locale"]);
			}

			if(JSON.Contains("location"))
			{
				IDictionary loc = JSON["location"] as IDictionary;
				m_location = System.Convert.ToString(loc["name"]);
			}

			if(JSON.Contains("gender"))
			{
				string g = System.Convert.ToString(JSON["gender"]);
				if(g.Equals("male"))
				{
					m_gender = SA_FB_Gender.Male;
				}
				else
				{
					m_gender = SA_FB_Gender.Female;
				}
			}

			if(JSON.Contains("age_range"))
			{
				IDictionary age = JSON["age_range"] as IDictionary;
				m_ageRange = (age.Contains("min")) ? age["min"].ToString() : "0";
				m_ageRange += "-";
				m_ageRange += (age.Contains("max")) ? age["max"].ToString() : "1000";
			}


			if(JSON.Contains("picture"))
			{
				IDictionary picDict = JSON["picture"] as IDictionary;
				if(picDict != null && picDict.Contains("data"))
				{
					IDictionary data = picDict["data"] as IDictionary;
					if(data != null && data.Contains("url"))
					{
						m_picUrl = Convert.ToString(data["url"]);
					}
				}
			}

		}

	}
}
