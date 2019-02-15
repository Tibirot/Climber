using UnityEngine;

using System;
using SA.Foundation.Templates;


namespace SA.CrossPlatform.Advertisement
{

    /// <summary>
    /// A client to interact with banner advertisements functionality.
    /// </summary>
    public interface UM_iBannerAds 
    {

        void Load(Action<SA_Result> callback);
        void Load(string id, Action<SA_Result> callback);
        void Show(Action callback);
        void Hide();
        void Destroy();


        /// <summary>
        /// Indicates if banner ad is ready to be shown
        /// </summary>
        bool IsReady { get; }
    }
}