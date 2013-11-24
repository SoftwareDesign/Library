$(function () {
    var getPage = function () {
        var $a = $(this);
        var options = {
            url: $a.attr("href"),
            data:$("form").serialize(),
            type: "get"
        };
        $.ajax(options).done(function (data) {
            var target = $a.parents("div.pageList").attr("mytarget");
            $(target).replaceWith(data);
        });

        return false;
    };
    
    $(".main-content").on("click", ".pageList a", getPage);
});

