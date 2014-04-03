$(document).ready(function () {
    $(".fancybox").click(function () {
        openPopup($(this).attr("href"));
    });

    $(".popup_close, .popup").click(function () {
        closePopup();
    });

    $(".popup_window").click(function (e) {
        e.stopPropagation();
    });

    $('a[href^="#"]').click(function () {
        var target = $(this).attr('href');
        $('html, body').animate({ scrollTop: $(target).offset().top }, 300);
        return false;
    });
});

openPopup = function (pWindow) {
    $(".popup_window").hide();
    $(pWindow).show();
    $(".popup").show();

    if (typeof u != 'undefined') {
        var unityObj = u.getUnity();
        $(unityObj).width(0);
        $(unityObj).height(0);
        unityObj.SendMessage("ApiGameObject", "PopupOpened", "");
    }
}

closePopup = function () {
    $(".popup").hide();
    $(".popup_window").hide();

    if (typeof u != 'undefined') {
        var unityObj = u.getUnity();
        $(unityObj).width(unityConfig.width);
        $(unityObj).height(unityConfig.height);
        unityObj.SendMessage("ApiGameObject", "PopupClosed", "");
    }
}


/*$(function () {
    
});*/