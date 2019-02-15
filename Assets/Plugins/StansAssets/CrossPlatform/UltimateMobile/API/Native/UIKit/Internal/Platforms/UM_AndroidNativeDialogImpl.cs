using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SA.Android.App;


namespace SA.CrossPlatform.UI
{
    public class UM_AndroidNativeDialogImpl : UM_iUIDialog
    {
        private AN_AlertDialog m_dialog;

        public UM_AndroidNativeDialogImpl(UM_NativeDialogBuilder builder) {
            m_dialog = new AN_AlertDialog(AN_DialogTheme.Light);

            m_dialog.Title = builder.Title;
            m_dialog.Message = builder.Message;

            if(builder.PositiveButton != null) {
                m_dialog.SetPositiveButton(builder.PositiveButton.Title, builder.PositiveButton.ButtonAction);
            }

            if (builder.NegativeButton != null) {
                m_dialog.SetNegativeButton(builder.NegativeButton.Title, builder.NegativeButton.ButtonAction);
            }

            if (builder.DestructiveButton != null) {
                m_dialog.SetNegativeButton(builder.DestructiveButton.Title, builder.DestructiveButton.ButtonAction);
            }

            if (builder.NeutralButton != null) {
                m_dialog.SetNeutralButton(builder.NeutralButton.Title, builder.NeutralButton.ButtonAction);
            }

        }

        public void Show() {
            m_dialog.Show();
        }

        public void Hide() {
            m_dialog.Hide();
        }

     
    }
}