using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.CrossPlatform.UI
{
    /// <summary>
    /// Static class with the collection of dialogs helper methods.
    /// </summary>
    public static class UM_DialogsUtility 
    {

        /// <summary>
        /// Creates new simple alert and immediately shows it.
        /// </summary>
        /// <param name="title">Alert title.</param>
        /// <param name="message">Alert message.</param>
        public static void ShowMessage(string title, string message)
        {
            var builder = new UM_NativeDialogBuilder(title, message);
            builder.SetPositiveButton("Okay", () => {});

            var dialog = builder.Build();
            dialog.Show();
        }
    }
}