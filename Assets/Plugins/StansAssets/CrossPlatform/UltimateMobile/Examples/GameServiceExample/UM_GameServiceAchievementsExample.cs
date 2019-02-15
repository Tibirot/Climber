using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using SA.Android.GMS.Auth;
using SA.Android.GMS.Common;
using SA.Android.GMS.Games;
using SA.Android.App;

using SA.Android.Utilities;
using SA.CrossPlatform.GameServices;
using SA.Foundation.Network.Web;

public class UM_GameServiceAchievementsExample : MonoBehaviour
{



    [SerializeField] Button m_nativeUI;
    [SerializeField] Button m_load;



    private void Start() {


        m_nativeUI.onClick.AddListener(() => {

            var client = UM_GameService.AchievementsClient;
            client.ShowUI();
        });



        m_load.onClick.AddListener(() => {
            var client = UM_GameService.AchievementsClient;
    
            client.Load((result) => {

                if (result.IsSucceeded) {
                    AN_Logger.Log("Load Achievements Succeeded, count: " + result.Achievements.Count);
                    foreach (var achievement in result.Achievements) {
                        AN_Logger.Log("------------------------------------------------");
                        AN_Logger.Log("achievement.Identifier: " + achievement.Identifier);
                        AN_Logger.Log("achievement.Name: " + achievement.Name);
                        AN_Logger.Log("achievement.State: " + achievement.State);
                        AN_Logger.Log("achievement.Type: " + achievement.Type);
                        AN_Logger.Log("achievement.TotalSteps: " + achievement.TotalSteps);
                        AN_Logger.Log("achievement.CurrentSteps: " + achievement.CurrentSteps);

                    }

                    AN_Logger.Log("------------------------------------------------");
                } else {
                    Debug.Log("Load Achievements Failed: " + result.Error.FullMessage);
                }
            });

        });
    }
}
