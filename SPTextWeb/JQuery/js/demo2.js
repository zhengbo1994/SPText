/// <reference path="jquery-1.10.2.min.js" />


$(function () {
    //全选 点击全选按钮
    $("#checkAll").click(function () {
        if (this.checked) {//如果全选按钮 选中
            $("input[class=ck]").prop("checked", true);//1 和2 选中
        } else {
            $("input[class=ck]").prop("checked", false);//1 和2 不勾选
        }
    });
});