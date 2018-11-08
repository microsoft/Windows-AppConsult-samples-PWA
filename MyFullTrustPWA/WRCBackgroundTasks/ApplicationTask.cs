using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Background;

namespace WRCBackgroundTasks
{
    public sealed class ApplicationTriggerTask: IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            ToastHelper.PopToast("Application trigger Task", "Applicaiton Trigger task is running", "OK");
            deferral.Complete();

        }
    }

}
