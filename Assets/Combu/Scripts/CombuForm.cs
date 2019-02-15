using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Combu
{
	public class CombuForm
	{
		DateTime now = DateTime.Now;
		Dictionary<string, string> textData = new Dictionary<string, string>();
		Dictionary<string, byte[]> binaryData = new Dictionary<string, byte[]>();

		public CombuForm () {
			textData.Add("app_id", CombuManager.instance.appId);
			textData.Add("app_secret", CombuManager.instance.appSecret);
			textData.Add("sig_time", now.Ticks.ToString ());
		}

		public static implicit operator WWWForm(CombuForm me)
		{
			return me.GetForm ();
		}

		/// <summary>
		/// Adds a string parameter to the form.
		/// </summary>
		/// <param name="fieldName">Field name.</param>
		/// <param name="value">Value.</param>
		public void AddField (string fieldName, string value) {
			if (!string.IsNullOrEmpty(fieldName))
				textData [fieldName] = (string.IsNullOrEmpty(value) ? "" : value);
		}

		/// <summary>
		/// Adds a binary parameter to the form.
		/// </summary>
		/// <param name="fieldName">Field name.</param>
		/// <param name="content">Content.</param>
		public void AddBinaryData (string fieldName, byte[] content) {
			if (!string.IsNullOrEmpty(fieldName))
				binaryData [fieldName] = (content == null ? new byte[0] : content);
		}

		/// <summary>
		/// Gets a string parameter from the form.
		/// </summary>
		/// <returns>The field.</returns>
		/// <param name="fieldName">Field name.</param>
		public string GetField (string fieldName) {
			if (!string.IsNullOrEmpty (fieldName) && textData.ContainsKey (fieldName))
				return textData [fieldName];
			return string.Empty;
		}

		/// <summary>
		/// Gets a binary parameter from the form.
		/// </summary>
		/// <returns>The binary field.</returns>
		/// <param name="fieldName">Field name.</param>
		public byte[] GetBinaryField (string fieldName) {
			if (!string.IsNullOrEmpty (fieldName) && binaryData.ContainsKey (fieldName))
				return binaryData [fieldName];
			return new byte[0];
		}

		/// <summary>
		/// Gets the WWWForm representation to pass to the web services with encrypted data.
		/// </summary>
		/// <returns>The form.</returns>
		public WWWForm GetForm() {
			WWWForm form = new WWWForm ();
			form.AddField ("token", Convert.ToBase64String (Encoding.UTF8.GetBytes (CombuManager.sessionToken)));
			if (!CombuManager.instance.language.Equals("en", StringComparison.CurrentCultureIgnoreCase))
				form.AddField ("lang", string.IsNullOrEmpty(CombuManager.instance.language) ? "en" : CombuManager.instance.language);
			form.AddField ("data", CombuManager.EncryptAES (textData.toJson ()));
			foreach (var fieldName in binaryData.Keys) {
				form.AddBinaryData (fieldName, binaryData [fieldName]);
			}
			return form;
		}
	}
}
