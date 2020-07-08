/// <reference path="jquery-1.10.2.min.js" />


$(function () {
    //获取到浏览器的高度
    var dHeigth = $(window).height();

    //页面滚动轴
    $(document).scroll(function () {
        var top = $(document).scrollTop();//获取到滚动条到顶部的距离
        if (top > dHeigth) {//如果页面的高度 大于 窗体的高度
            $("#d1").show();//箭头显示
        } else {
            $("#d1").hide();//箭头隐藏
        }
    });
    $("#d1").click(function () {//点击箭头
        var timer=setInterval(function () {//间隔1秒钟
            var backtop = $(document).scrollTop();//获取当前用户滚动轴距离顶部的top
            var step = backtop / 2;//每次移动的距离
            $(document).scrollTop(backtop - step);//用户移动的总高度-step值
            if (backtop == 0) {//如果到达顶部
                clearInterval(timer);//停止计时器
            }
        },1000);
    });

    





});