'use strict'
$(function () {
    var controllerName = "ActiveManage";

    var $divActiveManageQueryArea = $("#divActiveManage_QueryArea");
    var $gridActiveManageMain = $("#gridActiveManage_Main");
    var $pagerActiveManagegMain = $("#pagerActiveManageg_Main");
    var $mdlActiveManageInfo = $("#mdlActiveManage_Info");
    var $divActiveManageInfo = $("#divActiveManage_Info");

    var enum_EditTypes = { insert: 0, update: 1, view: 2 }
    var currentEditType;
    var activeCodeUseRecordId = 0;


    var getGridSelectedRowData = function ($gridActiveManageMain, noSelectionCallback) {
        var rowId = $gridActiveManageMain.jqGrid("getGridParam", "selrow");
        if (!rowId) {
            if ("function" == typeof (noSelectionCallback)) {
                noSelectionCallback();
            }
            return undefined;
        }
        var rowData = $gridActiveManageMain.jqGrid("getRowData", rowId);
        return rowData;
    }

    var initQueryArea = function () {
        var initQueryButton = function () {
            $("#btnActiveManage_Query").on("click", function () {
                var queryData = {};
                var divQueryArea = $divActiveManageQueryArea;
                queryData = getJson(divQueryArea);
                $gridActiveManageMain.jqGrid("setGridParam", { postData: queryData }).trigger("reloadGrid");
            });
        }

        var initSelectActiveCodeTypeName = function () {
            var $SelectActiveCodeType = $divActiveManageQueryArea.find("[name='ActiveCodeTypeId']");
            var getSelectActiveCodeTypeList = function () {
                var dataResult = {};
                ajaxRequest({
                    url: "/ActivationCodeUseingQuery/GetActiveCodeTypeList",
                    type: "post",
                    datatype: "json",
                    async: false,
                    success: function (jdata) {
                        dataResult = jdata;
                    },
                    error: function () {
                        dataResult = null;
                    }
                });
                return dataResult;
            }

            var activeCodeTypeList = getSelectActiveCodeTypeList();
            $SelectActiveCodeType.empty();
            var $optionAll = $("<option>");

            $optionAll.val("");
            $optionAll.text("全部");
            $SelectActiveCodeType.append($optionAll);
            for (var i = 0; i < activeCodeTypeList.length; i++) {
                var activeCodeType = activeCodeTypeList[i];
                var $option = $("<option>");
                $option.val(activeCodeType.ActiveCodeTypeId);
                $option.text(activeCodeType.ActiveCodeTypeName);
                $SelectActiveCodeType.append($option);
            }
        }

        var initSelectLessonTypeName = function () {
            var $SelectLessonType = $divActiveManageQueryArea.find("[name='LessonTypeId']");
            var getSelectLessonTypeList = function () {
                var dataResult = {};
                ajaxRequest({
                    url: "/" + controllerName + "/GetLessonTypeList",
                    type: "post",
                    datatype: "json",
                    async: false,
                    success: function (jdata) {
                        dataResult = jdata;
                    },
                    error: function () {
                        dataResult = null;
                    }
                });
                return dataResult;
            }

            var lessonTypeTypeList = getSelectLessonTypeList();
            $SelectLessonType.empty();
            var $optionAll = $("<option>");

            $optionAll.val("");
            $optionAll.text("全部");
            $SelectLessonType.append($optionAll);
            for (var i = 0; i < lessonTypeTypeList.length; i++) {
                var lessonType = lessonTypeTypeList[i];
                var $option = $("<option>");
                $option.val(lessonType.LessonTypeId);
                $option.text(lessonType.LessonTypeName);
                $SelectLessonType.append($option);
            }
        }

        initQueryButton();
        initSelectActiveCodeTypeName();
        initSelectLessonTypeName();
    }

    var initJqGrid = function () {
        var queryData = {};
        var divQueryArea = $divActiveManageQueryArea;
        queryData = getJson(divQueryArea);
        $gridActiveManageMain.jqGrid({
            url: "/" + controllerName + "/GetActiveCodeUseRecordListForJqGrid",
            datatype: "json",
            mtype: "post",
            postData: queryData,
            colNames: ["ActiveCodeUseRecordId", "激活码", "激活码类型", "激活课程", "剩余使用时间(天)", "使用人", "激活人", "激活日期", "到期日期"],
            colModel: [
                { name: "ActiveCodeUseRecordId", index: "ActiveCodeUseRecordId", width: 60, hidden: true },
                { name: "ActiveCodeNumber", index: "ActiveCodeNumber", align: "center", width: 230 },
                { name: "ActiveCodeTypeName", index: "ActiveCodeTypeId", align: "center", width: 150 },
                { name: "LessonTypeName", index: "LessonTypeId", align: "center", width: 150 },
                { name: "SurplusDays", index: "SurplusDays", align: "center", width: 120 },
                { name: "UseName", index: "UseId", align: "center", width: 80 },
                { name: "CreateBy", index: "CreateBy", align: "center", width: 80 },
                { name: "CreateDate", index: "CreateDate", align: "center", width: 120 },
                { name: "EndDate", index: "EndDate", align: "center", width: 120 }
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
            pager: $pagerActiveManagegMain,
            ondblClickRow: function (rowid, iRow, iCol, e) {
                $gridActiveManageMain.jqGrid("toggleSubGridRow", rowid);
            },
            subGrid: true,
            subGridOptions: {
                "plusicon": "ace-icon fa fa-plus",
                "minusicon": "ace-icon fa fa-minus",
                "openicon": "ace-icon fa fa-share"
            },
            subGridRowExpanded: function (subgrid_id, row_id) {
                var subgrid_table_id, pager_id;
                var rowData = $gridActiveManageMain.jqGrid("getRowData", row_id);
                subgrid_table_id = subgrid_id + "_t";
                pager_id = "p_" + subgrid_table_id;
                $("#" + subgrid_id).html("<div style='width:100%;overflow:auto'><table id='" + subgrid_table_id + "' class='scroll' ></table></div>");
                var subGridQueryData = {};

                subGridQueryData.ActiveCodeUseRecordId = rowData.ActiveCodeUseRecordId;
                $("#" + subgrid_table_id).jqGrid({
                    url: "/" + controllerName + "/GetActiveCodeDelayRecordListForJqGrid",
                    datatype: "json",
                    postData: subGridQueryData,
                    rownumbers: true,
                    rowNum: 10,
                    colNames: ["ActiveCodeDelayRecordId", "延期时长(天数)", "操作人", "操作时间"],
                    colModel: [
                        { name: "ActiveCodeDelayRecordId", index: "ActiveCodeDelayRecordId", width: "10", align: "center", hidden: true },
                        { name: "DelayDays", index: "DelayDays", width: "100", align: "center" },
                        { name: "CreateBy", index: "CreateBy", width: "100", align: "center" },
                        { name: "CreateDate", index: "CreateDate", width: "100", align: "center" }
                    ],
                    autoWidth: false,
                    shrinkToFit: false,
                    ondblClickRow: function (rowid, iRow, iCol, e) {
                        return false;
                    },
                    loadComplete: function () {
                        jqGridAutoWidth();
                    }
                });
            },

            loadComplete: function () {
                var table = this;
                updatePagerIcons(table);
                jqGridAutoWidth();
                setGridHeight($gridActiveManageMain.selector);
            }
        });


    }

    var initButtonArea = function () {
        //延期
        $("#btnActiveManage_Delay").on("click", function myfunction() {
            var rowData = getGridSelectedRowData($gridActiveManageMain, function () {
                alert("请选择数据！");
            });
            if (rowData == undefined) {
                return false;
            }

            activeCodeUseRecordId = rowData.ActiveCodeUseRecordId;
            var activeCodeNumber = rowData.ActiveCodeNumber;
            currentEditType = enum_EditTypes.update;
            $mdlActiveManageInfo.modal("show");

            var activeCodeUseRecordData = {};

            if (enum_EditTypes.update == currentEditType) {
                $divActiveManageInfo.find("[name='ActiveCodeNumber']").prop("disabled", true);
                activeCodeUseRecordData.ActiveCodeNumber = activeCodeNumber;
            }
            setJson($divActiveManageInfo, activeCodeUseRecordData);
            $divActiveManageInfo.find("[name='activeCodeUseRecordId']").val(activeCodeUseRecordId);
            return false;
        });
    }

    var initModel = function () {
        var initActiveCodeDelaySaveButton = function () {
            $("#saveActiveManage_Confirm").on("click", function () {
               
                var checkResult = verifyForm($divActiveManageInfo);
                if (!checkResult) {
                    return false;
                }
                var activeCodeUseRecordData = getJson($divActiveManageInfo);

                var functionName = "";
                if (currentEditType === enum_EditTypes.update) {
                    functionName = "/UpdateActiveManage";
                } else if (currentEditType === enum_EditTypes.insert) {
                    functionName = "/InsertActiveManage";
                }
                $divActiveManageInfo.ajaxSubmit({
                    url: "/" + controllerName + functionName,
                    type: "post",
                    date: activeCodeUseRecordData,
                    beforeSubmit: function (formArray, jqForm) {
                    },
                    success: function (jdata) {
                        if (jdata.isSuccess != null) {
                            alert(jdata.ErrorMessage);
                            return false;
                        }
                        alert("延期成功！");
                        $gridActiveManageMain.trigger("reloadGrid");
                        $mdlActiveManageInfo.modal("toggle");
                        return true;
                    }
                });
                return false;
            });
        };




        initActiveCodeDelaySaveButton();
    }





    $(document).ready(function () {
        initQueryArea();
        initJqGrid();
        initButtonArea();
        initModel();
    });
});