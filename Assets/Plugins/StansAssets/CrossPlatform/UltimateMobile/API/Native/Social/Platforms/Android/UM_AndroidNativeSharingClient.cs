using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using SA.Android.Social;
using SA.Foundation.Templates;


namespace SA.CrossPlatform.Social
{
    public class UM_AndroidNativeSharingClient : UM_iNativeSharingClient
    {

        public void SystemSharingDialog(UM_ShareDialogBuilder builder, Action<SA_Result> callback) {
            var composer = new AN_ShareComposer();
            InitFullComposer(composer, builder);
            ShareWithComposer(composer, callback);
        }

        public void ShareToFacebook(UM_ShareDialogBuilder builder, Action<SA_Result> callback) {

            if(!AN_FacebookSharing.IsAppInstalled) {
                callback.Invoke(new SA_Result(new SA_Error(100, "The Facebook app isn't found on device")));
                return;
            }

            var composer = new AN_FacebookSharing();
            InitImagesComposer(composer, builder);
            ShareWithComposer(composer, callback);
        }

        public void ShareToInstagram(UM_ShareDialogBuilder builder, Action<SA_Result> callback) {

            if (!AN_InstagramSharing.IsAppInstalled) {
                callback.Invoke(new SA_Result(new SA_Error(100, "The Instagram app isn't found on device")));
                return;
            }

            var composer = new AN_InstagramSharing();
            InitImagesComposer(composer, builder);
            ShareWithComposer(composer, callback);
        }

        public void ShareToTwitter(UM_ShareDialogBuilder builder, Action<SA_Result> callback) {

            if (!AN_TwitterSharing.IsAppInstalled) {
                callback.Invoke(new SA_Result(new SA_Error(100, "The Twitter app isn't found on device")));
                return;
            }

            var composer = new AN_TwitterSharing();
            InitImagesComposer(composer, builder);
            ShareWithComposer(composer, callback);
        }


        public void ShareToWhatsapp(UM_ShareDialogBuilder builder, Action<SA_Result> callback) {

            if (!AN_WhatsappSharing.IsAppInstalled) {
                callback.Invoke(new SA_Result(new SA_Error(100, "The Whatsapp app isn't found on device")));
                return;
            }

            var composer = new AN_WhatsappSharing();
            InitImagesComposer(composer, builder);
            ShareWithComposer(composer, callback);
        }


        public void ShowSendMailDialog(UM_EmailDialogBuilder builder, Action<SA_Result> callback) {
            var composer = new AN_EmailComposer();

            composer.SetText(builder.TextWithUrl);
            composer.SetSubject(builder.Subject);

            foreach(var image in builder.Images) {
                composer.AddImage(image);
            }

            foreach(var recipient in builder.Recipients) {
                composer.AddRecipient(recipient);
            }

            ShareWithComposer(composer, callback);
        }



        private void InitImagesComposer(AN_SocialImageShareBuilders composer, UM_ShareDialogBuilder builder) {
           foreach(var image in builder.Images) {
                composer.AddImage(image);
           }
           
        }


        private void InitFullComposer(AN_SocialFullShareBuilder composer, UM_ShareDialogBuilder builder) {
            InitImagesComposer(composer, builder);
            composer.SetText(builder.TextWithUrl);
        }

        private void ShareWithComposer(AN_SocialShareBuilder composer, Action<SA_Result> callback) {
            composer.Share(() => {
                callback.Invoke(new SA_Result());
            });
        }

    }
}