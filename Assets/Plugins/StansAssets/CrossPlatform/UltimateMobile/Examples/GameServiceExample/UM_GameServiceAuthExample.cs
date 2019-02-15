using UnityEngine;
using UnityEngine.UI;
using SA.CrossPlatform.GameServices;

public class UM_GameServiceAuthExample : MonoBehaviour {

    [Header("User Info")]
    [SerializeField] Text m_userId;
    [SerializeField] Text m_userName;
    [SerializeField] Text m_userAlias;
    [SerializeField] RawImage m_userAvatar;
    [SerializeField] GameObject m_userAvatarLoader;

    [Header("Buttons")]
    [SerializeField] Button m_connect;


   
    private void Awake() {

        UpdatePlayerUI();

        var client = UM_GameService.SignInClient;
        client.OnPlayerUpdated.AddListener(UpdatePlayerUI);

        m_connect.onClick.AddListener(() => {
            if (client.PlayerInfo.State == UM_PlayerState.SignedIn) {
                client.SingOut(result => {
                    if(result.IsSucceeded) {
                        Debug.Log("Singed Out");
                    } else {
                        Debug.Log("Failed to sing out: " + result.Error.FullMessage);
                    }
                });
            } else {
                 client.SingIn(result => {
                     if (result.IsSucceeded) {
                         Debug.Log("Singed In");
                         
                     } else {
                         Debug.Log("Failed to sing in: " + result.Error.FullMessage);
                     }
                 });
            }
        });
    }


    private void UpdatePlayerUI() {
        var client = UM_GameService.SignInClient;
        if(client.PlayerInfo.State == UM_PlayerState.SignedIn) {
            var player = client.PlayerInfo.Player;

            m_userId.text = player.Id;
            m_userAlias.text = player.Alias;
            m_userName.text = player.DisplayName;


            m_userAvatarLoader.gameObject.SetActive(true);
            player.GetAvatar((texure) => {

                //We need to make sure here that player is still singed 
                //by the time we are getting image load callback
                if(client.PlayerInfo.State == UM_PlayerState.SignedIn) {

                    if(texure == null) {
                        //You mau want to assing some default texture here
                    }

                    //Assign image to RawImage object
                    m_userAvatar.texture = texure;
                }
                m_userAvatarLoader.gameObject.SetActive(false);
            });



            m_connect.GetComponentInChildren<Text>().text = "Sing Out";
        } else {
            m_connect.GetComponentInChildren<Text>().text = "Sing In";

            //Display User info
            m_userName.text = "Signed out";
            m_userAlias.text = string.Empty;
            m_userId.text = string.Empty;
            m_userAvatar.texture = null;
            m_userAvatarLoader.gameObject.SetActive(false);
        }
    }
}
