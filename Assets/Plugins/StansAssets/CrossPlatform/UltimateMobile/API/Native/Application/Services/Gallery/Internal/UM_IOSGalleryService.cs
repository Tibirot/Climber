using System;
using System.Collections.Generic;

using UnityEngine;

using SA.Foundation.Templates;
using SA.iOS.UIKit;
using SA.iOS.AVFoundation;


namespace SA.CrossPlatform.App
{
    internal class UM_IOSGalleryService : UM_AbstractGalleryService, UM_iGalleryService
    {
        public void PickImage(int imageSize, Action<UM_MediaResult> callback) {
            ISN_UIImagePickerController picker = new ISN_UIImagePickerController();
            picker.SourceType = ISN_UIImagePickerControllerSourceType.Album;
            picker.MediaTypes = new List<string>() { ISN_UIMediaType.IMAGE };

            picker.MaxImageSize = imageSize;
            picker.ImageCompressionFormat = ISN_UIImageCompressionFormat.JPEG;
            picker.ImageCompressionRate = 0.8f;

            UM_MediaResult pickResult;
            picker.Present((result) => {
                if (result.IsSucceeded) {
                    var media = new UM_Media(result.Image, result.ImageURL, UM_MediaType.Image);
                    pickResult = new UM_MediaResult(media);
                } else {
                    pickResult = new UM_MediaResult(result.Error);
                }

                callback.Invoke(pickResult);
            });
        }

        public void PickVideo(int thumbnailSize, Action<UM_MediaResult> callback) {
            ISN_UIImagePickerController picker = new ISN_UIImagePickerController();
            picker.SourceType = ISN_UIImagePickerControllerSourceType.Album;
            picker.MediaTypes = new List<string>() { ISN_UIMediaType.MOVIE };


            picker.Present((result) => {
                UM_MediaResult pickResult;
                if (result.IsSucceeded) {
                    Texture2D image = ISN_AVAssetImageGenerator.CopyCGImageAtTime(result.MediaURL, 0);

                    var media = new UM_Media(image, result.MediaURL, UM_MediaType.Video);
                    pickResult = new UM_MediaResult(media);
                } else {
                    pickResult = new UM_MediaResult(result.Error);
                }

                callback.Invoke(pickResult);
            });
        }

        public override void SaveImage(Texture2D image, string fileName, Action<SA_Result> callback) {
            ISN_UIImagePickerController.SaveTextureToCameraRoll(image, callback);
        }
    }
}