using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA.CrossPlatform.App
{

    /// <summary>
    /// Information about current running build. Can be used foe debug and report's sending
    /// </summary>
    public interface UM_iBuildInfo
    {

        /// <summary>
        /// Application identifier;
        /// </summary>
        string Identifier { get; }

        /// <summary>
        /// Current app version code.
        /// </summary>
        string Version { get; }
    }
}