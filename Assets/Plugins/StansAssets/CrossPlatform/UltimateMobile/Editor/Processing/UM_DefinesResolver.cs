
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


using SA.Android;
using SA.Foundation.Editor;
using SA.Foundation.Utility;
using SA.Foundation.UtilitiesEditor;

namespace SA.CrossPlatform
{


    [InitializeOnLoad]
    public class UM_DefinesResolver : SA_PluginInstallationProcessor<UM_Settings>
    {
        private const string ADMOB_LIB_FOLDER_NAME = "GoogleMobileAds";
        private const string ADMOB_INSTALLED_DEFINE = "SA_ADMOB_INSTALLED";

        private const string UNITY_ADS_LIB_NAME = "UnityEngine.Advertisements.Editor.dll";
        private const string UNITY_ADS_INSTALLED_DEFINE = "SA_UNITY_ADS_INSTALLED";



        static UM_DefinesResolver() {
            var instalation = new UM_DefinesResolver();
            instalation.Init();
        }


        //--------------------------------------
        //  SA_PluginInstallationProcessor
        //--------------------------------------

        protected override void OnInstall() {

            //Let's check if we have FB SKD in the project
            ProcessAssets();
        }


        //--------------------------------------
        //  Public Methods
        //--------------------------------------

        public static void ProcessAssets() {

            //We are loocking for folder
            List<string> projectFolders = SA_AssetDatabase.FindAssetsWithExtentions("Assets", "");
            foreach (var lib in projectFolders) {
                ProcessAssetImport(lib);
            }

            //We are loocking for dll libs
            List<string> projectLibs = SA_AssetDatabase.FindAssetsWithExtentions("Assets", ".dll");
            foreach (var lib in projectLibs) {
                ProcessAssetImport(lib);
            }
        }

        public static void ProcessAssetImport(string assetPath) {
            CheckForAdMobSDK(assetPath, true);
            CheckForUnityAdsSDK(assetPath, true);
        }

        public static void ProcessAssetDelete(string assetPath) {
            CheckForAdMobSDK(assetPath, false);
            CheckForUnityAdsSDK(assetPath, false);
        }


        //--------------------------------------
        //  Get / Set
        //--------------------------------------


        public static bool IsAdMobInstalled {
            get {
#if SA_ADMOB_INSTALLED
                return true;
#else
                return false;
#endif
            }
        }


        public static bool IsUnityAdsInstalled {
            get {
#if SA_UNITY_ADS_INSTALLED
                return true;
#else
                return false;
#endif
            }
        }



        public static bool IsPlayMakerInstalled {
            get {
#if PLAYMAKER
                return true;
#else
                return false;
#endif
            }
        }


        //--------------------------------------
        //  Private Methods
        //--------------------------------------


        private static void CheckForAdMobSDK(string assetPath, bool enable) {
            string fileName = SA_PathUtil.GetFileName(assetPath);
            if (fileName.Equals(ADMOB_LIB_FOLDER_NAME)) {
                UpdateSDKDefine(enable, ADMOB_INSTALLED_DEFINE);
            } 
        }

        private static void CheckForUnityAdsSDK(string assetPath, bool enabled) {
            string fileName = SA_PathUtil.GetFileName(assetPath);
            if (fileName.Equals(UNITY_ADS_LIB_NAME)) {
                UpdateSDKDefine(enabled, UNITY_ADS_INSTALLED_DEFINE);
            }

            SA_EditorDefines.RemoveCompileDefine(UNITY_ADS_INSTALLED_DEFINE, BuildTarget.tvOS);
        }

        private static void UpdateSDKDefine(bool enabled, string define) {
            if (enabled) {
                if (!SA_EditorDefines.HasCompileDefine(define)) {
                    SA_EditorDefines.AddCompileDefine(define);
                }
                //TODO this has to be automatic and managed from AN
                AN_Preprocessor.ActicateJarResolver();
            } else {
                if (SA_EditorDefines.HasCompileDefine(define)) {
                    SA_EditorDefines.RemoveCompileDefine(define);
                }
            }
        }
    }
}