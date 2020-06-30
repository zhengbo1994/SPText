$(function () {

    var controllerName = "Login";

    var initLogin = function () {
        var initLoginButton = function () {
            var $loginInfo = $('#divLogin_AccountInfo');
            $("#btnLogin").on("click", function () {
                var loginData = getJson($loginInfo);
                if (loginData.password) {
                    loginData.password = hex_md5(loginData.password);
                }
                else {
                    alert("请输入密码！");
                    return false;
                }
                ajaxRequest({
                    url: "/" + controllerName + "/UserLogin",
                    data: loginData,
                    datatype: "string",
                    async: false,
                    cache: false,
                    success: function (jdata) {
                        if (jdata) {
                            if (jdata.IsSuccess === true) {
                                window.location.href = "home";
                                return true;

                            } else {
                                alert(jdata.ErrorMessage);
                                return false;
                            }
                        }
                    }
                });
            });

            //登陆框获取焦点
            $loginInfo.find("[name='loginname']").focus();
        }

        document.onkeydown = function (event) {

            var e = event || window.event || arguments.callee.caller.arguments[0];

            if (e && e.keyCode == 27) { // 按 Esc 
                //要做的事情
            }
            if (e && e.keyCode == 113) { // 按 F2 
                //要做的事情
            }
            if (e && e.keyCode == 13) { // enter 键
                //要做的事情
                $("#btnLogin").click();
            }
        }

        initLoginButton();
    }

    //页面加载时运行
    $(document).ready(function() {
        initLogin();
    });
})
