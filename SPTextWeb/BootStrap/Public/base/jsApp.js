(function () {

    var regVerify = function (type, checkString) {

        //验证手机号码
        //var _regPhoneNumber = /(^[0-9]{3,4}\-[0-9]{3,8}$)|(^[0-9]{3,8}$)|(^\([0-9]{3,4}\)[0-9]{3,8}$)|(^0{0,1}13[0-9]{9}$)/
        var yidongreg = /^(134[012345678]\d{7}|1[34578][012356789]\d{8})$/;
        var liantongreg = /^1[34578][01256]\d{8}$/;
        var dianxinreg = /^1[3578][01379]\d{8}$/;

        //验证邮箱格式
        var _regMailAddress = /^(\w-*\.*)+@(\w-?)+(\.\w{2,})+$/;

        //验证密码
        var _regPassword = /^[A-Za-z0-9]{6,}$/;

        //验证不可为空
        var _regNotNull = /\S/;

        //验证必须是数字
        var _regNumber = /^[0-9]*.[0-9]*$/;

        //验证必须是非负整数
        var _regNotNegativeInteger = /^[0-9]*[0-9]*$/;

        //身份证号
        var _regIDCard = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;

        var regString = "";
        var result = false;


        //枚举type类型
        if (type) {
            type = type.toString().toLowerCase();
        }
        switch (type) {
            case "mobilephone":
                regString = yidongreg || liantongreg || dianxinreg;
                break;
            case "email":
                regString = _regMailAddress;
                break
            case "password":
                regString = _regPassword;
                break;
            case "notempty":
                regString = _regNotNull;
                break;
            case "number":
                regString = _regNumber;
                break;
            case "int":
                regString = _regNotNegativeInteger;
                break;
            case "idnumber":
                regString = _regIDCard;
                break;
            default:
                return false;

        }

        //通过验证时
        if (regString.test(checkString)) {
            result = true;
        }
        return result;

    }

    var verifyForm = function ($form, errCallback) {
        var $arrInput = $form.find("input,textarea,select");
        for (var i = 0; i < $arrInput.length; i++) {
            var $input = $($arrInput[i]);
            var verifyTypes = $input.data("verify");

            if (!verifyTypes || "" == $.trim(verifyTypes) || $input.prop("disabled")) {
                continue;
            }

            var verifyFlag = verifyInput($input, errCallback)
            if (!verifyFlag) {
                return verifyFlag;
            }
        }
        return true;
    }

    var verifyInput = function ($input, errCallback) {
        var result = true;

        var verifyTypes = $input.data("verify");
        var errorMessage = $input.data("verify-errormessage");

        var arrTypes = verifyTypes.split(',');
        for (var t = 0; t < arrTypes.length ; t++) {
            var verifyFlag = regVerify(arrTypes[t], $input.val());
            if (false == verifyFlag) {
                if (errCallback && "function" == typeof (errCallback)) {
                    errCallback($input);
                } else {
                    alert(errorMessage);
                }
                result = verifyFlag;
            }
        }
        return result;
    }

    var jqGridAutoWidth = function () {
        var $gridWp = $(".ui-jqgrid");
        $gridWp.css("width", "100%");
        $(".ui-jqgrid-view", $gridWp).css("width", "100%");
        $(".ui-jqgrid-hdiv", $gridWp).css("width", "100%");
        $(".ui-jqgrid-htable", $gridWp).css("width", "100%");
        $(".ui-jqgrid-btable", $gridWp).css("width", "100%");
        $(".ui-jqgrid-pager", $gridWp).css("width", "100%");
        $(".ui-jqgrid-bdiv", $gridWp).css("width", "100%").css("padding-right", "20px");
        $(".ui-jqgrid-hbox", $gridWp).css("width", "100%");

    }

    //重置Pager按钮
    var updatePagerIcons = function (table) {
        var replacement =
        {
            'ui-icon-seek-first': 'ace-icon fa fa-angle-double-left bigger-140',
            'ui-icon-seek-prev': 'ace-icon fa fa-angle-left bigger-140',
            'ui-icon-seek-next': 'ace-icon fa fa-angle-right bigger-140',
            'ui-icon-seek-end': 'ace-icon fa fa-angle-double-right bigger-140'
        };
        $('.ui-pg-table:not(.navtable) > tbody > tr > .ui-pg-button > .ui-icon').each(function () {
            var icon = $(this);
            var $class = $.trim(icon.attr('class').replace('ui-icon', ''));

            if ($class in replacement) icon.attr('class', 'ui-icon ' + replacement[$class]);
        })
    }

    var setInputAsDatePlug = function (input) {
        input.datetimepicker({
            minView: "month",
            format: 'yyyy-mm-dd',
            autoclose: true,
            todayBtn: true,
            language: "zh-CN"
        });
        input.prop("readonly", true);
    }

    var setInputAsDatetimePlug = function (input, format) {
        var formatOpt = {
            format: 'yyyy-mm-dd hh:ii:ss',
            todayBtn: true,
            language: "zh-CN"
        };
        if (format) {
            formatOpt.format = format;
        }
        input.datetimepicker(formatOpt);
    }

    var setGridHeight = function (gridID) {
        if (gridID) {
            gridID = gridID.replace("#", "");
        }
        var windowHeight = $(window).height();
        var $gview = $("#gview_" + gridID);
        if ($gview.length > 0) {
            var gbody = $gview.find(".ui-jqgrid-bdiv");
            var gbodyTop = gbody.length > 0 ? gbody.offset().top : 0;
            gbody.height(windowHeight - gbodyTop - 80);
        }
    }


    this.regVerify = regVerify;//验证
    this.jqGridAutoWidth = jqGridAutoWidth;
    this.updatePagerIcons = updatePagerIcons;
    this.setInputAsDatePlug = setInputAsDatePlug;
    this.setInputAsDatetimePlug = setInputAsDatetimePlug;
    this.verifyForm = verifyForm;
    this.verifyInput = verifyInput;
    this.setGridHeight = setGridHeight;

}).call(this)