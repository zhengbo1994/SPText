'use strict'
$(function () {
    var controllerName = "TrainingTeacherManage";
    //培训讲师
    var $divTrainingTeacherManageQueryArea = $("#divTrainingTeacherManage_QueryArea");
    var $gridTrainingTeacherManageMain = $("#gridTrainingTeacherManage_Main");
    var $pagerTrainingTeacherManageMain = $("#pagerTrainingTeacherManage_Main");
    var $mdlTrainingTeacherManageInfo = $("#mdlTrainingTeacherManage_Info");
    var $divTrainingTeacherManageInfo = $("#divTrainingTeacherManage_Info");
    //培训机构
    var $mdlTrainingInstitutionInfo = $("#mdlTrainingInstitution_Info");
    var $divTrainingInstitutionQueryArea = $("#divTrainingInstitution_QueryArea");
    var $gridTrainingInstitutionMain = $("#gridTrainingInstitution_Main");
    var $pagerTrainingInstitutionMain = $("#pagerTrainingInstitution_Main");

    var enum_EditTypes = { insert: 0, update: 1, view: 2 }
    var currentEditType;
    var teacherId = 0;

    var getTeacherGridSelectedRowData = function ($gridTrainingTeacherManageMain, noSelectionCallback) {
        var rowId = $gridTrainingTeacherManageMain.jqGrid("getGridParam", "selrow");
        if (!rowId) {
            if ("function" == typeof (noSelectionCallback)) {
                noSelectionCallback();
            }
            return undefined;
        }
        var rowData = $gridTrainingTeacherManageMain.jqGrid("getRowData", rowId);
        return rowData;
    }

    var getInstitutionGridSelectedRowData = function ($gridTrainingInstitutionMain, noSelectionCallback) {
        var rowId = $gridTrainingInstitutionMain.jqGrid("getGridParam", "selrow");
        if (!rowId) {
            if ("function" == typeof (noSelectionCallback)) {
                noSelectionCallback();
            }
            return undefined;
        }
        var rowData = $gridTrainingInstitutionMain.jqGrid("getRowData", rowId);
        return rowData;
    }

    //添加培训机构
    var chooseInstitution = function () {
        var rowData = getInstitutionGridSelectedRowData($gridTrainingInstitutionMain, function () {
            alert("请选择数据！");
        });
        if (rowData == undefined) {
            return false;
        }

        $divTrainingTeacherManageInfo.find("[name='InstitutionId']").val(rowData.InstitutionId);
        $divTrainingTeacherManageInfo.find("[name='InstitutionName']").val(rowData.InstitutionName);


        $mdlTrainingInstitutionInfo.modal("toggle");
        $mdlTrainingTeacherManageInfo.modal("show");
    }



    var initQueryArea = function () {

        var initTrainingTeacherQuery = function () {
            var initQueryButton = function () {
                $("#btnTrainingTeacherManage_Query").on("click", function () {
                    var queryData = {};
                    var divQueryArea = $divTrainingTeacherManageQueryArea;
                    queryData = getJson(divQueryArea);
                    $gridTrainingTeacherManageMain.jqGrid("setGridParam", { postData: queryData }).trigger("reloadGrid");
                });
            }

            initQueryButton();
        }

        var initTrainingInstitutionQuery = function () {
            var initQueryButton = function () {
                $("#btnTrainingInstitution_Query").on("click", function () {
                    var queryData = {};
                    var divQueryArea = $divTrainingInstitutionQueryArea;
                    queryData = getJson(divQueryArea);
                    $gridTrainingInstitutionMain.jqGrid("setGridParam", { postData: queryData }).trigger("reloadGrid");
                });
            }

            initQueryButton();
        }

        initTrainingTeacherQuery();
        initTrainingInstitutionQuery();
    }

    var initJqGrid = function () {

        var initTrainingTeacherGrid = function () {
            var $queryData = {};
            $queryData = getJson($divTrainingTeacherManageQueryArea);
            $gridTrainingTeacherManageMain.jqGrid({
                url: "/" + controllerName + "/GetTrainingTeacherInfoListForJqgrid",
                datatype: "json",
                postData: $queryData,
                colNames: ["TeacherId", "讲师姓名", "身份证号", "联系电话", "所属培训机构", "InstitutionId", "讲师介绍"],
                colModel: [
                    { name: "TeacherId", index: "TeacherId", width: 10, hidden: true },
                    { name: "TeacherName", index: "TeacherName", align: "center", width: 150 },
                    { name: "IdNumber", index: "IdNumber", align: "center", width: 200 },
                    { name: "ContactNumber", index: "ContactNumber", align: "center", width: 150 },
                    { name: "InstitutionName", index: "InstitutionName", align: "center", width: 300 },
                    { name: "InstitutionId", index: "InstitutionId", width: 10, hidden: true },
                    { name: "TeacherIntroduction", index: "TeacherIntroduction", align: "center", width: 300 }
                ],
                multiselect: false,
                autowidth: true,
                rowNum: 20,
                altRows: true,
                pgbuttons: true,
                viewrecords: true,
                shrinkToFit: false,
                pginput: true,
                rowList: [10, 20, 30, 50, 70, 100],
                pager: $pagerTrainingTeacherManageMain,
                loadComplete: function () {
                    var table = this;
                    updatePagerIcons(table);
                    jqGridAutoWidth();
                    setGridHeight($gridTrainingTeacherManageMain.selector);
                }
            });
        }

        var initTrainingInstitutionGrid = function () {
            var $queryData = {};
            $queryData = getJson($divTrainingInstitutionQueryArea);
            $gridTrainingInstitutionMain.jqGrid({
                url: "/" + controllerName + "/GetTrainingInstitutionInfoListForJqgrid",
                datatype: "json",
                postData: $queryData,
                colNames: ["InstitutionId", "培训机构名称", "社会信用代码"],
                colModel: [
                    { name: "InstitutionId", index: "InstitutionId", width: 10, hidden: true },
                    { name: "InstitutionName", index: "InstitutionName", align: "center", width: 250 },
                    { name: "SocialCreditCode", index: "SocialCreditCode", align: "center", width: 350 }
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
                pager: $pagerTrainingInstitutionMain,
                //JqGrid双击事件
                ondblClickRow: function (rowid, iRow, iCol, e) {
                    chooseInstitution();
                },
                loadComplete: function () {
                    var table = this;
                    updatePagerIcons(table);
                    jqGridAutoWidth();
                }
            });
        }


        initTrainingTeacherGrid();
        initTrainingInstitutionGrid();
    }

    var initButtonArea = function () {

        //新增
        $("#btnTrainingTeacherManage_Insert").on("click", function () {
            currentEditType = enum_EditTypes.insert;
            $mdlTrainingTeacherManageInfo.modal("show");

            var teacherData = {};

            teacherData = getJson($divTrainingTeacherManageInfo);
            for (var p in teacherData) {
                teacherData[p] = "";
            }

            setJson($divTrainingTeacherManageInfo, teacherData);
        });

        //修改
        $("#btnTrainingTeacherManage_Update").on("click", function () {
            var rowData = getTeacherGridSelectedRowData($gridTrainingTeacherManageMain, function () {
                alert("请选择数据！");
            });

            if (rowData == undefined) {
                return false;
            }

            currentEditType = enum_EditTypes.update;
            $mdlTrainingTeacherManageInfo.modal("show");

            teacherId = rowData.TeacherId;
            var institutionName = rowData.InstitutionName;

            //根据teacherId获取Teacher信息
            var getTrainingTeacherManageInfoById = function () {
                var dataResult = {};
                ajaxRequest({
                    url: "/" + controllerName + "/GetTrainingTeacherManageInfoById",
                    data: { "teacherId": teacherId },
                    type: "get",
                    datatype: "json",
                    async: false,
                    success: function (jdata) {
                        dataResult = jdata;
                    }
                });
                return dataResult;
            }

            var teacherData = {};
            teacherData = getTrainingTeacherManageInfoById();
            teacherData.InstitutionName = institutionName;

            setJson($divTrainingTeacherManageInfo, teacherData);
            $divTrainingTeacherManageInfo.find("[name='teacherId']").val(teacherId);
        });

        //删除
        $("#btnTrainingTeacherManage_Delete").on("click", function () {
            var rowData = getTeacherGridSelectedRowData($gridTrainingTeacherManageMain, function () {
                alert("请选择数据！");
            });
            if (rowData == undefined) {
                return false;
            }
            teacherId = rowData.TeacherId;
            if (!confirm("确定要删除数据吗？")) {
                return false;
            }
            ajaxRequest({
                url: "/" + controllerName + "/DeleteTrainingTeacherManage",
                data: { "teacherId": teacherId },
                type: "post",
                datatype: "Json",
                ansyc: false,
                success: function (jdata) {
                    if (jdata.isSuccess != null) {
                        alert(jdata.ErrorMessage);
                        return false;
                    } else {
                        alert("删除成功！");
                        $gridTrainingTeacherManageMain.trigger("reloadGrid");
                        return true;
                    }
                }
            });
            return false;
        });

        //添加用户
        $("#btnTrainingTeacherManage_InsertInstitutionId").on("click", function () {
            $mdlTrainingTeacherManageInfo.modal("toggle");
            $mdlTrainingInstitutionInfo.modal("show");
        });

        //清空培训机构
        $("#btnTrainingTeacherManage_DeleteInstitutionId").on("click", function () {
            $divTrainingTeacherManageInfo.find("[name='InstitutionId']").val("");
            $divTrainingTeacherManageInfo.find("[name='InstitutionName']").val("");
        });
    }

    var initModel = function () {

        var initTrainingTeacherModel = function () {

            $divTrainingTeacherManageInfo.find('input[name=TrainingTeacherImagePath]').change(function () {
                $divTrainingTeacherManageInfo.find('input[name=ImageFile]').val($(this).val());
            });

            var initTrainingTeacherModelShow = function () {
                $mdlTrainingTeacherManageInfo.on('show.bs.modal', function(e) {

                    if (enum_EditTypes.update == currentEditType) {
                        $divTrainingTeacherManageInfo.find("[name='IdNumber']").prop("disabled", true);
                    } else {
                        $divTrainingTeacherManageInfo.find("[name='IdNumber']").prop("disabled", false);
                    }
                });
            }

            var initTrainingTeacherSaveButton = function () {
                $("#saveTrainingTeacherManage_Confirm").on("click", function () {
                    var checkResult = verifyForm($divTrainingTeacherManageInfo);
                    if (!checkResult) {
                        return false;
                    }

                    var teacherData = getJson($divTrainingTeacherManageInfo);

                    var functionName = "";
                    if (currentEditType === enum_EditTypes.update) {
                        functionName = "/UpdateTrainingTeacherManage";
                    } else if (currentEditType === enum_EditTypes.insert) {
                        functionName = "/InsertTrainingTeacherManage";
                    }

                    $divTrainingTeacherManageInfo.ajaxSubmit({
                        url: "/" + controllerName + functionName,
                        type: "post",
                        date: teacherData,
                        beforeSubmit: function (formArray, jqForm) {
                        },
                        success: function (jdata) {
                            if (jdata.isSuccess != null) {
                                alert(jdata.ErrorMessage);
                                return false;
                            }
                            alert("保存成功！");
                            $gridTrainingTeacherManageMain.trigger("reloadGrid");
                            $mdlTrainingTeacherManageInfo.modal("toggle");
                            return true;
                        }
                    });
                    return false;
                });
            }


            initTrainingTeacherModelShow();
            initTrainingTeacherSaveButton();
        }

        var initTrainingInstitutionModel = function () {

            var initTrainingInstitutionSaveButton = function () {
                $("#saveTrainingInstitution_Confirm").on("click", function () {
                    chooseInstitution();
                });
            }

            var initTrainingInstitutionCloseButton = function () {
                $("#closeTrainingInstitution_Confirm").on("click", function () {
                    $mdlTrainingInstitutionInfo.modal("toggle");
                    $mdlTrainingTeacherManageInfo.modal("show");
                });
            }

            initTrainingInstitutionSaveButton();
            initTrainingInstitutionCloseButton();
        }

        initTrainingTeacherModel();
        initTrainingInstitutionModel();
    }

    $(document).ready(function () {
        initQueryArea();
        initJqGrid();
        initButtonArea();
        initModel();
    });
})