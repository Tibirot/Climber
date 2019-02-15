using System;


namespace SA.CrossPlatform.UI { 

    public class UM_NativeDialogBuilder
    {

        public class Button
        {
            public string Title;
            public Action ButtonAction;

            public Button(string title, Action buttonAction) {
                Title = title;
                ButtonAction = buttonAction;
            }
        }


        private string m_title;
        private string m_message;

        private Button m_neutralButton;
        private Button m_positiveButton;
        private Button m_negativeButton;
        private Button m_destructiveButton;
        

        /// <summary>
        /// Create new native dialog builder instance.
        /// </summary>
        /// <param name="title">Alert Title.</param>
        /// <param name="message">Alert Message</param>
        public UM_NativeDialogBuilder(string title, string message) {
            m_title = title;
            m_message = message;
        }



        /// <summary>
        /// Set alert Title.
        /// </summary>
        /// <param name="title">New alert title.</param>
        public void SetTitle(string title) {
            m_title = title;
        }

        /// <summary>
        /// Set alert Message.
        /// </summary>
        /// <param name="message">New alert message.</param>
        public void SetMessage(string message) {
            m_message = message;
        }


        /// <summary>
        /// Alert Title.
        /// </summary>
        public string Title {
            get {
                return m_title;
            }
        }

        /// <summary>
        /// Alert Message.
        /// </summary>
        public string Message {
            get {
                return m_message;
            }
        }

        /// <summary>
        /// Gets the neutral button.
        /// </summary>
        public Button NeutralButton {
            get {
                return m_neutralButton;
            }
        }

        /// <summary>
        /// Gets the positive button.
        /// </summary>
        public Button PositiveButton {
            get {
                return m_positiveButton;
            }
        }

        /// <summary>
        /// Gets the negative button.
        /// </summary>
        public Button NegativeButton {
            get {
                return m_negativeButton;
            }
        }

        /// <summary>
        /// Gets the destructive button.
        /// </summary>
        public Button DestructiveButton {
            get {
                return m_destructiveButton;
            }
        }

        /// <summary>
        /// Set button with default style.
        /// </summary>
        /// <param name="text">button text</param>
        /// <param name="callback">click listener</param>
        public void SetNeutralButton(string text, Action callback) {
            m_neutralButton = new Button(text, callback);
        }

        /// <summary>
        /// Set button with positive style.
        /// </summary>
        /// <param name="text">button text</param>
        /// <param name="callback">click listener</param>
        public void SetPositiveButton(string text, Action callback) {
            m_positiveButton = new Button(text, callback);
        }

        /// <summary>
        /// Set button with negative style, 
        /// that indicates the action cancels the operation and leaves things unchanged.
        /// </summary>
        /// <param name="text">button text</param>
        /// <param name="callback">click listener</param>
        public void SetNegativeButton(string text, Action callback) {
            m_negativeButton = new Button(text, callback);
        }

        /// <summary>
        /// Set button with destructive style, 
        /// that indicates the action might change or delete data.
        /// </summary>
        /// <param name="text">button text</param>
        /// <param name="callback">click listener</param>
        public void SetDestructiveButton(string text, Action callback) {
            m_destructiveButton = new Button(text, callback);
        }

        /// <summary>
        /// Build the dialog based on a builder properties
        /// </summary>
        /// <returns></returns>
        public UM_iUIDialog Build() {
            return UM_DialogsFactory.CreateDialog(this);
        }
    }
}