/// <reference path="jquery-1.10.2.min.js" />
var i = 0;
var timer;
$(function () {
    //页面加载完成之后 第一张图片显示 其余的图片隐藏
    $(".ig").eq(0).show().siblings().hide();
    tim();
    //鼠标放上去
    $("#tabs li").hover(function () {
        clearInterval(timer);//停止计时器
        i = $(this).index();//获取当前鼠标选中的 索引  赋值给i
        start();
    }, function () {
        tim();
    });
    $(".btn1").click(function () {//点击左箭头
        clearInterval(timer);//停止计时器
        i--;//每点击一次 i减少1
        if (i == -1) {
            i = 4;
        }
        start();
    });

    $(".btn2").click(function () {
        clearInterval(timer);//停止计时器
        i++;
        if (i == 5) {
            i = 0;
        }
        start();
    });
});

function start() {
    $(".ig").eq(i).show().siblings().hide();//i++之后显示的 图片 
    $("#tabs li").eq(i).addClass("bg").siblings().removeClass("bg");//对应的 红色的原点
}


function tim() {
    timer = setInterval(function () {//间隔1秒钟
        i++;//i增加1
        if (i == 5) {//当显示到底5张图片的时候
            i = 0;//从第一张图片显示
        }
        start();
    }, 1000);
}