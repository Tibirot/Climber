using System;
using System.Collections.Generic;
using UnityEngine;
using SA.Foundation.Templates;


namespace SA.CrossPlatform.App
{
    public interface UM_iGalleryService
    {


        /// <summary>
        /// Application gallery will be opened to provide user na ability to pick na image 
        /// and use picked image when back to the Unity.
        /// </summary>
        /// <param name="thumbnailSize">
        /// Max image size. If picture size is bigger then imageSize value,
        /// picture will be scaled to meet the requiremnets 
        /// before transfering from native to unity side.
        /// </param>
        /// <param name="callback">Operation callback.</param>
        void PickImage(int thumbnailSize, Action<UM_MediaResult> callback);


        /// <summary>
        /// Application gallery will be opened to provide user na ability to pick na image 
        /// and use picked image when back to the Unity.
        /// </summary>
        /// <param name="thumbnailSize">
        /// Max image size. If picture size is bigger then imageSize value,
        /// picture will be scaled to meet the requiremnets 
        /// before transfering from native to unity side.
        /// </param>
        /// <param name="callback">Operation callback.</param>
        void PickVideo(int thumbnailSize, Action<UM_MediaResult> callback);

        /// <summary>
        /// Saves the provided image to the device gallery.
        /// </summary>
        /// <param name="image">Image that will be saved. The only requirements, texture has to be redeable.</param>
        /// <param name="fileName">The saved image will be named using the prodived file name if possible.</param>
        /// <param name="callback">Operation callback.</param>
        void SaveImage(Texture2D image, string fileName, Action<SA_Result> callback);


        /// <summary>
        /// Works the same as as <see cref="SaveImage"/> but uses current screen imagee as source.
        /// </summary>
        /// <param name="fileName">The saved image will be named using the prodived file name if possible.</param>
        /// <param name="callback">Operation callback.</param>
        void SaveScreenshot(string fileName, Action<SA_Result> callback);
    }
}