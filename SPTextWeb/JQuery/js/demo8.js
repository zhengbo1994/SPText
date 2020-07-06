/// <reference path="jquery-1.10.2.min.js" />
var minSize = 5;
var maxSize = 50;
var newOn = 100;//间隔多长时间产生一个雪片
var flakeColor = "#fff";
var $flake = $("<div></div>").css("position", "absolute").html("❄");//雪片对象
$(function () {
     var documentWidth = $(document).width();//获取到浏览器的宽度
     var documentHeight = $(document).height();//获取到浏览器的高度 
     setInterval(function () {//1.间隔多长时间产生一个雪片
         var startPositionLeft = Math.random() * documentWidth;//雪片一开始的时候距离浏览器的left值
         var startOpacity = 0.7 + Math.random() * 0.3;//雪片一开始的透明度  区间0-1  0完全不透明  1完全透明
         var endOpactity = 0.4 + Math.random() * 0.3;//雪片下落之后的透明度
         var endPostitionLeft = Math.random() * documentWidth;//雪片下落后距离浏览器的left
         var duration = documentHeight * 10 + Math.random() * 3000;//雪片从最上面落到最下面的一个时间  我们也给它一个随机数
         var sizeFlake = minSize+Math.random() * maxSize;//雪片的大小 区间 最大大不过50 最小小不过5
         $flake.clone().appendTo("body").css({
             "left": startPositionLeft,
             "opacity": startOpacity,
             "font-size": sizeFlake,
             "color": flakeColor,
             "top":"-55px"
         }).animate({
             "top": documentHeight,
             "left": endPostitionLeft,
             "opacity": endOpactity
         }, duration, function () {
             $(this).remove();
         });
     }, newOn);//间隔newOn 0.1秒 产生一个雪片 克隆一份添加到网页中去，经过duration, 这个时间由上面css的状态变成animate这个状态
});

