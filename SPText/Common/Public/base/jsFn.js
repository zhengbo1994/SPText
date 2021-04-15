(function () {
    //判断是否为空 
    var isNull = function (obj) {
        var result = false

        if (typeof (obj) == "undefined") {
            result = true;
        }

        if (obj == null) {
            result = true;
        }

        if (typeof (obj) == "string" && obj == "") {
            result = true;
        }

        return result;
    }

    //ajax统一请求接口
    var ajaxRequest = function (opts) {
        if (false == opts.async) {
            $("body").append('<div class="widget-box-overlay"><i class=" ace-icon loading-icon fa fa-spinner fa-spin fa-2x white"></i></div>');
        };
        var options;
        options = {
            type: "post",
            async: true,
            success: function (data, status, response) {
                $("body > div.widget-box-overlay").remove();
                if (!isNull(opts.success)) {
                    return opts.success(data);
                }
            },
            error: function (data, status, response) {
                $("body > div.widget-box-overlay").remove();
                if (!isNull(opts.error)) {
                    return opts.error(data, status, response);
                }
            }
        };
        if (!isNull(opts.url)) {
            options.url = opts.url;
        }
        if (!isNull(opts.type)) {
            options.type = opts.type;
        }
        if (!isNull(opts.async)) {
            options.async = opts.async;
        }
        if (!isNull(opts.data)) {
            options.data = opts.data;
        }
        if (!isNull(opts.dataType)) {
            options.dataType = opts.dataType;
        }
        if (!isNull(opts.contentType)) {
            options.contentType = opts.contentType;
        }
        if (!isNull(opts.processData)) {
            options.processData = opts.processData;
        }
        if (!isNull(opts.crossDomain)) {
            options.crossDomain = opts.crossDomain;
        }
        return $.ajax(options);
    };

    //创建GUID
    var newGuid = function () {
        var i, n;
        return ((function () {
            var j, results;
            results = [];
            for (i = j = 1; j <= 32; i = ++j) {
                n = Math.floor(Math.random() * 16.0).toString(16);
                if ((i === 8) || (i === 12) || (i === 16) || (i === 20)) {
                    results.push(n += "-");
                } else {
                    results.push(n);
                }
            }
            return results;
        })()).join("");
    };

    ///页面跳转
    var locationHref = function (url) {
        window.location.href = url;;
    }

    var arrContext = function (arr, key, value) {
        var result = -1;
        for (var i = 0, len = arr.length; i < len; i++) {
            var obj = arr[i]
            for (var objKey in obj) {
                if (objKey == key && obj[objKey] == value) {
                    result = i;
                    break;
                }
            }
        }
        return result;
    }

    //把字符串按分隔符拆分成数组
    var split = function (str, separator) {
        var arr = str.split(separator);
        return arr;
    }

    //生成年月日
    var formatDateString = function (strDate) {
        var date = new Date(strDate);
        var year = date.getFullYear().toString();
        var month = date.getMonth().toString().length == 1 ? "0" + (date.getMonth() + 1).toString() : date.getMonth().toString();
        var day = date.getDate().toString().length == 1 ? "0" + date.getDate().toString() : date.getDate().toString();
        return year + "-" + month + "-" + day
    }

    var abortThread = function (message) {
        alert(message);
        throw (message);
    }

    Array.prototype.remove = function (rowid) {
        if (isNull(rowid) || rowid > this.length) {
            return false;
        }
        for (var i = 0, n = 0, len = this.length; i < len; i++) {
            if (this[i] != this[rowid]) {
                this[n++] = this[i]
            }
        }
        this.length -= 1

    }

    //格式化日期字符串
    Date.prototype.toFormatString = function (format) {
        var date0 = this;
        if (!format) {
            return date0.toString();
        }
        var regYear = /y{4}/g;
        var regMonth = /M{2}/g;
        var regDay = /d{2}/g;
        var regHour = /H{2}/g;
        var regMinute = /m{2}/g;
        var regSecond = /s{2}/g;

        var strYear = date0.getFullYear().toString();
        var strMonth = (date0.getMonth() + 1).toString();
        var strDay = date0.getDate().toString();
        var strHour = date0.getHours().toString();
        var strMinute = date0.getMinutes().toString();
        var strSecond = date0.getSeconds().toString();

        strMonth = "00" + strMonth;
        strMonth = strMonth.substr(-2, 2);
        strDay = "00" + strDay;
        strDay = strDay.substr(-2, 2);
        strHour = "00" + strHour;
        strHour = strHour.substr(-2, 2);
        strMinute = "00" + strMinute;
        strMinute = strMinute.substr(-2, 2);
        strSecond = "00" + strSecond;
        strSecond = strSecond.substr(-2, 2);

        format = format.replace(regYear, strYear)
            .replace(regMonth, strMonth)
            .replace(regDay, strDay)
            .replace(regHour, strHour)
            .replace(regMinute, strMinute)
            .replace(regSecond, strSecond);
        return format;
    };

    Date.prototype.addSeconds = function (seconds) {
        var date0 = this;
        date0.setDate(date0.getSeconds() + days);
        return date0;
    };

    Date.prototype.addMinutes = function (minutes) {
        var date0 = this;
        date0.setDate(date0.getMinutes() + minutes);
        return date0;
    };

    Date.prototype.addHours = function (hours) {
        var date0 = this;
        date0.setDate(date0.getHours() + hours);
        return date0;
    };

    Date.prototype.addDays = function (days) {
        var date0 = this;
        date0.setDate(date0.getDate() + days);
        return date0;
    };

    Date.prototype.addMonths = function (months) {
        var date0 = this;
        date0.setMonth(date0.getMonth() + months);
        return date0;
    };

    Date.prototype.addYears = function (years) {
        var date0 = this;
        date0.setFullYear(date0.getFullYear() + years);
        return date0;
    };

    var isArray = function (obj) {
        return Object.prototype.toString.call(obj) === '[object Array]';
    }

    //获取当前容器内有name属性的控件值，返回一个对象。spliter:同名控件值的分隔符
    var getJson = function ($container, spliter) {
        spliter = spliter || "||";
        var jsonNameValue = {};
        var appendJsonData = function (jdata, name, value) {
            if ("" == name || undefined == name) {
                return jdata;
            }
            if (undefined != jdata[$.trim(name)]) {
                jdata[name] += spliter + $.trim(value);
            } else {
                jdata[$.trim(name)] = $.trim(value);
            }
            return jdata;
        };

        var simpleControls = $container.find("input[type='text'],input[type='hidden'],input[type='password'],textarea,select");
        for (var i = 0; i < simpleControls.length; i++) {
            var c = $(simpleControls[i]);
            var controlName = c.attr("name");
            appendJsonData(jsonNameValue, controlName, c.val());
        }

        //var checkboxControls = $container.find("input[type='checkbox']");
        //for (var i = 0; i < checkboxControls.length; i++) {
        //    var c = $(checkboxControls[i]);
        //    var controlName = c.attr("name");
        //    appendJsonData(jsonNameValue, controlName, c.prop("checked"));
        //}

        var radioControls = $container.find("input[type='radio'],input[type='checkbox']");
        for (var i = 0; i < radioControls.length; i++) {
            var c = $(radioControls[i]);
            var controlName = c.attr("name");
            if (c.is(":checked")) {
                appendJsonData(jsonNameValue, controlName, c.val());
            }
        }

        var fileControls = $container.find("input[type='file']");
        for (var i = 0; i < fileControls.length; i++) {
            var c = $(fileControls[i]);
            var controlName = c.attr("name");
            if (c[0].files.length > 0) {
                //appendJsonData(jsonNameValue, controlName, c[0].files[0]);
                //jsonNameValue[controlName] = c[0].files[0];
                if (jsonNameValue.hasOwnProperty(controlName) && !isArray(jsonNameValue[controlName])) {
                    var oldvalue = jsonNameValue[controlName];
                    jsonNameValue[controlName] = [];
                    jsonNameValue[controlName].push(oldvalue);
                }
                if (isArray(jsonNameValue[controlName])) {
                    jsonNameValue[controlName].push(c[0].files[0]);
                }
                else {
                    jsonNameValue[controlName] = c[0].files[0];
                }
            }
        }
        return jsonNameValue;
    }
    //获取当前容器内有name属性的控件值，返回一个对象。同名对象用List接收 只能在支持HTML5的浏览器使用
    var getForm = function ($container) {
        var jsondata = getJson($container);
        var formData = new FormData();
        for (var p in jsondata) {
            var pItem = jsondata[p];
            if (isArray(pItem)) {
                for (var i = 0; i < pItem.length; i++) {
                    formData.append(p, pItem[i]);
                }
            }
            else {
                formData.append(p, pItem || "");
            }
        }
        return formData;
    }
    //将json数据赋给某容器内对应name属性的控件。spliter:同名控件值的分隔符
    var setJson = function ($container, jdata, spliter) {
        spliter = spliter || "||";
        for (var p in jdata) {
            jdata[p] = $.trim(jdata[p]);
            var $c = $container.find("[name='" + p + "']");
            if ($c.length > 0) {
                if (false === jdata[p] || "false" == jdata[p].toLowerCase()) {
                    jdata[p] = "False";
                } else if (true === jdata[p] || "true" == jdata[p].toLowerCase()) {
                    jdata[p] = "True";
                }
                var cName = $c[0].tagName.toLowerCase() || $c[0].nodeName.toLowerCase() || $c[0].localName;
                var cType = $c[0].type.toLowerCase();
                if (("input" == cName && ("text" == cType || "hidden" == cType || "password" == cType))
                    || "textarea" == cName || "select" == cName) {
                    $c.val(jdata[p]);
                } else if ("input" == cName && "checkbox" == cType) {
                    //$c.prop("checked", jdata[p]);
                    var chkValue = jdata[p];
                    $c.prop("checked", false);
                    if (!chkValue) {
                        return false;
                    }
                    var arrChkValue = chkValue.split(spliter);
                    for (var i = 0; i < arrChkValue.length; i++) {
                        for (var k = 0; k < $c.length; k++) {
                            if ($c[k].value == arrChkValue[i]) {
                                $($c[k]).prop("checked", true);
                                break;
                            }
                        }
                        //$container.find("input[type='checkbox'][name='" + p + "'][value='" + arrChkValue[i] + "']").prop("checked", true);
                    }
                } else if ("input" == cName && "radio" == cType) {
                    for (var i = 0; i < $c.length; i++) {
                        if ($c[i].value == jdata[p]) {
                            $($c[i]).prop("checked", true);
                            break;
                        }
                    }
                } else if ("div" == cName) {
                    setJson($c, jdata);
                }

                var onchangeFlag = $c.data("event-change")
                if (onchangeFlag) {
                    $c.change();
                }

            }
        }
    }
    //获取地址栏参数
    var getQueryString = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }



    //获取分页信息
    var getpage = function (tatolCount, intThisPage, $tbody, $navStr) {
        ////总数据数量
        //var tatolCount = 0;
        ////当前页数
        //var intThisPage = parseInt(1);


        var intCount = tatolCount; //总记录数
        var intPageSize = 10; //每页显示
        var intPageCount = intCount % intPageSize == 0 ? parseInt(intCount / intPageSize) : parseInt(intCount / intPageSize) + 1; //总共页数
        var intPage = 7; //数字显示
        var intBeginPage = 0; //开始页数
        var intCrossPage = 0; //变换页数
        var intEndPage = 0; //结束页数
        var strPage = ""; //返回值

        intCrossPage = parseInt(intPage / 2);
        strPage = "共 <font color=\"#056dae\">" + intCount + "</font> 条记录 第 <font color=\"#056dae\">" + intThisPage + "/" + intPageCount + "</font> 页 每页 <font color=\"#056dae\">" + intPageSize + "</font> 条 &nbsp;&nbsp;&nbsp;&nbsp;";
        if (intThisPage > 1) {
            strPage = strPage + "<a href=\"#\" style=\"margin-left: 4px; margin-right: 4px;\" onClick=\"getpage(" + (tatolCount) + "," + (1) + ")\">首页</a> ";
            strPage = strPage + "<a href=\"#\" style=\"margin-left: 4px; margin-right: 4px;\" onClick=\"getpage(" + (tatolCount) + "," + (intThisPage - 1) + ")\">上一页</a> ";
        }
        if (intPageCount > intPage) {
            if (intThisPage > intPageCount - intCrossPage) {
                intBeginPage = intPageCount - intPage + 1;
                intEndPage = intPageCount;
            }
            else {
                if (intThisPage <= intPage - intCrossPage) {
                    intBeginPage = 1;
                    intEndPage = intPage;
                }
                else {
                    intBeginPage = intThisPage - intCrossPage;
                    intEndPage = intThisPage + intCrossPage;
                }
            }
        }
        else {
            intBeginPage = 1;
            intEndPage = intPageCount;
        }
        if (intCount > 0) {
            for (var i = intBeginPage; i <= intEndPage; i++) {
                if (i == intThisPage) {
                    strPage = strPage + "<a href=\"#\" onClick=\"getpage(" + (tatolCount) + "," + (i) + ")\"> <font color=\"#056dae\">" + i + "</font> </a>";
                }
                else {
                    strPage = strPage + "<a href=\"#\" onClick=\"getpage(" + (tatolCount) + "," + (i) + ")\"> <font>" + i + "</font> </a>";
                }
            }
        }
        if (intThisPage < intPageCount) {
            strPage = strPage + "<a href=\"#\" style=\"margin-left: 4px; margin-right: 4px;\" onClick=\"getpage(" + (tatolCount) + "," + (intThisPage + 1) + ")\">下一页</a>";
            strPage = strPage + "<a href=\"#\" style=\"margin-left: 4px; margin-right: 4px;\" onClick=\"getpage(" + (tatolCount) + "," + (intPageCount) + ")\">尾页</a>";
        }

        //var itable = document.getElementById("tbody");
        var itable = $tbody[0];
        var num = itable.rows.length;//表格所有行数(所有记录数)
        for (var i = 1; i < (num + 1); i++) {
            var irow = itable.rows[i - 1];
            if (i > (intThisPage - 1) * intPageSize && i < intThisPage * intPageSize + 1) {
                irow.style.display = "table-row";
            } else {
                irow.style.display = "none";
            }
        }

        $navStr.html(strPage);
        //$("#navStr").html(strPage);
    }

    // 定义一个深拷贝函数  接收目标target参数
    var deepClone = function (target) {
        // 定义一个变量
        let result;
        // 如果当前需要深拷贝的是一个对象的话
        if (typeof target === 'object') {
            // 如果是一个数组的话
            if (Array.isArray(target)) {
                result = []; // 将result赋值为一个数组，并且执行遍历
                for (let i in target) {
                    // 递归克隆数组中的每一项
                    result.push(deepClone(target[i]))
                }
                // 判断如果当前的值是null的话；直接赋值为null
            } else if (target === null) {
                result = null;
                // 判断如果当前的值是一个RegExp对象的话，直接赋值    
            } else if (target.constructor === RegExp) {
                result = target;
            } else {
                // 否则是普通对象，直接for in循环，递归赋值对象的所有值
                result = {};
                for (let i in target) {
                    result[i] = deepClone(target[i]);
                }
            }
            // 如果不是对象的话，就是基本数据类型，那么直接赋值
        } else {
            result = target;
        }
        // 返回最终结果
        return result;
    }

    var deepCloneConversion = function deepClone() {
        return JSON.parse(JSON.stringify(obj))
    }

    var deepPrototypeClone = function deepClone(obj) {
        function isClass(o) {
            if (o === null) return "Null";
            if (o === undefined) return "Undefined";
            return Object.prototype.toString.call(o).slice(8, -1);
        }
        var result;
        var oClass = isClass(obj);
        if (oClass === "Object") {
            result = {};
        } else if (oClass === "Array") {
            result = [];
        } else {
            return obj;
        }
        for (var key in obj) {
            var copy = obj[key];
            if (isClass(copy) == "Object") {
                result[key] = arguments.callee(copy);//递归调用    
            } else if (isClass(copy) == "Array") {
                result[key] = arguments.callee(copy);
            } else {
                result[key] = obj[key];
            }
        }
        return result;
    }

    //cache 缓存数组
    var deepCopy = function deepCopy(obj, cache = []) {
        if (obj === null || typeof obj !== 'object') {
            return obj
        }

        const hit = cache.filter(c => c.original === obj)[0]
        if (hit) {
            return hit.copy
        }

        const copy = Array.isArray(obj) ? [] : {}
        cache.push({
            original: obj,
            copy
        })
        Object.keys(obj).forEach(key => {
            copy[key] = deepCopy(obj[key], cache)
        })

        return copy
    }

    //防抖
    function debounce(func, wait) {
        let timeout;
        return function () {
            let context = this;
            let args = arguments;

            if (timeout) clearTimeout(timeout);

            timeout = setTimeout(() => {
                func.apply(context, args)
            }, wait);
        }
    }

    //节流
    function throttle(func, wait) {
        let previous = 0;
        return function () {
            let now = Date.now();
            let context = this;
            let args = arguments;
            if (now - previous > wait) {
                func.apply(context, args);
                previous = now;
            }
        }
    }

    //判断小数是否相等
    function epsEqu(x, y) {
        return Math.abs(x - y) < Math.pow(2, -52);
    }

    function getCookie(name) {
        var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");

        if (arr = document.cookie.match(reg))

            return unescape(arr[2]);
        else
            return null;
    }

    function setCookie(name, value) {
        var Days = 30;
        var exp = new Date();
        exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
        document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
    }

    //删除cookies
    function delCookie(name) {
        var exp = new Date();
        exp.setTime(exp.getTime() - 1);
        var cval = getCookie(name);
        if (cval != null)
            document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
    }

    this.getQueryString = getQueryString;//获取地址栏参数
    this.isNull = isNull;//判断是否为空 
    this.ajaxRequest = ajaxRequest;//ajax统一请求接口
    this.newGuid = newGuid;//创建GUID
    this.arrContext = arrContext;//匹配数组，返回索引
    this.split = split;//分割
    this.locationHref = locationHref;//界面跳转
    this.formatDateString = formatDateString;//年-月-日
    this.getJson = getJson; //获取当前容器内有name属性的控件值，返回一个对象
    this.getForm = getForm;//获取当前容器内有name属性的控件值，返回一个对象
    this.setJson = setJson;//讲json值赋给容器
    this.isArray = isArray;//是数组
    this.abortThread = abortThread;//终止进程
    this.deepClone = deepClone;//深拷贝（自定义）
    this.deepPrototypeClone = deepPrototypeClone;//深拷贝（递归）
    this.deepCloneConversion = deepCloneConversion;//深拷贝（序列化方式）
    this.debounce = debounce;//防抖
    this.throttle = throttle;//节流
    this.epsEqu = epsEqu;//判断小数是否相等
    this.deepCopy = deepCopy;//数据缓存
    this.getpage = getpage;//获取分页
    this.getCookie = getCookie;//获取缓存
    this.setCookie = setCookie;//设置缓存
    this.delCookie = delCookie;//删除缓存
}).call(this)
//1、find 查询数组中符合条件的第一个元素，如果没有符合条件的元素则返回undefined
//var dogs = arr.find(v => v === 4);
//2、filter 过滤数组元素，返回过滤后的数组，如果没有符合条件的元素则返回空数组
//var ar = arr.filter(v => v > 5);
//3、map 对每个数组元素执行相同操作，返回执行后的新数组
//var tr = arr.map(v => v + 1);
//4、splice 删除元素
//var dogs = arr.splice(1, 1);