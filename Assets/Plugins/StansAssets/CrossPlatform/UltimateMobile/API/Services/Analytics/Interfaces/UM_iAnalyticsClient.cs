namespace SA.CrossPlatform.Analytics
{

    /// <summary>
    /// Analytics cleint interface.
    /// Contain methods that allow report application events to the analytics server.
    /// </summary>
    public interface UM_iAnalyticsClient  : UM_iAnalyticsInternalClient
    {

        /// <summary>
        /// Init Analytics client, should be called as early as possible on app launch.
        /// </summary>
        void Init();


        /// <summary>
        /// Returns <c>true</c> in case client is initted, otherwise <c>false</c>.
        /// </summary>
        bool IsInitialized { get; }
            
    }
}