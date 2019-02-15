using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Combu;

/*
 * This script include sample code to demonstrate how to implement a simple message conversations workflow
 * by using Unity UI components.
 * 
 * The examples (both scenes and scripts) are provided as-are and can be eventually modified over time to reflect major changes to API or just to be improved.
 * So we discourage the approach of editing any demo file, but you could create your own scripts and inherit their classes from these examples if you want,
 * and eventually override the methods that you want to change or add more input fields and stuff to fit your needs.
 */
public class CombuDemoMessages : CombuDemoScene
{
	public Text textMessages;
	public Transform tfmRecipients;
	public GameObject prefabRecipient;
	public InputField sendCurrentText;

	public InputField sendTo;
	public InputField sendText;
	public Text sendError;

	ArrayList conversations = new ArrayList();
	MailCount[] mailCounts = new MailCount[0];
	Mail[] messages = new Mail[0];

	protected override void OnStart ()
	{
		textMessages.text = "";
	}

	void LoadConversationMessages ()
	{
		if (messages.Length > 0)
		{
			if (messages[0].idGroup > 0)
				Mail.Load(eMailList.Both, 0, 0, messages[0].idGroup, 1, 0, OnMessagesLoaded);
			else
				Mail.Load(eMailList.Both, (messages[0].toUser.id.Equals(CombuManager.localUser.id) ? messages[0].fromUser.idLong : messages[0].toUser.idLong), 0, 0, 1, 0, OnMessagesLoaded);
		}
		else
		{
			Mail.LoadConversations(OnLoadedConversations);
		}
	}

	protected override void OnUserLogin (bool success, string error)
	{
		base.OnUserLogin (success, error);
		if (success)
			Mail.LoadConversations(OnLoadedConversations);
	}

	void OnLoadedConversations (ArrayList entries, int entryCount, string mailError)
	{
		conversations = entries;
		for (int i = tfmRecipients.childCount - 1; i >= 0; --i)
		{
			Destroy(tfmRecipients.GetChild(i).gameObject);
		}
		List<long> idUsers = new List<long>(), idGroups = new List<long>();
		foreach (object sender in conversations)
		{
			if (sender is User)
				idUsers.Add((sender as User).idLong);
			else
				idGroups.Add((sender as UserGroup).id);
		}
		Mail.Count(idUsers.ToArray(), idGroups.ToArray(), (MailCount[] counts, string countError) => {
			mailCounts = counts;

			float y = 0, height = prefabRecipient.GetComponent<RectTransform>().sizeDelta.y;
			foreach (object sender in conversations)
			{
				GameObject go = (GameObject)Instantiate(prefabRecipient);
				go.transform.SetParent(tfmRecipients, false);

				RectTransform rectTfm = go.GetComponent<RectTransform>();
				rectTfm.localPosition = Vector3.zero;
				if (tfmRecipients.childCount > 1)
					rectTfm.localPosition = new Vector3(0, y, 0);
				y += height;

				string buttonText = "";
				UnityAction<BaseEventData> callback;

				if (sender is User)
				{
					User user = (User)sender;
					buttonText = user.userName;
					foreach (MailCount c in mailCounts)
					{
						if (c.unread > 0 && c.idSender.Equals(user.idLong))
						{
							buttonText += " (" + c.unread + ")";
							break;
						}
					}

					callback = new UnityAction<BaseEventData>( (BaseEventData baseEvent) => {
						Mail.Load(eMailList.Both, user.idLong, 0, 0, 1, 0, OnMessagesLoaded);
					});

				}
				else
				{
					UserGroup group = (UserGroup)sender;
					if (!string.IsNullOrEmpty(group.name))
					{
						buttonText = group.name;
					}
					else
					{
						foreach (User user in group.users)
						{
							buttonText += (buttonText == "" ? "" : ", ") + user.userName;
						}
					}
					foreach (MailCount c in mailCounts)
					{
						if (c.unread > 0 && c.idGroup.Equals(group.id))
						{
							buttonText += " (" + c.unread + ")";
							break;
						}
					}

					callback = new UnityAction<BaseEventData>( (BaseEventData baseEvent) => {
						Mail.Load(eMailList.Both, 0, 0, group.id, 1, 0, OnMessagesLoaded);
					});

				}

				Text newButtonText = go.transform.GetChild(0).GetComponent<Text>();
				newButtonText.text = buttonText;
				
				// Add an EventTrigger to load the messages of a conversation
				EventTrigger eventTrigger = go.AddComponent<EventTrigger>();
				EventTrigger.Entry entry = new EventTrigger.Entry();
				entry.eventID = EventTriggerType.PointerClick;
				entry.callback = new EventTrigger.TriggerEvent();
				entry.callback.AddListener(callback);
				eventTrigger.triggers = new List<EventTrigger.Entry>();
				eventTrigger.triggers.Add(entry);

			}
		});
	}

	void OnMessagesLoaded (Mail[] entries, int recordsCount, int pagesCount, string error)
	{
		messages = entries;
		string text = "";
		foreach (Mail message in entries)
		{
			if (text != "")
				text += "\n";
			text += message.fromUser.userName + ": " + message.message;
		}
		textMessages.text = text;
	}

	public void MailSendCurrent ()
	{
		if (string.IsNullOrEmpty(sendCurrentText.text) || messages.Length == 0)
			return;
		System.Action<bool, string> callback = (bool success, string error) => {
			if (success)
				LoadConversationMessages();
		};
		if (messages[0].idGroup > 0)
			Mail.SendMailToGroup(messages[0].idGroup, "", sendCurrentText.text, false, callback);
		else
			Mail.Send(messages[0].toUser.idLong, "", sendCurrentText.text, false, callback);
		sendCurrentText.text = "";
	}

	public void MailSendNew ()
	{
		if (string.IsNullOrEmpty(sendTo.text) || string.IsNullOrEmpty(sendText.text))
			return;
		sendError.text = "Loading...";
		Mail.Send(sendTo.text, "", sendText.text, false, (bool success, string error) => {
			if (success)
			{
				messages = new Mail[0];
				OpenPanel(panelMenu);
				LoadConversationMessages();
			}
			else
			{
				sendError.text = "ERROR: " + error;
			}
		});
		sendTo.text = "";
		sendText.text = "";
	}
}
