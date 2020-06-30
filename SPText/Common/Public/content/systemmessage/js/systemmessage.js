'use strict'
$(function() {
    var controllerName = "SystemMessage";
    //系统消息
    var $divSystemMessageQueryArea = $("#divSystemMessage_QueryArea");
    var $gridSystemMessageMain = $("#gridSystemMessage_Main");
    var $pagerSystemMessageMain = $("#pagerSystemMessage_Main");
    var $mdlSystemMessageInfo = $("#mdlSystemMessage_Info");
    var $divSystemMessageInfo = $("#divSystemMessage_Info");
    //用户信息
    var $mdlUserInfo = $("#mdlUser_Info");
    var $divUserQueryArea = $("#divUser_QueryArea");
    var $gridUserMain = $("#gridUser_Main");
    var $pagerUserMain = $("#pagerUser_Main");

    var enum_EditTypes = { insert: 0, update: 1, view: 2 }
    var currentEditType;
    var systemMessageId = 0;


    var getSystemMessageGridSelectedRowData = function($gridSystemMessageMain, noSelectionCallback) {
        var rowId = $gridSystemMessageMain.jqGrid("getGridParam", "selrow");
        if (!rowId) {
            if ("function" == typeof (noSelectionCallback)) {
                noSelectionCallback();
            }
            return undefined;
        }
        var rowData = $gridSystemMessageMain.jqGrid("getRowData", rowId);
        return rowData;
    }

    var getUserGridSelectedRowData = function($gridUserMain, noSelectionCallback) {
        var rowId = $gridUserMain.jqGrid("getGridParam", "selrow");
        if (!rowId) {
            if ("function" == typeof (noSelectionCallback)) {
                noSelectionCallback();
            }
            return undefined;
        }
        var rowData = $gridUserMain.jqGrid("getRowData", rowId);
        return rowData;
    }

    //添加用户
    var chooseUser = function() {
        var rowData = getUserGridSelectedRowData($gridUserMain, function() {
            alert("请选择数据！");
        });
        if (rowData == undefined) {
            return false;
        }
      
        $mdlUserInfo.modal("toggle");
        $mdlSystemMessageInfo.modal("show");

        $divSystemMessageInfo.find("[name='ReceiveUserId']").val(rowData.UserId);
        $divSystemMessageInfo.find("[name='ReceiveUserName']").val(rowData.UserName);

    }


    var initQueryArea = function() {

        var initSystemMessageQuery = function() {
            var initQueryButton = function() {
                $("#btnSystemMessage_Query").on("click", function() {
                    var queryData = {};
                    var divQueryArea = $divSystemMessageQueryArea;
                    queryData = getJson(divQueryArea);
                    $gridSystemMessageMain.jqGrid("setGridParam", { postData: queryData }).trigger("reloadGrid");
                });
            }

            initQueryButton();
        }

        var initUserQuery = function() {
            var initQueryButton = function() {
                $("#btnUser_Query").on("click", function() {
                    var queryData = {};
                    var divQueryArea = $divUserQueryArea;
                    queryData = getJson(divQueryArea);
                    $gridUserMain.jqGrid("setGridParam", { postData: queryData }).trigger("reloadGrid");
                });
            }

            initQueryButton();
        }

        initSystemMessageQuery();
        initUserQuery();
    }

    var initJqGrid = function() {

        var initSystemMessageGrid = function() {
            var $queryData = {};
            $queryData = getJson($divSystemMessageQueryArea);
            $gridSystemMessageMain.jqGrid({
                url: "/" + controllerName + "/GetSystemMessageInfoListForJqgrid",
                datatype: "json",
                postData: $queryData,
                colNames: ["SystemMessageId", "ReceiveUserId", "接收用户", "标题", "内容", "阅览状态", "阅览日期", "发送时间"],
                colModel: [
                    { name: "SystemMessageId", index: "SystemMessageId", width: 10, hidden: true },
                    { name: "ReceiveUserId", index: "ReceiveUserId", align: "center", width: 10, hidden: true },
                    { name: "ReceiveUserName", index: "ReceiveUserName", align: "center", width: 150 },
                    { name: "Title", index: "Title", align: "center", width: 100 },
                    { name: "Content", index: "Content", align: "center", width: 300 },
                    { name: "Read", index: "Read", align: "center", width: 100 },
                    { name: "ReadDate", index: "ReadDate", align: "center", width: 150 },
                    { name: "CreateDate", index: "CreateDate", align: "center", width: 150 }
                ],
                multiselect: false,
                autowidth: false,
                rowNum: 20,
                altRows: true,
                pgbuttons: true,
                viewrecords: true,
                shrinkToFit: false,
                pginput: true,
                rowList: [10, 20, 30, 50, 70, 100],
                pager: $pagerSystemMessageMain,
                loadComplete: function() {
                    var table = this;
                    updatePagerIcons(table);
                    jqGridAutoWidth();
                    setGridHeight($gridSystemMessageMain.selector);
                }
            });
        }

        var initUserGrid = function() {
            var $queryData = {};
            $queryData = getJson($divUserQueryArea);
            $gridUserMain.jqGrid({
                url: "/" + controllerName + "/GetUserInfoListForJqgrid",
                datatype: "json",
                postData: $queryData,
                colNames: ["UserId", "用户姓名", "证件编号"],
                colModel: [
                    { name: "UserId", index: "UserId", width: 10, hidden: true },
                    { name: "UserName", index: "UserName", align: "center", width: 250 },
                    { name: "UserCode", index: "UserCode", align: "center", width: 350 }
                ],
                multiselect: false,
                autowidth: true,
                rowNum: 20,
                altRows: true,
                pgbuttons: true,
                viewrecords: true,
                width: "100%",
                shrinkToFit: false,
                pginput: true,
                rowList: [10, 20, 30, 50, 70, 100],
                pager: $pagerUserMain,
                //JqGrid双击事件
                ondblClickRow: function(rowid, iRow, iCol, e) {
                    chooseUser();
                },
                loadComplete: function() {
                    var table = this;
                    updatePagerIcons(table);
                    jqGridAutoWidth();
                }
            });
        }


        initSystemMessageGrid();
        initUserGrid();
    }

    var initButtonArea = function() {

        //新增
        $("#btnSystemMessage_Insert").on("click", function() {
            currentEditType = enum_EditTypes.insert;
            $mdlSystemMessageInfo.modal("show");

            var systemMessageData = {};

            systemMessageData = getJson($divSystemMessageInfo);
            for (var p in systemMessageData) {
                systemMessageData[p] = "";
            }

            setJson($divSystemMessageInfo, systemMessageData);
        });

        //修改
        $("#btnSystemMessage_Update").on("click", function() {

            var rowData = getSystemMessageGridSelectedRowData($gridSystemMessageMain, function() {
                alert("请选择数据！");
                return false;
            });

            if (rowData == undefined) {
                return false;
            }

            currentEditType = enum_EditTypes.update;
            $mdlSystemMessageInfo.modal("show");

            systemMessageId = rowData.SystemMessageId;
            var userId = rowData.ReceiveUserId;
            var userName = rowData.ReceiveUserName;

            //根据systemMessageId获取系统信息
            var getSystemMessageInfoById = function() {
                var dataResult = {};
                ajaxRequest({
                    url: "/" + controllerName + "/GetSystemMessageInfoById",
                    data: { "systemMessageId": systemMessageId },
                    type: "get",
                    datatype: "json",
                    async: false,
                    success: function(jdata) {
                        dataResult = jdata;
                    }
                });
                return dataResult;
            }
            
            var messageData = {};

            messageData = getJson($divSystemMessageInfo);

            messageData = getSystemMessageInfoById();
            messageData.ReceiveUserId = userId;
            messageData.ReceiveUserName = userName;

            setJson($divSystemMessageInfo, messageData);
            $divSystemMessageInfo.find("[name='systemMessageId']").val(systemMessageId);
        });

        //删除
        $("#btnSystemMessage_Delete").on("click", function() {
            var rowData = getSystemMessageGridSelectedRowData($gridSystemMessageMain, function() {
                alert("请选择数据！");
            });
            if (rowData == undefined) {
                return false;
            }
            systemMessageId = rowData.SystemMessageId;
            if (!confirm("确定要删除数据吗？")) {
                return false;
            }
            ajaxRequest({
                url: "/" + controllerName + "/DeleteSystemMessage",
                data: { "systemMessageId": systemMessageId },
                type: "post",
                datatype: "Json",
                ansyc: false,
                success: function(jdata) {
                    if (jdata.isSuccess != null) {
                        alert(jdata.ErrorMessage);
                        return false;
                    } else {
                        alert("删除成功！");
                        $gridSystemMessageMain.trigger("reloadGrid");
                        return true;
                    }
                }
            });
            return false;
        });

        //添加用户
        $("#btnSystemMessage_Choose").on("click", function() {
            $mdlSystemMessageInfo.modal("toggle");
            $mdlUserInfo.modal("show");
        });

        //清空
        $("#btnSystemMessage_Clear").on("click", function() {
            $divSystemMessageInfo.find("[name='ReceiveUserId']").val("");
            $divSystemMessageInfo.find("[name='ReceiveUserName']").val("");
        });
    }

    var initModel = function() {

        var initSystemMessageModel = function() {

            var initSystemMessageModelShow = function() {
                $mdlSystemMessageInfo.on('show.bs.modal', function(e) {

                    if (enum_EditTypes.update == currentEditType || enum_EditTypes.insert == currentEditType) {
                        $divSystemMessageInfo.find("[name='UserName']").prop("disabled", true);
                    } else {
                        $divSystemMessageInfo.find("[name='UserName']").prop("disabled", false);
                    }
                });
            }

            var initSystemMessageSaveButton = function() {
                $("#saveSystemMessage_Confirm").on("click", function() {
                    var checkResult = verifyForm($divSystemMessageInfo);
                    if (!checkResult) {
                        return false;
                    }
                    var systemMessageData = getJson($divSystemMessageInfo);

                    var functionName = "";
                    if (currentEditType === enum_EditTypes.update) {
                        functionName = "/UpdateSystemMessage";
                    } else if (currentEditType === enum_EditTypes.insert) {
                        functionName = "/InsertSystemMessage";
                    }
                    $divSystemMessageInfo.ajaxSubmit({
                        url: "/" + controllerName + functionName,
                        type: "post",
                        date: systemMessageData,
                        beforeSubmit: function(formArray, jqForm) {
                        },
                        success: function(jdata) {
                            if (jdata.isSuccess != null) {
                                alert(jdata.ErrorMessage);
                                return false;
                            }
                            alert("保存成功！");
                            $gridSystemMessageMain.trigger("reloadGrid");
                            $mdlSystemMessageInfo.modal("toggle");
                            return true;
                        }
                    });
                    return false;
                });
            }


            initSystemMessageModelShow();
            initSystemMessageSaveButton();
        }

        var initUserModel = function() {

            var initUserSaveButton = function() {
                $("#saveUser_Confirm").on("click", function() {
                    chooseUser();
                });
            }

            var initUserCloseButton = function() {
                $("#closeUser_Confirm").on("click", function() {
                    $mdlUserInfo.modal("toggle");
                    $mdlSystemMessageInfo.modal("show");
                });
            }

            initUserSaveButton();
            initUserCloseButton();
        }

        initSystemMessageModel();
        initUserModel();
    }

    $(document).ready(function() {
        initQueryArea();
        initJqGrid();
        initButtonArea();
        initModel();
    });
});