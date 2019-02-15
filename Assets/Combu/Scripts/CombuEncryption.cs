using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Xml;
using System.Globalization;

#if NETFX_CORE || UNITY_WSA
using UnityEngine.Windows;
#if NETFX_CORE
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
#endif
#endif

namespace Combu
{
	public class CombuEncryption
	{
		string token = "";
		string rsaXml = "";
		string rsaModulus = "";
		string rsaExponent = "";
		byte[] aesKey = new byte[0];
		byte[] aesIV = new byte[0];

		public string Token { get { return token; } }
		public string Key { get { return Convert.ToBase64String (aesKey); } }
		public string IV { get { return Convert.ToBase64String (aesIV); } }

		public CombuEncryption ()
		{
			GenerateAES ();
		}

		/// <summary>
		/// Sets the session token.
		/// </summary>
		/// <param name="sessionToken">Session token.</param>
		public void SetToken (string sessionToken)
		{
			token = sessionToken;
		}

		/// <summary>
		/// Sets the session token and loads the RSA data from XML (Public Key).
		/// </summary>
		/// <param name="sessionToken">Session token.</param>
		/// <param name="xml">RSA Xml.</param>
		public void LoadRSA (string sessionToken, string xml)
		{
			// Set the session token
			try {
				token = Encoding.UTF8.GetString(Convert.FromBase64String(sessionToken));
			}
			catch (Exception e) {
				token = "";
				Debug.LogWarning ("Could not decrypt Session Token: " + e.Message);
			}

			// Reset the RSA data
			rsaXml = string.Empty;
			rsaModulus = string.Empty;
			rsaExponent = string.Empty;

			// Read RSA data from XML
			if (!string.IsNullOrEmpty (xml)) {
				try {
					rsaXml = Encoding.UTF8.GetString(Convert.FromBase64String(xml));
					if (!string.IsNullOrEmpty (rsaXml)) {
						try {
							var xmlDoc = new XmlDocument ();
							XmlNodeList nodes;
							xmlDoc.LoadXml(rsaXml);
							nodes = xmlDoc.GetElementsByTagName("Modulus");
							if (nodes != null && nodes.Count > 0)
								rsaModulus = nodes[0].InnerText;
							nodes = xmlDoc.GetElementsByTagName("Exponent");
							if (nodes != null && nodes.Count > 0)
								rsaExponent = nodes[0].InnerText;
						} catch (Exception e) {
							rsaModulus = "";
							rsaExponent = "";
							Debug.LogWarning ("Could not get Modulus and Exponent from the Public Key: " + e.Message);
						}
					}
				} catch (Exception e) {
					rsaXml = string.Empty;
					rsaModulus = string.Empty;
					rsaExponent = string.Empty;
					Debug.LogWarning ("Could not decrypt Public Key: " + e.Message);
				}
			}
		}

		/// <summary>
		/// Sets the session token and loads the Modulus and Exponent of the RSA Public Key.
		/// </summary>
		/// <param name="sessionToken">Session token.</param>
		/// <param name="modulus">Modulus.</param>
		/// <param name="exponent">Exponent.</param>
		public void LoadRSA (string sessionToken, string modulus, string exponent)
		{
			LoadRSA (sessionToken, string.Empty);
			rsaModulus = modulus;
			rsaExponent = exponent;
		}

		/// <summary>
		/// Generates the AES Key and IV.
		/// </summary>
		void GenerateAES ()
		{
			// https://msdn.microsoft.com/it-it/library/system.security.cryptography.rijndaelmanaged(v=vs.110).aspx
			using (var aes = new RijndaelManaged ()) {
				aes.GenerateIV ();
				aes.GenerateKey ();
				aesKey = aes.Key;
				aesIV = aes.IV;
			}
		}

		/// <summary>
		/// Encrypts a string with RSA.
		/// </summary>
		/// <returns>The RS.</returns>
		/// <param name="inputString">Text to encrypt.</param>
		public string EncryptRSA (string inputString)
		{
			string encryptedText = "";
			if (!string.IsNullOrEmpty (token) && (!string.IsNullOrEmpty (rsaXml) || (!string.IsNullOrEmpty (rsaModulus) && !string.IsNullOrEmpty (rsaExponent)))) {
				try {
					byte[] encryptedData = new byte[0];

					#if NETFX_CORE

					var asnMessage = AsnKeyBuilder.PublicKeyToX509(Convert.FromBase64String(rsaModulus), Convert.FromBase64String(rsaExponent));
					var cryptoBuffer = CryptographicBuffer.CreateFromByteArray(asnMessage.GetBytes());

					var asym = AsymmetricKeyAlgorithmProvider.OpenAlgorithm(AsymmetricAlgorithmNames.RsaOaepSha1);
					var key = asym.ImportPublicKey(cryptoBuffer, CryptographicPublicKeyBlobType.X509SubjectPublicKeyInfo);

					var plainBuffer = CryptographicBuffer.ConvertStringToBinary(inputString, BinaryStringEncoding.Utf8);
					var encryptedBuffer = CryptographicEngine.Encrypt(key, plainBuffer, null);

					CryptographicBuffer.CopyToByteArray(encryptedBuffer, out encryptedData);

					#else

					RSACryptoServiceProvider RSA = new RSACryptoServiceProvider (2048);
					if (!string.IsNullOrEmpty (rsaXml)) {
						// Import the Public Key XML
						// https://msdn.microsoft.com/en-us/library/system.security.cryptography.rsa.fromxmlstring(v=vs.110).aspx
						RSA.FromXmlString (rsaXml);
					} else if (!string.IsNullOrEmpty (rsaModulus) && !string.IsNullOrEmpty (rsaExponent)) {
						// Import he RSA Parameters
						// https://msdn.microsoft.com/en-us/library/system.security.cryptography.rsa.importparameters(v=vs.110).aspx
						RSAParameters RSAParams = new RSAParameters ();
						RSAParams.Modulus = Convert.FromBase64String (rsaModulus);
						RSAParams.Exponent = Convert.FromBase64String (rsaExponent);
						RSA.ImportParameters (RSAParams);
					}

					encryptedData = RSA.Encrypt (Encoding.UTF8.GetBytes (inputString), true);

					#endif

					encryptedText = Convert.ToBase64String (encryptedData);
				} catch (Exception e) {
					Debug.LogError ("Error encrypting RSA: " + e.Message);
				}
			} else {
				Debug.LogError ("Session Token or RSA data have not been set");
			}
			return encryptedText;
		}

		/// <summary>
		/// Encrypts a string with AES.
		/// </summary>
		/// <returns>The AE.</returns>
		/// <param name="inputString">Input string.</param>
		public string EncryptAES (string inputString)
		{
			// https://msdn.microsoft.com/it-it/library/system.security.cryptography.rijndaelmanaged(v=vs.110).aspx
			byte[] encrypted;
			using (var aes = new RijndaelManaged ()) {
				
				// Set the Key and IV already generated
				aes.Key = aesKey;
				aes.IV = aesIV;

				// Create an encrytor to perform the stream transform.
				ICryptoTransform encryptor = aes.CreateEncryptor (aes.Key, aes.IV);

				// Create the streams used for encryption.
				using (MemoryStream msEncrypt = new MemoryStream ()) {
					using (CryptoStream csEncrypt = new CryptoStream (msEncrypt, encryptor, CryptoStreamMode.Write)) {
						// Write all data to the stream.
						using (StreamWriter swEncrypt = new StreamWriter (csEncrypt)) {
							swEncrypt.Write (inputString);
						}
						encrypted = msEncrypt.ToArray ();
					}
				}
			}
			return Convert.ToBase64String (encrypted);
		}

		/// <summary>
		/// Decrypts a string what was encrypted with the current AES Key/IV.
		/// </summary>
		/// <returns>The decrypted string.</returns>
		/// <param name="inputString">Input string.</param>
		public string DecryptAES (string inputString)
		{
			string decrypted = "";
			using (var aes = new RijndaelManaged ()) {
				
				// Set the Key and IV already generated
				aes.Key = aesKey;
				aes.IV = aesIV;

				try {
					// Create a decrytor to perform the stream transform.
					var decrypt = aes.CreateDecryptor (aes.Key, aes.IV);

					// Create the streams used for decryption.
					byte[] xBuff = null;
					using (var ms = new MemoryStream ()) {
						// Write all data to the stream.
						using (var cs = new CryptoStream (ms, decrypt, CryptoStreamMode.Write)) {
							byte[] xXml = Convert.FromBase64String (inputString);
							cs.Write (xXml, 0, xXml.Length);
						}
						xBuff = ms.ToArray ();
					}

					decrypted = Encoding.UTF8.GetString (xBuff);
				}
				catch (Exception e) {
					Debug.LogError ("DecryptAES error: " + e.Message);
					decrypted = "";
				}
			}
			return decrypted;
		}

		/// <summary>
		/// Converts computed hash to string.
		/// </summary>
		/// <returns>The hash as string.</returns>
		/// <param name="strBuilder">String builder.</param>
		/// <param name="result">Content in bytes.</param>
		string HashToString (byte[] result)
		{
			var strBuilder = new StringBuilder();
			for (int i = 0; i < result.Length; i++) {
				// Change it into 2 hexadecimal digits for each byte
				strBuilder.Append (result [i].ToString ("x2"));
			}
			return strBuilder.ToString();
		}

		/// <summary>
		/// Encrypts a string in MD5 hash.
		/// </summary>
		/// <returns>The MD5 encrypted string.</returns>
		/// <param name="inputString">Input string.</param>
		public string EncryptMD5 (string inputString)
		{
			if (!string.IsNullOrEmpty (inputString)) {
				byte[] result;

				#if UNITY_WSA

				result = Crypto.ComputeMD5Hash(Encoding.UTF8.GetBytes(inputString));

				#else

				var encrypt = new MD5CryptoServiceProvider ();
				// Compute the hash from the bytes of text
				encrypt.ComputeHash (Encoding.UTF8.GetBytes (inputString));
				// Get the hash result after compute it
				result = encrypt.Hash;

				#endif

				return HashToString (result);
			}
			return "";
		}

		/// <summary>
		/// Encrypts a string in SHA1 hash.
		/// </summary>
		/// <returns>The SHA1 encrypted.</returns>
		/// <param name="inputString">Input string.</param>
		public string EncryptSHA1 (string inputString)
		{
			if (!string.IsNullOrEmpty (inputString)) {
				byte[] result;

				#if UNITY_WSA

				result = Crypto.ComputeSHA1Hash (Encoding.UTF8.GetBytes(inputString));

				#else

				var encrypt = new SHA1CryptoServiceProvider ();
				// Compute the hash from the bytes of text
				encrypt.ComputeHash (Encoding.UTF8.GetBytes (inputString));
				// Get the hash result after compute it
				result = encrypt.Hash;

				#endif

				return HashToString (result);
			}
			return "";
		}

		/// <summary>
		/// Decrypts the response from server.
		/// </summary>
		/// <returns>The response decrypted.</returns>
		/// <param name="text">Response text.</param>
		public string DecryptResponse(string text)
		{
			string decrypted = text;
			if (CombuManager.instance.serverInfo != null && CombuManager.instance.serverInfo.responseEncrypted) {
				Hashtable json = text.hashtableFromJson ();
				string data = (json != null ? json ["d"] + "" : "");
				DateTime time;
				if (json != null && !string.IsNullOrEmpty (data) && DateTime.TryParseExact (json ["t"] + "", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out time)) {
					// Decrypt the response data with the AES keys
					string decryptedData = DecryptAES (data);
					if (!string.IsNullOrEmpty (decryptedData)) {
						decrypted = decryptedData;
						// Update the server info time to the latest received
						CombuManager.instance.serverInfo.time = time;
					}
				}
			}
			return decrypted;
		}
	}
}