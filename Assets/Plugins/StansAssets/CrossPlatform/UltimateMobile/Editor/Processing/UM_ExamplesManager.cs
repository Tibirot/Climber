using System.Collections;
using System.Collections.Generic;
using SA.Android.Utilities;
using SA.CrossPlatform.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SA.CrossPlatform
{

    public static class UM_ExamplesManager 
    {

        public static void OpenWelcomeScene()
        {
            EditorSceneManager.OpenScene(UM_Settings.WELCOME_EXAMPLE_SCENE_PATH, OpenSceneMode.Single);
        }

        public static void BuildWelcomeScene()
        {
            #if SA_DEVELOPMENT_PROJECT
            AN_TestManager.ApplyExampleConfig();
            PlayerSettings.productName = "Ultimate Mobile";
            #endif
           
            var playerOptions = new BuildPlayerOptions();
            playerOptions.scenes = new[]
            {
                UM_Settings.WELCOME_EXAMPLE_SCENE_PATH,
                UM_Settings.IN_APP_EXAMPLE_SCENE_PATH,
                UM_Settings.GAME_SERVICE_EXAMPLE_SCENE_PATH,
                UM_Settings.SHARING_EXAMPLE_SCENE_PATH
            };

            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.iOS:
                    playerOptions.target = BuildTarget.iOS;
                    playerOptions.locationPathName = "ultimate_mobile_plugin";
                    break;
                case BuildTarget.Android:
                    playerOptions.target = BuildTarget.Android;
                    playerOptions.locationPathName = "ultimate_mobile_plugin.apk";
                    break;
                default:
                    UM_DialogsUtility.ShowMessage("Wrong Platform", "Make sure current editor platform set to iOS or Android");
                    break;
            }
          
            playerOptions.options = BuildOptions.Development | BuildOptions.AutoRunPlayer;
            BuildPipeline.BuildPlayer(playerOptions);
        }
       
    }
}