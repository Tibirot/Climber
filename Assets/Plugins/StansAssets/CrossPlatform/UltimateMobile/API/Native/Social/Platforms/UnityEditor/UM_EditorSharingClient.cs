using UnityEngine;

using System;
using SA.CrossPlatform.UI;
using SA.iOS.Social;
using SA.Foundation.Templates;


namespace SA.CrossPlatform.Social
{
    public class UM_EditorSharingClient : UM_iNativeSharingClient
    {

        public void SystemSharingDialog(UM_ShareDialogBuilder builder, Action<SA_Result> callback)
        {
            UM_EditorAPIEmulator.WaitForNetwork(() =>
            {
                UM_DialogsUtility.ShowMessage("System Sharing Dialog", "Editor API Sharing Emulation.");
                callback.Invoke(new SA_Result());
            });
        }

        public void ShareToFacebook(UM_ShareDialogBuilder builder, Action<SA_Result> callback) {
            UM_EditorAPIEmulator.WaitForNetwork(() =>
            {
                UM_DialogsUtility.ShowMessage("Share To Facebook", "Editor API Sharing Emulation.");
                callback.Invoke(new SA_Result());
            });
        }

        public void ShareToInstagram(UM_ShareDialogBuilder builder, Action<SA_Result> callback) {
            UM_EditorAPIEmulator.WaitForNetwork(() =>
            {
                UM_DialogsUtility.ShowMessage("Share To Instagram", "Editor API Sharing Emulation.");
                callback.Invoke(new SA_Result());
            });
        }

        public void ShareToTwitter(UM_ShareDialogBuilder builder, Action<SA_Result> callback) {
            UM_EditorAPIEmulator.WaitForNetwork(() =>
            {
                UM_DialogsUtility.ShowMessage("Share To Twitter", "Editor API Sharing Emulation.");
                callback.Invoke(new SA_Result());
            });
        }


        public void ShareToWhatsapp(UM_ShareDialogBuilder builder, Action<SA_Result> callback) {
            UM_EditorAPIEmulator.WaitForNetwork(() =>
            {
                UM_DialogsUtility.ShowMessage("Share To Whatsapp", "Editor API Sharing Emulation.");
                callback.Invoke(new SA_Result());
            });
        }

        public void ShowSendMailDialog(UM_EmailDialogBuilder builder, Action<SA_Result> callback) {
            UM_EditorAPIEmulator.WaitForNetwork(() =>
            {
                UM_DialogsUtility.ShowMessage("Send Mail Dialog", "Editor API Sharing Emulation.");
                callback.Invoke(new SA_Result());
            });
        }
    }
}