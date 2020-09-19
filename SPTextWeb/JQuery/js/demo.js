/// <reference path="jquery-1.10.2.min.js" />
$(function () {
    //1、 隐藏嵌套的ul
    $(".ul-item").hide();
    //2.鼠标放上去显示
    $(".li-item").hover(function () {
        $(this).find(".ul-item").stop(true,true).slideDown();
    }, function () {//鼠标离开后隐藏
        $(this).find(".ul-item").stop(true, true).slideUp();
    });

    //子菜单里的 鼠标 t添加背景颜色
    $(".ul-item li a").hover(function () {
        $(this).addClass("bgs");
    }, function () {
        $(this).removeClass();
    });
});
