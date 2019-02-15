using SA.CrossPlatform.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UM_WelcomeController : MonoBehaviour
{
    [SerializeField] private GameObject m_ButtonsPanel;
    [SerializeField] private GameObject m_FeatureViewport;

    private Scene m_currentlyFeaturedScene;
    
    private void Start()
    {
        
        /*
       SA.Facebook.SA_FB.Init(() =>
       {
            UM_DialogsUtility.ShowMessage("Facebook", "Facebook IsInitialized: " + SA.Facebook.SA_FB.IsInitialized);    
       });*/
       
       
       SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
       var buttons =  m_ButtonsPanel.GetComponentsInChildren<Button>();
       foreach (var button in buttons)
       {
           var currentButton = button;
           button.onClick.AddListener(() =>
           {
               var sceneLink = currentButton.GetComponent<UM_SceneLink>();
               if (sceneLink != null)
               {
                   LoadScene(sceneLink.SceneName);
               }
           });
       }
    }

    private void SceneManagerOnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        m_currentlyFeaturedScene = scene;
        foreach (var rootGameObject in scene.GetRootGameObjects())
        {
            if (rootGameObject.GetComponent<Canvas>() == null)
            {
                Destroy(rootGameObject);
            }
            else
            {
                var canvasRect = rootGameObject.GetComponent<RectTransform>();
                rootGameObject.transform.SetParent(m_FeatureViewport.transform);
                canvasRect.anchorMin = new Vector2(0, 0);
                canvasRect.anchorMax = new Vector2(1, 1);
                
                canvasRect.transform.localScale = Vector3.one;
                canvasRect.anchoredPosition  = Vector2.zero;

                canvasRect.offsetMin = Vector2.zero;
                canvasRect.offsetMax = Vector2.zero;

            }
        }
    }

    private void LoadScene(string sceneName)
    {
        if (sceneName.Equals(m_currentlyFeaturedScene.name))
        {
            return;
        }

        m_FeatureViewport.Clear();
        if (m_currentlyFeaturedScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(m_currentlyFeaturedScene);
        }
       
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }
    
    
}



