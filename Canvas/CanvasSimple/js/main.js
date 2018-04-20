// Your code here!

var canvasContext;
try {
    var Inking = Windows.UI.Input.Inking;
    var drawingAttributes = new Inking.InkDrawingAttributes();
    var InkInputProcessingMode = Inking.InkInputProcessingMode;

    

}
catch (e) { }

function setRed() {
    if (!runningWindows10withInkCanvasSupport()) return;
    drawingAttributes.color = Windows.UI.Colors.red;
    canvasContext.msInkPresenter.updateDefaultDrawingAttributes(drawingAttributes);
}

function setBlue() {
    if (!runningWindows10withInkCanvasSupport()) return;
    drawingAttributes.color = Windows.UI.Colors.blue;
    canvasContext.msInkPresenter.updateDefaultDrawingAttributes(drawingAttributes);
}

function setGreen() {
    if (!runningWindows10withInkCanvasSupport()) return;
    drawingAttributes.color = Windows.UI.Colors.green;
    canvasContext.msInkPresenter.updateDefaultDrawingAttributes(drawingAttributes);
}

function setOrange() {
    if (!runningWindows10withInkCanvasSupport()) return;
    drawingAttributes.color = Windows.UI.Colors.orange;
    canvasContext.msInkPresenter.updateDefaultDrawingAttributes(drawingAttributes);
}

function setThickness(value) {
    var strokeSize = parseInt(value) * 3;
    drawingAttributes.size = { height: strokeSize, width: strokeSize };
    canvasContext.msInkPresenter.updateDefaultDrawingAttributes(drawingAttributes);
}

function initializeInkOnMsInkCanvas() {
    if (!runningWindows10withInkCanvasSupport()) {
        document.getElementById("msInkCanvas").style.visibility = "hidden";
        return;
    }

    var msInkCanvas = document.getElementById("msInkCanvas");
    canvasContext = msInkCanvas.getContext('ms-ink');

    var redButton = document.getElementById("red");
    redButton.addEventListener("click", setRed, false);

    var greenButton = document.getElementById("green");
    greenButton.addEventListener("click", setGreen, false);

    var blueButton = document.getElementById("blue");
    blueButton.addEventListener("click", setBlue, false);

    var orangeButton = document.getElementById("orange");
    orangeButton.addEventListener("click", setOrange, false);

    if (!runningWindows10withInkCanvasSupport()) return;
        // Set initial ink stroke attributes.
        drawingAttributes.color = Windows.UI.Colors.black;
        drawingAttributes.ignorePressure = false;
        drawingAttributes.fitToCurve = true;
        canvasContext.msInkPresenter.updateDefaultDrawingAttributes(drawingAttributes);
        canvasContext.msInkPresenter.inputDeviceTypes =
            Windows.UI.Core.CoreInputDeviceTypes.mouse |
            Windows.UI.Core.CoreInputDeviceTypes.pen |
            Windows.UI.Core.CoreInputDeviceTypes.touch;
}

function log(l) {
    document.getElementById("log").textContent += "\n" + l;
}

function runningWindows10withInkCanvasSupport() {
    if (typeof Windows !== 'undefined' &&
        typeof Windows.UI !== 'undefined' &&
        typeof Windows.UI.Input.Inking.InkPresenter !== 'undefined')
        return true;
    else
        return false;
}

document.addEventListener("DOMContentLoaded", function () {
    initializeInkOnMsInkCanvas();

    // Temporary bug in ms-ink requires us to force paint the canvas when the window becomes invisible
    document.addEventListener("visibilitychange", function () {
        if (document.visibilityState == "visible") {
            requestAnimationFrame(function () {
                inkCanvas = document.getElementById("msInkCanvas");
                inkCanvas.width = inkCanvas.width + 1;
                inkCanvas.width = inkCanvas.width - 1;
            });
        }
    });
});