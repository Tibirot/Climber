using UnityEngine;
using UnityEditor;
using SA.iOS;
using SA.Android;

using SA.Foundation.Editor;

namespace SA.CrossPlatform
{
    public class UM_SummaryTab : SA_GUILayoutElement
    {

        private SA_CollapsableWindowBlockLayout m_iosRequirements;
        private SA_CollapsableWindowBlockLayout m_androidRequirements;
        private SA_CollapsableWindowBlockLayout m_unityRequirements;

        private SA_PluginActiveTextLink m_learnMoreLink;

        public override void OnLayoutEnable()
        {

            base.OnLayoutEnable();

            var content = new GUIContent("iOS Build Requirements", UM_Skin.GetPlatformIcon("ios_icon.png"));
            m_iosRequirements = new SA_CollapsableWindowBlockLayout(content, () => { XCodeRequirements(); });

            content = new GUIContent("Android Build Requirements", UM_Skin.GetPlatformIcon("android_icon.png"));
            m_androidRequirements = new SA_CollapsableWindowBlockLayout(content, () => { AndroidRequirements(); });

            content = new GUIContent("Unity Project Requirements ", UM_Skin.GetPlatformIcon("unity_icon.png"));
            m_unityRequirements = new SA_CollapsableWindowBlockLayout(content, () => { DefineSymbols(); });

            m_learnMoreLink = new SA_PluginActiveTextLink("[?] Learn More");

        }

        public override void OnGUI()
        {

            EditorGUILayout.HelpBox("The summary tab provides useful summary information " +
                                    "for example you can check platform requirements based on current plugin services configuration.",
                MessageType.Info);




            using (new SA_GuiBeginHorizontal())
            {
                GUILayout.FlexibleSpace();
                var click = m_learnMoreLink.Draw(GUILayout.Width(90));
                if (click)
                {
                    Application.OpenURL("https://unionassets.com/ultimate-mobile-pro/summary-tab-768");
                }
            }

            m_iosRequirements.OnGUI();
            m_androidRequirements.OnGUI();
            m_unityRequirements.OnGUI();


            using (new SA_WindowBlockWithSpace("Plugin Use Examples"))
            {
                EditorGUILayout.HelpBox("Check out some usage example we made for you. " +
                                        "More code samples can be found inside the documentation.", MessageType.Info);

                var width = 150;
                using (new SA_GuiBeginHorizontal())
                {
                    EditorGUILayout.Space();
                    var pressed = GUILayout.Button("Open Example Scene", GUILayout.Width(width));
                    if (pressed)
                    {
                        UM_ExamplesManager.OpenWelcomeScene();
                    }

                    pressed = GUILayout.Button("Build Example Scene", GUILayout.Width(width));
                    if (pressed)
                    {
                        UM_ExamplesManager.BuildWelcomeScene();
                    }
                }
            }


        }

        private void DefineSymbols()
        {


            using (new SA_H2WindowBlockWithSpace(new GUIContent("SCRIPTING DEFINE SYMBOLS")))
            {
                var defines = SA_EditorDefines.GetScriptingDefines();
                if (defines.Length > 0)
                {
                    foreach (var define in defines)
                    {
                        var icon = UM_Skin.GetDefaultIcon("hash_tag_icon.png");
                        SA_EditorGUILayout.SelectableLabel(new GUIContent(define, icon));
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("No additional scripting defines required.", MessageType.Info);
                }

            }
        }

        private void AndroidRequirements()
        {
            var buildRequirements = new AN_AndroidBuildRequirements();
            foreach (var resolver in AN_Preprocessor.Resolvers)
            {

                if (!resolver.IsSettingsEnabled)
                {
                    continue;
                }

                foreach (var activity in resolver.BuildRequirements.Activities)
                {
                    buildRequirements.AddActivity(activity);
                }

                foreach (var property in resolver.BuildRequirements.ApplicationProperties)
                {
                    buildRequirements.AddApplicationProperty(property);
                }

                foreach (var permission in resolver.BuildRequirements.Permissions)
                {
                    if (!buildRequirements.Permissions.Contains(permission))
                    {
                        buildRequirements.AddPermission(permission);
                    }
                }

                foreach (var dependency in resolver.BuildRequirements.BinaryDependencies)
                {
                    buildRequirements.AddBinaryDependency(dependency);
                }
            }


            AN_ServiceSettingsUI.DrawRequirementsUI(buildRequirements);
        }


        private void XCodeRequirements()
        {
            var requirements = new ISN_XcodeRequirements();
            foreach (var resolver in ISN_Preprocessor.Resolvers)
            {

                if (!resolver.IsSettingsEnabled)
                {
                    continue;
                }

                foreach (var framework in resolver.XcodeRequirements.Frameworks)
                {
                    requirements.AddFramework(framework);
                }

                foreach (var lib in resolver.XcodeRequirements.Libraries)
                {
                    requirements.AddLibrary(lib);
                }

                foreach (var capability in resolver.XcodeRequirements.Capabilities)
                {
                    requirements.Capabilities.Add(capability);
                }


                foreach (var key in resolver.XcodeRequirements.PlistKeys)
                {
                    requirements.AddInfoPlistKey(key);
                }

                foreach (var property in resolver.XcodeRequirements.Properties)
                {
                    bool duplicate = false;
                    foreach (var p in requirements.Properties)
                    {
                        if (p.Name.Equals(property.Name))
                        {
                            duplicate = true;
                            break;
                        }
                    }

                    if (!duplicate)
                    {
                        requirements.AddBuildProperty(property);
                    }

                }

            }

            ISN_ServiceSettingsUI.DrawRequirementsUI(requirements);

        }
    }
}