using System;
using System.Collections.Generic;
using UnityEngine;


namespace SA.iOS.Foundation
{
    /// <summary>
    /// Information about linguistic, cultural, and technological conventions 
    /// for use in formatting data for presentation.
    /// </summary>
    [Serializable]
    public class ISN_NSLocale 
    {

        [SerializeField] string m_identifier;
        [SerializeField] string m_countryCode;
        [SerializeField] string m_languageCode;
        [SerializeField] string m_currencySymbol;
        [SerializeField] string m_currencyCode;



        /// <summary>
        /// The identifier for the locale.
        /// </summary>
        public string Identifier {
            get {
                return m_identifier;
            }
        }


        /// <summary>
        /// The country code for the locale.
        /// Examples of country codes include "GB", "FR", and "HK".
        /// </summary>
        public string CountryCode {
            get {
                return m_countryCode;
            }
        }

        /// <summary>
        /// The language code for the locale.
        /// Examples of language codes include "en", "es", and "zh".
        /// </summary>
        public string LanguageCode {
            get {
                return m_languageCode;
            }
        }

        /// <summary>
        /// The currency symbol for the locale.
        /// Example currency symbols include "$", "€", and "¥".
        /// </summary>
        public string CurrencySymbol {
            get {
                return m_currencySymbol;
            }
        }


        /// <summary>
        /// The currency code for the locale.
        /// Example currency codes include "USD", "EUR", and "JPY".
        /// </summary>
        public string CurrencyCode {
            get {
                return m_currencyCode;
            }
        }
    }
}