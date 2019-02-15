using System;
using System.Collections.Generic;
using UnityEngine;


namespace SA.iOS.UIKit.Internal
{
    [Serializable]
    internal class ISN_UIAlertActionId
    {
        [SerializeField] int m_alertId;
        [SerializeField] int m_actionId;

        public int AlertId {
            get {
                return m_alertId;
            }
        }

        public int ActionId {
            get {
                return m_actionId;
            }
        }
    }
}