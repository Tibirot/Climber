using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SocialPlatforms;
using Combu;

/*
 * This script include sample code to demonstrate how to implement a simple registration and login workflow
 * by using Unity UI components.
 * 
 * The examples (both scenes and scripts) are provided as-are and can be eventually modified over time to reflect major changes to API or just to be improved.
 * So we discourage the approach of editing any demo file, but you could create your own scripts and inherit their classes from these examples if you want,
 * and eventually override the methods that you want to change or add more input fields and stuff to fit your needs.
 */
public class CombuDemo : CombuDemoScene
{
	public InputField registerUsername;
	public InputField registerPassword;
	public Text registerError;

	public Text textNews;
	
	public Text textCustom;
	public InputField customKey;
	public InputField customValue;
    public Text customError;

    public Text appCustomText;
    public InputField appCustomKey;
    public InputField appCustomValue;
    public Text appCustomError;

	public Text textContacts;
	public InputField contactUsername;
	public Text contactError;

	public string leaderboardId;
	public Text textLeaderboardScores;
	public InputField leaderboardScore;
	public Text leaderboardError;

	public Text textAchievements;
	public InputField achievementId;
	public InputField achievementProgress;
	public Text achievementError;

	public Text textMail;
	public InputField mailRecipient;
	public InputField mailSubject;
	public InputField mailMessage;
	public Text mailError;

	public Text textFiles;

	public void UserRegister ()
	{
		if (string.IsNullOrEmpty(registerUsername.text) || string.IsNullOrEmpty(registerPassword.text))
		{
			registerError.text = "Enter your credentials";
			return;
		}

		User user = new User();
		user.userName = registerUsername.text;
		user.password = registerPassword.text;

		registerError.text = "Loading...";
		user.Update( (bool success, string error) => {
			registerError.text = "";
			if (success)
			{
				registerUsername.text = "";
				registerPassword.text = "";
				registerError.text = "Account created!";
			}
			else
			{
				registerError.text = "Registration failed: " + error;
			}
		});
	}

	public void NewsLoad ()
	{
		textNews.text = "Loading...";
		News.Load(1, 3, (News[] news, int count, int pagesCount, string error) => {
			if (news.Length == 0)
			{
				textNews.text = "List is empty";
			}
			else
			{
				// Open the news reader only if there's any news
				textNews.text = "";
				for (int i = 0; i < news.Length; ++i)
				{
					if (i > 0)
						textNews.text += "\n\n";
					textNews.text += string.Format("{0} - {1}\n{2}", news[i].subject, news[i].date.ToString("d MMMM yyyy"), news[i].message);
				}
			}
		});
	}

	public void CustomDataLoad ()
	{
		customError.text = "";
		CombuDemoUser user = (CombuDemoUser)CombuManager.localUser;
		textCustom.text = "";
		foreach (string key in CombuManager.localUser.customData.Keys)
		{
			if (textCustom.text != "")
				textCustom.text += "\n";
			textCustom.text += string.Format("{0}: {1}", key, user.customData[key]);
		}
	}

	public void CustomDataPost ()
	{
		if (string.IsNullOrEmpty(customKey.text))
		{
			customError.text = "Enter the data key";
		}
		else
		{
			textCustom.text = "Loading...";
			CombuDemoUser user = (CombuDemoUser)CombuManager.localUser;
			user.customData[customKey.text] = customValue.text;
			user.Update((bool success, string error) => {
				if (success)
				{
					CustomDataLoad();
					customKey.text = "";
					customValue.text = "";
					customError.text = "Data was registered";
				}
				else
				{
					customError.text = "Failed: " + error;
				}
			});
		}
    }

    public void AppCustomDataLoad()
    {
        appCustomError.text = "";
        CombuDemoUser user = (CombuDemoUser)CombuManager.localUser;
        appCustomText.text = "";
        foreach (string key in CombuManager.localUser.appCustomData.Keys)
        {
            if (appCustomText.text != "")
                appCustomText.text += "\n";
            appCustomText.text += string.Format("{0}: {1}", key, user.appCustomData[key]);
        }
    }

    public void AppCustomDataPost()
    {
        if (string.IsNullOrEmpty(appCustomKey.text))
        {
            appCustomError.text = "Enter the data key";
        }
        else
        {
            appCustomText.text = "Loading...";
            CombuDemoUser user = (CombuDemoUser)CombuManager.localUser;
            user.appCustomData[appCustomKey.text] = appCustomValue.text;
            user.Update((bool success, string error) => {
                if (success)
                {
                    AppCustomDataLoad();
                    appCustomKey.text = "";
                    appCustomValue.text = "";
                    appCustomError.text = "Data was registered";
                }
                else
                {
                    appCustomError.text = "Failed: " + error;
                }
            });
        }
    }

	public void UserFriends ()
	{
		textContacts.text = "Loading...";
		CombuManager.localUser.LoadFriends(eContactType.Friend, (bool success) => {
			if (CombuManager.localUser.friends.Length == 0)
			{
				textContacts.text = "List is empty";
			}
			else
			{
				textContacts.text = "";
				for (int i = 0; i < CombuManager.localUser.friends.Length; ++i)
				{
					if (i > 0)
						textContacts.text += "\n";
					textContacts.text += CombuManager.localUser.friends[i].userName;
				}
			}
		});
	}

	public void UserFriendsAdd ()
	{
		string username = contactUsername.text.Trim();
		if (string.IsNullOrEmpty(username))
		{
			contactError.text = "Digit the username";
		}
		else
		{
			string oldText = textContacts.text;
			textContacts.text = "Loading...";
			contactError.text = "";
			CombuManager.localUser.AddContact(username, eContactType.Friend, (bool success, string error) => {
				textContacts.text = oldText;
				if (success)
					UserFriends();
				else
					contactError.text = error;
			});
		}
	}
	
	public void UserFriendsRemove ()
	{
		string username = contactUsername.text.Trim();
		if (string.IsNullOrEmpty(username))
		{
			contactError.text = "Digit the username";
		}
		else
		{
			string oldText = textContacts.text;
			textContacts.text = "Loading...";
			contactError.text = "";
			CombuManager.localUser.RemoveContact(username, (bool success, string error) => {
				textContacts.text = oldText;
				if (success)
					UserFriends();
				else
					contactError.text = error;
			});
		}
	}

	public void LeaderboardsScores ()
	{
		leaderboardError.text = "";
		textLeaderboardScores.text = "Loading...";
		// Load the leaderboard to retrieve the title, description, scores etc.
		CombuManager.platform.LoadScores(leaderboardId, (IScore[] scores) => {
			if (scores.Length == 0)
			{
				textLeaderboardScores.text = "List is empty";
			}
			else
			{
				textLeaderboardScores.text = "";
				for (int i = 0; i < scores.Length; ++i)
				{
					Score score = (Score)scores[i];
					if (i > 0)
						textLeaderboardScores.text += "\n";
					textLeaderboardScores.text += string.Format("{0}) {1}: {2}",
					                                            score.rank,
					                                            score.user.userName,
					                                            score.formattedValue);
				}
			}
		});
	}

	public void LeaderboardsPost ()
	{
		if (string.IsNullOrEmpty(leaderboardScore.text))
		{
			leaderboardError.text = "Enter a score";
			return;
		}
		leaderboardError.text = "Loading...";
		CombuManager.platform.ReportScore(leaderboardScore.text, leaderboardId, (bool success) => {
			if (success)
			{
				LeaderboardsScores();
				leaderboardScore.text = "";
				leaderboardError.text = "Score registered";
			}
			else
			{
				leaderboardError.text = "Failed to register the score";
			}
		});
	}

	public void AchievementsLoad ()
	{
		achievementError.text = "";
		textAchievements.text = "Loading...";
		CombuManager.platform.LoadAchievements( (IAchievement[] achievements) => {
			textAchievements.text = "";
			for (int i = 0; i < achievements.Length; ++i)
			{
				if (i > 0)
					textAchievements.text += "\n";
				Achievement achievement = (Achievement)achievements[i];
				textAchievements.text += "[" + achievement.id + "] " + achievement.title + ": ";
				if (achievement.completed)
					textAchievements.text += "Completed";
				else
					textAchievements.text += Mathf.RoundToInt((float)achievement.percentCompleted * 100f) + "%";
				textAchievements.text += " (completed " + achievement.finished + " times)";
			}
		});
	}

	public void AchievementsPost ()
	{
		int progress = 0;
		if (!int.TryParse(achievementProgress.text, out progress))
			progress = 0;
		if (progress <= 0)
		{
			leaderboardError.text = "Progress must be a number greater than 0";
			return;
		}
		achievementError.text = "Loading...";
		CombuManager.platform.ReportProgress(achievementId.text, progress, (bool success) => {
			if (success)
			{
				AchievementsLoad();
				achievementId.text = "";
				achievementProgress.text = "";
				achievementError.text = "Progress registered";
			}
			else
			{
				achievementError.text = "Failed to register the progress";
			}
		});
	}

	public void MessagesLoad ()
	{
		mailError.text = "";
		textMail.text = "Loading...";
		Mail.Load(eMailList.Received, 1, 3, (Mail[] messages, int count, int pagesCount, string error) => {
			if (messages.Length == 0)
			{
				textMail.text = "List is empty";
			}
			else
			{
				textMail.text = "";
				for (int i = 0; i < messages.Length; ++i)
				{
					if (i > 0)
						textMail.text += "\n\n";
					textMail.text += string.Format("<color=#EB96F2>id:{0}</color> <color=#6DD67D>{1}</color> <color=#B1B5B2>[{2}]</color> <color=#83E8FC>{3}</color> {4}",
					                               messages[i].id,
					                               messages[i].fromUser.userName,
					                               messages[i].sendDate.ToString("d MMM yyyy"),
					                               messages[i].subject,
					                               messages[i].message);
				}
			}
		});
	}

	public void MessagesPost ()
	{
		if (string.IsNullOrEmpty(mailRecipient.text) || string.IsNullOrEmpty(mailSubject.text) || string.IsNullOrEmpty(mailMessage.text))
		{
			mailError.text = "Enter To, Subject and Message";
		}
		else
		{
			mailError.text = "Loading...";
			Mail.Send(mailRecipient.text.Split(','), mailSubject.text, mailMessage.text, false, (bool success, string error) => {
				if (success)
				{
					mailError.text = "Message has been sent";
					mailRecipient.text = "";
					mailSubject.text = "";
					mailMessage.text = "";
				}
				else
				{
					mailError.text = "Send failed: " + error;
				}
			});
		}
	}

	public void UserFilesLoad ()
	{
		textFiles.text = "Loading...";
		UserFile.Load (CombuManager.localUser.id, false, 1, 10, (UserFile[] files, int recordCount, int pageCount, string error) => {
			Debug.Log("Files loaded: " + files.Length);
			string text = "";
			foreach (var file in files)
			{
				text += file.url + "\n";
			}
			textFiles.text = text;
		});
	}

	public void UserFilesUpload ()
	{
		var screenshot = CombuManager.CaptureScreenshot ();
		UserFile file = new UserFile ();
		file.name = "Screenshot " + System.DateTime.Now.ToString("yyyy-MM-dd T");
		file.customData ["ScreenSize"] = Screen.width + "x" + Screen.height;
		file.Update (screenshot, (bool success, string error) => {
			Debug.Log("Upload success: " + success + " -- error: " + error);
		});
	}
}
