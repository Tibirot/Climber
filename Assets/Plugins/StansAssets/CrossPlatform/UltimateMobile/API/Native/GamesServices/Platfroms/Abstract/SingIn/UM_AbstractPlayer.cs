using System;
using System.Collections.Generic;
using UnityEngine;


namespace SA.CrossPlatform.GameServices
{
    [Serializable]
    public abstract class UM_AbstractPlayer {

        [SerializeField] protected string m_id;
        [SerializeField] protected string m_alias;
        [SerializeField] protected string m_displayName;

        public string Id {
            get {
                return m_id;
            }
        }

        public string Alias {
            get {
                return m_alias;
            }
        }

        public string DisplayName {
            get {
                return m_displayName;
            }
        }
    }
}

