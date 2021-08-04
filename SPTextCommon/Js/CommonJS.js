
/************************ 用户在button以外的页面元素中按回车转换为按tab键************************/
function OnKeyDownDefault()  
{  
    if(window.event.keyCode == 13 && window.event.ctrlKey == false && window.event.altKey == false) 
    { 
        if (window.event.srcElement.type !='button' && window.event.srcElement.type !='file' && window.event.srcElement.type !='submit') 
        { 
            window.event.keyCode = 9;
        }
        else
        { 
            return true;
        } 
    } 
}

/************************ 在隐藏控件中保存Select选中的Value ************************/
function SetHiddenValue(obj , hdnObj)
{
    hdnObj.value = obj.options[obj.selectedIndex].value;
}

/************************ 在隐藏控件中保存Select选中的Text ************************/
function SetHiddenText(obj, hdnObj) {
    hdnObj.value = obj.options[obj.selectedIndex].text;
}

/************************ 在隐藏控件中保存Radio选中的Text,RadioList中每个Radio均调用此方法 ************************/
function SetRadioHiddenValue( checkedText, hdnObj)
{
    hdnObj.value = checkedText ;
}

/************************ 在隐藏控件中保存Select选中的Text ************************/
var oldValue = '';
function GetFocusValue(objControl)
{
    oldValue = objControl.value;
}

/************************ 去掉原值检查新输入值的JS ************************/
function RemoveOldValue( objControl, checkValue)
{
    var newValue = '';
    if( objControl.value.search (checkValue) == 0)
    {
        //当在原值基础上输入时,从开始处去掉原值部分,仅检查新输入内容
        newValue = objControl.value.substring(checkValue.length);
    }
    else
    {
        newValue = objControl.value;
    }
    return newValue;
}

/************************ 生成检查输入值是否重复的脚本函数 ************************/
function CheckRepate(infields,invals,inQuotes,strBackId) 
{ 
    var feildName = 'feildName=';
    var inputVal = 'values='; 
    var haveQuotes = 'quotes=';
    var infieldsAry = infields.split(',');
    var invalsAry = invals.split(',');
    var inQuotesAry = inQuotes.split(',');
    for(var i=0; i<infieldsAry.length; i++ )
    { 
        feildName = feildName+ infieldsAry[i] +','; 
        inputVal = inputVal  + escape(document.getElementById(invalsAry[i]).value)+ ','; 
        haveQuotes = haveQuotes+ inQuotesAry[i] +','; 
    } 
    feildName=feildName.substr(0, feildName.length-1); 
    inputVal=inputVal.substr(0, inputVal.length-1); 
    haveQuotes=haveQuotes.substr(0, haveQuotes.length-1); 
    var strData = feildName+'&'+inputVal+'&'+haveQuotes;
    var cbo = new CallBackObject(strBackId);
    cbo.DoCallBack('',strData );
} 

/***************************从Cookie中取值****************************************/
function Get_Cookie(check_name) {
    var a_all_cookies = document.cookie.split(';');
    var a_temp_cookie = '';
    var cookie_name = '';
    var cookie_value = '';
    var b_cookie_found = false;
    var i = '';

    for (i = 0; i < a_all_cookies.length; i++) {
        a_temp_cookie = a_all_cookies[i].split('=');
        cookie_name = a_temp_cookie[0].replace(/^\s+|\s+$/g, '');
        if (cookie_name == check_name) {
            b_cookie_found = true;
            if (a_temp_cookie.length > 1) {
                cookie_value = unescape(a_temp_cookie[1].replace(/^\s+|\s+$/g, ''));
            }
            return cookie_value;
            break;
        }
        a_temp_cookie = null;
        cookie_name = '';
    }
    if (!b_cookie_found) {
        return null;
    }
}

/******************保存值到Cookie中**************************/
function Set_Cookie(name, value, expires, path, domain, secure) {
    var today = new Date();
    today.setTime(today.getTime());
    if (expires) {
        expires = expires * 1000 * 60 * 60 * 24;
    }
    var expires_date = new Date(today.getTime() + (expires));
    document.cookie = name + "=" + escape(value) +
		((expires) ? ";expires=" + expires_date.toGMTString() : "") + //expires.toGMTString()
		((path) ? ";path=" + path : "") +
		((domain) ? ";domain=" + domain : "") +
		((secure) ? ";secure" : "");
}

/********************删除Cookie*******************************/
function Delete_Cookie(name, path, domain) {
    if (Get_Cookie(name)) document.cookie = name + "=" +
			((path) ? ";path=" + path : "") +
			((domain) ? ";domain=" + domain : "") +
			";expires=Thu, 01-Jan-1970 00:00:01 GMT";
}

//功能：去掉字符串两边空格
//返回：true ---- 包含此不合法字符  false ---- 不包含
function TrimString(str) {
    var i, j;
    if (str == "") return "";
    for (i = 0; i < str.length; i++)
        if (str.charAt(i) != ' ') break;
    if (i >= str.length) return "";

    for (j = str.length - 1; j >= 0; j--)
        if (str.charAt(j) != ' ') break;

    return str.substring(i, j + 1);
}

//--除去前空白符
function Ltrim(str) {
    return str.replace(/^\s+/, "");
}

//--除去后空白符
function Rtrim(str) {
    return str.replace(/\s+$/, "");
}

//--除去前后空白符
function Trim(str) {
    return Ltrim(Rtrim(str));
}

//--人民币转化为大写
function convertCurrency(currencyDigits) {
    // Constants: 
    var MAXIMUM_NUMBER = 99999999999.99;
    // Predefine the radix characters and currency symbols for output: 
    var CN_ZERO = "零";
    var CN_ONE = "壹";
    var CN_TWO = "贰";
    var CN_THREE = "叁";
    var CN_FOUR = "肆";
    var CN_FIVE = "伍";
    var CN_SIX = "陆";
    var CN_SEVEN = "柒";
    var CN_EIGHT = "捌";
    var CN_NINE = "玖";
    var CN_TEN = "拾";
    var CN_HUNDRED = "佰";
    var CN_THOUSAND = "仟";
    var CN_TEN_THOUSAND = "万";
    var CN_HUNDRED_MILLION = "亿";
    var CN_SYMBOL = "人民币";
    var CN_DOLLAR = "元";
    var CN_TEN_CENT = "角";
    var CN_CENT = "分";
    var CN_INTEGER = "整";

    // Variables: 
    var integral;    // Represent integral part of digit number. 
    var decimal;    // Represent decimal part of digit number. 
    var outputCharacters;    // The output result. 
    var parts;
    var digits, radices, bigRadices, decimals;
    var zeroCount;
    var i, p, d;
    var quotient, modulus;

    // Validate input string: 
    currencyDigits = currencyDigits.toString();
    if (currencyDigits == "") {
        alert("请输入小写金额！");
        return "";
    }
    if (currencyDigits.match(/[^,.\d]/) != null) {
        alert("小写金额含有无效字符！");
        return "";
    }
    if ((currencyDigits).match(/^((\d{1,3}(,\d{3})*(.((\d{3},)*\d{1,3}))?)|(\d+(.\d+)?))$/) == null) {
        alert("小写金额的格式不正确！");
        return "";
    }

    // Normalize the format of input digits: 
    currencyDigits = currencyDigits.replace(/,/g, "");    // Remove comma delimiters. 
    currencyDigits = currencyDigits.replace(/^0+/, "");    // Trim zeros at the beginning. 
    // Assert the number is not greater than the maximum number. 
    if (Number(currencyDigits) > MAXIMUM_NUMBER) {
        alert("金额过大，应小于1000亿元！");
        return "";
    }

    // Process the coversion from currency digits to characters: 
    // Separate integral and decimal parts before processing coversion: 
    parts = currencyDigits.split(".");
    if (parts.length > 1) {
        integral = parts[0];
        decimal = parts[1];
        // Cut down redundant decimal digits that are after the second. 
        decimal = decimal.substr(0, 2);
    }
    else {
        integral = parts[0];
        decimal = "";
    }
    // Prepare the characters corresponding to the digits: 
    digits = new Array(CN_ZERO, CN_ONE, CN_TWO, CN_THREE, CN_FOUR, CN_FIVE, CN_SIX, CN_SEVEN, CN_EIGHT, CN_NINE);
    radices = new Array("", CN_TEN, CN_HUNDRED, CN_THOUSAND);
    bigRadices = new Array("", CN_TEN_THOUSAND, CN_HUNDRED_MILLION);
    decimals = new Array(CN_TEN_CENT, CN_CENT);
    // Start processing: 
    outputCharacters = "";
    // Process integral part if it is larger than 0: 
    if (Number(integral) > 0) {
        zeroCount = 0;
        for (i = 0; i < integral.length; i++) {
            p = integral.length - i - 1;
            d = integral.substr(i, 1);
            quotient = p / 4;
            modulus = p % 4;
            if (d == "0") {
                zeroCount++;
            }
            else {
                if (zeroCount > 0) {
                    outputCharacters += digits[0];
                }
                zeroCount = 0;
                outputCharacters += digits[Number(d)] + radices[modulus];
            }
            if (modulus == 0 && zeroCount < 4) {
                outputCharacters += bigRadices[quotient];
                zeroCount = 0;
            }
        }
        outputCharacters += CN_DOLLAR;
    }
    // Process decimal part if there is: 
    if (decimal != "") {
        for (i = 0; i < decimal.length; i++) {
            d = decimal.substr(i, 1);
            if (d != "0") {
                outputCharacters += digits[Number(d)] + decimals[i];
            }
        }
    }
    // Confirm and return the final output string: 
    if (outputCharacters == "") {
        outputCharacters = CN_ZERO + CN_DOLLAR;
    }
    if (decimal == "") {
        outputCharacters += CN_INTEGER;
    }
    outputCharacters = CN_SYMBOL + outputCharacters;
    return outputCharacters;
} 

//--汉字转化为拼音
function pinyin(cc) {
    var str = '';
    var s;
    for (var i = 0; i < cc.length; i++) {
        //alert(cc.charAt(i)+" = "+cc.charCodeAt(i));
        if (sd.indexOf(cc.charAt(i)) != -1 && cc.charCodeAt(i) > 200) {
            s = 1;
            while (sd.charAt(sd.indexOf(cc.charAt(i)) + s) != ",") {
                str += sd.charAt(sd.indexOf(cc.charAt(i)) + s);
                s++;
            }

            str += " ";
        }
        else {
            str += cc.charAt(i);
        }
    }
    return str;
}