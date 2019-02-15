using System;
using System.Collections.Generic;
using UnityEngine;

using SA.Android.Media;

using SA.iOS.Foundation;
using SA.iOS.AVFoundation;
using SA.iOS.AVKit;

namespace SA.CrossPlatform.Media
{

    /// <summary>
    /// UM_MediaPlayer class can be used to control playback of audio/video files and streams.
    /// </summary>
    public class UM_MediaPlayer 
    {

        /// <summary>
        /// Will show native play activity and open video that avaliable by the <see cref="url"/> param.
        /// </summary>
        /// <param name="url">Remove video url.</param>
        public static void ShowRemoteVideo(string url, Action onClose) {

            if(Application.isEditor) {
                Application.OpenURL(url);
                onClose.Invoke();
                return;
            }

            switch (Application.platform) {
                case RuntimePlatform.Android:
                    AN_MediaPlayer.ShowRemoteVideo(url, onClose);
                    break;
                case RuntimePlatform.IPhonePlayer:
                    var isn_uri = ISN_NSURL.URLWithString(url);
                    var player = new ISN_AVPlayer(isn_uri);

                    var viewController = new ISN_AVPlayerViewController();
                    viewController.Player = player;
                    viewController.Show();

                    onClose.Invoke();

                    break;
            }
          
        }

    }
}