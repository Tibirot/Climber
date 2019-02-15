using UnityEngine;
using SA.CrossPlatform.Notifications;
using UnityEngine.UI;
using SA.CrossPlatform.UI;

public class UM_NotificationsExample : MonoBehaviour {



    [SerializeField] Button m_schegule;
    [SerializeField] Button m_removeDelivered;
    [SerializeField] Button m_removePending;


    private void Start() {

        var client = UM_NotificationCenter.Client;
        client.OnNotificationReceived.AddListener((UM_NotificationRequest request) => {
            //Notification was received while app is runing
            ShowNotificationInfo("On Notification Received", request);
        });


        UM_NotificationRequest startRequest = client.LastOpenedNotificationt;
        if (startRequest != null) {
            //if this isn't null on your app launch, means user laucnhed your app by clicking on notification icon
            ShowNotificationInfo("Launch Notification", startRequest);
        }
       

        m_schegule.onClick.AddListener(() => {
            Schegule();
        });

        m_removePending.onClick.AddListener(() => {
            RemovePendingNotification();
        });

        m_removeDelivered.onClick.AddListener(() => {
            RemoveDeliveredNotification();
        });

    }

    private void RemoveDeliveredNotification() {
        var client = UM_NotificationCenter.Client;

        //Will remove all the notification from the device tray
        client.RemoveAllDeliveredNotifications();

        //Will remove notification with id 100 from the device tray
        client.RemoveDeliveredNotification(100);
    }

    private void RemovePendingNotification() {
        var client = UM_NotificationCenter.Client;


        //Will cancel all the pending notifications
        client.RemoveAllPendingNotifications();

        //Will cancel pending notification with id 100
        client.RemovePendingNotification(100);
    }



    private void Schegule() {
        var client = UM_NotificationCenter.Client;

        UM_NotificationRequest startRequest = client.LastOpenedNotificationt;
        if (startRequest != null) {
            //if this isn't null on your app lauch, means user laucnhed your app by clicking on notification icon
        }

        client.OnNotificationClick.AddListener((UM_NotificationRequest request) => {
            //User clicked on notification while app is runing
        });

        client.OnNotificationReceived.AddListener((UM_NotificationRequest request) => {
            //Notification was received while app is runing
        });

        //2 seconds
        var trigger = new UM_TimeIntervalNotificationTrigger(2);
        var content = new UM_Notification();

        content.SetTitle("Title");
        content.SetBody("Body message");
        //content.SetSmallIconName("myIcon.png");
        //content.SetSoundName("mySound.wav");

        int requestId = 1; //Get uniqie request id, if notification will be posted with the same request id old one will be replaced with the new one
        var requets = new UM_NotificationRequest(requestId, content, trigger);

        client.AddNotificationRequest(requets, (result) => {
            if (result.IsSucceeded) {
                //Notification was successfully scheduled
            } else {
                //There was an isseu with scheduling
                Debug.Log(result.Error.FullMessage);
            }
        });
    }


    private void ShowNotificationInfo(string title, UM_NotificationRequest request) {
        string message = request.Content.Title + " / " + request.Content.Body;
        var builder = new UM_NativeDialogBuilder(title, message);
        builder.SetPositiveButton("Okay", () => {
            
        });

        var dialog = builder.Build();
        dialog.Show();
    }
}
