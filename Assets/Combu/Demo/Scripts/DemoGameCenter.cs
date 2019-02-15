using UnityEngine;
using System.Collections;
using Combu;

/*
 * This script include sample code to demonstrate how to implement a smoothless integration with iOS Game Center authentication service workflow
 * by using Unity UI components.
 * 
 * The examples (both scenes and scripts) are provided as-are and can be eventually modified over time to reflect major changes to API or just to be improved.
 * So we discourage the approach of editing any demo file, but you could create your own scripts and inherit their classes from these examples if you want,
 * and eventually override the methods that you want to change or add more input fields and stuff to fit your needs.
 */
public class DemoGameCenter : MonoBehaviour {

	IEnumerator Start ()
	{
		while (!CombuManager.isInitialized)
			yield return null;

		Social.localUser.Authenticate((bool success) => {
			if (success)
			{
				CombuManager.localUser.AuthenticatePlatform("GameCenter", Social.localUser.id, (bool loginSuccess, string loginError) => {

					if (loginSuccess)
					{
						// Store the GameCenter name
						CombuManager.localUser.customData["GameCenterName"] = Social.localUser.userName;
						CombuManager.localUser.Update((bool updateSuccess, string updateError) => {

							if (success) {
								Debug.LogWarning("User updated: [" + CombuManager.localUser.id + "] " + CombuManager.localUser.customData["GameCenterName"]);
								var leaderboard = CombuManager.platform.CreateLeaderboard();
								leaderboard.SetUserFilter(new string[]{CombuManager.localUser.id});
								leaderboard.timeScope = UnityEngine.SocialPlatforms.TimeScope.AllTime;
								leaderboard.LoadScores((bool scoresSuccess) => {
									if (success)
										Debug.Log("Score: " + leaderboard.localUserScore.formattedValue);
								});
							}
							else
								Debug.LogError("User update failed: " + updateError);
						});
					}
					else
					{
						Debug.LogError("Login failed: " + loginError);
					}

				});
			}
			else
			{
				Debug.LogError("GameCenter login failed");
			}
		});
	}
}
