using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Background;

namespace WRCBackgroundTasks
{
    public sealed class PreInstallTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            ToastHelper.PopToast("Pre-Install Task", "Pre install task is running", "OK");

            await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();

            deferral.Complete();

        }
    }
}
