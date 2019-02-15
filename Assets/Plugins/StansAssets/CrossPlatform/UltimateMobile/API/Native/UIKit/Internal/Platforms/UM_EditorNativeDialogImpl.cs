using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SA.iOS.UIKit;

namespace SA.CrossPlatform.UI
{
    public class UM_EditorNativeDialogImpl : UM_iUIDialog
    {
        #if UNITY_EDITOR
        private UM_NativeDialogBuilder m_builder = null;
        #endif


        public UM_EditorNativeDialogImpl(UM_NativeDialogBuilder builder) {
            #if UNITY_EDITOR
            m_builder = builder;
            #endif

        }

        public void Show() {
#if UNITY_EDITOR

            UM_NativeDialogBuilder.Button noButton;
            if (m_builder.NegativeButton != null) {
                noButton = m_builder.NegativeButton;
            } else {
                noButton = m_builder.DestructiveButton;
            }

            if (m_builder.NeutralButton == null) {

                if(m_builder.NegativeButton == null && m_builder.DestructiveButton == null) {
                    UnityEditor.EditorUtility.DisplayDialog(m_builder.Title, m_builder.Message, m_builder.PositiveButton.Title);
                    m_builder.PositiveButton.ButtonAction.Invoke();
                } else {
                    bool result = UnityEditor.EditorUtility.DisplayDialog(m_builder.Title, m_builder.Message, m_builder.PositiveButton.Title, noButton.Title);
                    if (result) {
                        m_builder.PositiveButton.ButtonAction.Invoke();
                    } else {
                        noButton.ButtonAction.Invoke();
                    }
                }
            } else {
                int option = UnityEditor.EditorUtility.DisplayDialogComplex(m_builder.Title, m_builder.Message, m_builder.PositiveButton.Title, noButton.Title, m_builder.NeutralButton.Title);
                switch (option) {
                    case 0:
                        m_builder.PositiveButton.ButtonAction.Invoke();
                        break;
                    case 1:
                        noButton.ButtonAction.Invoke();
                        break;
                    case 2:
                        m_builder.NeutralButton.ButtonAction.Invoke();
                        break;
                }
            }
#endif
        }

        public void Hide() {
            //Don't supported by editor popups
        }
    }
}