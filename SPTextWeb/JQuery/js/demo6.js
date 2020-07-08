/// <reference path="jquery-1.10.2.min.js" />
var ranNum = 0;//存放产生的随机数
var i = 0;
$(function () {//页面加载完成之后
    $("#dsow").click(function () {//点击div（图片）
        i++;
        if (i >= 4) {
            alert("只有3次机会");
            return;
        }
      //第一个需求：产生一个随机数
          ranNum = Math.random() * 360;//产生一个1-360之间的随机数
      //第二个需求 让其旋转
        //  在jquery里没有直接旋转的方法 我们可以借助一个插件
         $(this).rotate({//让当前对象 旋转
            duration: 3000,//旋转一圈所需的时间 3秒
            angle: 0,//起始角度
            animateTo: ranNum+720,//终止的角度 也就是随机产生的值 想让它多转几圈 就给它加个值720转2圈
      //第三个需求 判断旋转之后所停的位置  
     //一共有8块 每块占360除以8=45  沿X轴划分 谢谢参与就是45除以2=22.5
       callback: function () {
                aRut();
            }
        });
    });
});
function aRut() {
    if (0 < ranNum && ranNum <= 22.5) {
        alert("谢谢参与，木有中奖");
        return;
//return 作用有两种1.跳出整个方法体2.返回值
//continue 跳出当前条件的判断 然后继续执行
//break 跳出当前循环体
    }
    else if (22.5 < ranNum && ranNum <= 67.5) {
        alert("恭喜，您中了5元代金券");
        return;
    } else if (67.5 < ranNum && ranNum <= 112.5) {
        alert("恭喜，您中了1元代金券");
        return;
    } else if (112.5 < ranNum && ranNum<=157.5) {
        alert("恭喜，您中了10元代金券");
        return;
    } else if (157.5 < ranNum && ranNum<=202.5) {
        alert("谢谢参与，木有中奖");
        return;
    } else if (202.5 < ranNum && ranNum <= 247.5) {
        alert("恭喜，您中了20元代金券");
        return;
    } else if (247.5 < ranNum && ranNum <= 292.5) {
        alert("恭喜，您中了50元代金券");
        return;
    } else if (292.5 < ranNum && ranNum <= 337.5) {
        alert("恭喜，您中了30元代金券");
        return;
    } else {
        alert("谢谢参与，木有中奖");
        return;
    }
}