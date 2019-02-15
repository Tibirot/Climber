using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Button))]
public class AN_SwitchScene : MonoBehaviour {

    [SerializeField] Object asset;


    private void Start() {
        GetComponent<Button>().onClick.AddListener(() => {
            SceneManager.LoadScene(asset.name);
        });
    }

}
