using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA.Foundation.Editor;

namespace SA.CrossPlatform
{
    public abstract class UM_PluginSettingsUI : SA_ServiceLayout
    {
        protected override IEnumerable<string> SupportedPlatforms {
            get {
                return new List<string>() { "iOS", "Android", "Unity Editor" };
            }
        }

        protected override int IconSize {
            get {
                return 25;
            }
        }

        
        protected override int TitleVerticalSpace {
            get {
                return 2;
            }
        }

        protected override void DrawServiceRequirements() {

            
        }
    }
}