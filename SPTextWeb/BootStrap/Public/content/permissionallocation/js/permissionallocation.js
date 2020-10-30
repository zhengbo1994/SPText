'use strict';
$(function() {
    var controllerName = "PermissionAllocation";

    var $divPermissionAllocationQueryArea = $("#divPermissionAllocation_QueryArea");
    var $gridPermissionAllocationMain = $("#gridPermissionAllocation_Main");
    var $pagerPermissionAllocationMain = $("#pagerPermissionAllocation_Main");
    var $mdlPermissionAllocationInfo = $("#mdlPermissionAllocation_Info");
    var $divPermissionAllocationInfo = $("#divPermissionAllocation_Info");

    var $gridLessonTypeMain = $("#gridLessonType_Main");
    var $pagerLessonTypeMain = $("#pagerLessonType_Main");


    var $mdlUserInfo = $("#mdlUser_Info");
    var $divUserQueryArea = $("#divUser_QueryArea");
    var $gridUserMain = $("#gridUser_PermissionAllocationMain");
    var $pagerUserMain = $("#pagerUser_PermissionAllocationMain");

    var userId = 0;


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

    //添加角色
    var chooseUser = function() {
        var rowData = getUserGridSelectedRowData($gridUserMain, function() {
            alert("请选择数据！");
        });
        if (rowData == undefined) {
            return false;
        }

        $divPermissionAllocationInfo.find("[name='UserId']").val(rowData.UserId);
        $divPermissionAllocationInfo.find("[name='UserName']").val(rowData.UserName);


        $mdlUserInfo.modal("toggle");
        $mdlPermissionAllocationInfo.modal("show");
    }


    var initQueryArea = function() {
        var initRelQueryButton = function() {
            $("#btnPermissionAllocation_Query").on("click", function() {
                var queryData = {};
                var divQueryArea = $divPermissionAllocationQueryArea;
                queryData = getJson(divQueryArea);
                $gridPermissionAllocationMain.jqGrid("setGridParam", { postData: queryData }).trigger("reloadGrid");
            });
        }

        var initUserQueryButton = function() {
            $("#btnUser_Query").on("click", function() {
                var queryData = {};
                var divQueryArea = $divUserQueryArea;
                queryData = getJson(divQueryArea);
                $gridUserMain.jqGrid("setGridParam", { postData: queryData }).trigger("reloadGrid");
            });
        }

        initRelQueryButton();
        initUserQueryButton();
    }

    var initJqGrid = function() {
        var initPermissionAllocationJqGrid = function() {
            var queryData = {};
            var divQueryArea = $divPermissionAllocationQueryArea;
            queryData = getJson(divQueryArea);
            $gridPermissionAllocationMain.jqGrid({
                url: "/" + controllerName + "/GetRelUserListForJqGrid",
                datatype: "json",
                mtype: "post",
                postData: queryData,
                colNames: ["UserId", "用户名称", "证件编号", "角色类型"],
                colModel: [
                    { name: "UserId", index: "UserId", width: 10, hidden: true },
                    { name: "UserName", index: "UserName", align: "center", width: 150 },
                    { name: "UserCode", index: "UserCode", align: "center", width: 150 },
                    { name: "UserType", index: "UserType", align: "center", width: 150 }
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
                pager: $pagerPermissionAllocationMain,
                ondblClickRow: function(rowid, iRow, iCol, e) {
                    $gridPermissionAllocationMain.jqGrid("toggleSubGridRow", rowid);
                },
                subGrid: true,
                subGridOptions: {
                    "plusicon": "ace-icon fa fa-plus",
                    "minusicon": "ace-icon fa fa-minus",
                    "openicon": "ace-icon fa fa-share"
                },
                subGridRowExpanded: function(subgrid_id, row_id) {
                    var subgrid_table_id, pager_id;
                    var rowData = $gridPermissionAllocationMain.jqGrid("getRowData", row_id);
                    subgrid_table_id = subgrid_id + "_t";
                    pager_id = "p_" + subgrid_table_id;
                    $("#" + subgrid_id).html("<div style='width:100%;overflow:auto'><table id='" + subgrid_table_id + "' class='scroll' ></table></div>");
                    var subGridQueryData = {};

                    subGridQueryData.UserId = rowData.UserId;
                    $("#" + subgrid_table_id).jqGrid({
                        url: "/" + controllerName + "/GetRelLessonTypeListByUserId",
                        datatype: "json",
                        postData: subGridQueryData,
                        rownumbers: true,
                        rowNum: 10,
                        colNames: ["RelRoomUserLessonTypeId", "LessonTypeId", "课程名称", "培训类型"],
                        colModel: [
                            { name: "RelRoomUserLessonTypeId", index: "RelRoomUserLessonTypeId", width: "10", align: "center", hidden: true },
                            { name: "LessonTypeId", index: "LessonTypeId", width: "10", align: "center", hidden: true },
                            { name: "LessonTypeName", index: "LessonTypeName", align: "center", width: 200 },
                            { name: "TrainingTypeName", index: "TrainingTypeName", align: "center", width: 200 },
                        ],
                        autoWidth: false,
                        shrinkToFit: false,
                        ondblClickRow: function(rowid, iRow, iCol, e) {
                            return false;
                        },
                        loadComplete: function() {
                            jqGridAutoWidth();
                        }
                    });
                },
                loadComplete: function() {
                    var table = this;
                    updatePagerIcons(table);
                    jqGridAutoWidth();
                    setGridHeight($gridPermissionAllocationMain.selector);
                }
            });
        }

        var initUserJqGrid = function() {
            var $queryData = {};
            $queryData = getJson($divUserQueryArea);
            $gridUserMain.jqGrid({
                url: "/" + controllerName + "/GetUserListForJqGrid",
                datatype: "json",
                postData: $queryData,
                colNames: ["UserId", "用户名称", "证件名称", "角色类型"],
                colModel: [
                    { name: "UserId", index: "UserId", width: 10, hidden: true },
                    { name: "UserName", index: "UserName", align: "center", width: 250 },
                    { name: "UserCode", index: "UserCode", align: "center", width: 250 },
                    { name: "UserType", index: "UserType", align: "center", width: 350 }
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

        //加载LessonType JqGrid
        var lessonTypeJqGrid = function() {
            $gridLessonTypeMain.jqGrid({
                url: "/" + controllerName + "/GetLessonTypeListForJqGrid",
                datatype: "json",
                mtype: "post",
                colNames: ["LessonId", "课程类型", "培训类型"],
                colModel: [
                    { name: "LessonTypeId", index: "LessonTypeId", width: 50, hidden: true },
                    { name: "LessonTypeName", index: "LessonTypeName", align: "center", width: 200 },
                    { name: "TrainingTypeId", index: "TrainingTypeId", align: "center", width: 200 }
                ],
                multiselect: true,
                autowidth: true,
                rowNum: 50,
                altRows: true,
                pgbuttons: false,
                viewrecords: true,
                shrinkToFit: false,
                pginput: false,
                pager: $pagerLessonTypeMain,
                loadComplete: function() {
                    jqGridAutoWidth();
                }
            });
        }

        initPermissionAllocationJqGrid();;
        initUserJqGrid();
        lessonTypeJqGrid();
    }

    var initButtonArea = function() {

        //新增
        $("#btnPermissionAllocation_Insert").on("click", function() {
            //重新加载LessonType
            $gridLessonTypeMain.jqGrid("setGridParam", { page: 1 }).trigger("reloadGrid");
            $divPermissionAllocationInfo.find("[name='UserName']").prop("disabled", true);
            $mdlPermissionAllocationInfo.modal("show");

            var rowId = $gridPermissionAllocationMain.jqGrid("getGridParam", "selrow");
            var rowData = $gridPermissionAllocationMain.jqGrid("getRowData", rowId);

            if (rowData == undefined) {
                return false;
            }

            userId = rowData.UserId;
            var userName = rowData.UserName;
            
            var relRoomUserLessonTypeData = {};

            relRoomUserLessonTypeData.UserId = userId
            relRoomUserLessonTypeData.UserName = userName

            setJson($divPermissionAllocationInfo, relRoomUserLessonTypeData);
            return false;
        });

        //删除
        $("#btnPermissionAllocation_Delete").on("click", function() {
            var rowId = $gridPermissionAllocationMain.jqGrid("getGridParam", "selrow");
            if (isNull(rowId)) {
                alert("请选择数据！");
                return false;
            }

            var rowData = $gridPermissionAllocationMain.jqGrid("getRowData", rowId);

            var userId = rowData.UserId;

            if (!confirm("确定要删除数据吗？")) {
                return false;
            }

            ajaxRequest({
                url: "/" + controllerName + "/DeleteRelRoomUserLessonTypeByUserId",
                data: { "userId": userId },
                type: "post",
                ansyc: false,
                success: function(jdata) {
                    if (jdata.isSuccess != null) {
                        alert(jdata.ErrorMessage);
                        return false;
                    }
                    alert("删除成功！");
                    $gridPermissionAllocationMain.trigger("reloadGrid");
                    return true;
                }
            });
            return false;
        });

        //添加用户
        $("#btnPermissionAllocation_InsertUserId").on("click", function() {
            $mdlPermissionAllocationInfo.modal("toggle");
            $mdlUserInfo.modal("show");
        });
    }

    var initModel = function() {

        var initPermissionAllocationModel = function() {
            var initPermissionAllocationSaveButton = function() {

                //保存
                $("#savePermissionAllocation_Confirm").on("click", function() {
                    var arrRowid = $gridLessonTypeMain.jqGrid("getGridParam", "selarrrow");
                    var lessonTypeIdList = [];

                    for (var i = 0; i < arrRowid.length; i++) {
                        var lessonTypeId = $gridLessonTypeMain.jqGrid("getRowData", arrRowid[i]).LessonTypeId;
                        lessonTypeIdList.push(lessonTypeId);
                    }

                    var postData = {};
                    postData.lessonTypeIdList = [];
                    for (var j = 0; j < lessonTypeIdList.length; j++) {
                        postData.lessonTypeIdList.push(lessonTypeIdList[j]);
                    }

                    var userData = getJson($divPermissionAllocationInfo);

                    postData.userId = userData.UserId;
                    var strData = JSON.stringify(postData);
                    var ajaxOpt = {
                        url: "/" + controllerName + "/InsertRelRoomUserLessonType",
                        data: { "strData": strData },
                        type: "post",
                        async: false,
                        success: function(jdata) {
                            if (jdata.isSuccess != null) {
                                alert(jdata.ErrorMessage);
                                return false;
                            }
                            alert("保存成功！");
                            $gridPermissionAllocationMain.trigger("reloadGrid");
                            $mdlPermissionAllocationInfo.modal("toggle");
                            return true;
                        }
                    }
                    ajaxRequest(ajaxOpt);

                });
            }


            initPermissionAllocationSaveButton();
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
                    $mdlPermissionAllocationInfo.modal("show");
                });
            }

            initUserSaveButton();
            initUserCloseButton();
        }

        initPermissionAllocationModel();
        initUserModel();
    }

    $(document).ready(function() {
        initQueryArea();
        initJqGrid();
        initButtonArea();
        initModel();
    });
});