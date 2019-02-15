using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


using SA.Android;
using SA.Foundation.Editor;
using SA.Foundation.Utility;
using SA.Foundation.UtilitiesEditor;

namespace SA.Facebook
{


    [InitializeOnLoad]
    public class SA_FB_InstallationProcessing : SA_PluginInstallationProcessor<SA_FB_Settings>
    {
        private const string FACEBOOK_LIB_NAME = "Facebook.Unity.dll";
        private const string SA_FB_INSTALLED_DEFINE = "SA_FB_INSTALLED";
       
       

        static SA_FB_InstallationProcessing() {
            var installation = new SA_FB_InstallationProcessing();
            installation.Init();
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
            var projectLibs = SA_AssetDatabase.FindAssetsWithExtentions("Assets", ".dll");
            foreach (var lib in projectLibs) {
                ProcessAssetImport(lib);
            }
        }

        public static void ProcessAssetImport(string assetPath) {
            var isFBLibDetected = IsPathEqualsFacebookSDKName(assetPath);
            if (isFBLibDetected) {
                UpdateLibState(true);
            }
        }

        public static void ProcessAssetDelete(string assetPath) {
            var isFBLibDetected = IsPathEqualsFacebookSDKName(assetPath);
            if (isFBLibDetected) {
                UpdateLibState(false);
            }
        }


        //--------------------------------------
        //  Get / Set
        //--------------------------------------


        public static bool IsSDKInstalled {
            get {
#if SA_FB_INSTALLED
                return true;
#else
                return false;
#endif
            }
        }


        //--------------------------------------
        //  Private Methods
        //--------------------------------------


        private static bool IsPathEqualsFacebookSDKName(string assetPath) {
           string fileName = SA_PathUtil.GetFileName(assetPath);
            if (fileName.Equals(FACEBOOK_LIB_NAME)) {
                return true;
            } else {
                return false;
            }
                
        }

        private static void UpdateLibState(bool fbLibFound) {
            if (fbLibFound) {
                if (!SA_EditorDefines.HasCompileDefine(SA_FB_INSTALLED_DEFINE)) {
                    SA_EditorDefines.AddCompileDefine(SA_FB_INSTALLED_DEFINE);
                }

                //TODO need to figure out this dependency 
                //it should not have AN ref. Unless it's shipped as part of UM
                AN_Preprocessor.ActicateJarResolver();
            } else {
                if (SA_EditorDefines.HasCompileDefine(SA_FB_INSTALLED_DEFINE)) {
                    SA_EditorDefines.RemoveCompileDefine(SA_FB_INSTALLED_DEFINE);
                }
            }
        }
    }
}