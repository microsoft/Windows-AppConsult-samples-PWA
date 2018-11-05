using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace WRCBackgroundTasks
{

    internal static class ToastHelper
    {
        public const string DEFAULT_GROUP = "test-group";
        public static ToastNotification PopToast(string title, string content, string buttonlabel, string tag = null, string group = DEFAULT_GROUP)
        {
            string xml = $@"<toast activationType='foreground' duration='long'>
                                            <visual>
                                                <binding template='ToastGeneric'>
                                                </binding>
                                            </visual>
                                            <actions>
                                            </actions>
                                             </toast>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            var binding = doc.SelectSingleNode("//binding");

            // add title
            var el = doc.CreateElement("text");
            el.InnerText = title;
            binding.AppendChild(el);

            // add content
            el = doc.CreateElement("text");
            el.InnerText = content;
            binding.AppendChild(el);

            // add button
            var actions = doc.SelectSingleNode("//actions");
            var action = doc.CreateElement("action");
            action.SetAttribute("activationType", "foreground");
            action.SetAttribute("content", buttonlabel);
            action.SetAttribute("arguments", "OK");
            actions.AppendChild(action);

            var s = doc.GetXml();

            return PopCustomToast(doc, tag, group);
        }

        private static ToastNotification PopCustomToast(XmlDocument doc, string tag, string group)
        {
            var toast = new ToastNotification(doc);

            if (tag != null)
                toast.Tag = tag;

            if (group != null)
                toast.Group = group;

            ToastNotificationManager.History.Clear();
            ToastNotificationManager.CreateToastNotifier().Show(toast);

            return toast;
        }
    }
}