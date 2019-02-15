using UnityEngine;
using System;

using SA.Android.App;
using SA.iOS.UIKit;

namespace SA.CrossPlatform.UI
{
    /// <summary>
    /// A simple dialog containing an DatePicker.
    /// </summary>
    public class UM_DatePickerDialog
    {
#pragma warning disable 414
        [SerializeField] int m_year;
#pragma warning restore 414

        /// <summary>
        /// Creates a new date picker dialog for the specified date.
        /// </summary>
        /// <param name="year">the initially selected year.</param>
        public UM_DatePickerDialog(int year = 0) {
            if(m_year == 0) {
                m_year = DateTime.Now.Year;
            }
        }

        /// <summary>
        /// Start the dialog and display it on screen.
        /// </summary>
        public void Show(Action<UN_DatePickerResult> callback) {
            
            if(Application.isEditor) {
#if UNITY_EDITOR
                UnityEditor.EditorUtility.DisplayDialog(
                    "Not Supported", 
                    "The date picked view emulation is not supported insdie the Editor. " +
                    "The DateTime.Now valu will be returned as dialog result.", 
                    "Okay");
                var result = new UN_DatePickerResult(DateTime.Now);
                callback.Invoke(result);
#endif
            } else {
                switch (Application.platform) {
                    case RuntimePlatform.Android:

                        var date = DateTime.Now;
                        int year = m_year;
                        int month = date.Month - 1; //Compatibility with Android Calendar..
                        int day = date.Day;

                        AN_DatePickerDialog picker = new AN_DatePickerDialog(year, month, day);
                        picker.Show((pickerResult) => {
                            UN_DatePickerResult result;
                            if (pickerResult.IsSucceeded) {
                                var pickedDate = new DateTime(pickerResult.Year, pickerResult.Month + 1, pickerResult.Day);
                                result = new UN_DatePickerResult(pickedDate);
                            } else {
                                result = new UN_DatePickerResult(pickerResult.Error);
                            }
                            callback.Invoke(result);
                        });
                        break;
                    case RuntimePlatform.IPhonePlayer:

                        ISN_UICalendar.PickDate((pickedDate) => {
                            var result = new UN_DatePickerResult(pickedDate);
                            callback.Invoke(result);
                        }, m_year);
                        break;
                }
            }


        }


    }
}