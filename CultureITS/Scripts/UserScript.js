$(document).ready(function(){
    $(".fancybox").click(function () {
        openPopup($(this).attr("href"));
    });

    $(".popup_close, .popup").click(function(){
        closePopup();
    });

    $(".popup_window").click(function(e){
        e.stopPropagation();
    });
});

openPopup = function (pWindow) {
    $(".popup_window").hide();
    $(pWindow).show();
    $(".popup").show();

    if (typeof u != 'undefined') {
        $(u.getUnity()).width(0);
        $(u.getUnity()).height(0);
        $("#unityPlayer").height(700);
    }
}

closePopup = function () {
    $(".popup").hide();
    $(".popup_window").hide();

    if (typeof u != 'undefined') {
        $(u.getUnity()).width(1170);
        $(u.getUnity()).height(700);
    }
}
