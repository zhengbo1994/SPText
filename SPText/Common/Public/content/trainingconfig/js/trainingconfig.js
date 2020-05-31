'use strict';
$(function () {
    var controllerName = "TrainingConfig";
    var $divTrainingConfigQueryArea = $("#divTrainingConfig_QueryArea");
    var $gridTrainingConfigMain = $("#gridTrainingConfig_Main");
    var $pagerTrainingConfigMain = $("#pagerTrainingConfig_Main");
    var $mdlTrainingConfigTrainingConfigInfo = $("#mdlTrainingConfig_TrainingConfigInfo");
    var $divTrainingConfigTrainingConfigInfo = $("#divTrainingConfig_TrainingConfigInfo");

    var enumEditTypes = { insert: 0, update: 1}
    var currentEditType;
    var trainingId = 0;

    var initQueryArea = function () {
        var initQueryButton = function () {
            $("#btnTrainingConfig_Query").on("click", function () {
                var queryData = {};
                var divQueryArea = $divTrainingConfigQueryArea;
                queryData = getJson(divQueryArea);
                $gridTrainingConfigMain.jqGrid("setGridParam", { page: 1, postData: queryData }).trigger("reloadGrid");
            });
        }

        initQueryButton();
    }


    var initTrainingConfigJqGrid = function () {
        var queryData = {};
        var divQueryArea = $divTrainingConfigQueryArea;
        queryData = getJson(divQueryArea);
        $gridTrainingConfigMain.jqGrid({
            url: "/" + controllerName + "/GetTrainingConfigListForJqGrid",
            datatype: "json",
            mtype: "post",
            postData: queryData,
            colNames: ["TrainingId", "培训类型", "序号", "是否隐藏", "创建人","创建时间"],
            colModel: [
                { name: "TrainingId", index: "TrainingId", width: 50, hidden: true },
                { name: "TrainingTypeName", index: "TrainingTypeName", align: "center", width: 200 },
                { name: "Seq", index: "Seq", align: "center", width: 50 },
                { name: "IsHide", index: "IsHide", align: "center", width: 50 },
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
            pager: $pagerTrainingConfigMain,
            loadComplete: function () {
                var table = this;
                updatePagerIcons(table);
                jqGridAutoWidth();
                setGridHeight($gridTrainingConfigMain.selector);
            }
        });
    }


    var initButtonArea = function () {

        //新增
        $("#btnTrainingConfig_Insert").on("click", function () {
            currentEditType = enumEditTypes.insert;
            $mdlTrainingConfigTrainingConfigInfo.modal("show");
        });

        //修改
        $("#btnTrainingConfig_Update").on("click", function () {

            var rowId = $gridTrainingConfigMain.jqGrid("getGridParam", "selrow");
            if (isNull(rowId)) {
                alert("请选择数据！");
                return false;
            }

            var rowData = $gridTrainingConfigMain.jqGrid("getRowData", rowId);

            trainingId = rowData.TrainingId;
            currentEditType = enumEditTypes.update;
            $mdlTrainingConfigTrainingConfigInfo.modal("show");
            return false;
        });


        //删除
        $("#btnTrainingConfig_Delete").on("click", function () {

            var rowId = $gridTrainingConfigMain.jqGrid("getGridParam", "selrow");
            if (isNull(rowId)) {
                alert("请选择数据！");
                return false;
            }

            var rowData = $gridTrainingConfigMain.jqGrid("getRowData", rowId);

            trainingId = rowData.TrainingId;
            if (!confirm("确定要删除数据吗？")) {
                return false;
            }
            ajaxRequest({
                url: "/" + controllerName + "/DeleteTrainingInfo",
                data: { "trainingId": trainingId },
                type: "post",
                ansyc: false,
                success: function (jdata) {
                    if (jdata.isSuccess != null) {
                        alert(jdata.ErrorMessage);
                        return false;
                    }

                    alert("删除成功！");
                    $gridTrainingConfigMain.trigger("reloadGrid");
                    return true;

                }
            });
            return false;
        });
    }


    var initTrainingConfigModel = function () {

        var initTrainingConfigInfoModel = function () {

            $mdlTrainingConfigTrainingConfigInfo.on('show.bs.modal', function (e) {
                var getTrainingConfigListDataByTrainingId = function () {
                    var queryData = {};
                    var dataResult = {};
                    queryData.trainingId = trainingId;
                    ajaxRequest({
                        url: "/" + controllerName + "/GetTrainingConfigListDataByTrainingId",
                        data: queryData,
                        type: "get",
                        datatype: "json",
                        async: false,
                        success: function (jdata) {
                            dataResult = jdata;
                        }
                    });
                    return dataResult;
                }

                var trainingData = {};
                if (enumEditTypes.insert == currentEditType) {
                    trainingData = getJson($divTrainingConfigTrainingConfigInfo);
                    for (var p in trainingData) {
                        trainingData[p] = "";
                    }
                    trainingData.IsHide = "否";
                } else if (enumEditTypes.update == currentEditType) {
                    trainingData = getTrainingConfigListDataByTrainingId();
                }
                setJson($divTrainingConfigTrainingConfigInfo, trainingData);
                $divTrainingConfigTrainingConfigInfo.find("[name='trainingId']").val(trainingId);
            });
        }

        var initTrainingConfigSaveButton = function () {
            $mdlTrainingConfigTrainingConfigInfo.find("[name='saveTrainingConfig_TrainingConfigInfoConfirm']").on("click", function () {
                var checkResult = verifyForm($divTrainingConfigTrainingConfigInfo);
                if (!checkResult) {
                    return false;
                }
                var trainingData = getJson($divTrainingConfigTrainingConfigInfo);

                var functionName = "";
                if (currentEditType === enumEditTypes.update) {
                    functionName = "/UpdateTrainingInfo";
                } else if (currentEditType === enumEditTypes.insert) {
                    functionName = "/InsertTrainingInfo";
                }
                $divTrainingConfigTrainingConfigInfo.ajaxSubmit({
                    url: "/" + controllerName + functionName,
                    type: "post",
                    date: trainingData,
                    beforeSubmit: function (formArray, jqForm) {
                    },
                    success: function (jdata) {
                        if (jdata.isSuccess != null) {
                            alert(jdata.ErrorMessage);
                            return false;
                        }
                        alert("保存成功！");
                        $gridTrainingConfigMain.trigger("reloadGrid");
                        $mdlTrainingConfigTrainingConfigInfo.modal("toggle");
                        return true;

                    }
                });
                return false;
            });
        }

        initTrainingConfigInfoModel();
        initTrainingConfigSaveButton();
    }


    $(document).ready(function() {
        initQueryArea();
        initTrainingConfigJqGrid();
        initButtonArea();
        initTrainingConfigModel();
    });
})