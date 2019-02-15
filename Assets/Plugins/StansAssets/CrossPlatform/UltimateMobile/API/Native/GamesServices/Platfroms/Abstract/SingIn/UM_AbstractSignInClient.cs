using System;

using SA.Foundation.Templates;
using SA.Foundation.Events;

using SA.CrossPlatform.Analytics;

namespace SA.CrossPlatform.GameServices
{
    internal abstract class UM_AbstractSignInClient 
    {

        private UM_PlayerInfo m_currentPlayerInfo = new UM_PlayerInfo(UM_PlayerState.SignedOut, null);
        private SA_Event m_onPlayerChanged = new SA_Event();

        private SA_Event<SA_Result> m_singInCallback = new SA_Event<SA_Result>();

        private bool m_singInFlowInProgress = false;


        //--------------------------------------
        // Abstract Methods
        //--------------------------------------

        protected abstract void StartSingInFlow(Action<SA_Result> callback);


        //--------------------------------------
        // Public Methods
        //--------------------------------------

        public void SingIn(Action<SA_Result> callback) {

            m_singInCallback.AddListener(callback);

            //Preventing double sing in
            if (m_singInFlowInProgress) { return;}

            m_singInFlowInProgress = true;
            StartSingInFlow((result) => {

                m_singInFlowInProgress = false;
                m_singInCallback.Invoke(result);

                m_singInCallback.RemoveAllListeners();
            });
        }

        //--------------------------------------
        // Get / Set
        //--------------------------------------

        public SA_iEvent OnPlayerUpdated {
            get {
                return m_onPlayerChanged;
            }
        }


        public UM_PlayerInfo PlayerInfo {
            get {
                return m_currentPlayerInfo;
            }
        }

        public bool IsSingInFlowInProgress {
            get {
                return m_singInFlowInProgress;
            }
        }


        //--------------------------------------
        // Protected Methods 
        //--------------------------------------

        protected void UpdateSignedPlater(UM_PlayerInfo info) {
            m_currentPlayerInfo = info;
            m_onPlayerChanged.Invoke();

            UM_AnalyticsInternal.OnPlayerUpdated(info);
        }


    
    }
}