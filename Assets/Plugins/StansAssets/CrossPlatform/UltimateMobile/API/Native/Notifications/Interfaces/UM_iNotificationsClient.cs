using System;
using System.Collections.Generic;
using UnityEngine;

using SA.Foundation.Events;
using SA.Foundation.Templates;

namespace SA.CrossPlatform.Notifications
{
    public interface UM_iNotificationsClient 
    {


        /// <summary>
        /// Unschedules all pending notification requests.
        /// This method executes asynchronously, removing all pending notification requests on a secondary thread.
        /// </summary>
        void RequestAuthorization(Action<SA_Result> callback);


        /// <summary>
        /// Schedules a local notification for delivery.
        /// 
        /// This method schedules local notifications only; 
        /// You cannot use it to schedule the delivery of push notifications. 
        /// The notification is delivered when the trigger condition in the request parameter is met. 
        /// If the request does not contain a <see cref="UM_iNotificationTrigger"/> object, the notification is delivered right away.
        /// 
        /// You may call this method from any thread of your app.
        /// </summary>
        /// <param name="request">The notification request to schedule.This parameter must not be <c>null</c></param>
        /// <param name="callback">The block to execute with the results.</param>
        void AddNotificationRequest(UM_NotificationRequest request, Action<SA_Result> callback);

        /// <summary>
        /// Unschedules all pending notification requests.
        /// This method executes asynchronously, removing all pending notification requests on a secondary thread.
        /// </summary>
        void RemoveAllPendingNotifications();


        /// <summary>
        /// Unschedules the specified notification requests.
        /// This method executes asynchronously, removing the pending notification requests on a secondary thread.
        /// </summary>
        /// <param name="identifier">
        /// Thee Identifier of notification requests you want to remove.
        /// If the identifier belongs to a non repeating request whose notification has already been delivered, 
        /// this method ignores the identifier.
        /// </param>
        void RemovePendingNotification(int identifier);

        /// <summary>
        /// Removes all of the app’s notifications from Notification Center.
        /// 
        /// Use this method to remove all of your app’s notifications from Notification Center. 
        /// The method executes asynchronously, returning immediately 
        /// and removing the identifiers on a background thread.
        /// </summary>
        void RemoveAllDeliveredNotifications();

        /// <summary>
        /// Removes the specified notifications from Notification Center.
        /// 
        /// Use this method to selectively remove notifications that you no longer want displayed in Notification Center. 
        /// The method executes asynchronously, returning immediately and removing the identifiers on a background thread.
        /// </summary>
        /// <param name="identifier">
        /// The identifier associated with a notification request object. 
        /// This method ignores the <c>identifiers</c> of request objects whose notifications 
        /// are not currently displayed in Notification Center.
        /// </param>
        void RemoveDeliveredNotification(int identifier);


        /// <summary>
        /// Contains last received <see cref="UM_NotificationRequest"/> object by delegate.
        /// 
        /// You must subscribe to this class events as soon as possible. 
        /// However, delegate may already receive action while app was luanching. For example if user has launched the app
        /// by clicking on notifications. You may check <see cref="LastOpenedNotificationt"/> to find out of app was launched
        /// using the notification. If Property is null after your app is launched it means that application was laucnhed
        /// wihout interaction with the notification object.
        /// </summary>
      
        UM_NotificationRequest LastOpenedNotificationt { get; }


        /// <summary>
        /// Called when user is clicked on notification while your application was in background.
        /// The notification will be delivered as soon as app enters foreground.
        /// </summary>
        SA_iEvent<UM_NotificationRequest> OnNotificationClick { get; }


        /// <summary>
        /// Called when a notification is delivered to a foreground app.
        /// 
        /// If your app is in the foreground when a notification arrives, 
        /// the notification center calls this method to deliver the notification directly to your app. 
        /// If you implement this method, you can take whatever actions are necessary to process the notification 
        /// and update your app. 
        /// User will not be alreated by a system;
        /// </summary>
        SA_iEvent<UM_NotificationRequest> OnNotificationReceived { get; }

    }
}