$(function () {
    if ($.cookie("username") !== undefined) {
        $("#email").val($.cookie("username"));
        $("#rememberme").attr("checked", true);
    }
});

function LoginClick() {
    var oData = new FormData();
    var email = $("#email").val();
    var password = $("#password").val();
    var rememberMe = $("#rememberme").prop('checked');
    if (email === "") {
        //alert("Please enter Email");
        $("#errordivlogin").css("display", "block");
        $("#errormesslogin").text("Please enter username or email!");
        return;
    }
    if (password === "") {
        $("#errordivlogin").css("display", "block");
        $("#errormesslogin").text("Please enter password!");
        return;
    }
    oData.append("username", email);
    oData.append("password", password);
    oData.append("rememberMe", rememberMe);
    $.ajax({
        type: "POST",
        url: "../Login/Login",
        data: oData,
        contentType: false,
        processData: false,
        timeout: 100000,
        success: function (data) {
            var json = JSON.parse(data);
            if (json.Code === 200) {
                //window.location.href = "/";
                //return;
                //登录验证成功则发生验证码:
                $("#loginform").css("display","none");
                $("#verificationform").fadeIn();
                if (json.Message.indexOf('|') != -1) {
                    var useremail = json.Message.split('|')[0];
                    var userphone = json.Message.split('|')[1];
                    if (userphone.trim().length > 0) {
                        $("#smsid").css("display", "block");
                        $("#phonenumber").text("+" + userphone.substr(0, 3) + "****" + userphone.substr(7, userphone.length - 7));
                        $("#phonenumber").attr("title", userphone);
                        $("#verificationEmail").val(useremail);
                    }
                    else {
                        $("#smsid").css("display", "none");
                        $("#verificationEmail").val(useremail);
                        
                    }
                }
                else {
                    $("#smsid").css("display", "none");
                    $("#verificationEmail").val(json.Message);
                }
                //getVerification(email, useremail);
            } else {
                $("#errordivlogin").css("display", "block");
                $("#errormesslogin").text(json.Message);
                return;
            }
        }
    });
}


function LoginClickNotVerification() {
    var oData = new FormData();
    var email = $("#email").val();
    var password = $("#password").val();
    var rememberMe = $("#rememberme").prop('checked');
    if (email === "") {
        alert("Please enter Email");
        return;
    }
    if (password === "") {
        alert("Please enter Password");
        return;
    }
    oData.append("username", email);
    oData.append("password", password);
    oData.append("rememberMe", rememberMe);
    $.ajax({
        type: "POST",
        url: "../Login/LoginNotVerification",
        data: oData,
        contentType: false,
        processData: false,
        timeout: 100000,
        success: function (data) {
            var json = JSON.parse(data);
            if (json.Code === 200) {
                window.location.href = window.location.origin+ "/Purchase/PurchaseAdd";
            } else {
                alert(json.Message);
            }
        }
    });
}

function LoginClickOne() {
    var oData = new FormData();
    var email = $("#email").val();
    var password = $("#password").val();
    var rememberMe = $("#rememberme").prop('checked');
    if (email === "") {
        $("#errordivlogin").css("display", "block");
        $("#errormesslogin").text("Please enter username or email!");
        return;
    }
    if (password === "") {
        $("#errordivlogin").css("display", "block");
        $("#errormesslogin").text("Please enter password!");
        return;
    }
    oData.append("username", email);
    oData.append("password", password);
    oData.append("rememberMe", rememberMe);
    $.ajax({
        type: "POST",
        url: "../Login/Login",
        data: oData,
        contentType: false,
        processData: false,
        timeout: 100000,
        success: function (data) {
            var json = JSON.parse(data);
            if (json.Code === 200) {
                var useremail = email;
                var user = email;
                var verificationcode = "1";
                var rememberMe = $("#rememberme").prop('checked');


                //检查验证码是否正确及有效
                $.ajax({
                    type: "post",
                    url: "/Login/checkVerificationCode",
                    data: {
                        useremail: useremail,
                        user: user,
                        verificationcode: verificationcode,
                        password: password,
                        rememberMe: rememberMe
                    },
                    success: function (data) {
                        var json = JSON.parse(data);
                        if (json.Code == 200) {
                            window.location.href = "/";
                            return;
                        }
                        else {
                            $("#errordiv").css("display", "block");
                            $("#errormess").text(json.Message);
                            $("#erroricon").removeClass("text-info");
                            $("#errormess").removeClass("text-info");
                            $("#erroricon").removeClass("text-danger");
                            $("#errormess").removeClass("text-danger");
                            $("#erroricon").addClass("text-danger");
                            $("#errormess").addClass("text-danger");
                            return;
                        }
                    }
                });
            } else {
                $("#errordivlogin").css("display", "block");
                $("#errormesslogin").text(json.Message);
                return;
            }
        }
    });
}

$('#password').keyup((event) => {
    if (event.keyCode == 13) {
        LoginClickNotVerification();
    }
});
//20200803summer-add
function getVerification(user, useremail) {
    if ($.trim(useremail).length <= 0) {
        $("#errordiv").css("display", "block");
        $("#errormess").text("Email cannot be empty!");
        return false;
    }
    if (!isEmail(useremail)) {
        $("#errordiv").css("display", "block");
        $("#errormess").text("Email format error!");
        return false;
    }
    $("#verificationCode").focus();
    $("#sendEmailid").addClass("hidetext");
    //发生邮件给用户
    $.ajax({
        type: "post",
        url: "/Login/sentVerificationCode",
        data: {
            useremail: useremail,
            user: user
        },
        success: function (data) {
            var json = JSON.parse(data);
            if (json.Code != 200) {
                $("#errordiv").css("display", "block");
                $("#errormess").text(json.Message);
                return false;
            }
        }
    });    
    setTime($("#secondemail"));//开始倒计时

}
//60s倒计时实现逻辑
var countdown = 60;
function setTime(obj) {
    if (countdown == 0) {
        obj.text("");
        $("#sendEmailid").removeClass("hidetext")
        countdown = 60;//60秒过后button上的文字初始化,计时器初始化;
        return;
    }
    else {
        obj.text("(" + countdown + "s)");
        $("#sendEmailid").addClass("hidetext")
        countdown--;
    }
    setTimeout(function () { setTime(obj); }, 1000) //每1000毫秒执行一次
}
//20200825summer-add
function sentVeriftyToPhone(user, userphone) {
    $("#verificationCode").focus();
    if (userphone.length <= 0) {
        $("#errordiv").css("display", "block");
        $("#errormess").text("mobile number cannot be empty!");
        return false;
    }
    $("#sendSMSid").addClass("hidetext");
    //发生SMS给用户
    $.ajax({
        type: "post",
        url: "/Login/sentVerifyToPhoneSMS", /*HKSMSPROsentVerifyToSMS*/
        data: {
            user: user,
            userphone: userphone
        },
        success: function (data) {
            var json = JSON.parse(data);
            if (json.Code != 200) {
                $("#errordiv").css("display", "block");
                $("#errormess").text(json.Message);
            }
        }
    });
    setTimeSMS($("#second"));//开始倒计时
}
//60s倒计时实现逻辑
var countdownSMS = 60;
function setTimeSMS(obj) {
    if (countdownSMS == 0) {
        obj.text("");
        $("#sendSMSid").removeClass("hidetext")
        countdownSMS = 60;//60秒过后button上的文字初始化,计时器初始化;
        return;
    }
    else {
        obj.text("(" + countdownSMS + "s)");
        $("#sendSMSid").addClass("hidetext")
        countdownSMS--;
    }
    setTimeout(function () { setTimeSMS(obj); }, 1000) //每1000毫秒执行一次
}


function loginDone() {
    $("#errordiv").css("display", "none");
    var useremail = $("#verificationEmail").val();
    var user = $("#email").val();
    var verificationcode = $("#verificationCode").val();
    var passward = $("#password").val();
    var rememberMe = $("#rememberme").prop('checked');
    if ($.trim(useremail).length <= 0) {
        $("#verificationEmail").focus();
        return;
    }
    else if ($.trim(verificationcode).length<=0) {
        $("#verificationCode").focus();
        return;
    }
    //检查验证码是否正确及有效
    $.ajax({
        type: "post",
        url: "/Login/checkVerificationCode",
        data: {
            useremail: useremail,
            user: user,
            verificationcode: verificationcode,
            password: passward,
            rememberMe: rememberMe
        },
        success: function (data) {
            var json = JSON.parse(data);
            if (json.Code == 200)
            {
                window.location.href = "/";
                return;
            }
            else {
                $("#errordiv").css("display", "block");
                $("#errormess").text(json.Message);
                $("#erroricon").removeClass("text-info");
                $("#errormess").removeClass("text-info");
                $("#erroricon").removeClass("text-danger");
                $("#errormess").removeClass("text-danger");
                $("#erroricon").addClass("text-danger");
                $("#errormess").addClass("text-danger");
                return;
            }
        }
    });
   
}
function isEmail(str) {
    var reg = /^[A-Za-z0-9\u4e00-\u9fa5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$/;
    return reg.test(str);
}

