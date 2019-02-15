using System;
using UnityEngine;

namespace SA.CrossPlatform.GameServices { 

    /// <summary>
    /// Game service player model
    /// </summary>
    public interface UM_iPlayer {

        /// <summary>
        /// Player identifier.
        /// </summary>
        string Id { get; }


        /// <summary>
        /// Player Alias.
        /// Typically, you never display the alias string directly in your user interface. 
        /// Instead use the <see cref="DisplayName"/> property.
        /// </summary>
        string Alias { get; }

        /// <summary>
        /// Player display name.
        /// </summary>
        string DisplayName { get; }


        /// <summary>
        /// Method will load user avatar asynchronously.
        /// </summary>
        /// <param name="callback">Callback with user avatar texture.</param>
        void GetAvatar(Action<Texture2D> callback);
    }
}