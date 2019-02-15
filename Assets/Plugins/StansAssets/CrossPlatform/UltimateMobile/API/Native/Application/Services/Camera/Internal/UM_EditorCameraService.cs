using UnityEngine;
using System.Collections;


using System;
using SA.Foundation.Utility;

namespace SA.CrossPlatform.App
{
    internal class UM_EditorCameraService : UM_iCameraService
    {
        public void TakePicture(int maxThumbnailSize, Action<UM_MediaResult> callback) {
            SA_ScreenUtil.TakeScreenshot(maxThumbnailSize, (image) => {
                var media = new UM_Media(image, string.Empty, UM_MediaType.Image);
                var result = new UM_MediaResult(media);
                callback.Invoke(result);
            });
        }

        public void TakeVideo(int maxThumbnailSize, Action<UM_MediaResult> callback) {
            SA_ScreenUtil.TakeScreenshot(maxThumbnailSize, (image) => {
                var media = new UM_Media(image, string.Empty, UM_MediaType.Video);
                var result = new UM_MediaResult(media);
                callback.Invoke(result);
            });
        }
    }
}