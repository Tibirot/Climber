using UnityEngine;
using System.Collections.Generic;
using Combu;

/**
 * DISCLAIMER
 * This script include sample code to demonstrate how to implement the notification of new incoming messages
 * by polling the list of unread messages every XX seconds.
 * 
 * Feel free to use, edit or share yourself, though remember that this is only the code here can be probably optimized
 * but it was meant to give you a start for your own implementation.
 * 
 * To use this, you only need to attach it to your CombuManager prefab and then in your script that requires the notification add this code:
 * 
    IEnumerator Start()
    {
        while (!CombuManager.isInitialized || CheckMail.instance == null)
            yield return new WaitForEndOfFrame();
        
        CheckMail.RegisterNotification((int unread) => {
            Debug.Log("You have " + unread + " new messages");
        });
    }
 * 
 **/
public class CheckMail : MonoBehaviour
{
    /// <summary>
    /// Gets the singleton instance of this class (if instantiated).
    /// </summary>
    /// <value>The singleton instance.</value>
    public static CheckMail instance { get; private set; }

    /// <summary>
    /// Registers a listener callback to the notification of new messages.
    /// </summary>
    /// <param name="callback">Callback.</param>
    public static void RegisterNotification(System.Action<int> callback)
    {
        if (instance != null && callback != null)
        {
            instance.onNewMessages.Add(callback);
        }
    }

    /// <summary>
    /// Unregisters a listener callback from the notification of new messages.
    /// </summary>
    /// <param name="callback">Callback.</param>
    public static void UnregisterNotification(System.Action<int> callback)
    {
        if (instance != null && callback != null)
        {
            instance.onNewMessages.Remove(callback);
        }
    }

    /// <summary>
    /// The interval in seconds to check for unread messages.
    /// </summary>
    public float tick = 30f;

    /// <summary>
    /// Should print the debug log on events?
    /// </summary>
    public bool printLog = true;

    /// <summary>
    /// Next tick time.
    /// </summary>
    float nextTick = 0;

    /// <summary>
    /// Already checking for unread messages.
    /// </summary>
    bool checking = false;

    /// <summary>
    /// List of listener callbacks.
    /// </summary>
    List<System.Action<int>> onNewMessages = new List<System.Action<int>>();


    void OnEnable()
    {
        // Allow only one instance of this script running
        if (instance == null)
            instance = this;
        else
            enabled = false;
    }

    void FixedUpdate()
    {
        // Only check if it's not already checking and localUser is authenticated
        if (!checking && CombuManager.isInitialized && CombuManager.localUser.authenticated)
        {
            // Check whether it's time to check
            if (Time.time >= nextTick)
            {
                checking = true;
                if (printLog)
                {
                    Debug.Log("Checking new messages");
                }
                Mail.Load(eMailList.Unread, 1, 1, (Mail[] messages, int countMessages, int countPages, string error) =>
                {
                    if (countMessages > 0)
                    {
                        if (printLog)
                        {
                            Debug.Log((countMessages == 1 ? "There is " : "There are ") + countMessages + " unread messages");
                        }
                        foreach (var action in onNewMessages)
                        {
                            action(countMessages);
                        }
                    }
                    nextTick = Time.time + tick;
                    checking = false;
                });
            }
        }
    }
}
