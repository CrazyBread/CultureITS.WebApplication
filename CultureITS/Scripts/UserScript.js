$(document).ready(function(){
    $(".fancybox").click(function(){
        $($(this).attr("href")).show();
        $(".popup").show();

        if (typeof u != 'undefined') {
            $(u.getUnity()).width(0);
            $(u.getUnity()).height(0);
            $("#unityPlayer").height(700);
        }
    });
    $(".popup_close, .popup").click(function(){
        $(".popup").hide();
        $(".popup_window").hide();
        
        if (typeof u != 'undefined') {
            $(u.getUnity()).width(1170);
            $(u.getUnity()).height(700);
        }
    });
    $(".popup_window").click(function(e){
        e.stopPropagation();
    });
});