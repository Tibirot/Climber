

using SA.Foundation.Templates;
using SA.CrossPlatform.Analytics;

namespace SA.CrossPlatform.GameServices
{
    internal abstract class UM_AbstractLeaderboardsClient 
    {   

        protected void ReportScoreSubmited(string leaderboardId, long score, SA_Result result) {
            UM_AnalyticsInternal.OnScoreSubmit(leaderboardId, score, result);
        }
    }
}