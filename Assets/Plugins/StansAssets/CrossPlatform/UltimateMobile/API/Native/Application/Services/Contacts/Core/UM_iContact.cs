using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.CrossPlatform.App
{
    public interface UM_iContact {

        /// <summary>
        /// Contact name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Phone number
        /// </summary>
        string Phone { get; }

        /// <summary>
        /// The email addres
        /// </summary>
        string Email { get; }
    }
}

