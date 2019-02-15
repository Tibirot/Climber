using System;
using System.Collections.Generic;
using UnityEngine;


namespace SA.CrossPlatform.App
{
    [Serializable]
    public abstract class UM_AbstractContact 
    {
        [SerializeField] protected string m_name;
        [SerializeField] protected string m_phone;
        [SerializeField] protected string m_email;


        public string Name {
            get {
                return m_name;
            }
        }

        public string Phone {
            get {
                return m_phone;
            }
        }

        public string Email {
            get {
                return m_email;
            }
        }
    }
}