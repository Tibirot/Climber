using UnityEngine;
using System.Collections;

namespace Combu
{
	public class ProfilePlatform
	{
		public string platformKey = "";
		public string platformId = "";

		public ProfilePlatform (Hashtable data)
		{
			if (data == null)
				return;
			if (data.ContainsKey("PlatformKey"))
				platformKey = data["PlatformKey"].ToString();
			if (data.ContainsKey("PlatformId"))
				platformId = data["PlatformId"].ToString();
		}
	}
}
