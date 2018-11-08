var applicationTask = new Windows.ApplicationModel.Background.ApplicationTrigger();
BgServiceManager.registerTask("WRCBackgroundTasks.ApplicationTriggerTask", applicationTask);
BgServiceManager.registerTask("WRCBackgroundTasks.TimerTriggerTask", new Windows.ApplicationModel.Background.TimeTrigger(15, false));
var sessionConnectedTrigger = new Windows.ApplicationModel.Background.SystemTrigger(Windows.ApplicationModel.Background.SystemTriggerType.sessionConnected, false);
BgServiceManager.registerTask("WRCBackgroundTasks.SessionConnectedTask", sessionConnectedTrigger);

window.onload = function () {
    document.getElementById("button").onclick = function (evt) {
        applicationTask.requestAsync().then((res) => { });
    };

    document.getElementById("cancel").onclick = function (evt) {
        BgServiceManager.unregisterTask("WRCBackgroundTasks.SessionConnectedTask");
    };
};

