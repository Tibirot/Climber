using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Combu
{
	[Serializable]
	public class MatchAccount
	{
		public long id = 0;
		public long idMatch = 0;
		public long idAccount = 0;
		public Hashtable customData = new Hashtable();
		public float score = 0;
		public DateTime? dateScore = null;
		public Profile user = null;

		List<MatchRound> _rounds = new List<MatchRound>();
		public List<MatchRound> rounds { get { return _rounds; } }

		public MatchAccount ()
		{
		}
		public MatchAccount (string jsonString)
		{
			FromJson(jsonString);
		}
		public MatchAccount (Hashtable data)
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
			if (hash.ContainsKey("IdMatch") && hash["IdMatch"] != null)
			{
				long.TryParse(hash["IdMatch"].ToString(), out idMatch);
			}
			if (hash.ContainsKey("IdAccount") && hash["IdAccount"] != null)
			{
				long.TryParse(hash["IdAccount"].ToString(), out idAccount);
			}
			if (hash.ContainsKey("CustomData") && hash["CustomData"] != null && hash["CustomData"] is Hashtable)
			{
				customData = (Hashtable)hash["CustomData"];
			}
			if (hash.ContainsKey("User") && hash["User"] != null)
			{
				user = new Profile((Hashtable)hash["User"]);
			}
			if (hash.ContainsKey("Rounds") && hash["Rounds"] != null)
			{
				_rounds.Clear();
				ArrayList listRounds = (ArrayList)hash["Rounds"];
				foreach (Hashtable data in listRounds)
				{
					_rounds.Add(new MatchRound(data));
				}
			}
		}
	}
}
