using UnityEngine;
using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;

namespace Combu
{
	public static class CombuUtils
	{
        /// <summary>
        /// The string for boolean true value.
        /// </summary>
        public const string STRING_TRUE = "TRUE";

        /// <summary>
        /// The string for boolean false value.
        /// </summary>
        public const string STRING_FALSE = "FALSE";

        /// <summary>
        /// Converts a string into a nullable datetime.
        /// </summary>
        /// <returns>The datetime.</returns>
        /// <param name="me">Me.</param>
        /// <param name="format">Format.</param>
        /// <param name="culture">Culture.</param>
		public static DateTime? ToDatetime(this string me, string format = "yyyy-MM-dd HH:mm:ss", CultureInfo culture = null)
		{
			if (culture == null)
				culture = CultureInfo.InvariantCulture;
			DateTime date;
			if (DateTime.TryParseExact (me, format, culture, DateTimeStyles.None, out date))
				return date;
			return null;
		}

        /// <summary>
        /// Converts a string into a datetime.
        /// </summary>
        /// <returns>The datetime or default.</returns>
        /// <param name="me">Me.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <param name="format">Format.</param>
        /// <param name="culture">Culture.</param>
		public static DateTime ToDatetimeOrDefault(this string me, DateTime? defaultValue = null, string format = "yyyy-MM-dd HH:mm:ss", CultureInfo culture = null)
		{
			DateTime? date = me.ToDatetime(format, culture);
			if (date.HasValue)
				return date.Value;
			return (defaultValue.HasValue ? defaultValue.Value : DateTime.MinValue);
		}

        /// <summary>
        /// Compare two version strings and returns whether the other version is higher than the current.
        /// </summary>
        /// <returns><c>true</c>, if higher version was ised, <c>false</c> otherwise.</returns>
        /// <param name="currentVersionNumber">Current version number.</param>
        /// <param name="otherVersionNumber">Other version number.</param>
        public static bool IsHigherVersion(string currentVersionNumber, string otherVersionNumber)
        {
            // Fail if at least one is empty
            if (string.IsNullOrEmpty(currentVersionNumber) || string.IsNullOrEmpty(otherVersionNumber))
            {
                return false;
            }
            // Fail if they're equals (case insentitive)
            if (string.Equals(currentVersionNumber, otherVersionNumber, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
            // Split remote and local version by "." and convert to lower-case
            List<string> versionOther = new List<string>(currentVersionNumber.Split('.'));
            List<string> versionLocal = new List<string>(otherVersionNumber.Split('.'));
            // Make sure that the array of remote and local versions have the same size
            while (versionOther.Count < versionLocal.Count)
            {
                versionOther.Add("");
            }
            while (versionLocal.Count < versionOther.Count)
            {
                versionLocal.Add("");
            }
            // Compare the remote mayor/minor/revision/build with the local version
            for (int i = 0; i < versionOther.Count; ++i)
            {
                // Remove "beta" string from the current mayor/minor/revision/build number
                string versionOtherNumber = versionOther[i].Replace("beta", "");
                string localRemoteNumber = versionLocal[i].Replace("beta", "");
                // Add '0' to the left of the current mayor/minor/revision/build to transform it like '000'
                versionOtherNumber = versionOtherNumber.PadLeft(3, '0');
                localRemoteNumber = localRemoteNumber.PadLeft(3, '0');
                // Compare the current mayor/minor/revision/build
                if (string.Compare(versionOtherNumber, localRemoteNumber, true) > 0)
                    return true;
            }
            return false;
        }
	}
}