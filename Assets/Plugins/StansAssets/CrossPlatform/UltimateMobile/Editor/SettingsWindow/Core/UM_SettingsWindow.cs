using UnityEngine;
using UnityEditor;
using SA.Foundation.Editor;

using SA.iOS;
using SA.Android;
using System;
using System.Collections.Generic;

namespace SA.CrossPlatform
{

    public class UM_SettingsWindow : SA_PluginSettingsWindow<UM_SettingsWindow> {


        [Serializable]
        public class SelectedBlockInfo {
            public UM_UIPlatform Platform;
            public string SettingsBlockTypeName;
        }

        private SA_ServicesTab m_currentServiceTab;
        private SA_ServicesTab m_3rdPartyServiceTab;

        private const int TOOLBAR_BUTTONS_HEIGHT = 19;
        private const int TOOLBAR_BUTTONS_SPACE = -10;

        private const string DESCRIPTION = "The Ultimate plugin for the mobile development and more. " +
                "All the service and APIs for diffrent platfroms in one place. " +
                "Bound with unified API";


        [SerializeField] UM_UIPlatform m_selectedPlatform = UM_UIPlatform.Unified;
        [SerializeField] SA_HyperToolbar m_pluginsToolbar;

        [SerializeField] SA_HyperLabel m_backLink;
        [SerializeField] List<SelectedBlockInfo> m_history = new List<SelectedBlockInfo>();



        //--------------------------------------
        // Initialization
        //--------------------------------------

        protected override void OnAwake() 
        {
            SetHeaderTitle(UM_Settings.PLUGIN_NAME);
            SetHeaderVersion(UM_Settings.FormattedVersion);

            SetHeaderDescription(DESCRIPTION);
            SetDocumentationUrl(UM_Settings.DOCUMENTATION_URL);

        
            var backIcon = SA_Skin.GetGenericIcon("back.png");
            m_backLink = new SA_HyperLabel(new GUIContent("Back", backIcon), EditorStyles.miniLabel);
            m_backLink.SetMouseOverColor(SA_PluginSettingsWindowStyles.SelectedElementColor);



            UpdateToolBarByPluginIndex();

        }


        protected override void OnCreate() 
        {
            base.OnCreate();
            m_pluginsToolbar = new SA_HyperToolbar();
            m_pluginsToolbar.SetButtonsHeight(TOOLBAR_BUTTONS_HEIGHT);
            m_pluginsToolbar.SetItemsSapce(TOOLBAR_BUTTONS_SPACE);


            AddPlatform("Unified", UM_Skin.GetDefaultIcon("ultimate_icon_pro.png"));
            AddPlatform("Android", UM_Skin.GetPlatformIcon("android_icon.png"));
            AddPlatform("iOS", UM_Skin.GetPlatformIcon("ios_icon.png"));

            m_pluginsToolbar.SetSelectedIndex((int) UM_UIPlatform.Unified);
        }


        //--------------------------------------
        // Static Methods
        //--------------------------------------

        public static void SelectBlock(SelectedBlockInfo info) 
        {
            var wnd = GetWindow<UM_SettingsWindow>();
            wnd.m_history.Add(info);
        }

        //--------------------------------------
        // Private Methods
        //--------------------------------------


        private void UpdateToolBarByPluginIndex(bool forced = false) 
        {

            if(forced) 
            {
                m_menuToolbar = new SA_HyperToolbar();
                m_tabsLayout.Clear();
            }
          

            switch (m_pluginsToolbar.SelectionIndex) {
                case (int)UM_UIPlatform.IOS:
                    m_3rdPartyServiceTab = null;
                    m_currentServiceTab = CreateInstance<ISN_ServicesTab>();
                    AddMenuItem("SERVICES", m_currentServiceTab, forced);
                    AddMenuItem("XCODE", CreateInstance<ISN_XCodeTab>(), forced);
                    AddMenuItem("SETTINGS", CreateInstance<ISN_SettingsTab>(), forced);

                    SetHeaderTitle(ISN_Settings.PLUGIN_NAME);
                    SetHeaderVersion(ISN_Settings.FormattedVersion);
                    SetHeaderDescription(ISN_SettingsWindow.DESCRIPTION);
                    SetDocumentationUrl(ISN_Settings.DOCUMENTATION_URL);

                    break;
                case (int)UM_UIPlatform.Android:
                    m_3rdPartyServiceTab = null;
                    m_currentServiceTab = CreateInstance<AN_ServicesTab>();
                    AddMenuItem("SERVICES", m_currentServiceTab, forced);
                    AddMenuItem("MANIFEST", CreateInstance<AN_ManifestTab>(), forced);
                    AddMenuItem("SETTINGS", CreateInstance<AN_SettingsTab>(), forced);

                    SetHeaderTitle(AN_Settings.PLUGIN_NAME);
                    SetHeaderVersion(AN_Settings.FormattedVersion);

                    SetHeaderDescription(AN_SettingsWindow.DESCRIPTION);
                    SetDocumentationUrl(AN_Settings.DOCUMENTATION_URL);


                    break;
                case (int)UM_UIPlatform.Unified:
                  
                    m_currentServiceTab = CreateInstance<UM_ServicesTab>();
                    m_3rdPartyServiceTab = CreateInstance<UM_3rdPartyServicesTab>();
                    AddMenuItem("SERVICES", m_currentServiceTab, forced);
                    AddMenuItem("3RD-PARTY", m_3rdPartyServiceTab, forced);

                    AddMenuItem("SUMMARY", CreateInstance<UM_SummaryTab>(), forced);

                    SetHeaderTitle(UM_Settings.PLUGIN_NAME);
                    SetHeaderVersion(UM_Settings.FormattedVersion);
                    SetHeaderDescription(DESCRIPTION);
                    SetDocumentationUrl(UM_Settings.DOCUMENTATION_URL);
                    break;
            }

            AddMenuItem("ABOUT", CreateInstance<SA_PluginAboutLayout>(), forced);

            foreach (var layout in m_tabsLayout) {
                layout.OnLayoutEnable();
            }

        }



        private void AddPlatform(string platformName, Texture icon) {

            var style = new GUIStyle(EditorStyles.miniLabel);
            if (!EditorGUIUtility.isProSkin) {
               style.normal.textColor =  SA_PluginSettingsWindowStyles.GerySilverColor;
            }
            var button = new SA_HyperLabel(new GUIContent(platformName, icon), style);
            button.SetMouseOverColor(SA_PluginSettingsWindowStyles.SelectedElementColor);

            if(!EditorGUIUtility.isProSkin) {
                button.GuiColorOverride(true);
            }
            m_pluginsToolbar.AddButtons(button);
        }

     

        protected override void OnLayoutGUI() {
            var newSelection = m_currentServiceTab.SelectedService;

            if(newSelection == null && m_3rdPartyServiceTab != null) {
                newSelection =  m_3rdPartyServiceTab.SelectedService;
            }

            if (newSelection != null) {
                var info = new SelectedBlockInfo();
                info.Platform = m_selectedPlatform;
                info.SettingsBlockTypeName = newSelection.GetType().Name;
                newSelection.UnSelect();


                SelectBlock(info);
            }


            if (m_history.Count > 0) {

                var activeBlockInfo = m_history[m_history.Count - 1];
                SetSelectedPlatform(activeBlockInfo.Platform);

                var block = m_currentServiceTab.GetBlockByTypeName(activeBlockInfo.SettingsBlockTypeName);

                //Probably that was another services block
                if (block == null && m_3rdPartyServiceTab != null) {
                    block = m_3rdPartyServiceTab.GetBlockByTypeName(activeBlockInfo.SettingsBlockTypeName);
                }

                DrawBrowserTopbar();
                block.DrawHeaderUI();
                DrawScrollView(() => {
                    block.DrawServiceUI();
                });
               

            } else {
                DrawTopbar();
                DrawHeader();

                var tabIndex = DrawMenu();
                
                if (!string.IsNullOrEmpty(m_SearchString))
                {
                    DrawScrollView(() => {
                        m_currentServiceTab.OnSearchGUI(m_SearchString);
                        if (m_3rdPartyServiceTab != null)
                        {
                            m_3rdPartyServiceTab.OnSearchGUI(m_SearchString);
                        }
                    });
                }
                else
                {
                    DrawScrollView(() => {
                        OnTabsGUI(tabIndex);
                    });
                }
                
              
            }
                
        }

        protected override void BeforeGUI() {
            EditorGUI.BeginChangeCheck();
        }



        protected override void AfterGUI() {
            if(EditorGUI.EndChangeCheck()) {
                AN_SettingsWindow.SaveSettins();
                ISN_SettingsWindow.SaveSettings();

                UM_Settings.Save();
            }
        }


        private void DrawTopbar() {
            GUILayout.Space(2);
            using (new SA_GuiBeginHorizontal()) {
               // DrawDocumentationLink();
              //  GUILayout.FlexibleSpace();
                using (new SA_GuiBeginVertical()) {
                    GUILayout.Space(-1);
                    var index = m_pluginsToolbar.Draw();
                    if (index != (int)m_selectedPlatform) {
                        m_selectedPlatform = (UM_UIPlatform)index;
                        UpdateToolBarByPluginIndex(true);
                    }
                }
                GUILayout.FlexibleSpace();

                using (new SA_GuiBeginVertical())
                {
                    //GUILayout.Space(5);
                    DrawSearchBar();
                }
                
               

            }
            GUILayout.Space(5);
        }


        private void DrawBrowserTopbar() {
            GUILayout.Space(2);
            using (new SA_GuiBeginHorizontal()) {
                var width = m_backLink.CalcSize().x + 5f;
                var clicked = m_backLink.Draw(GUILayout.Width(width));
                if (clicked) {
                    m_history.RemoveAt(m_history.Count - 1);
                }
                GUILayout.FlexibleSpace();
                using (new SA_GuiBeginVertical()) {
                    GUILayout.Space(-1);
                    var currentSelectedButton = m_pluginsToolbar.Buttons[m_pluginsToolbar.SelectionIndex];
                    width = currentSelectedButton.CalcSize().x + TOOLBAR_BUTTONS_SPACE;

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.Space();
                        currentSelectedButton.Draw(GUILayout.Width(width), GUILayout.Height(TOOLBAR_BUTTONS_HEIGHT));
                        EditorGUILayout.Space();
                    }
                    EditorGUILayout.EndHorizontal();
                }


            }
            GUILayout.Space(5);
        }



        private void SetSelectedPlatform(UM_UIPlatform platform) {
            if(m_selectedPlatform == platform) {
                return;
            }

            m_selectedPlatform  = platform;
            m_pluginsToolbar.SetSelectedIndex((int)m_selectedPlatform);
            UpdateToolBarByPluginIndex(true);

        }

       
    }
}