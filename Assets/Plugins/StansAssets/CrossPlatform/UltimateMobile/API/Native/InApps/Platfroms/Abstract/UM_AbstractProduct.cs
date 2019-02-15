using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA.CrossPlatform.InApp
{
    public abstract class UM_AbstractProduct<T>
    {
        [SerializeField] protected string m_id;
        [SerializeField] protected long m_priceInMicros;
        [SerializeField] protected string m_price;
        [SerializeField] protected string m_title;
        [SerializeField] protected string m_description;
        [SerializeField] protected string m_priceCurrencyCode;
        [SerializeField] protected Texture2D m_icon;
        [SerializeField] protected UM_ProductType m_type;

        private bool m_isActive = false;



        public abstract void OnOverride(T productTemplate);

        public void Override(T productTemplate) {
            OnOverride(productTemplate);
            m_isActive = true;
        }

        /*
    public void Override(UM_AbstractProduct product) {
        var json = JsonUtility.ToJson(product);
        JsonUtility.FromJsonOverwrite(json, this);
    }*/


        public string Id {
            get {
                return m_id;
            }
        }

        public string Price {
            get {
                return m_price;
            }
        }

        public long PriceInMicros {
            get {
                return m_priceInMicros;
            }
        }

        public string Title {
            get {
                return m_title;
            }
        }

        public string Description {
            get {
                return m_description;
            }
        }

        public string PriceCurrencyCode {
            get {
                return m_priceCurrencyCode;
            }
        }

        public Texture2D Icon {
            get {
                return m_icon;
            }
        }

        public UM_ProductType Type {
            get {
                return m_type;
            }
        }

        public bool IsActive {
            get {
                return m_isActive;
            }
        }
    }
}