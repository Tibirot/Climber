using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Combu;

/*
 * This script include sample code to demonstrate how to implement a simple login workflow
 * by using Unity UI components.
 * 
 * The examples (both scenes and scripts) are provided as-are and can be eventually modified over time to reflect major changes to API or just to be improved.
 * So we discourage the approach of editing any demo file, but you could create your own scripts and inherit their classes from these examples if you want,
 * and eventually override the methods that you want to change or add more input fields and stuff to fit your needs.
 */
public class CombuDemoScene : CombuDemoPanel
{
    public Text textCombuVersion;

    public InputField loginUsername;
    public InputField loginPassword;
    public Text loginError;

    public Animator panelMenu;

    IEnumerator Start()
    {

        while (CombuManager.instance == null)
        {
            yield return null;
        }
        if (textCombuVersion != null)
        {
            textCombuVersion.text = "Client Version " + CombuManager.COMBU_VERSION;
        }

        var panel = Current;
        if (panel != null)
        {
            List<MonoBehaviour> disableUI = new List<MonoBehaviour>();
            var texts = panel.GetComponentsInChildren<Text>();
            disableUI.AddRange(texts);
            disableUI.AddRange(panel.GetComponentsInChildren<InputField>());
            disableUI.AddRange(panel.GetComponentsInChildren<Button>());
            foreach (var i in disableUI)
            {
                i.gameObject.SetActive(false);
            }
            string oldTitle = "";
            if (texts.Length > 0)
            {
                oldTitle = texts[0].text;
                texts[0].text = "Connecting to server...";
                texts[0].gameObject.SetActive(true);
            }

            while (!CombuManager.isInitialized)
            {
                yield return null;
            }

            if (textCombuVersion != null)
            {
                textCombuVersion.text += " - Require update: " + (CombuManager.instance.serverInfo.requireUpdate ? "YES" : "NO");
            }

            foreach (var i in disableUI)
            {
                i.gameObject.SetActive(true);
            }

            if (texts.Length > 0)
                texts[0].text = oldTitle;
        }
        OnStart();
    }

    protected virtual void OnStart()
    {
    }

    public virtual void UserLogin()
    {
        if (string.IsNullOrEmpty(loginUsername.text) || string.IsNullOrEmpty(loginPassword.text))
        {
            loginError.text = "Enter your credentials";
            return;
        }
        Debug.Log("Trying to login with username " + loginUsername.text + " and password " + loginPassword.text);
        loginError.text = "Loading...";
        // We can specify our custom user type to Authenticate (to be able to cast CombuManager.localUser later)
        // or use the other override to use the basic User type.
        CombuManager.platform.Authenticate<CombuDemoUser>(loginUsername.text, loginPassword.text, OnUserLogin);
    }

    public virtual void UserLogout()
    {
        CombuManager.platform.Logout(null);
    }

    protected virtual void OnUserLogin(bool success, string error)
    {
        loginError.text = "";
        if (success)
        {
            loginUsername.text = "";
            loginPassword.text = "";
            OpenPanel(panelMenu);
        }
        else
        {
            loginError.text = "Login failed: " + error;
        }
    }
}
