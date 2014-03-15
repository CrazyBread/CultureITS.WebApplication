$(document).ready(function () {
    $(".fancybox").fancybox({
        maxWidth: 960,
        maxHeight: 600,
        topRatio: 0.33,
        fitToView: false,
        width: '70%',
        height: '70%',
        autoSize: false,
        closeClick: false,
        openEffect: 'none',
        closeEffect: 'none',
        afterClose: function () {
            if (typeof u != 'undefined') {
                $(u.getUnity()).width(960);
                $(u.getUnity()).height(600);
            }
        },
        beforeLoad: function () {
            if (typeof u != 'undefined') {
                $(u.getUnity()).width(0);
                $(u.getUnity()).height(0);
            }
        }
    });
});