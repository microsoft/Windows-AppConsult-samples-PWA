//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Windows.Forms;
using PushSharp.Windows;
using System.Diagnostics;
using System.Xml.Linq;

namespace PushTestserver
{
    public partial class Form1 : Form
    {
        string secret = "CyazfQ4UXylFNIDBj1WqHEbXSpsForss"; // need to replace this with your own secret
        string sid = "ms-app://s-1-15-2-1739691208-2413059114-2912612588-87913382-4138524900-106472690-871635211";    // need to replace this with your own sid
        string package = "29949MatteoPagani.ExpenseIt-Test"; // need to replace this with your package family name;

        public Form1()
        {
            InitializeComponent();
        }

        private void OnSendPushNotification(object sender, EventArgs e)
        {
            SendNotification(txtUrl.Text, txtTitle.Text, txtText.Text, txtNewsId.Text);
        }

        public void SendNotification(string uri, string title, string text, string newsId)
        {
            var config = new WnsConfiguration(package, sid, secret);
            var wnsBroker = new WnsServiceBroker(config);

            wnsBroker.OnNotificationFailed += (notification, aggregateEx) => {

                aggregateEx.Handle(ex => {

                    // See what kind of exception it was to further diagnose
                    if (ex is WnsNotificationException)
                    {
                        var notificationException = (WnsNotificationException)ex;
                        Debug.WriteLine($"WNS Notification Failed: {notificationException.Message}");
                    }
                    else
                    {
                        Debug.WriteLine("WNS Notification Failed for some (Unknown Reason)");
                    }

                    // Mark it as handled
                    return true;
                });
            };

            wnsBroker.OnNotificationSucceeded += (notification) => {
                Debug.WriteLine("WNS Notification Sent!");
            };

            // Start the broker
            wnsBroker.Start();
            string payload = $"<toast launch=\"{newsId}\"><visual><binding template=\"ToastGeneric\"><text>{title}</text><text>{text}</text></binding></visual></toast>";

            string tile = $@"<tile>
                               <visual>
                                    <binding template='TileMedium'>
                                        <text hint-style='subtitle'>{title}</text>
                                        <text hint-style='captionSubtle'>{text}</text>
                                    </binding>
                                    <binding template='TileWide'>
                                        <text hint-style='subtitle'>{title}</text>
                                        <text hint-style='captionSubtle'>{text}</text>
                                    </binding>
                                    <binding template='TileLarge'>
                                        <text hint-style='subtitle'>{title}</text>
                                        <text hint-style='captionSubtle'>{text}</text>
                                    </binding>
                             </visual>
                       </tile>";

            WnsToastNotification toastNotification = new WnsToastNotification
            {
                ChannelUri = uri,
                Payload = XElement.Parse(payload)
            };

            WnsTileNotification tileNotification = new WnsTileNotification
            {
                ChannelUri = uri,
                Payload = XElement.Parse(tile)
            };

            wnsBroker.QueueNotification(toastNotification);
            wnsBroker.QueueNotification(tileNotification);

            wnsBroker.Stop();
        }
    }
}
