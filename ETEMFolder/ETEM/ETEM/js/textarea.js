function doBeforePaste(control) {
    maxLength = control.attributes["maxLength"].value;
    if (maxLength) {
        event.returnValue = false;
    }
}
function doPaste(control) {
    maxLength = control.attributes["maxLength"].value;
    value = control.value;
    if (maxLength) {
        event.returnValue = false;
        maxLength = parseInt(maxLength);
        var oTR = control.document.selection.createRange();
        var iInsertLength = maxLength - value.length + oTR.text.length;
        var sData = window.clipboardData.getData("Text").substr(0, iInsertLength);
        oTR.text = sData;
    }
}
function LimitInput(control) {
    if (control.value.length > control.attributes["maxLength"].value) {
        control.value = control.value.substring(0, control.attributes["maxLength"].value);
    }
};