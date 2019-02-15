using System;
using System.Collections.Generic;
using UnityEngine;

using SA.Foundation.Events;
using SA.Foundation.Templates;


namespace SA.CrossPlatform.GameServices
{

    /// <summary>
    /// A client to interact with sing-in flow.
    /// </summary>
    public interface UM_iSignInClient 
    {
        /// <summary>
        /// Start's Sing In flow
        /// </summary>
        /// <param name="callback">Operation assync callback</param>
        void SingIn(Action<SA_Result> callback);

        /// <summary>
        /// Start's Sing Out flow
        /// </summary>
        /// <param name="callback">Operation assync callback</param>
        void SingOut(Action<SA_Result> callback);

        
        /// <summary>
        /// Fired when pleyaer info is changed.
        /// Player Singed in / Singned out / Changed Account
        /// </summary>
        SA_iEvent OnPlayerUpdated { get; }

        /// <summary>
        /// Current Player info
        /// Use this property to find out current <see cref="UM_PlayerState"/> 
        /// and get singed <see cref="UM_iPlayer"/> object
        /// </summary>
        UM_PlayerInfo PlayerInfo { get; }

    }
}