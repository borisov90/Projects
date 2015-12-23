
var xPos, yPos, xPosW, yPosW;
var prm = Sys.WebForms.PageRequestManager.getInstance();
prm.add_beginRequest(BeginRequestHandler);
prm.add_endRequest(EndRequestHandler);

function BeginRequestHandler(sender, args) {
//    xPosW = document.body.scrollLeft;
    //    yPosW = document.body.scrollTop;    //ContentPlaceHolder_StudentMainData_TabContainer_tabMainData
    xPosW = $(document).scrollLeft();
    yPosW = $(document).scrollTop();
    if ($('#UpdatePanel') && $('#UpdatePanel')) {
        xPos = $('#UpdatePanel').scrollLeft;
        yPos = $('#UpdatePanel').scrollTop;
    }
    
}
function EndRequestHandler(sender, args) {
//    document.body.scrollLeft = xPosW;
    //    document.body.scrollTop = yPosW;
    $(document).scrollLeft(xPosW);
    $(document).scrollTop(yPosW);
    if ($('#UpdatePanel') && $('#UpdatePanel')) {

        $('#UpdatePanel').scrollLeft = xPos;
        $('#UpdatePanel').scrollTop = yPos; //UpdatePanel  //PersonDataDIV
    }
   
}