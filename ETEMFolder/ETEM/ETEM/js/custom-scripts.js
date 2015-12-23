/// <reference path="../Scripts/jquery-1.10.2.min.js" />




$(document).ready(function () {



    styleAllGrids();
    stopDefaultAutocomplete();



    //make fields fixed max len

    $("#UpdatePanel").on("keyup", ".maxLengthable480", function (sender) {
        console.log(sender);
        var text = sender.target.value;
        if (text.length > 480) {
            sender.target.value = sender.target.value.substring(0, 480);
        }
    });

    $("#UpdatePanel").on("keyup", ".maxLengthable1000", function (sender) {
        console.log(sender);
        var text = sender.target.value;
        if (text.length > 1000) {
            sender.target.value = sender.target.value.substring(0, 1000);
        }
    });

    $("#UpdatePanel").on("keyup", ".maxLengthable1500", function (sender) {
        console.log(sender);
        var text = sender.target.value;
        if (text.length > 1500) {
            sender.target.value = sender.target.value.substring(0, 1500);
        }
    });

    $("#UpdatePanel").on("keyup", ".maxLengthable2000", function (sender) {
        console.log(sender);
        var text = sender.target.value;
        if (text.length > 2000) {
            sender.target.value = sender.target.value.substring(0, 2000);
        }
    });

    $("#UpdatePanel").on("keyup", ".maxLengthable3000", function (sender) {
        console.log(sender);
        var text = sender.target.value;
        if (text.length > 3000) {
            sender.target.value = sender.target.value.substring(0, 3000);
        }
    });

    $("#UpdatePanel").on("keyup", ".maxLengthable4000", function (sender) {
        console.log(sender);
        var text = sender.target.value;
        if (text.length > 4000) {
            sender.target.value = sender.target.value.substring(0, 4000);
        }
    });



    //    $("#UpdatePanel").on("click", ".btn-comfirmable", function (sender) {
    //        var key = "MessegeFor";

    //      
    //        var btn = sender.currentTarget;
    //        var clickedButtonMessage = $(btn).attr(key);
    //        var lastClickedMessageFor = localStorage.getItem(key);
    //        


    //    });


    $("#UpdatePanel").on("click", ".comfirmable", function (sender) {

        var key = "MessegeFor";
        var allMessagesDictionary = new Dictionary();
        allMessagesDictionary.setData("StudentsStatuses", "return confirm('След запис системата ще промени текущите данни(Kурс/Година на обучение, Статус, Учебен план, Академичен период) на студента/докторанта. Сигурни ли сте ?');");
        allMessagesDictionary.setData("PhdStatuses", "return confirm('След запис системата ще промени текущите данни(Kурс/Година на обучение, Статус, Учебен план, Академичен период) на студента/докторанта. Сигурни ли сте ?');");
        allMessagesDictionary.setData("ExamProtocolPrint", "return confirm('Изпитните протоколи ще се генерират в бекграунд режим.След като са готови ще получите известие в системата.Сигурни ли сте ?');");
        allMessagesDictionary.setData("CourseListPrint", "return confirm('Курсовите списъци ще се генерират в бекграунд режим.След като са готови ще получите известие в системата.Сигурни ли сте ?');");


        console.log(sender);
        console.log(sender.currentTarget);

        var isChecked = ($(sender.target).is(":checked"));
        var messegeFor = $(sender.currentTarget).attr(key);

        var allBtns = $(".btn-comfirmable");
        var btn;

        for (var i = 0; i < allBtns.length; i++) {
            if ($(allBtns[i]).attr(key)) {
                btn = $(allBtns[i]);
            }
        }

        if (isChecked) {

            //localStorage.getItem("modalWindow");
            //localStorage.setItem(key, messegeFor);
            // if (lastClickedMessageFor) {
            switch (messegeFor) {
                case "StudentsStatuses":
                    $(btn).attr("onclick", allMessagesDictionary.getData("StudentsStatuses")); break;
                case "PhdStatuses":
                    $(btn).attr("onclick", allMessagesDictionary.getData("PhdStatuses")); break;
                case "ExamProtocolPrint":
                    $(btn).attr("onclick", allMessagesDictionary.getData("ExamProtocolPrint")); break;
                case "CourseListPrint":
                    $(btn).attr("onclick", allMessagesDictionary.getData("CourseListPrint")); break;

                default:

            }
            //            } else {
            //                $(btn).removeAttr("onclick");
            //            }
            //get the right message depending on the attibute MessegeFor of the button



        } else {
            //localStorage.removeItem(key);
            $(btnSave).removeAttr("onclick"); //attr("onclick", "return confirm('Сигурни ли сте, че искате да създадете автоматично ВСИЧКИ изпитни протоколи?');");
        }






        // var   $("div.icon-maximize").children().first().removeClass("color-white");


    });

    function Dictionary() {
        var dictionary = {};

        this.setData = function (key, val) { dictionary[key] = val; }
        this.getData = function (key) { return dictionary[key]; }
    }


    $("#UpdatePanel").on("keyup", ".maxLengthable1000", function (sender) {
        console.log(sender);
        var text = sender.target.value;
        if (text.length > 1000) {
            sender.target.value = sender.target.value.substring(0, 1000);
        }
    });

    $('#UpdatePanel').on("click", ".depenableCheckBox", function () {
        console.log(this);
        if ($(this).parent().next("p").children("span").hasClass("depenableCheckBox")) {
            $(this).parent().next("p").children("span").children("input").prop('checked', false);
        }
        else {
            $(this).parent().prev("p").children("span").children("input").prop('checked', false);
        }
    });


    //    $('#UpdatePanel').on("ready", "div#ContentPlaceHolder_WeekSchedule_DayHours", function () {
    //        $('.marked-class').popover();
    //    });


    $('#UpdatePanel').on("mouseover", ".scheduleHolder", function () {
        $('.marked-class').popover({ html: true });
    });

    $('#UpdatePanel').on("ready", "div.resizeableModal", function () {
        console.log(this);
    });

    //    var isModalOpen = localStorage.getItem("modalWindow");
    //    if (isModalOpen==true) {
    //        $('<div id="modalWindow" class="blind"></div>').insertAfter(".global-container");
    //        localStorage.setItem("modalWindow", false);
    //    }


    $('#UpdatePanel').on("click", ".modalWindow", function () {
        var button = $(this);
        //localStorage.setItem("modalWindow", true);
        $('<div id="modalWindow" class="blind"></div>').insertAfter(".global-container");
        console.log(this + "works add");
    });

    $('#UpdatePanel').on("click", ".closeModalWindow", function () {
        var button = $(this);
        $("#modalWindow").remove();

        console.log(this + "works remove");
    });

    //for the bootstrap modal window which is out of #UpdatePanel
    $('.global-container').on("click", ".closeModalWindow", function () {
        var button = $(this);
        $("#modalWindow").remove();

        console.log(this + "works remove");
    });



    $('#UpdatePanel').on('mouseover', "div.pnl-draggable", function () {
        $("pnl-draggable").draggable();
    });



    var lastClickedBtnName = localStorage.getItem('activeNode');
    if (lastClickedBtnName) {
        var menuNodes = $("#cbp-hrmenu ul#MainNavUl > li > a");
        for (var i = 0; i < menuNodes.length; i++) {
            if (menuNodes[i].innerText == lastClickedBtnName) {
                menuNodes[i].className += " activeMenuNode";
            }
        }
    }



    //makes row in grid selected
    $("#UpdatePanel").on("click", ".rowColor", function () {
        if ($(this).children().first().is(':checked')) {
            $(this).parents("tr").first().addClass("selected-row-red");
        }
        else {
            $(this).parents("tr").first().removeClass("selected-row-red");
        }



    });

    $("#UpdatePanel").on("click", "div.icon-close", function () {
        $(this).parents(".resizeableModal").first().hide();

    });

    $("#UpdatePanel").on("click", "div.icon-maximize", function () {
        //        $(this).addClass("active-maximized-icon");

        $("div.icon-minimize").children().first().removeClass("color-white");
        $(this).children().first().addClass("color-white");
        var parentToMaximaze = $(this).parents(".resizeableModal").first();
        if (!parentToMaximaze.hasClass("fixedMaximazedPnl")) {
            var windowWidth = $(window).width();
            var windowHeight = $(window).height();

            localStorage.setItem("lastModalClass", parentToMaximaze.attr("class"));
            parentToMaximaze.addClass("fixedMaximazedPnl");
        }

    });


    $("#UpdatePanel").on("click", "div.icon-minimize", function () {

        $("div.icon-maximize").children().first().removeClass("color-white");
        $(this).children().first().addClass("color-white");
        var parentToMinimize = $(this).parents(".resizeableModal").first();
        parentToMinimize.removeClass("fixedMaximazedPnl");
        //        var lastModalClasses = localStorage.getItem("lastModalClass");

    });





    $("#UpdatePanel").on("click", "input.disableable", function () {
        var listBtns = $(this);
        for (var i = 0; i < listBtns.length; i++) {

            var text = listBtns[i].value;
            var cssClasses = $(listBtns[i]).attr("class");
            var btn = $(listBtns[i]);

            //this is needed if there is comfirm button of the disableable button, then i don't need to disable it
            var cbxConfirm = $(".comfirmable input");
            if (cbxConfirm) {
                cbxConfirm = cbxConfirm.first();
            }


            //if this check box exists and it's checked i don't need to disable the button
            if (cbxConfirm[0] && cbxConfirm[0].checked) {
            //do nothing
               // $(listBtns[i]).hide();
               // $('<input type="submit" disabled="disabled"  class="' + cssClasses + '" value="' + text + '" >').insertAfter(btn);
            }
            else {
                $(listBtns[i]).hide();
                $('<input type="submit" disabled="disabled"  class="' + cssClasses + '" value="' + text + '" >').insertAfter(btn);
            }

        }

    });

    //set active menu item
    $(".cbp-hrsub-inner").on('click', 'a', function () {//.cbp-hrsub-inner 

        var mainNodes = $(".cbp-hropen > a");
        for (var i = 0; i < mainNodes.length; i++) {
            $(mainNodes[i]).removeClass("activeMenuNode");
        }

        localStorage.setItem("activeNode", this.parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.firstChild.innerHTML);

    });


    $(".mainNodeItem").on("click", function () {

        $("#lbNodeName").hide();
    });

    $("#logout-btn").on("click", function () {
        localStorage.removeItem("activeNode");
    });



});

//autocomplete = "off"

function stopDefaultAutocomplete() {

    var allForms = $("form");
    for (var i = 0; i < allForms.length; i++) {
        allForms[i].autocomplete = "off";
    }
    
    var allInputs = $("input[type='text']");
    for (var i = 0; i < allInputs.length; i++) {
        allInputs[i].autocomplete = "off";
    }
    
}

function styleAllGrids() {
    var allGrids = $(".MainGrid");
    for (var grid in allGrids) {
        $(grid).addClass("table").addClass("table-hover");
    }
}


function removeModalWindow() {
    $("#modalWindow").remove();
}


function SelectSingleRadiobutton(rdbtnid) {
    
    var rdBtn = document.getElementById(rdbtnid);
    var rdBtnList = document.getElementsByTagName("input");
    for (i = 0; i < rdBtnList.length; i++) {
        if (rdBtnList[i].type == "radio" && rdBtnList[i].id != rdBtn.id) {
            rdBtnList[i].checked = false;
        }
    }
}

function clearTextBoxValue(defaultValue, params) {

    for (i = 0; i < params.length; i++) {
        var ctrl = document.getElementById(params[i]);
        ctrl.value = defaultValue;
    }
}
//function isNumeric(evt, isDouble) { 
//        var c = (evt.which) ? evt.which : event.keyCode;
//        if ((c >= 48  && c <= 57) || (isDouble && (c == 44 || c == 46)) || c == 8) {
//            return true; 
//        }
//        return false;
//    }

//    function calculateEstValue(evt, isDouble) {
//        var c = (evt.which) ? evt.which : event.keyCode;
//        if ((c >= 48 && c <= 57) || (isDouble && (c == 44 || c == 46)) || c == 8) {
//            var textValue1 = document.getElementById('<%=tbxQuantity.ClientID%>').value;
//            var textValue2 = document.getElementById('<%=tbxDetailUnitPriceWithVAT.ClientID%>').value;

//            if ($.trim(textValue1) != '' && $.trim(textValue2) != '') {
//                document.getElementById('<%=tbxDetailEstimatedValueWithVAT.ClientID%>').value = Math.round(textValue1 * textValue2, 2);
//            }
//        }
//        return false;
//    }

//$(function () { 
//    $('.two-digits').keyup(function () {
//        if ($(this).val().indexOf('.') != -1) {
//            if ($(this).val().split('.')[1].length > 2) {
//                if (isNaN(parseFloat(this.value))) return;
//                this.value = parseFloat(this.value).toFixed(2);
//            }
//        }
//        if ($(this).val().indexOf(',') != -1) {
//            if ($(this).val().split(',')[1].length > 2) {
//                if (isNaN(parseFloat(this.value))) return;
//                this.value = parseFloat(this.value).toFixed(2);
//            }
//        }
//        return this;
//    });
//});