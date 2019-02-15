using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using SA.iOS.Social;
using SA.Foundation.Templates;


namespace SA.CrossPlatform.Social
{
    public class UM_IOSNativeSharingClient : UM_iNativeSharingClient
    {

        public void SystemSharingDialog(UM_ShareDialogBuilder builder, Action<SA_Result> callback) {


            ISN_UIActivityViewController controller = new ISN_UIActivityViewController();
            controller.SetText(builder.TextWithUrl);

            foreach(var image in builder.Images) {
                controller.AddImage(image);
            }
           
            controller.Present((result) => {
                if (result.IsSucceeded && result.Completed) {
                    callback.Invoke(new SA_Result());
                } else {
                    callback.Invoke(result);
                }

            });
        }

        public void ShareToFacebook(UM_ShareDialogBuilder builder, Action<SA_Result> callback) {
            ISN_Facebook.Post(builder.Text, builder.Url, builder.Images.ToArray(), callback);
        }

        public void ShareToInstagram(UM_ShareDialogBuilder builder, Action<SA_Result> callback) {
           
            Texture2D image = null;
            if (builder.Images.Count > 0) {
                image = builder.Images[0];
            }

            ISN_Instagram.Post(image, builder.TextWithUrl, callback);
        }

        public void ShareToTwitter(UM_ShareDialogBuilder builder, Action<SA_Result> callback) {
            ISN_Twitter.Post(builder.Text, builder.Url, builder.Images.ToArray(), callback);
        }


        public void ShareToWhatsapp(UM_ShareDialogBuilder builder, Action<SA_Result> callback) {
            if (builder.Images.Count > 0) {
                ISN_Whatsapp.Post(builder.Images[0]);
            } else {
                ISN_Whatsapp.Post(builder.TextWithUrl);
            }

            callback.Invoke(new SA_Result());
        }

        public void ShowSendMailDialog(UM_EmailDialogBuilder builder, Action<SA_Result> callback) {
            ISN_Mail.Send(builder.Subject,
                builder.TextWithUrl,
                builder.Recipients.ToArray(),
                builder.Images.ToArray(),
                callback
                );
        }


       

    }
}