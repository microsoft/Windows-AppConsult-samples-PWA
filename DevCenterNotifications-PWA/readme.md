﻿With the combination of the [Engagement feature of Partner Center](https://docs.microsoft.com/en-us/windows/uwp/publish/send-push-notifications-to-your-apps-customers) and the [Microsoft Store Services SDK](https://docs.microsoft.com/en-us/windows/uwp/mosnetize/microsoft-store-services-sdk) you can easily add Push Notifications to your [Windows Progressive Web App](https://docs.microsoft.com/en-us/microsoft-edge/progressive-web-apps) (PWA), [UWP](https://docs.microsoft.com/en-us/windows/uwp/get-started/universal-application-platform-guide), or [Desktop Bridge - Win32 app](https://aka.ms/desktopbridge).

## Easy Push with Microsoft Store Services Notifications

In contrast to 'WNS', 'Microsoft Store Services Notifications' sends general toast and tile updates to user groups you define. This can be:

* All of the users of all of your apps and games.
* All of the users of a specific app.
* All of the users of some of your apps.
* Subsets of the above, [grouped by demographic criteria](https://docs.microsoft.com/en-us/windows/uwp/publish/create-customer-groups) you specify. **The marketers in your organization will love this feature.** For example, you may want to reward your big spending customers with a [promotional code](https://docs.microsoft.com/en-us/windows/uwp/publish/generate-promotional-codes) that can be used for a free license for a new app you are releasing. You may want to up-sell your low spending customers on in app purchases.

 Note that with Microsoft Store Services notifications you don't send specific data to specific users. This is very useful for:

* Cross promoting your apps. For example, if you have launched a new app, ask users of your other apps to check it out.
* Letting your users know when your apps or IAP go on [sale](https://docs.microsoft.com/en-us/windows/uwp/publish/set-and-schedule-app-pricing).
* Requesting feedback.
* Sending ad hoc announcements and information to your users. For example, providing down time schedules, new feature announcements, or service alerts.

Using this sample:

1) Install the [Microsoft Store Services SDK](https://docs.microsoft.com/en-us/windows/uwp/mosnetize/microsoft-store-services-sdk)

2) Associate the sample with a Partner Center app in Visual Studio. Go to the Project menu -> Store -> Associate App with Store

3) Publish the app to Partner Center using the ['Limited Audience' or 
'Hidden' visibility options](https://docs.microsoft.com/en-us/windows/uwp/publish/choose-visibility-options). You need to do this because in order to setup the push notifications Partner Center will need to explicitly reference your app. In order to do this, your app must be published with a reference to the `Microsoft.Services.Store.SDK`. Note that the publishing process can take up to three business days.

4) Once your app has published, you don't need to install it from the Store. You can continue to develop the app - just make sure the [identity](https://docs.microsoft.com/en-us/windows/uwp/publish/view-app-identity-details) of the app matches what you have submitted to Partner Center.

5) In the `Engagement` section of `Partner Center` define and send your Toast and Tile Push Notifications.

   * In `Partner Center`, select `Engage` and you should see the `Notifications` page as shown below.

   * Click `New Notification`

   * Click `Blank Toast`, `OK`

   * From the drop down, select the app or apps to send notifications. 

   * Here are some sample values for this toast notification:

   **Name:** My First Engagement Push

   **Customer Groups:** All Users
  
   **Send Notification Immediately:** Check
  
   **Notification Never Expires:** Check
  
   **Language:** English (default)
  
   **Activation Type:** Foreground
  
   **Duration:** Short
  
   **Scenario:** Default
  
   **Visual (text 1):** Hello from My App!

   Click `Send`

Your notification will be sent immediately it will take up to one minute to receive the notification. Note that the app's registration code should have been run at least once. Note that the app doesn't need to be running in order to receive the notification.

This sample implements the Notifications registration code in a Windows Runtime Component (**PushWinRTComponent**). 

PushWinRTComponent exposes the method `PushNotifications.init()` that will be called via JavaScript as follows:

```javascript
// index.html
// Set up Push notifications
if (typeof Windows !== 'undefined' &&
  typeof Windows.UI !== 'undefined' &&
  typeof Windows.UI.Notifications !== 'undefined') {
  PushWinRTComponent.PushNotifications.init().then(
    function (result) {
      console.log("push init result: " + result);
    });
}
```
Happy Coding!

References:

[Add Push Notifications the easy way with Partner Center + Microsoft Store Services SDK](http://blogs.msdn.microsoft.com/appconsult/2018/10/31/add-push-notifications-the-easy-way-with-partner-center-microsoft-store-services-sdk)

[Send notifications to your app's customers](https://docs.microsoft.com/en-us/windows/uwp/publish/send-push-notifications-to-your-apps-customers)

[Push Notifications in a PWA Running on Windows 10](https://blogs.msdn.microsoft.com/appconsult/2018/06/07/push-notifications-in-a-pwa-running-on-windows-10/)
