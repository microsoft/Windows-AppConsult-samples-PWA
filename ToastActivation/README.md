# Toast activation
This sample shows how a Progressive Web App on Windows can generate a native toast notification, using the adaptive template model introduced in Windows 10 and documented [here](https://docs.microsoft.com/en-us/windows/uwp/design/shell/tiles-and-notifications/adaptive-interactive-toasts).

The sample handles also the activation event of the web app from a toast noatification, so that you can redirect the user to the right section of your web application based on the context. In the sample app, the user cand send a toast notification related to a breaking news. When he clicks on the notification, he's redirected to a page where he can read the details of the news he has selected.

The sample is made by a:

- An ASP.NET Core 2.0 project, which can be published on a web server 
- A Progressive Web App project, which should be pointed to the URL where the web application has been published

This sample has been published as a companion of the following blog post [https://blogs.msdn.microsoft.com/appconsult/2018/05/18/activating-a-progressive-web-app-on-windows-10-using-a-toast-notification/](https://blogs.msdn.microsoft.com/appconsult/2018/05/18/activating-a-progressive-web-app-on-windows-10-using-a-toast-notification/)

