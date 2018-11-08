using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Background;

namespace WRCBackgroundTasks
{
    public sealed class SessionConnectedTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
            ToastHelper.PopToast("Connected Session Task", "Session Connected task is running", "OK");
            deferral.Complete();

        }
    }
}
