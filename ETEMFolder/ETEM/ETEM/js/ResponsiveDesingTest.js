/// <reference path="../Scripts/jquery-1.10.2.min.js" />
/// <reference path="../Scripts/jquery-ui-1.10.3.custom.js" />


$(document).ready(function () {



    //    $(window).on("keyup",  function (e) {
    //        alert(e.which);
    //    });


    //    $(".valuesCell").keyup(function (e) {

    //        if (e.which == 37) {
    //            alert("left");
    //        }
    //        if (e.which == 38) {
    //            alert("up");
    //        }
    //        if (e.which == 39) {
    //            alert("right");
    //        }
    //        if (e.which == 40) {
    //            alert("down");
    //        }

    //    });


    $("#UpdatePanel").addClass("main-container");

    $(window).resize(function () {


        if ($(window).width() < 490) {
            $("#login-form .loginInputs").removeClass("offset4").addClass("offset1");
        } else if ($(window).width() > 491) {
            $("#login-form .loginInputs").removeClass("offset1").addClass("offset4");

        }

        if ($(window).width() < 630) {
            $(".modalPopup-middle").removeClass("modalPopup-middle").addClass("modalPopup-dynamicChanged");
        }
        else {

            $(".modalPopup-dynamicChanged").removeClass("modalPopup-dynamicChanged").addClass("modalPopup-middle");
        }

    });
})

$(document).on("change", '#ContentPlaceHolder_MenuNode_gvMainManu input[type=checkbox]', function () {
    if (this.checked == true) {
        var parentsTd = $(this).parents("td");
        for (var i = 0; i < parentsTd.length; i++) {
            var parentCheckBox = $(parentsTd[i]).children("input[type=checkbox]");
            $(parentCheckBox).prop("checked", true);
        }

    }
    else {
        var parentsTd = $(this).parent("td");
        var childrenCheckBoxes = $(parentsTd).find("input[type=checkbox]");
        for (var i = 0; i < childrenCheckBoxes.length; i++) {
            $(childrenCheckBoxes[i]).prop("checked", false);
        }

    }
});

$(document).on("click", '.btn-hide-show', function () {
    var clickedElement = $(this);
    var nestedGrid = clickedElement.nextAll("div");
    if ($(nestedGrid).hasClass("hidden")) {
        $(nestedGrid).removeClass("hidden");
        $(this).html("<i class='icon-minus-sign'></i>");

    }
    else {
        $(nestedGrid).addClass("hidden");
        $(this).html("<i class='icon-plus-sign'></i>");

    }
});




//$("input.valuesCell").on("keyup", function (e) {
//    alert(e.which);
//});



$(document).on("keyup", 'input.valuesCell', function (e) {
    if (e.which == 37) {//left
    
        $(this).parent().prev().children("input[type='text']").focus();
    }
    if (e.which == 38) {//up
        var cellNumer = -1;
        var tds = $(this).parent().parent().children();               //input->td->tr
        for (var i = 0; i < tds.length; i++) {
            if (tds[i].className && tds[i].className == this.parentElement.className) {
                cellNumer = i;
                break;
            }
        }

        var tdsPrev = $(this).parent().parent().prev().children();
        for (var i = 0; i < tdsPrev.length; i++) {
            if (i == cellNumer) {
                $(tdsPrev[i]).children("input[type='text']").focus();
                break;
            }
        }

    }
    if (e.which == 39) {//right
        $(this).parent().next().children("input[type='text']").focus();
    }
    if (e.which == 40) {//down
        var cellNumer = -1;
        var tds = $(this).parent().parent().children();               //input->td->tr
        for (var i = 0; i < tds.length; i++) {
            //get the number of the selected cell
            if (tds[i].className && tds[i].className == this.parentElement.className) {
                cellNumer = i;
                break;
            }
        }

        var tdsPrev = $(this).parent().parent().next().children();
        for (var i = 0; i < tdsPrev.length; i++) {
            if (i == cellNumer) {
                $(tdsPrev[i]).children("input[type='text']").focus();
                break;
            }
        }
    }

}); 