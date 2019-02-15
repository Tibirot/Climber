using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SA.iOS.UIKit;

namespace SA.CrossPlatform.UI
{
    public class UM_IOSNativeDialogImpl : UM_iUIDialog
    {

        private ISN_UIAlertController m_dialog;

        public UM_IOSNativeDialogImpl(UM_NativeDialogBuilder builder) {

            m_dialog = new ISN_UIAlertController(builder.Title, builder.Message, ISN_UIAlertControllerStyle.Alert);

            if (builder.PositiveButton != null) {
                m_dialog.AddAction(new ISN_UIAlertAction(builder.PositiveButton.Title, ISN_UIAlertActionStyle.Default, builder.PositiveButton.ButtonAction));
            }

            if (builder.NeutralButton != null) {
                m_dialog.AddAction(new ISN_UIAlertAction(builder.NeutralButton.Title, ISN_UIAlertActionStyle.Default, builder.NeutralButton.ButtonAction));
            }

            if (builder.NegativeButton != null) {
                m_dialog.AddAction(new ISN_UIAlertAction(builder.NegativeButton.Title, ISN_UIAlertActionStyle.Cancel, builder.NegativeButton.ButtonAction));
            }

            if (builder.DestructiveButton != null) {
                m_dialog.AddAction(new ISN_UIAlertAction(builder.DestructiveButton.Title, ISN_UIAlertActionStyle.Destructive, builder.DestructiveButton.ButtonAction));
            }

        }

        public void Show() {
            m_dialog.Present();
        }

        public void Hide() {
            m_dialog.Dismiss();
        }
    }
}