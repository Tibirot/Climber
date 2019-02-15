using UnityEngine;
using System;

using SA.Foundation.Templates;

namespace SA.CrossPlatform.UI
{
    /// <summary>
    /// Object that constains a result of picking a date
    /// using the <see cref="UM_DatePickerDialog"/>
    /// </summary>
    public class UN_DatePickerResult : SA_Result
    {
        private DateTime m_date;

        public UN_DatePickerResult(DateTime date):base() {
            m_date = date;
        }

        public UN_DatePickerResult(SA_Error error):base(error) { }


        /// <summary>
        /// User picked date.
        /// </summary>
        public DateTime Date {
            get {
                return m_date;
            }
        }
    }
}