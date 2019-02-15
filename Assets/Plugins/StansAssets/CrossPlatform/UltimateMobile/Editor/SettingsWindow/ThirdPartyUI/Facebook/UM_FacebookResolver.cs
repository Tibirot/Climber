using SA.Facebook;
using SA.Foundation.Editor;

namespace SA.CrossPlatform
{
    public class UM_FacebookResolver : SA_iAPIResolver
    {
        public bool IsSettingsEnabled {
            get {
                return SA_FB_InstallationProcessing.IsSDKInstalled;
            }

            set { }
        }
    }
}