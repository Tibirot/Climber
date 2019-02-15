using System;
using System.Collections.Generic;
using UnityEngine;
using SA.Foundation.Templates;


namespace SA.CrossPlatform.App
{
    public interface UM_iCameraService
    {

        /// <summary>
        /// Take picture using the device camera.
        /// </summary>
        /// <param name="maxThumbnailSize">
        /// Max image size. If picture size is bigger then imageSize value,
        /// picture will be scaled to meet the requiremnets 
        /// before transfering from native to unity side.
        /// </param>
        /// <param name="callback">Operation callback.</param>
        void TakePicture(int maxThumbnailSize, Action<UM_MediaResult> callback);

        /// <summary>
        /// Take video using the device camera.
        /// </summary>
        /// <param name="maxThumbnailSize">
        /// Max image size. If picture size is bigger then imageSize value,
        /// picture will be scaled to meet the requiremnets 
        /// before transfering from native to unity side.
        /// </param>
        /// <param name="callback">Operation callback.</param>
        void TakeVideo(int maxThumbnailSize, Action<UM_MediaResult> callback);
    }
}