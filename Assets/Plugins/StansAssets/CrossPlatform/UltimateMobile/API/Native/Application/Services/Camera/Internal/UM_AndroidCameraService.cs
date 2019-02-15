using System;
using System.Collections.Generic;
using UnityEngine;
using SA.Foundation.Templates;
using SA.Android.Camera;

namespace SA.CrossPlatform.App
{
    internal class UM_AndroidCameraService : UM_iCameraService
    {
        public void TakePicture(int thumbnailSize, Action<UM_MediaResult> callback) {
            AN_Camera.CaptureImage(thumbnailSize, (result) => {
                UM_MediaResult pickResult;
                if (result.IsSucceeded) {
                    var media = new UM_Media(result.Media.Thumbnail, result.Media.Path, UM_MediaType.Image);
                    pickResult = new UM_MediaResult(media);
                } else {
                    pickResult = new UM_MediaResult(result.Error);
                }

               callback.Invoke(pickResult);
            });
        }

        public void TakeVideo(int thumbnailSize, Action<UM_MediaResult> callback) {
            AN_Camera.CaptureVideo(thumbnailSize, (result) => {
                UM_MediaResult pickResult;
                if (result.IsSucceeded) {
                    var media = new UM_Media(result.Media.Thumbnail, result.Media.Path, UM_MediaType.Image);
                    pickResult = new UM_MediaResult(media);
                } else {
                    pickResult = new UM_MediaResult(result.Error);
                }

                callback.Invoke(pickResult);
            });
        }
    }
}