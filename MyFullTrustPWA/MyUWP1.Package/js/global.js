const getTask = (taskName) => {
  var iter = Windows.ApplicationModel.Background.BackgroundTaskRegistration.allTasks.first();
  var hascur = iter.hasCurrent;
  while (hascur) {
    var cur = iter.current.value;
    if (cur.name === taskName) {
      return cur;
    }
    hascur = iter.moveNext();
  }

  return null;
};

const registerTask = (task, trigger) => {
    WRCBackgroundTasks.BackgroundTask.create(task, trigger);
};


const unregisterTask = (taskName) => {
    WRCBackgroundTasks.BackgroundTask.unregister(taskName);
};

window.BgServiceManager = {
    registerTask: registerTask,
    unregisterTask: unregisterTask,
    getTask: getTask
};

