/// <reference path="jquery-1.10.2.min.js" />
var ipronum = 0;
$(function () {
    //点击加入购物车按钮
    $(".dbtn").click(function () {
        ipronum++;//购物车的计数 加1
        var addImg = $(this).parent().find(".dpic").find("img");//当前的按钮 找到父类里的图片
        var cloneImg = addImg.clone();//复制一份图片 加入到购物车
        cloneImg.css({//图片的状态
            "width": "250px",
            "height": "250px",
            "position": "absolute",
            "top": addImg.offset().top,//获取到当前图片的位置
            "left": addImg.offset().left,
            "z-index": 1000,//复制的图片在最上层
            "opactiy": "0.5"//图片的透明度
        });
        cloneImg.appendTo($("body")).animate({//将克隆的图片添加到购物车
            "width": "50px",
            "height": "50px",
            "top": $("#dcar").offset().top,//购物车的位置
            "left": $("#dcar").offset().left,

        }, 1000, function () {
            $("#dprocount").html(ipronum);//显示数量
            $(this).remove();//清空
        });
    });
});