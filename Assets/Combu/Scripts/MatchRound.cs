using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Combu
{
	[Serializable]
	public class MatchRound
	{
		public long id = 0;
		public long idMatchAccount = 0;
		public float score = 0;
		public DateTime? dateScore = null;

		public bool hasScore { get { return (dateScore.HasValue && dateScore != null); } }
		
		public MatchRound ()
		{
		}
		public MatchRound (string jsonString)
		{
			FromJson(jsonString);
		}
		public MatchRound (Hashtable data)
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
			if (hash.ContainsKey("IdMatchAccount") && hash["IdMatchAccount"] != null)
			{
				long.TryParse(hash["IdMatchAccount"].ToString(), out idMatchAccount);
			}
			if (hash.ContainsKey("Score") && hash["Score"] != null)
			{
				float.TryParse(hash["Score"].ToString(), out score);
			}
			if (hash.ContainsKey("DateScore") && hash["DateScore"] != null && !string.IsNullOrEmpty(hash["DateScore"].ToString()))
			{
				dateScore = hash ["DateScore"].ToString ().ToDatetime ();
			}
		}
	}
}
