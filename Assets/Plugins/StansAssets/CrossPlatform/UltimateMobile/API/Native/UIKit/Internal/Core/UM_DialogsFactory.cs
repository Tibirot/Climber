using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA.CrossPlatform.UI
{
    internal static class UM_DialogsFactory 
    {
        public static UM_iUIDialog CreateDialog(UM_NativeDialogBuilder builder) {

            switch (Application.platform) {
                case RuntimePlatform.Android:
                    return new UM_AndroidNativeDialogImpl(builder);
                case RuntimePlatform.IPhonePlayer:
                    return new UM_IOSNativeDialogImpl(builder);
                default:
                    return new UM_EditorNativeDialogImpl(builder);

            }
        }
    }
}