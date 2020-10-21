//#region 参数辅助对象
//公共参数辅助对象
var ObjParameter = {

    //获取url参数值
    //注意：参数值最好不用中文，会有编码问题
    //name:参数名称
    getQueryString: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    },


    //decodeURI转码过的，主要防止中文乱码
    //获取url参数值
    //注意：参数值最好不用中文，会有编码问题
    //name:参数名称
    getQueryStringDecodeURI: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = decodeURI(window.location.search).substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }
};
//#endregion

//#region  日期时间辅助对象
var dateObj;
//日期时间辅助方法
var objDate = {
    //获取当前日期：格式 2016-11-15
    //Separate分隔符 默认"-"
    GetCurrentDate: function (Separate) {
        dateObj = new Date();
        if (!objValidate.NotNull(Separate)) {
            Separate = "-";
        }

        var _Year = dateObj.getFullYear();
        var _Month = (dateObj.getMonth() + 1);
        var _Date = dateObj.getDate();

        if (_Month < 10) {
            _Month = "0" + _Month;
        }

        if (_Date < 10) {
            _Date = "0" + _Date;
        }

        var result = _Year + Separate + _Month + Separate + _Date;
        return result;
    },

    //获取当前时间：格式 17:36:12
    //IsSeconds是否精确到秒，1是，0否，默认不需要秒
    GetCurrentTime: function (IsSeconds) {
        dateObj = new Date();

        var result = "";

        var Hours = dateObj.getHours();
        var Minutes = dateObj.getMinutes();

        if (Hours < 10) {
            Hours = "0" + Hours;
        }

        if (Minutes < 10) {
            Minutes = "0" + Minutes;
        }

        result = Hours + ":" + Minutes;

        if (IsSeconds == 1) {

            var Seconds = dateObj.getSeconds();

            if (Seconds < 10) {
                Seconds = "0" + Seconds;
            }

            result += ":" + Seconds;
        }

        return result;
    },

    //获取当前日期与时间：格式 2016-11-15 17:47:27
    //Separate日期分隔符 默认"-"
    //IsSeconds是否精确到秒，1是，0否，默认不需要秒
    GetCurrentDateTime: function (Separate, IsSeconds) {

        var result = objDate.GetCurrentDate(Separate) + " " + objDate.GetCurrentTime(IsSeconds);

        return result;
    },

    ///获取指定年月的天数
    //year：年份
    //month：月份
    GetMonthDays: function (year, month) {
        var day = new Date(year, month, 0);
        //获取天数：
        var daycount = day.getDate();
        return daycount;
    },

    //日期加减天数
    //datestr:要运算的时间，默认当前日期
    //type：运算类型 ("+" or "-")，默认 +
    //separate：时间分隔符 ("-" or "/" )，默认 -
    //numberDays：天数，默认 1
    //返回加减后的日期
    DateOperation: function (datestr, type, separate, numberDays) {

        datestr = objValidate.NotNull(datestr) ? datestr : objDate.GetCurrentDate();
        type = objValidate.NotNull(type) ? type : "+";
        separate = objValidate.NotNull(separate) ? separate : "-";
        numberDays = objValidate.NotNull(numberDays) ? numberDays : 1;



        if (numberDays == 0) {
            var date = new Date(datestr);
            var myyear = date.getFullYear();
            var mymonth = date.getMonth() + 1;
            var myweekday = date.getDate();

            return (myyear + separate + mymonth + separate + myweekday);
        }

        for (var i = 0; i < numberDays; i++) {

            var date = new Date(datestr);
            var myyear = date.getFullYear();
            var mymonth = date.getMonth() + 1;
            var myweekday = date.getDate();
            //var myhours = date.getHours();
            //var myminutes = date.getMinutes();

            if (type == "+") {
                myweekday = myweekday + 1;
                //超过当月所有天数就加一个月（进入下一月）
                if (objDate.GetMonthDays(myyear, mymonth) < myweekday) {
                    mymonth = mymonth + 1;
                    myweekday = 1;

                    //超过12月就加一年（进入下一年）
                    if (mymonth > 12) {
                        myyear = myyear + 1;
                        mymonth = 1;
                        myweekday = 1;
                    }
                }
            }

            if (type == "-") {
                myweekday = myweekday - 1;

                //低于1号就减一个月（进入上一月）
                if (1 > myweekday) {
                    mymonth = mymonth - 1;
                    myweekday = objDate.GetMonthDays(myyear, mymonth);

                    //低于1月就减一年（进入上一年）
                    if (mymonth < 1) {
                        myyear = myyear - 1;
                        mymonth = 12;
                        myweekday = objDate.GetMonthDays(myyear, mymonth);
                    }
                }
            }

            if (mymonth < 10) {
                mymonth = "0" + mymonth;
            }
            if (myweekday < 10) {
                myweekday = "0" + myweekday;
            }

            datestr = myyear + separate + mymonth + separate + myweekday;
        }

        return (myyear + separate + mymonth + separate + myweekday);

    },

    //时间转换字符
    //datetime要转换的时间字符串 默认为：0000/00/00
    //separate 分隔符2009-06-12或2009/06/12 （"/","-"） 默认 "-"
    //length长度2009-06-12(10)或2009-06-12 17:18(15)或2009-06-12 17:18:05(18) 默认10
    //空时间返回0000/00/00 或者 0000-00-00
    DateToStr: function (datetime, separate, length) {

        separate = objValidate.NotNull(separate) ? separate : "-";
        length = objValidate.NotNull(length) ? length : 10;

        if (!objValidate.NotNull(datetime)) {

            return "0000" + separate + "00" + separate + "00";
        }
        var datetime = new Date(datetime);
        var year = datetime.getFullYear();
        var month = datetime.getMonth() + 1;//js从0开始取 
        var date = datetime.getDate();
        var hour = datetime.getHours();
        var minutes = datetime.getMinutes();
        var second = datetime.getSeconds();

        if (month < 10) {
            month = "0" + month;
        }
        if (date < 10) {
            date = "0" + date;
        }
        if (hour < 10) {
            hour = "0" + hour;
        }
        if (minutes < 10) {
            minutes = "0" + minutes;
        }
        if (second < 10) {
            second = "0" + second;
        }
        var time = year + "-" + month + "-" + date + " " + hour + ":" + minutes + ":" + second;

        if (length == 10) {

            time = year + separate + month + separate + date;
        }
        if (length == 15) {

            time = year + separate + month + separate + date + " " + hour + ":" + minutes;
        }
        if (length == 18) {

            time = year + separate + month + separate + date + " " + hour + ":" + minutes + ":" + second;
        }

        //2009-06-12 17:18:05
        return time;
    },


    //时间转换短格式（10/26 21:29）
    //datetime要转换的时间字符串
    //separate 分隔符2009-06-12或2009/06/12 （"/","-"）
    dateToStrShort: function (datetime, separate) {

        if (datetime == "" || datetime == null) {
            return "0000/00/00";
        }
        var datetime = new Date(datetime);
        var year = datetime.getFullYear();
        var month = datetime.getMonth() + 1;//js从0开始取 
        var date = datetime.getDate();
        var hour = datetime.getHours();
        var minutes = datetime.getMinutes();
        var second = datetime.getSeconds();

        if (month < 10) {
            month = "0" + month;
        }
        if (date < 10) {
            date = "0" + date;
        }
        if (hour < 10) {
            hour = "0" + hour;
        }
        if (minutes < 10) {
            minutes = "0" + minutes;
        }
        if (second < 10) {
            second = "0" + second;
        }
        var time = year + "-" + month + "-" + date + " " + hour + ":" + minutes + ":" + second;

        time = month + separate + date + " " + hour + ":" + minutes;

        //
        return time;

    },

    //获取当前时间段
    //凌晨早上上午下午晚上
    GetTimeSection: function () {

        var hour = new Date().getHours();

        var s = "早上";
        if (hour >= 0 && hour < 6) {
            s = "凌晨";
        }
        else if (hour >= 6 && hour < 8) {
            s = "早上";
        }
        else if (hour >= 8 && hour < 13) {
            s = "上午";
        }
        else if (hour >= 13 && hour < 19) {
            s = "下午";
        }
        else {
            s = "晚上";
        }

        return s;
    },


    //获取本周开始日期（周一到周日）
    //返回时间需要调用dateObj.dateToStr(weekStartDate, "/", 10)格式化
    getWeekStartDate: function () {
        var testdate = new Date();
        var weekStartDate = new Date(testdate.getFullYear(), testdate.getMonth(), testdate.getDate() - testdate.getDay() + 1);
        return (weekStartDate);

    },


    //获取本周结束日期（周一到周日）
    //返回时间需要调用dateObj.dateToStr(getWeekEndDate, "/", 10)格式化
    getWeekEndDate: function () {

        var testdate = new Date();
        var weekEndDate = new Date(testdate.getFullYear(), testdate.getMonth(), testdate.getDate() + (6 - testdate.getDay() + 1));
        return (weekEndDate);

    },



    ///获取当前月的第一天
    GetMonthFirstDay: function () {

        dateObj = new Date();

        var year = dateObj.getFullYear();
        var month = dateObj.getMonth() + 1;

        if (month < 10) {
            month = "0" + month;
        }

        var month_first = year + '-' + month + '-01'

        return month_first;

    },

    ///获取当前月的最后一天
    GetMonthLastDay: function () {

        dateObj = new Date();

        var year = dateObj.getFullYear();
        var month = dateObj.getMonth() + 1;
        var day = new Date(year, month, 0);

        if (month < 10) {
            month = "0" + month;
        }
        var month_last = year + '-' + month + '-' + day.getDate();

        return month_last;

    }

};
//#endregion

//#region  公共验证辅助对象
//公共验证辅助对象
var objValidate = {


    //手机号码验证
    //返回true or false
    IsMobilePhone: function (val) {
        val = val.ResetBlank().trim();
        var reg = /^0?1[2|3|4|5|6|7|8|9][0-9]\d{8}$/;
        if (reg.test(val)) {
            return true;
        } else {
            return false;
        };
    },

    //固定电话号码验证
    //返回true or false
    IsTelephone: function (val) {

        val = val.ResetBlank().trim();
        var reg = /^(\(\d{3,4}\)|\d{3,4}-|\s)?\d{7,14}$/;
        if (reg.test(val)) {
            return true;
        } else {
            return false;
        };
    },


    //验证日期格式是否正确
    //正确格式为：2012/02/03 2012-02-03 
    //正确返回 true  错误返回 false
    checkDate: function (_date) {

        var DATE_FORMAT = /^[0-9]{4}-[0-1]?[0-9]{1}-[0-3]?[0-9]{1}$/;
        var DATE_FORMAT2 = /^[0-9]{4}\/[0-1]?[0-9]{1}\/[0-3]?[0-9]{1}$/;

        if (DATE_FORMAT.test(_date) || DATE_FORMAT2.test(_date)) {
            return true;
        } else {
            return false;
        }

    },


    //非空验证，""，null，undefined
    //空则返回 false 非空返回 true
    NotNull: function (val) {

        if (typeof (val) == "string") {
            val = val.trim();
        }

        if (val === "" || val === "null" || val === "undefined" || val === null || val === undefined) {
            return false;
        }
        return true;
    },


    //邮箱验证
    //str:邮箱
    emailCheck: function (str) {

        var reg = /^([\.a-zA-Z0-9_-])+@@([a-zA-Z0-9_-])+(\.[a-zA-Z0-9_-])+/;
        return reg.test(str);

    },

    //验证QQ格式，是否合适
    //QQ必须为纯数字，且长度为5-11位
    //val:QQ号
    //返回bool
    IsQQGeShi: function (val) {
        if (!objValidate.isDigital(val) || val.length > 11 || val.length < 5) {
            return false;
        }
        return true;

    },

    //是否为纯汉字
    //返回bool
    IsChinese: function (val) {

        var myReg = /^[\u4e00-\u9fa5]+$/;
        if (myReg.test(val)) {
            return true;
        } else {
            return false;
        }

    },

    //验证中文姓名
    //必须为纯中文
    //长度必须在2-5之间
    IsNamezhGeShi: function (val) {
        val = val.trim();
        if (!objValidate.IsChinese(val) || val.length > 5 || val.length < 2) {
            return false;
        }
        else {
            return true;
        }
    },


    //身份证验证
    //card:身份证号码
    //返回：1 请输入身份证号，身份证号不能为空
    //返回：2 您输入的身份证号码不正确，请重新输入
    //返回：3 您输入的身份证号码不正确,请重新输入
    //返回：4 您输入的身份证号码生日不正确,请重新输入
    //返回：5 您的身份证校验位不正确,请重新输入
    //返回：0 验证通过
    checkIDCard: function (card) {


        //是否为空
        if (!objValidate.NotNull(card)) {
            //alert('请输入身份证号，身份证号不能为空');
            //document.getElementById('card_no').focus;
            return 1;
        }



        //校验长度，类型
        if (isCardNo(card) === false) {
            //alert('您输入的身份证号码不正确，请重新输入');

            return 2;
        }
        //检查省份
        if (checkProvince(card) === false) {
            //alert('您输入的身份证号码不正确,请重新输入');

            return 3;
        }
        //校验生日
        if (checkBirthday(card) === false) {
            //alert('您输入的身份证号码生日不正确,请重新输入');

            return 4;
        }
        //检验位的检测
        if (checkParity(card) === false) {
            //alert('您的身份证校验位不正确,请重新输入');

            return 5;
        }
        return 0;


    },

    ///是否为纯数字
    isDigital: function (val) {
        var reg = new RegExp("^[0-9]*$");
        return reg.test(val);
    },


    ///只能输入字母与数字
    isLettersOrNumber: function (val) {
        var reg = new RegExp("^[A-Za-z0-9]*$");
        return reg.test(val);
    },


    ////单价只能输入数字和小数点
    clearNoNum: function (obj) {

        obj.value = obj.value.replace(/[^\d.]/g, "");  //清除“数字”和“.”以外的字符
        obj.value = obj.value.replace(/^\./g, "");  //验证第一个字符是数字而不是.
        obj.value = obj.value.replace(/\.{2,}/g, "."); //只保留第一个. 清除多余的.
        obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");

        //调用示例：
        //            onkeyup = "clearNoNum(this);"

    },

    //用户名验证，只可以用字母数字下划线做用户名，必须是字母开头，6-16字
    ValiUser: function (Name) {
        var patrn = /^[a-zA-Z]{1}([a-zA-Z0-9]|[._]){5,15}$/;
        if (!patrn.exec(Name)) return false
        return true
    },


    //验证对象是否为json对象
    //obj:要验证的对象
    //是json对象则返回true 否则返回false
    isJsonObj: function (obj) {

        var isjson = typeof (obj) == "object" && Object.prototype.toString.call(obj).toLowerCase() == "[object object]" && !obj.length;
        return isjson;

    },

    //验证字符是否为json格式
    //str:要验证的字符串
    //是json格式则返回true 否则返回false
    isJsonStr: function (str) {

        try {

            if (!objValidate.NotNull(str)) {
                return false;
            }

            if (typeof (str) == "number" || objValidate.isDigital(str)) {
                return false;
            }


            if (typeof (str) == "number") {
                return false;
            }

            $.parseJSON(str);
            return true;
        } catch (e) {
            return false;
        }

    }
};
//#endregion

//#region  身份证验证所用到方法
var vcity = {
    11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古",
    21: "辽宁", 22: "吉林", 23: "黑龙江", 31: "上海", 32: "江苏",
    33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南",
    42: "湖北", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆",
    51: "四川", 52: "贵州", 53: "云南", 54: "西藏", 61: "陕西", 62: "甘肃",
    63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外"
};

//检查号码是否符合规范，包括长度，类型
isCardNo = function (card) {
    //身份证号码为15位或者18位，15位时全为数字，18位前17位为数字，最后一位是校验位，可能为数字或字符X
    var reg = /(^\d{15}$)|(^\d{17}(\d|X)$)/;
    if (reg.test(card) === false) {
        return false;
    }

    return true;
};


//取身份证前两位,校验省份
checkProvince = function (card) {
    var province = card.substr(0, 2);
    if (vcity[province] == undefined) {
        return false;
    }
    return true;
};


//检查生日是否正确
checkBirthday = function (card) {
    var len = card.length;
    //身份证15位时，次序为省（3位）市（3位）年（2位）月（2位）日（2位）校验位（3位），皆为数字
    if (len == '15') {
        var re_fifteen = /^(\d{6})(\d{2})(\d{2})(\d{2})(\d{3})$/;
        var arr_data = card.match(re_fifteen);
        var year = arr_data[2];
        var month = arr_data[3];
        var day = arr_data[4];
        var birthday = new Date('19' + year + '/' + month + '/' + day);
        return verifyBirthday('19' + year, month, day, birthday);
    }
    //身份证18位时，次序为省（3位）市（3位）年（4位）月（2位）日（2位）校验位（4位），校验位末尾可能为X
    if (len == '18') {
        var re_eighteen = /^(\d{6})(\d{4})(\d{2})(\d{2})(\d{3})([0-9]|X)$/;
        var arr_data = card.match(re_eighteen);
        var year = arr_data[2];
        var month = arr_data[3];
        var day = arr_data[4];
        var birthday = new Date(year + '/' + month + '/' + day);
        return verifyBirthday(year, month, day, birthday);
    }
    return false;
};



//校验日期
verifyBirthday = function (year, month, day, birthday) {
    var now = new Date();
    var now_year = now.getFullYear();
    //年月日是否合理
    if (birthday.getFullYear() == year && (birthday.getMonth() + 1) == month && birthday.getDate() == day) {
        //判断年份的范围（3岁到100岁之间)
        var time = now_year - year;
        if (time >= 3 && time <= 100) {
            return true;
        }
        return false;
    }
    return false;
};



//校验位的检测
checkParity = function (card) {
    //15位转18位
    card = changeFivteenToEighteen(card);
    var len = card.length;
    if (len == '18') {
        var arrInt = new Array(7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2);
        var arrCh = new Array('1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2');
        var cardTemp = 0, i, valnum;
        for (i = 0; i < 17; i++) {
            cardTemp += card.substr(i, 1) * arrInt[i];
        }
        valnum = arrCh[cardTemp % 11];
        if (valnum == card.substr(17, 1)) {
            return true;
        }
        return false;
    }
    return false;
};



//15位转18位身份证号
changeFivteenToEighteen = function (card) {
    if (card.length == '15') {
        var arrInt = new Array(7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2);
        var arrCh = new Array('1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2');
        var cardTemp = 0, i;
        card = card.substr(0, 6) + '19' + card.substr(6, card.length - 6);
        for (i = 0; i < 17; i++) {
            cardTemp += card.substr(i, 1) * arrInt[i];
        }
        card += arrCh[cardTemp % 11];
        return card;
    }
    return card;
};
//#endregion

//#region  公共常用辅助方法
var objCommon = {
    //生成唯一标识
    //len长度
    //radix基数，指0到几,全数字是10，数字加大写字母是36，数字加大小写字母是62
    // 8 character ID (base=2)
    //uuid(8, 2)  //  "01001010"
    // 8 character ID (base=10)
    //uuid(8, 10) // "47473046"
    // 8 character ID (base=16)
    //uuid(8, 16) // "098F4D35"
    //0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz
    GetGuid: function (len, radix) {

        var chars = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz'.split('');
        var uuid = [], i;
        radix = radix || chars.length;

        if (len) {
            // Compact form
            for (i = 0; i < len; i++) uuid[i] = chars[0 | Math.random() * radix];
        } else {
            // rfc4122, version 4 form
            var r;

            // rfc4122 requires these characters
            uuid[8] = uuid[13] = uuid[18] = uuid[23] = '-';
            uuid[14] = '4';

            // Fill in random data.  At i==19 set the high bits of clock sequence as
            // per rfc4122, sec. 4.1.5
            for (i = 0; i < 36; i++) {
                if (!uuid[i]) {
                    r = 0 | Math.random() * 16;
                    uuid[i] = chars[(i == 19) ? (r & 0x3) | 0x8 : r];
                }
            }
        }

        return uuid.join('');

    }

};
//#endregion

//#region  字符辅助方法
//字符辅助对象
var objStr = {

    //截取字符,val字符值，num截取数，char标志符
    Intercept: function (val, num, char) {

        val = val.toString();
        num = parseInt(num);
        var str = val;
        if (val.indexOf("" + char + "") != -1) {
            str = val.substring(0, val.indexOf("" + char + "") + 1 + num);
        }
        return str.toString();
    },

    //提取汉字
    GetChinese: function (str) {

        var reg = /[\u4e00-\u9fa5]/g;
        var strs = str.match(reg);
        str = strs.join("");
        return str;
    }

};
//#endregion

//#region  数字辅助方法
//数字辅助对象
var objNumber = {

    //保留小数位，intval值，num位数从1开始是一位
    ReservedDecimal: function (intval, num) {

        intval = intval.toString();
        num = parseInt(num);
        var str = intval;
        if (intval.indexOf(".") != -1) {
            str = intval.substring(0, intval.indexOf(".") + 1 + num);
        }
        return str.toString();
    },



    //生成随机数
    //minNum开始数
    //maxNum结束数
    randomNum: function (minNum, maxNum) {

        switch (arguments.length) {
            case 1:
                return parseInt(Math.random() * minNum + 1);
                break;
            case 2:
                return parseInt(Math.random() * (maxNum - minNum + 1) + minNum);
                break;
            default:
                return 0;
                break;
        }
    },
    //金额大小写转换
    DaXie: function (n) {

        if (!/^(0|[1-9]\d*)(\.\d+)?$/.test(n))
            return "数据非法";
        var unit = "千百拾亿千百拾万千百拾元角分", str = "";

        if (n <= 0) {
            return "零元";
        }

        n += "00";
        var p = n.indexOf('.');
        if (p >= 0) {
            n = n.substring(0, p) + n.substr(p + 1, 2);
        }
        unit = unit.substr(unit.length - n.length);
        for (var i = 0; i < n.length; i++)
            str += '零壹贰叁肆伍陆柒捌玖'.charAt(n.charAt(i)) + unit.charAt(i);
        return str.replace(/零(千|百|拾|角)/g, "零").replace(/(零)+/g, "零").replace(/零(万|亿|元)/g, "$1").replace(/(亿)万|壹(拾)/g, "$1$2").replace(/^元零?|零分/g, "").replace(/元$/g, "元整");


    }

};

//#endregion

//#region  数组辅助方法
///数组辅助对象
var objArray = {
    //获取数组最大值
    //空数组返回0
    ArrayMax: function (shuzu) {
        if (objValidate.NotNull(shuzu)) {
            return Math.max.apply(Math, shuzu);
        }
        return 0;
    },

    //获取数组最小值
    //空数组返回0
    ArrayMin: function (shuzu) {
        if (objValidate.NotNull(shuzu)) {

            return Math.min.apply(Math, shuzu);
        }
        return 0;
    }
};
//#endregion

//#region  父页面操作
//父页面辅助对象
var objParent = {
    //设置框架高度
    //iframe:框架id
    setIframeHeight: function (iframe) {
        if (iframe) {
            var iframeWin = iframe.contentWindow || iframe.contentDocument.parentWindow;
            if (iframeWin.document.body) {
                iframe.height = iframeWin.document.documentElement.scrollHeight || iframeWin.document.body.scrollHeight;
            }
        }
    },

    //子页面设置父页面框架高度
    setParentIframeHeight: function () {
        objParent.setIframeHeight(parent.document.getElementById('frameID'));
    }
};
//#endregion

//#region  自定义对象
//自定义字典对象
function Dictionary() {
    this.data = new Array();
    //键不可为0
    this.set = function (key, value) {
        //键已存在值后不准重新赋值
        //                if (this.data[key] == null) {
        //                    this.data[key] = value;
        //                }
        this.data[key] = value;
    };

    this.get = function (key) {
        return this.data[key];
    };

    this.remove = function (key) {
        this.data[key] = null;
    };

    this.removeall = function () {
        this.data.length = 0;
    };

    this.isEmpty = function () {
        return this.data.length == 0;
    };

    this.size = function () {
        return this.data.length;
    };
}

//#endregion

//#region  扩展方法
//清除字符串所有空白
String.prototype.ResetBlank = function () {

    var regEx = /\s+/g;
    return this.replace(regEx, '');
}


//去除前后空格
String.prototype.trim = function () {
    return this.replace(/^\s\s*/, '').replace(/\s\s*$/, '');
}


//自定义字符生成器
//测试
//var buffer = new StringBuffer();
//buffer.append("Hello ").append("javascript");
//var result = buffer.toString();
//console.log(result);
function StringBuffer() {
    this.__strings__ = new Array();
}
StringBuffer.prototype.append = function (str) {
    this.__strings__.push(str);
    return this;    //方便链式操作
}
StringBuffer.prototype.toString = function () {
    return this.__strings__.join("");
}
//#endregion
