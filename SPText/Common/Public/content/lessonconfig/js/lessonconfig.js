'use strict'
$(function () {
    var controllerName = "LessonConfig";
    var $divLessonConfigQueryArea = $("#divLessonConfig_QueryArea");
    var $gridLessonConfigMain = $("#gridLessonConfig_Main");
    var $pagerLessonConfigMain = $("#pagerLessonConfig_Main");
    var $mdlLessonConfigLessonConfigInfo = $("#mdlLessonConfig_LessonConfigInfo");
    var $divLessonConfigLessonConfigInfo = $("#divLessonConfig_LessonConfigInfo");

    var enum_EditTypes = { insert: 0, update: 1 }
    var currentEditType;
    var lessonId = 0;


    var getGridSelectedRowData = function ($gridLessonConfigMain, noSelectionCallback) {
        var rowId = $gridLessonConfigMain.jqGrid("getGridParam", "selrow");
        if (!rowId) {
            if ("function" == typeof (noSelectionCallback)) {
                noSelectionCallback();
            }
            return undefined;
        }
        var rowData = $gridLessonConfigMain.jqGrid("getRowData", rowId);
        return rowData;
    };


    var initQueryArea = function () {
        var initQueryButton = function () {
            $("#btnLessonConfig_Query").on("click", function () {
                var queryData = {};
                var divQueryArea = $divLessonConfigQueryArea;
                queryData = getJson(divQueryArea);
                $gridLessonConfigMain.jqGrid("setGridParam", { page: 1, postData: queryData }).trigger("reloadGrid");
            });
        }

        initQueryButton();
    }


    var initLessonConfigJqGrid = function () {
        var queryData = {};
        var divQueryArea = $divLessonConfigQueryArea;
        queryData = getJson(divQueryArea);
        $gridLessonConfigMain.jqGrid({
            url: "/" + controllerName + "/GetLessonConfigListForJqGrid",
            datatype: "json",
            mtype: "post",
            postData: queryData,
            colNames: ["LessonId", "课程类型", "培训类型", "序号", "是否免费", "是否隐藏", "允许拖拽", "要求学时", "创建人", "创建时间"],
            colModel: [
                { name: "LessonId", index: "LessonId", width: 50, hidden: true },
                { name: "LessonTypeName", index: "LessonTypeName", align: "center", width: 200 },
                { name: "TrainingTypeId", index: "TrainingTypeId", align: "center", width: 200 },
                { name: "Seq", index: "Seq", align: "center", width: 50 },
                { name: "Free", index: "Free", align: "center", width: 50 },
                { name: "IsHide", index: "IsHide", align: "center", width: 50 },
                { name: "ControlProgressDrag", index: "ControlProgressDrag", align: "center", width: 50 },
                { name: "HoursOfStudy", index: "HoursOfStudy", align: "center", width: 50 },
                { name: "CreateBy", index: "CreateBy", align: "center", width: 100 },
                { name: "CreateDate", index: "CreateDate", align: "center", width: 150 }
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
            pager: $pagerLessonConfigMain,
            loadComplete: function () {
                var table = this;
                updatePagerIcons(table);
                jqGridAutoWidth();
                setGridHeight($gridLessonConfigMain.selector);
            }
        });
    }


    var initButtonArea = function () {

        //新增
        $("#btnLessonConfig_Insert").on("click", function () {
            currentEditType = enum_EditTypes.insert;
            $mdlLessonConfigLessonConfigInfo.modal("show");
        });

        //修改
        $("#btnLessonConfig_Update").on("click", function () {
            var rowData = getGridSelectedRowData($gridLessonConfigMain, function () {
                alert("请选择数据！");
            });
            if (rowData == undefined) {
                return false;
            }
            lessonId = rowData.LessonId;
            currentEditType = enum_EditTypes.update;
            $mdlLessonConfigLessonConfigInfo.modal("show");
            return false;
        });

        //删除
        $("#btnLessonConfig_Delete").on("click", function () {
            var rowData = getGridSelectedRowData($gridLessonConfigMain, function () {
                alert("请选择数据！");
            });
            if (rowData == undefined) {
                return false;
            }
            lessonId = rowData.LessonId;
            if (!confirm("确定要删除数据吗？")) {
                return false;
            }
            ajaxRequest({
                url: "/" + controllerName + "/DeleteLessonInfo",
                data: { "lessonId": lessonId },
                type: "post",
                ansyc: false,
                success: function (jdata) {
                    if (jdata.isSuccess != null) {
                        alert(jdata.ErrorMessage);
                        return false;
                    } else {
                        alert("删除成功！");
                        $gridLessonConfigMain.trigger("reloadGrid");
                        return true;
                    }
                }
            });
            return false;
        });
    }


    var initLessonConfigModel = function () {

        $divLessonConfigLessonConfigInfo.find('input[name=LessonConfigImagePath]').change(function () {
            $divLessonConfigLessonConfigInfo.find('input[name=ImageFile]').val($(this).val());
        });

        var initLessonConfigInfoModel = function () {

            $mdlLessonConfigLessonConfigInfo.on('show.bs.modal', function (e) {
                var getLessonConfigListDataByLessonId = function () {

                    var dataResult = {};
                    ajaxRequest({
                        url: "/" + controllerName + "/GetLessonConfigListDataByLessonId",
                        data: { "LessonId": lessonId },
                        type: "get",
                        datatype: "json",
                        async: false,
                        success: function (jdata) {
                            dataResult = jdata;
                        }
                    });
                    return dataResult;
                }

                var lessonData = {};
                if (enum_EditTypes.insert == currentEditType) {
                    lessonData = getJson($divLessonConfigLessonConfigInfo);
                    for (var p in lessonData) {
                        lessonData[p] = "";
                    }
                    lessonData.IsHide = "否";
                    lessonData.Free = "否";
                    lessonData.ControlProgressDrag = "否";
                } else if (enum_EditTypes.update == currentEditType) {
                    lessonData = getLessonConfigListDataByLessonId();
                }
                setJson($divLessonConfigLessonConfigInfo, lessonData);
                $divLessonConfigLessonConfigInfo.find("[name='lessonId']").val(lessonId);
            });
        }

        var initLessonConfigaveButton = function () {
            $mdlLessonConfigLessonConfigInfo.find("[name='saveLessonConfig_LessonConfigInfoConfirm']").on("click", function () {
                var checkResult = verifyForm($divLessonConfigLessonConfigInfo);
                if (!checkResult) {
                    return false;
                }
                var lessonData = getJson($divLessonConfigLessonConfigInfo);

                var functionName = "";
                if (currentEditType === enum_EditTypes.update) {
                    functionName = "/UpdateLessonInfo";
                } else if (currentEditType === enum_EditTypes.insert) {
                    functionName = "/InsertLessonInfo";
                }
                $divLessonConfigLessonConfigInfo.ajaxSubmit({
                    url: "/" + controllerName + functionName,
                    type: "post",
                    date: lessonData,
                    beforeSubmit: function (formArray, jqForm) {
                    },
                    success: function (jdata) {
                        if (jdata.isSuccess != null) {
                            alert(jdata.ErrorMessage);
                            return false;
                        } else {
                            alert("保存成功！");
                            $gridLessonConfigMain.trigger("reloadGrid");
                            $mdlLessonConfigLessonConfigInfo.modal("toggle");
                            return true;
                        }
                    }
                });
                return false;
            });
        }

        //培训类型下拉框绑定
        var initSelectTrainingTypeButton = function () {
            var $SelectTrainingTypeDetails = $divLessonConfigLessonConfigInfo.find("[name='TrainingTypeId']");
            var getTrainingTypeDetailsList = function () {
                var dataResult = {};
                ajaxRequest({
                    url: "/" + controllerName + "/GetTrainingTypeList",
                    type: "post",
                    datatype: "json",
                    async: false,
                    success: function (jdata) {
                        dataResult = jdata;
                    }
                });
                return dataResult;
            }

            var trainingTypeList = getTrainingTypeDetailsList();
            var $optionAll = $("<option>");
            $optionAll.val("");
            $optionAll.text("全部");
            $SelectTrainingTypeDetails.append($optionAll);

            for (var i = 0; i < trainingTypeList.length; i++) {
                var trainingTypeInfo = trainingTypeList[i];
                var $option = $("<option>");
                $option.val(trainingTypeInfo.TrainingTypeId);
                $option.text(trainingTypeInfo.TrainingTypeName);
                $SelectTrainingTypeDetails.append($option);
            }
        }

        initLessonConfigInfoModel();
        initLessonConfigaveButton();
        initSelectTrainingTypeButton();
    }

    $(document).ready(function () {
        initQueryArea();
        initLessonConfigJqGrid();
        initButtonArea();
        initLessonConfigModel();
    });
});