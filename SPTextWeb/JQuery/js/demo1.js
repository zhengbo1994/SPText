/// <reference path="jquery-1.10.2.min.js" />

$(function () {
    //点击 换肤 dpics显示
    $("#dcbg a").click(function () {
        $("#dpics").show(1000);
    });
    //点击收起 dpics隐藏
    $("#dup a").click(function () {
        $("#dpics").hide(1000);
    });
    //给body添加背景图片
    $(".dpic img").click(function () {
        //获取到当前 点击的图片的 src 值
        var bg = $(this).attr("src");
        $("body").css("background-image", "url(" + bg + ")");
    });
});