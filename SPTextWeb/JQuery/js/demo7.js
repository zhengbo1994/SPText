window.onload = function () {//页面加载完成之后
    document.getElementById("btn").onclick = function () {
       var iNum =  Math.floor(Math.random() * 100 + 1);//产生的一个1~100之间的随机数
       for (var i = 1; i < 7; i++) {
           if (i == 6) {
               alert("游戏结束，中奖号码是" + iNum);
               return;
           } else {
       var sInputs =  prompt("请输入1~100之间的一个整数");
       var nInput = Number(sInputs);
       if (isNaN(nInput)) {
           alert("你输入的不是一个数字");
       } else {//输入的是一个数字
           if (nInput == parseInt(nInput)) {//判断是不是一个整数  12.3  12  12  12
               if (nInput < 1 || nInput > 100) {
                   alert("你输入的不在1~100之间");
               } else {
                   if (nInput < iNum) {
                       alert("你输入的数字太小");
                   }else if (nInput > iNum) {
                       alert("你输入的数字太大");
                   } else {
                       alert("恭喜你中奖了");
                   }
               }
           } else {
               alert("你输入的不是整数");
           }
       }
           }
       }
    }
}