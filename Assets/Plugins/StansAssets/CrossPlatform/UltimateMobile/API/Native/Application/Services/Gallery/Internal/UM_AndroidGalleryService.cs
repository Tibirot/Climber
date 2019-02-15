using System;
using System.Collections.Generic;
using UnityEngine;

using SA.Android.Gallery;
using SA.Foundation.Templates;

namespace SA.CrossPlatform.App
{
    internal class UM_AndroidGalleryService : UM_AbstractGalleryService, UM_iGalleryService
    {

        public void PickImage(int thumbnailSize, Action<UM_MediaResult> callback) {
            PickMedia(thumbnailSize, AN_MediaType.Image, callback);
        }

        public void PickVideo(int thumbnailSize, Action<UM_MediaResult> callback) {
            PickMedia(thumbnailSize, AN_MediaType.Video, callback);
        }


        private void PickMedia(int thumbnailSize, AN_MediaType type, Action<UM_MediaResult> callback) {
            AN_MediaPicker picker = new AN_MediaPicker(type);
            picker.AllowMultiSelect = false;
            picker.MaxSize = thumbnailSize;

            picker.Show((result) => {
                UM_MediaResult pickResult;

                if (result.IsSucceeded) {
                    UM_MediaType um_type;
                    switch (type) {
                        case AN_MediaType.Video:
                            um_type = UM_MediaType.Video;
                            break;
                        default:
                            um_type = UM_MediaType.Image;
                            break;
                    } 
                    var media = new UM_Media(result.Media[0].Thumbnail, result.Media[0].Path, um_type);
                    pickResult = new UM_MediaResult(media);
                } else {
                    pickResult = new UM_MediaResult(result.Error);
                }

                callback.Invoke(pickResult);
            });
        }

        public override void SaveImage(Texture2D image, string fileName, Action<SA_Result> callback) {
            AN_Gallery.SaveImageToGallery(image, fileName, (result) => {
                callback.Invoke(result);
            });
        }
    }
}
