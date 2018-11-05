using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Background;

namespace WRCBackgroundTasks
{
    public sealed class TimerTriggerTask: IBackgroundTask
    {
        BackgroundTaskCancellationReason _cancelReason = BackgroundTaskCancellationReason.Abort;
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);

            ToastHelper.PopToast("Timer trigger Task", "Timer Trigger task is running", "OK");
            deferral.Complete();

        }
        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            // 
            // Indicate that the background task is canceled. 
            // 
            _cancelReason = reason;

            Debug.WriteLine("Background " + sender.Task.Name + " Cancel Requested...");
        }
    }
}
