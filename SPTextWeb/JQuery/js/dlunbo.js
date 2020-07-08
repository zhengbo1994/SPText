/// <reference path="jquery-1.10.2.min.js" />
var i = 0;
var timer;
$(function () {
    timer = setInterval(function () {
        i++;
        if (i < 5) {
            $("#igs").animate({ "left": -520 * i }, 1000);
        } else {
            $("#igs").animate({ "left": -520 * i }, 1000, function () {
                $(this).css("left", "0px");
            });
            i = 0;
        }
        $("#tags li").eq(i).addClass("bg").siblings().removeClass("bg");
    }, 1000);



    //鼠标放上去之后 
    $("#tags li").hover(function () {

        clearInterval(timer);
        ////获取到当前鼠标选中的 index
        i = $(this).index();
        //alert(i);
        if (i < 5) {
            $("#igs").animate({ "left": -520 * i }, 100);
        } else {
            $("#igs").animate({ "left": -520 * i }, 100, function () {
                $(this).css("left", "0px");
            });
            i = 0;
        }
        $("#tags li").eq(i).addClass("bg").siblings().removeClass("bg");



    }, function () {
        timer = setInterval(function () {
            i++;
            if (i < 5) {
                $("#igs").animate({ "left": -520 * i }, 100);
            } else {
                $("#igs").animate({ "left": -520 * i }, 100, function () {
                    $(this).css("left", "0px");
                });
                i = 0;
            }
            $("#tags li").eq(i).addClass("bg").siblings().removeClass("bg");
        }, 1000);
    });

    //左右箭头
    $(".btn1").click(function () {
        clearInterval(timer);
        i--;

        if (i == -1) {
            $("#igs").css("left", -520 * 5);
            i = 4;
        }

        if (i < 5) {
            $("#igs").animate({ "left": -520 * i }, 100);
        } else {
            $("#igs").animate({ "left": -520 * i }, 100, function () {
                $(this).css("left", "0px");
            });
            i = 0;
        }
        $("#tags li").eq(i).addClass("bg").siblings().removeClass("bg");

    });

    $(".btn2").click(function () {
        clearInterval(timer);
        i++;

        if (i < 5) {
            $("#igs").animate({ "left": -520 * i }, 100);
        } else {
            $("#igs").animate({ "left": -520 * i }, 100, function () {
                $(this).css("left", "0px");
            });
            i = 0;
        }
        $("#tags li").eq(i).addClass("bg").siblings().removeClass("bg");
    });

});

