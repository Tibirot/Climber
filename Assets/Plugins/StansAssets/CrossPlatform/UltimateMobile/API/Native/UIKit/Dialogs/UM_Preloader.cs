using UnityEngine;

using SA.iOS.UIKit;
using SA.Android.App;

namespace SA.CrossPlatform.UI
{


    /// <summary>
    /// Calss allows to show preloaders and lock application screen
    /// </summary>
    public static class UM_Preloader
    {


        /// <summary>
        /// Locks the screen and displayes a preloader spinner
        /// </summary>
        public static void LockScreen() {

#if UNITY_EDITOR
            if(Application.isEditor) {
                UnityEditor.EditorUtility.DisplayProgressBar("Please Wait..", string.Empty, 0.4f);
                return;
            }
#endif

            switch (Application.platform) {
                case RuntimePlatform.Android:
                    AN_Preloader.LockScreen("Please Wait..");
                    break;
                case RuntimePlatform.IPhonePlayer:
                    ISN_Preloader.LockScreen();
                    break;
            }

        }


        /// <summary>
        /// Unlocks the screen and hide a preloader spinner
        /// In case there is no preloader displayed, method does nothing
        /// </summary>
        public static void UnlockScreen() {

#if UNITY_EDITOR
            if (Application.isEditor) {
                UnityEditor.EditorUtility.ClearProgressBar();
                return;
            }
#endif

            switch (Application.platform) {
                case RuntimePlatform.Android:
                    AN_Preloader.UnlockScreen();
                    break;
                case RuntimePlatform.IPhonePlayer:
                    ISN_Preloader.UnlockScreen();
                    break;
            }
        }
    }
}