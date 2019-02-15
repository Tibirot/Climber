using System;
using System.Collections;


namespace Combu
{
    /// <summary>
    /// Class to handle Combu server informations.
    /// </summary>
    public class CombuServerInfo
    {
        public string version = string.Empty;
        public bool requireUpdate;
        public DateTime time = DateTime.MinValue;
        public Hashtable settings = new Hashtable();
        public bool responseEncrypted;

        public CombuServerInfo()
        {
        }

        public CombuServerInfo(Hashtable data)
        {
            if (data == null)
            {
                return;
            }
            if (data.ContainsKey("version") && data["version"] != null)
            {
				version = data["version"].ToString();
            }
            if (data.ContainsKey("time") && data["time"] != null)
            {
                var t = data["time"].ToString().ToDatetime();
                if (t.HasValue)
                    time = t.Value;
            }
            if (data.ContainsKey("requireUpdate") && data["requireUpdate"] != null)
            {
                if (!bool.TryParse(data["requireUpdate"].ToString(), out requireUpdate))
                {
                    requireUpdate = false;
                }
            }
            if (data.ContainsKey("settings") && data["settings"] != null)
            {
				settings = data["settings"].ToString().hashtableFromJson();
            }
            if (data.ContainsKey("responseEncrypted") && data["responseEncrypted"] != null)
            {
                if (!bool.TryParse(data["responseEncrypted"].ToString(), out responseEncrypted))
                {
                    responseEncrypted = false;
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:Combu.CombuServerInfo"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:Combu.CombuServerInfo"/>.</returns>
        public override string ToString()
        {
            string versionCompare;
            switch (string.Compare(version, CombuManager.COMBU_VERSION, StringComparison.InvariantCultureIgnoreCase))
            {
                case -1:
                    versionCompare = "lower: " + CombuManager.COMBU_VERSION;
                    break;
                case 1:
                    versionCompare = "greater: " + CombuManager.COMBU_VERSION;
                    break;
                default:
                    versionCompare = "match";
                    break;
            }
            return string.Format("[Combu Server Info] Version: {0} | Time: {1} | Update required: {2}",
                                 version + " (" + versionCompare + ")",
                                 time.ToString("yyyy-MM-dd HH:mm"),
                                 requireUpdate);
        }
    }
}
