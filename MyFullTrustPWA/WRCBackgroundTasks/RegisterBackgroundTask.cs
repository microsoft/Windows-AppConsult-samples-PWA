using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Popups;

namespace WRCBackgroundTasks
{

    public class BackgroundTask
    {
        private static string _myTaskName;
        private static IBackgroundTrigger _trigger;
        private BackgroundTask()
        {
        }

        public static async void Create(String task, IBackgroundTrigger trigger)
        {
            _myTaskName = task;
            _trigger = trigger;

            // check if task is already registered
            foreach (var cur in BackgroundTaskRegistration.AllTasks)
                if (cur.Value.Name == _myTaskName)
                {
#if DEBUG
                    await (new MessageDialog("Task already registered")).ShowAsync();
#endif
                    return;
                }

            await BackgroundExecutionManager.RequestAccessAsync();

            // register a new task
            BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder { Name = _myTaskName, TaskEntryPoint = _myTaskName };
            taskBuilder.SetTrigger(_trigger);
            BackgroundTaskRegistration myFirstTask = taskBuilder.Register();

#if DEBUG
            await (new MessageDialog("Task registered")).ShowAsync();
#endif
        }

        //public static async void Unregister(IBackgroundTask task)
        public static async void Unregister(String task)
        {
            var taskName = task;
            foreach (var cur in BackgroundTaskRegistration.AllTasks)
                if (cur.Value.Name == taskName)
                {
                    cur.Value.Unregister(true);
#if DEBUG
                    await (new MessageDialog("Task unregistered")).ShowAsync();
#endif
                    return;
                }
            }
        }
    }



