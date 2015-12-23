//function confirmMsg(msg) {

//    var res = confirm(msg);
//    
//    if (res) {
//        return true; 
//    }
//    else {
//        return false;
//    }
//}

//function checkTextAreaMaxLength(textBox, e, length) {

//    var mLen = textBox["MaxLength"];
//    if (null == mLen)
//        mLen = length;

//    var maxLength = parseInt(mLen);
//    if (!checkSpecialKeys(e)) {
//        if (textBox.value.length > maxLength - 1) {
//            if (window.event)//IE
//            {
//                e.returnValue = false;
//                return false;
//            }
//            else//Firefox
//                e.preventDefault();
//        }
//    }
//}

//function checkSpecialKeys(e) {
//    if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 35 && e.keyCode != 36 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
//        return false;
//    else
//        return true;
//}

$(function () {
    $('.two-digits').keyup(function () {
        if ($(this).val().indexOf('.') != -1) {
            if ($(this).val().split(".")[1].length > 2) {
                if (isNaN(parseFloat(this.value))) return;
                this.value = parseFloat(this.value).toFixed(2);
            }
        }
        return this; //for chaining
    });
});


