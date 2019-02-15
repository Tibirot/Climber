using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Combu
{
	[System.Serializable]
	public class Mail
	{
		public long id = 0;
		public DateTime sendDate = DateTime.MinValue;
		public DateTime readDate = DateTime.MinValue;
		public string subject = "";
		public string message = "";
		public bool isPublic = false;
		public User fromUser;
		public User toUser;
		public long idGroup = 0;
		public UserGroup toGroup;
		
		public bool isRead { get { return (readDate != DateTime.MinValue); } }
		
		public Mail ()
		{
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
					long.TryParse(hash["Id"].ToString(), out id);
				}
				if (hash.ContainsKey("Subject") && hash["Subject"] != null)
				{
					subject = hash["Subject"].ToString();
				}
				if (hash.ContainsKey("Message") && hash["Message"] != null)
				{
					message = hash["Message"].ToString();
				}
				if (hash.ContainsKey("SendDate") && hash["SendDate"] != null && !string.IsNullOrEmpty(hash["SendDate"].ToString()))
				{
					sendDate = hash ["SendDate"].ToString ().ToDatetimeOrDefault ();
				}
				if (hash.ContainsKey("ReadDate") && hash["ReadDate"] != null && !string.IsNullOrEmpty(hash["ReadDate"].ToString()))
				{
					readDate = hash ["ReadDate"].ToString ().ToDatetimeOrDefault ();
				}
				if (hash.ContainsKey("IsPublic") && hash["IsPublic"] != null)
				{
					bool.TryParse(hash["IsPublic"].ToString(), out isPublic);
				}
				if (hash.ContainsKey("From") && hash["From"] != null)
				{
					fromUser = new User(hash["From"].ToString());
				}
				if (hash.ContainsKey("To") && hash["To"] != null)
				{
					toUser = new User(hash["To"].ToString());
				}
				if (hash.ContainsKey("IdGroup") && hash["IdGroup"] != null)
				{
					long.TryParse(hash["IdGroup"].ToString(), out idGroup);
				}
				if (hash.ContainsKey("ToGroup") && hash["ToGroup"] != null)
				{
					toGroup = new UserGroup(hash["ToGroup"] is Hashtable ? (Hashtable)hash["ToGroup"] : hash["ToGroup"].ToString().hashtableFromJson());
				}
			}
		}

		public static void Load (eMailList listType, int pageNumber, int limit, System.Action<Mail[], int, int, string> callback)
		{
			Load<Mail>(listType, 0, 0, 0, pageNumber, limit, callback);
		}
		public static void Load (eMailList listType, long idRecipient, long idSender, long idGroup, int pageNumber, int limit, System.Action<Mail[], int, int, string> callback)
		{
			Load<Mail>(listType, idRecipient, idSender, idGroup, pageNumber, limit, callback);
		}
		/// <summary>
		/// Load mails list from specified page and number of records.
		/// </summary>
		/// <param name="pageNumber">Page number.</param>
		/// <param name="limit">Limit.</param>
		/// <typeparam name="T">Type of returned objects.</typeparam>
		public static void Load<T> (eMailList listType, long idRecipient, long idSender, long idGroup, int pageNumber, int limit, System.Action<T[], int, int, string> callback) where T: Mail, new()
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "list");
			form.AddField("Type", ((int)listType).ToString());
			form.AddField("IdRecipient", idRecipient.ToString());
			form.AddField("IdSender", idSender.ToString());
			form.AddField("IdGroup", idGroup.ToString());
			form.AddField("Limit", limit.ToString());
			form.AddField("Page", pageNumber.ToString());
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("mail.php"), form, (string text, string error) => {
				List<T> mails = new List<T>();
				int count = 0, pagesCount = 0;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null && result.ContainsKey("total"))
					{
						count = int.Parse(result["total"].ToString());
						pagesCount = int.Parse(result["pages"].ToString());
						ArrayList list = (ArrayList)result["results"];
						if (list != null)
						{
							foreach (Hashtable data in list)
							{
								// Create a new object from the result
								T article = new T();
								article.FromHashtable(data);
								// Add to the list
								mails.Add(article);
							}
						}	
					}
				}
				if (callback != null)
					callback(mails.ToArray(), count, pagesCount, error);
			});
		}

		/// <summary>
		/// Sends a mail to a user.
		/// </summary>
        /// <param name="recipientId">Identifier Id of the destination user.</param>
		/// <param name="subject">Subject.</param>
		/// <param name="message">Message.</param>
		/// <param name="isPublic">If set to <c>true</c> is public.</param>
		public static void Send (long recipientId, string subject, string message, bool isPublic, System.Action<bool, string> callback)
		{
			Send(new long[] { recipientId }, null, 0, subject, message, isPublic, callback);
		}
		/// <summary>
		/// Sends the mail to multiple users.
		/// </summary>
		/// <param name="recipientsId">Recipients identifier.</param>
		/// <param name="subject">Subject.</param>
		/// <param name="message">Message.</param>
		/// <param name="isPublic">If set to <c>true</c> is public.</param>
		public static void Send (long[] recipientsId, string subject, string message, bool isPublic, System.Action<bool, string> callback)
		{
			Send(recipientsId, null, 0, subject, message, isPublic, callback);
		}
		/// <summary>
		/// Sends a mail to a user.
		/// </summary>
        /// <param name="recipientUsername">Username of the destination user.</param>
		/// <param name="subject">Subject.</param>
		/// <param name="message">Message.</param>
		/// <param name="isPublic">If set to <c>true</c> is public.</param>
		public static void Send (string recipientUsername, string subject, string message, bool isPublic, System.Action<bool, string> callback)
		{
			Send(null, new string[] { recipientUsername }, 0, subject, message, isPublic, callback);
		}
		/// <summary>
		/// Sends the mail to multiple users.
		/// </summary>
		/// <param name="recipientsUsername">Recipients username.</param>
		/// <param name="subject">Subject.</param>
		/// <param name="message">Message.</param>
		/// <param name="isPublic">If set to <c>true</c> is public.</param>
		public static void Send (string[] recipientsUsername, string subject, string message, bool isPublic, System.Action<bool, string> callback)
		{
			Send(null, recipientsUsername, 0, subject, message, isPublic, callback);
		}
		/// <summary>
		/// Sends the mail to group.
		/// </summary>
		/// <param name="groupId">Group identifier.</param>
		/// <param name="subject">Subject.</param>
		/// <param name="message">Message.</param>
		/// <param name="isPublic">If set to <c>true</c> is public.</param>
		public static void SendMailToGroup (long groupId, string subject, string message, bool isPublic, System.Action<bool, string> callback)
		{
			Send(null, null, groupId, subject, message, isPublic, callback);
		}
		/// <summary>
		/// Sends the mail.
		/// </summary>
		/// <param name="recipientsId">Recipients identifier.</param>
		/// <param name="recipientsUsername">Recipients username.</param>
		/// <param name="recipientGroupId">Recipient group identifier.</param>
		/// <param name="subject">Subject.</param>
		/// <param name="message">Message.</param>
		/// <param name="isPublic">If set to <c>true</c> is public.</param>
		protected static void Send (long[] recipientsId, string[] recipientsUsername, long recipientGroupId, string subject, string message, bool isPublic, System.Action<bool, string> callback)
		{
            SendMessage(recipientsId, recipientsUsername, recipientGroupId, subject, message, isPublic, (bool success, string savedMessage, string error) => {
                if (callback != null)
                    callback(success, error);
            });
		}

        /// <summary>
        /// Gets the message form.
        /// </summary>
        /// <returns>The message form.</returns>
        /// <param name="recipientsId">Recipients identifier.</param>
        /// <param name="recipientsUsername">Recipients username.</param>
        /// <param name="recipientGroupId">Recipient group identifier.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="message">Message.</param>
        /// <param name="isPublic">If set to <c>true</c> is public.</param>
        protected static CombuForm GetMessageForm(long[] recipientsId, string[] recipientsUsername, long recipientGroupId, string subject, string message, bool isPublic)
        {
            var form = CombuManager.instance.CreateForm();
            form.AddField("action", "send");
            if (recipientGroupId > 0)
            {
                form.AddField("IdGroup", recipientGroupId.ToString());
            }
            else if (recipientsId != null && recipientsId.Length > 0)
            {
                string recipients = "";
                foreach (long idUser in recipientsId)
                {
                    recipients += (recipients == "" ? "" : ",") + idUser;
                }
                form.AddField("Id", recipients);
            }
            else if (recipientsUsername != null && recipientsUsername.Length > 0)
            {
                form.AddField("Username", string.Join(",", recipientsUsername));
            }
            form.AddField("Subject", subject);
            form.AddField("Message", message);
            form.AddField("IsPublic", isPublic ? "1" : "0");
            return form;
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="recipientsId">Recipients identifier.</param>
        /// <param name="recipientsUsername">Recipients username.</param>
        /// <param name="recipientGroupId">Recipient group identifier.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="message">Message.</param>
        /// <param name="isPublic">If set to <c>true</c> is public.</param>
        /// <param name="callback">Callback.</param>
        protected static void SendMessage(long[] recipientsId, string[] recipientsUsername, long recipientGroupId, string subject, string message, bool isPublic, Action<bool, string, string> callback)
        {
            CombuForm form = GetMessageForm(recipientsId, recipientsUsername, recipientGroupId, subject, message, isPublic);
            CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("mail.php"), form, (string text, string error) => {
                string savedMessage = "";
                bool success = false;
                if (string.IsNullOrEmpty(error))
                {
                    Hashtable result = text.hashtableFromJson();
                    if (result != null && result.ContainsKey("success"))
                    {
                        if (bool.TryParse(result["success"].ToString(), out success) && result.ContainsKey("message"))
                        {
                            if (success)
                            {
                                savedMessage = result["message"].ToString();
                            }
                            else
                            {
                                error = result["message"].ToString();
                            }
                        }
                    }
                }
                if (callback != null)
                    callback(success, savedMessage, error);
            });
        }

        /// <summary>
        /// Sends a mail to a user.
        /// </summary>
        /// <param name="recipientId">Identifier Id of the destination user.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="message">Message.</param>
        /// <param name="isPublic">If set to <c>true</c> is public.</param>
        public static void Send (long recipientId, string subject, string message, bool isPublic, Action<Mail, string> callback)
        {
            Send(new long[] { recipientId }, null, 0, subject, message, isPublic, callback);
        }
        /// <summary>
        /// Sends the mail to multiple users.
        /// </summary>
        /// <param name="recipientsId">Recipients identifier.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="message">Message.</param>
        /// <param name="isPublic">If set to <c>true</c> is public.</param>
        public static void Send (long[] recipientsId, string subject, string message, bool isPublic, Action<Mail, string> callback)
        {
            Send(recipientsId, null, 0, subject, message, isPublic, callback);
        }
        /// <summary>
        /// Sends a mail to a user.
        /// </summary>
        /// <param name="recipientUsername">Username of the destination user.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="message">Message.</param>
        /// <param name="isPublic">If set to <c>true</c> is public.</param>
        public static void Send (string recipientUsername, string subject, string message, bool isPublic, Action<Mail, string> callback)
        {
            Send(null, new string[] { recipientUsername }, 0, subject, message, isPublic, callback);
        }
        /// <summary>
        /// Sends the mail to multiple users.
        /// </summary>
        /// <param name="recipientsUsername">Recipients username.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="message">Message.</param>
        /// <param name="isPublic">If set to <c>true</c> is public.</param>
        public static void Send (string[] recipientsUsername, string subject, string message, bool isPublic, Action<Mail, string> callback)
        {
            Send(null, recipientsUsername, 0, subject, message, isPublic, callback);
        }
        /// <summary>
        /// Sends the mail to group.
        /// </summary>
        /// <param name="groupId">Group identifier.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="message">Message.</param>
        /// <param name="isPublic">If set to <c>true</c> is public.</param>
        public static void SendMailToGroup (long groupId, string subject, string message, bool isPublic, Action<Mail, string> callback)
        {
            Send(null, null, groupId, subject, message, isPublic, callback);
        }
        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="recipientsId">Recipients identifier.</param>
        /// <param name="recipientsUsername">Recipients username.</param>
        /// <param name="recipientGroupId">Recipient group identifier.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="message">Message.</param>
        /// <param name="isPublic">If set to <c>true</c> is public.</param>
        protected static void Send(long[] recipientsId, string[] recipientsUsername, long recipientGroupId, string subject, string message, bool isPublic, Action<Mail, string> callback)
        {
            SendMessage(recipientsId, recipientsUsername, recipientGroupId, subject, message, isPublic, (bool success, string savedMessageJson, string error) => {
                if (callback != null)
                {
                    Mail savedMessage = null;
                    if (success)
                    {
                        savedMessage = new Mail();
                        savedMessage.FromJson(savedMessageJson);
                    }
                    callback(savedMessage, error);
                }
            });
        }

        /// <summary>
        /// Sends this message and auto-loads the stored data from server on callback (including id after creation).
        /// </summary>
        /// <param name="callback">Callback.</param>
        //public void Send(Action<bool, string> callback)
        //{
        //    List<long> recipientsId = new List<long>();
        //    List<string> recipientsUsername = new List<string>();
        //    long recipientGroupId = 0;
        //    if (toUser != null && (toUser.idLong > 0 || !string.IsNullOrEmpty(toUser.userName)))
        //    {
        //        if (toUser.idLong > 0)
        //        {
        //            recipientsId.Add(toUser.idLong);
        //        }
        //        else
        //        {
        //            recipientsUsername.Add(toUser.userName);
        //        }
        //    }
        //    else if (toGroup != null)
        //    {
        //        if (toGroup.id > 0)
        //        {
        //            recipientGroupId = toGroup.id;
        //        }
        //        else if (toGroup.users != null && toGroup.users.Length > 0)
        //        {
        //            foreach (User user in toGroup.users)
        //            {
        //                if (user.idLong > 0)
        //                {
        //                    recipientsId.Add(user.idLong);
        //                }
        //                else if (!string.IsNullOrEmpty(user.userName))
        //                {
        //                    recipientsUsername.Add(user.userName);
        //                }
        //            }
        //        }
        //    }
        //    if (recipientsId.Count > 0 || recipientsUsername.Count > 0 || recipientGroupId > 0)
        //    {
        //        SendMessage(recipientsId.ToArray(), recipientsUsername.ToArray(), recipientGroupId, subject, message, isPublic, (bool success, string savedMessage, string error) =>
        //        {
        //            if (success)
        //            {
        //                FromJson(savedMessage);
        //            }
        //            if (callback != null)
        //                callback(success, error);
        //        });
        //    }
        //}

		/// <summary>
		/// Mark this mail as read.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public void Read (Action<bool, string> callback)
		{
			Read(id, (bool success, string error) => {
				if (success)
					readDate = DateTime.Now;
				if (callback != null)
					callback(success, error);
			});
		}
		/// <summary>
		/// Mark a single mail as read.
		/// </summary>
		/// <param name="idMail">Identifier mail.</param>
		/// <param name="callback">Callback.</param>
		public static void Read (long idMail, Action<bool, string> callback)
		{
			Read(idMail, new long[0], new long[0], (bool success, string error) => {
				if (callback != null)
					callback(success, error);
			});
		}
		/// <summary>
		/// Mark a set of mails as read.
		/// </summary>
		/// <param name="idSenders">Identifier senders.</param>
		/// <param name="idGroups">Identifier groups.</param>
		public static void Read (long[] idSenders, long[] idGroups, Action<bool, string> callback)
		{
			Read(-1, idSenders, idGroups, callback);
		}
		/// <summary>
		/// Mark a single mail or a set of mails as read.
		/// </summary>
		/// <param name="idMail">Identifier mail.</param>
		/// <param name="idSenders">Identifier senders.</param>
		/// <param name="idGroups">Identifier groups.</param>
		/// <param name="callback">Callback.</param>
		protected static void Read (long idMail, long[] idSenders, long[] idGroups, Action<bool, string> callback) {
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "read");
			form.AddField("Id", idMail.ToString());
			if (idMail == -1)
			{
                if (idSenders != null && idSenders.Length > 0)
                {
                    string filter = "";
                    foreach (long id in idSenders)
                    {
                        filter += (filter == "" ? "" : ",") + id;
                    }
                    form.AddField("IdSender", filter);
                }
                if (idGroups != null && idGroups.Length > 0)
                {
                    string filter = "";
                    foreach (long id in idGroups)
                    {
                        filter += (filter == "" ? "" : ",") + id;
                    }
                    form.AddField("IdGroup", filter);
                }
			}
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("mail.php"), form, (string text, string error) => {
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
					callback(success, error);
			});
		}

		/// <summary>
		/// Mark this mail as read.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public void Unread (Action<bool, string> callback)
		{
			Unread(id, (bool success, string error) => {
				if (success)
					readDate = DateTime.Now;
				if (callback != null)
					callback(success, error);
			});
		}
		/// <summary>
		/// Mark a single mail as read.
		/// </summary>
		/// <param name="idMail">Identifier mail.</param>
		/// <param name="callback">Callback.</param>
		public static void Unread (long idMail, Action<bool, string> callback)
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "unread");
			form.AddField("Id", idMail.ToString());
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("mail.php"), form, (string text, string error) => {
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
					callback(success, error);
			});
		}

		/// <summary>
		/// Delete this instance.
		/// </summary>
		public virtual void Delete (Action<bool, string> callback)
		{
			Delete(id, callback);
		}

		/// <summary>
		/// Delete the specified Mail.
		/// </summary>
		/// <param name="idMail">Identifier mail.</param>
		/// <param name="callback">Callback.</param>
		public static void Delete (long idMail, Action<bool, string> callback)
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "delete");
			form.AddField("Id", idMail.ToString());
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("mail.php"), form, (string text, string error) => {
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
					callback(success, error);
			});
		}

		/// <summary>
		/// Loads the conversations (list of senders as both users and groups).
		/// </summary>
		public static void LoadConversations (Action<ArrayList, int, string> callback)
		{
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "list_conv");
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("mail.php"), form, (string text, string error) => {
				ArrayList conversations = new ArrayList();
				int count = 0;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null && result.ContainsKey("total"))
					{
						count = int.Parse(result["total"].ToString());
						ArrayList list = (ArrayList)result["results"];
						if (list != null)
						{
							foreach (Hashtable data in list)
							{
								// Create a new object from the result
								if (data.ContainsKey("Username"))
									conversations.Add(new User(data));
								else
									conversations.Add(new UserGroup(data));
							}
						}	
					}
				}
				if (callback != null)
					callback(conversations, count, error);
			});
		}
		
		/// <summary>
		/// Loads the read/unread messages from list of senders and groups.
		/// </summary>
		public static void Count (long[] idUsers, long[] idGroups, Action<MailCount[], string> callback)
		{
			string filterUsers = "", filterGroups = "";
			foreach (long id in idUsers)
			{
				filterUsers += (filterUsers == "" ? "" : ",") + id;
			}
			foreach (long id in idGroups)
			{
				filterGroups += (filterGroups == "" ? "" : ",") + id;
			}
			var form = CombuManager.instance.CreateForm();
			form.AddField("action", "count");
			form.AddField("IdSender", filterUsers);
			form.AddField("IdGroup", filterGroups);
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("mail.php"), form, (string text, string error) => {
				List<MailCount> counts = new List<MailCount>();
				if (string.IsNullOrEmpty(error))
				{
					ArrayList results = text.arrayListFromJson();
					if (results != null)
					{
						foreach (Hashtable data in results)
						{
							// Create a new object from the result
							counts.Add(new MailCount(data));
						}
					}
				}
				if (callback != null)
					callback(counts.ToArray(), error);
			});
		}
	}

	[System.Serializable]
	public class MailCount
	{
		public long idSender = 0;
		public long idGroup = 0;
		public int read = 0;
		public int unread = 0;
		
		public MailCount (Hashtable hash)
		{
			if (hash.ContainsKey("IdSender") && hash["IdSender"] != null)
				idSender = long.Parse(hash["IdSender"].ToString());
			if (hash.ContainsKey("IdGroup") && hash["IdGroup"] != null)
				idGroup = long.Parse(hash["IdGroup"].ToString());
			if (hash.ContainsKey("Read") && hash["Read"] != null)
				read = int.Parse(hash["Read"].ToString());
			if (hash.ContainsKey("Unread") && hash["Unread"] != null)
				unread = int.Parse(hash["Unread"].ToString());
		}
	}
}