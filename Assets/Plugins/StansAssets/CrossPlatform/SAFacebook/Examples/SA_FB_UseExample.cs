using UnityEngine;
using UnityEngine.UI;

using SA.Facebook;

public class SA_FB_UseExample : MonoBehaviour
{
    [Header("User Info")]
    [SerializeField] Text m_userName;
    [SerializeField] Text m_userMail;
    [SerializeField] RawImage m_userAvatar;

    [Header("Buttons")]
    [SerializeField] Button m_connect;


    //Make sure that this method will be called only once per app session
    private void Start() {

        //This can be done via editor menu 
        SA_FB_Settings.Instance.SetAppId("1605471223039154");


        //This can also be done via the settings
        //We need email scope to be able to get user email
        if(SA_FB_Settings.Instance.Scopes.Contains("email")) {
            SA_FB_Settings.Instance.Scopes.Add("email");
        }
       

        m_connect.interactable = false;
        SA_FB.Init(() => {
            Debug.Log("Init Completed");
            m_connect.interactable = true;
            UpdateAccountUI();
        });

       

        //let's define button action based on user state
        m_connect.onClick.AddListener(() => {
            if (!SA_FB.IsLoggedIn) {
                SignInFlow();
            } else {
                SignOutFlow();
            }
        });
    }

    private void SignInFlow() {
        SA_FB.Login((result) => {
            if(result.IsSucceeded) {
                Debug.Log("Login Succeeded");
                UpdateAccountUI();
            } else {
                Debug.Log("Failed to login: " + result.Error.FullMessage);
            }
        });
    }

    private void SignOutFlow() {
        SA_FB.LogOut();
        UpdateAccountUI();
    }


    private void UpdateAccountUI() {
        if (SA_FB.IsLoggedIn) {
           if(SA_FB.CurrentUser != null) {
                SetUserInfoUI(SA_FB.CurrentUser);
           } else {
                SA_FB.GetLoggedInUserInfo((result) => {
                    if(result.IsSucceeded) {
                        SetUserInfoUI(result.User);

                        Debug.Log("result.User.Id: " + result.User.Id);
                        Debug.Log("result.User.Name: " + result.User.Name);
                        Debug.Log("result.User.UserName: " + result.User.UserName);
                        Debug.Log("result.User.FirstName: " + result.User.FirstName);
                        Debug.Log("result.User.LastName: " + result.User.LastName);
                       
                        Debug.Log("result.User.Locale: " + result.User.Locale);
                        Debug.Log("result.User.Location: " + result.User.Location);
                        Debug.Log("result.User.PictureUrl: " + result.User.PictureUrl);
                        Debug.Log("result.User.ProfileUrl: " + result.User.ProfileUrl);
                        Debug.Log("result.User.AgeRange: " + result.User.AgeRange);
                        Debug.Log("result.User.Birthday: " + result.User.Birthday);
                        Debug.Log("result.User.Gender: " + result.User.Gender);
                        Debug.Log("result.User.AgeRange: " + result.User.AgeRange);
                        Debug.Log("result.User.RawJSON: " + result.User.RawJSON);
                       

                    } else {
                        Debug.Log("Failed to load user Info: " + result.Error.FullMessage);
                    }
                });
            }
        } else {
            m_connect.GetComponentInChildren<Text>().text = "Sing in";
            m_userName.text = "Signed out";
            m_userMail.text = "Signed out";

            m_userAvatar.texture = null;
        }
    }

    private void SetUserInfoUI(SA_FB_User user) {
        m_connect.GetComponentInChildren<Text>().text = "Sing out";
        m_userName.text = user.Name;
        m_userMail.text = user.Email;


        user.GetProfileImage(SA_FB_ProfileImageSize.large, (texture) => {
            m_userAvatar.texture = texture;
        });

    }
}
