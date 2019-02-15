using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using SA.CrossPlatform.UI;
using SA.CrossPlatform.Advertisement;

using SA.Foundation.Threading;


using SA.Foundation.Utility;

public class UM_AdvertisementExample : MonoBehaviour {

    [Header("Settings")]
    [SerializeField] Dropdown m_adsClientDropdown;

    [Header("Banner")]
    [SerializeField] Button m_loadBanner;
    [SerializeField] Button m_showBanner;
    [SerializeField] Button m_showWhenReadyBanner;
    [SerializeField] Button m_hideBanner;
    [SerializeField] Button m_destroyBanner;


    [Header("Rewarded")]
    [SerializeField] Button m_loadRewarded;
	[SerializeField] Button m_showRewarded;
	[SerializeField] Button m_showWhenReadyRewarded;


    [Header("Non-Rewarded")]
    [SerializeField] Button m_loadNonRewarded;
    [SerializeField] Button m_showNonRewarded;
    [SerializeField] Button m_showWhenReadyNonRewarded;

    private UM_iAdsClient m_adsClient;

    private const string NONE_CLIENT_OPTION = "None";

    private readonly List<string> m_adCleintsList =  new List<string>();

    void Start () {
        //Adding all UM supported ad platfroms to the Dropdown.

        m_adCleintsList.Add(NONE_CLIENT_OPTION);
        foreach (UM_AdPlatform platform in (UM_AdPlatform[])Enum.GetValues(typeof(UM_AdPlatform))) {
            m_adCleintsList.Add(platform.ToString());
        }

        m_adsClientDropdown.ClearOptions();
        m_adsClientDropdown.AddOptions(m_adCleintsList);

        m_adsClientDropdown.onValueChanged.AddListener((index) => {
            SwitchAdPlatform(index);
        });
        

        SetUpBannerButtons();
        SetUpRewardedButtons();
        SetUpNonRewardedButtons();

        SetInteractable(false);

    }

    private void Update() {
        if (m_adsClient == null) {
            return;
        }


        m_hideBanner.interactable = m_adsClient.Banner.IsReady;
        m_destroyBanner.interactable = m_adsClient.Banner.IsReady;

        m_showBanner.interactable = m_adsClient.Banner.IsReady;
        m_showRewarded.interactable = m_adsClient.RewardedAds.IsReady;
        m_showNonRewarded.interactable = m_adsClient.NonRewardedAds.IsReady;


    }


    private void SetUpBannerButtons() {
        m_loadBanner.onClick.AddListener(() => {
            Debug.Log("Banner ad load request sent");
            m_loadBanner.interactable = false;
            m_adsClient.Banner.Load((result) => {
                if(result.IsSucceeded) {
                    Debug.Log("Banner ad loaded");
                } else {
                    m_loadBanner.interactable = true;
                    Debug.Log("Failed to load banner ads: " + result.Error.Message);
                }
            });
        });


        m_showBanner.onClick.AddListener(() => {
            m_adsClient.Banner.Show(() => {
                Debug.Log("Banner Appeared");
            });
        });



        m_showWhenReadyBanner.onClick.AddListener(() => {
            m_loadBanner.interactable = false;
            m_showWhenReadyBanner.interactable = false;
            m_adsClient.Banner.Load((result) => {

                m_loadBanner.interactable = true;
                m_showWhenReadyBanner.interactable = true;

                if (result.IsSucceeded) {
                    m_adsClient.Banner.Show(() => { });
                }
            });
        });

        m_hideBanner.onClick.AddListener(() => {
            m_adsClient.Banner.Hide();
        });

        m_destroyBanner.onClick.AddListener(() => {
            m_loadBanner.interactable = true;
            m_adsClient.Banner.Destroy();
        });
    }


    private void SetUpRewardedButtons() {
        m_loadRewarded.onClick.AddListener(() => {
            m_loadRewarded.interactable = false;
            Debug.Log("Rewarded Ads load request sent");
            m_adsClient.RewardedAds.Load((result) => {
                if (result.IsSucceeded) {
                    Debug.Log("RewardedAds loaded");
                } else {
                    m_loadRewarded.interactable = true;
                    Debug.Log("Failed to load RewardedAds: " + result.Error.Message);
                }
            });
        });


        m_showRewarded.onClick.AddListener(() => {
            m_adsClient.RewardedAds.Show((result) => {
                Debug.Log("RewardedAds Finished");
                ShowMessage("RewardedAds Result", "Ads finished with result: " + result);
                m_loadRewarded.interactable = true; 
            });
        });



        m_showWhenReadyRewarded.onClick.AddListener(() => {

            m_loadRewarded.interactable = false;
            m_showWhenReadyRewarded.interactable = false;
            m_adsClient.RewardedAds.Load((result) => {

                m_loadRewarded.interactable = true;
                m_showWhenReadyRewarded.interactable = true;

                if (result.IsSucceeded) {
                    m_adsClient.RewardedAds.Show((adsResult) => {
                        if(adsResult == UM_RewardedAdsResult.Finished) {
                            //reward a player
                        }
                        Debug.Log("RewardedAds Finished");
                        ShowMessage("RewardedAds Result", "Ads finished with result: " + adsResult);
                    });
                }
            });
        });

    }


    private void SetUpNonRewardedButtons() {
        m_loadNonRewarded.onClick.AddListener(() => {
            m_loadNonRewarded.interactable = false;
            m_adsClient.NonRewardedAds.Load((result) => {
                if (result.IsSucceeded) {
                    Debug.Log("NonRewardedAds loaded");
                } else {
                    Debug.Log("Failed to load NonRewardedAds: " + result.Error.Message);
                }
            });
        });


        m_showNonRewarded.onClick.AddListener(() => {
            m_adsClient.NonRewardedAds.Show(() => {
                Debug.Log("Non Rewarded Ads closed");
            });
            m_loadNonRewarded.interactable = true;
        });



        m_showWhenReadyNonRewarded.onClick.AddListener(() => {

            m_loadNonRewarded.interactable = false;
            m_showWhenReadyNonRewarded.interactable = false;

            m_adsClient.NonRewardedAds.Load((result) => {

                m_loadNonRewarded.interactable = true;
                m_showWhenReadyNonRewarded.interactable = true;

                if (result.IsSucceeded) {
                    m_adsClient.NonRewardedAds.Show(() => {
                        Debug.Log("Non Rewarded Ads closed");
                    });
                }
            });
        });

    }

  
    private void SwitchAdPlatform(int index) {

        string platfromName = m_adCleintsList[index];
        if (platfromName.Equals(NONE_CLIENT_OPTION)) {
            return;
        }

        UM_AdPlatform platfrom = SA_EnumUtil.ParseEnum<UM_AdPlatform>(platfromName);
        SwitchAdPlatform(platfrom);
    }

    private void SwitchAdPlatform(UM_AdPlatform platfrom) {

        Debug.Log(platfrom + " Selected.");
        m_adsClient = UM_AdvertisementService.GetClient(platfrom);

        if (m_adsClient == null) {
            ShowMessage("Error", "Cleint not configured");
            SetInteractable(false);
        } else {
            if (m_adsClient.IsInitialized) {
                SetInteractable(true);
                ShowMessage(platfrom.ToString(), platfrom.ToString() + " advertesment platfrom is ready to be used.");

            } else {
                SetInteractable(false);
                m_adsClient.Initialize((result) => {
                    if(result.IsSucceeded) {
                        SetInteractable(true);
                        ShowMessage(platfrom.ToString(), platfrom.ToString() + " advertesment platfrom is ready to be used.");
                    } else {
                        ShowMessage(platfrom.ToString() + " failed to init", result.Error.FullMessage);
                    }
                });
            }
            
        }
            
    }


    private void SetInteractable(bool interactable) {
        Button[] buttons = GameObject.FindObjectsOfType<Button>();
        foreach(Button button in buttons) {
            button.interactable = interactable;
        }
    }


    private void ShowMessage(string title, string message) {

     
        var builder = new UM_NativeDialogBuilder(title, message);
        builder.SetPositiveButton("Okay", () => {
           
        });

        var dialog = builder.Build();
        dialog.Show();

    }
}
