using System;
using System.Collections.Generic;
using UnityEngine;

using SA.Android.Gallery;
using SA.Foundation.Templates;
using SA.Foundation.Utility;

namespace SA.CrossPlatform.App
{
    internal class UM_EditorGalleryService : UM_AbstractGalleryService, UM_iGalleryService
    {

        public void PickImage(int thumbnailSize, Action<UM_MediaResult> callback) {

            SA_ScreenUtil.TakeScreenshot(thumbnailSize, (image) => {
                var media = new UM_Media(image, string.Empty, UM_MediaType.Image);
                var result = new UM_MediaResult(media);
                callback.Invoke(result);
            });
        }

        public void PickVideo(int thumbnailSize, Action<UM_MediaResult> callback) {
            SA_ScreenUtil.TakeScreenshot(thumbnailSize, (image) => {
                var media = new UM_Media(image, string.Empty, UM_MediaType.Video);
                var result = new UM_MediaResult(media);
                callback.Invoke(result);
            });
        }

        public override void SaveImage(Texture2D image, string fileName, Action<SA_Result> callback) {
            var result = new SA_Result();
            callback.Invoke(result);
        }
    }
}
