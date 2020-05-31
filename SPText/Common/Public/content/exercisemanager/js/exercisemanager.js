'use strict'
$(function () {
    var controllerName = "ExerciseManager";
    var $divExerciseManageQueryArea = $("#divExerciseManage_QueryArea");
    var $gridExerciseManageMain = $("#gridExerciseManage_Main");
    var $pagerExerciseManageMain = $("#pagerExerciseManage_Main");
    var $divExerciseManageExerciseInfo = $("#divExerciseManage_ExerciseInfo");
    var $mdlExerciseManageExerciseInfo = $("#mdlExerciseManage_ExerciseInfo");
    var enumEditTypes = { insert: 0, update: 1, view: 2 }
    var currentEdittype;
    var exerciseId = 0;

    var initQueryArea = function () {
        var initQueryButton = function () {
            $("#btnExerciseManage_Query").on("click", function () {
                var queryData = {};
                var divQueryArea = $divExerciseManageQueryArea;
                queryData = getJson(divQueryArea);
                $gridExerciseManageMain.jqGrid("setGridParam", { postData: queryData }).trigger("reloadGrid");
            });
        }
        initQueryButton();
    }

    var initExerciseManageGrid = function () {
        var queryData = {};
        var divQueryArea = $divExerciseManageQueryArea;
        queryData = getJson(divQueryArea);
        $gridExerciseManageMain.jqGrid({
            url: "/" + controllerName + "/GetExerciseInfoListForJqgrid",
            datatype: "json",
            mtype: "post",
            postData: queryData,
            colNames: ["ExerciseId", "练习名称", "试卷编号", "是否随机出题", "是否显示答案", "是否限制时长", "限制时长(分钟)", "是否存储答案"],
            colModel: [
                { name: "ExerciseId", index: "ExerciseId", width: 60, hidden: true },
                { name: "ExerciseName", index: "ExerciseName", align: "center", width: 150 },
                { name: "PaperCode", index: "PaperCode", align: "center", width: 100 },
                { name: "IsRandom", index: "IsRandom", align: "center", width: 100 },
                { name: "IsShowRightAnswer", index: "IsShowRightAnswer", align: "center", width: 100 },
                { name: "IsLimitTime", index: "IsLimitTime", align: "center", width: 100 },
                { name: "LimitTime", index: "LimitTime", align: "center", width: 130 },
                { name: "IsSaveAnswer", index: "IsSaveAnswer", align: "center", width: 100 }
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
            pager: $pagerExerciseManageMain,
            loadComplete: function () {
                var table = this;
                updatePagerIcons(table);
                jqGridAutoWidth();
               setGridHeight($gridExerciseManageMain.selector);
            }
        });
    }

    var initButtonArea = function () {

        //新增
        $("#divExerciseManage_ExerciseList").find("[name='btnExerciseInsert']").on("click", function() {
            currentEdittype = enumEditTypes.insert;
            $mdlExerciseManageExerciseInfo.modal("show");
        });

        //修改
        $("#divExerciseManage_ExerciseList").find("[name='btnExerciseUpdate']").on("click", function () {

            var rowId = $gridExerciseManageMain.jqGrid("getGridParam", "selrow");
            if (isNull(rowId)) {
                alert("请选择数据！");
                return false;
            }

            var rowData = $gridExerciseManageMain.jqGrid("getRowData", rowId);

            exerciseId = rowData.ExerciseId;
            currentEdittype = enumEditTypes.update;
            $mdlExerciseManageExerciseInfo.modal("show");
            return false;
        });

        //删除
        $("#divExerciseManage_ExerciseList").find("[name='btnExerciseDelete']").on("click", function () {

            var rowId = $gridExerciseManageMain.jqGrid("getGridParam", "selrow");
            if (isNull(rowId)) {
                alert("请选择数据！");
                return false;
            }

            var rowData = $gridExerciseManageMain.jqGrid("getRowData", rowId);

            var arrPid = rowData.ExerciseId;
            if (!confirm("确定要删除数据吗？")) {
                return false;
            }

            ajaxRequest({
                url: "/" + controllerName + "/DeleteExerciseManager",
                data: { "ExerciseManageIdList": arrPid },
                type: "post",
                ansyc: false,
                success: function (jdata) {
                    if (jdata.IsSuccess != null) {
                        alert(jdata.ErrorMessage);
                        return false;
                    }

                    alert("删除成功！");
                    $gridExerciseManageMain.trigger("reloadGrid");
                    return true;
                }
            });
            return false;
        });
    }

    var initExerciseManageModel = function () {

        var iniExerciseManageInfoModel = function () {
            $mdlExerciseManageExerciseInfo.on('show.bs.modal', function (e) {

                var exerciseData = {};
                exerciseData = getJson($divExerciseManageExerciseInfo);

                var initRadioCheck = function () {
                    $divExerciseManageExerciseInfo.find('input[name=IsLimitTime]').change(function () {
                        var checked = $(this).prop('checked');

                        if (!checked) {
                            return false;
                        }

                        var val = $(this).val();
                        exerciseData.IsLimitTime = val;
                        isShow(exerciseData);
                    });
                }

                var isShow = function (exerciseData) {
  
                    if (exerciseData.IsLimitTime === "否") {
                        $("#trExerciseManageLimitTime").hide();
                        $divExerciseManageExerciseInfo.find('input[name = LimitTime]').removeAttr("data-verify", "int").removeAttr("data-verify-errormessage", "限制时长必须是正整数，请检查!");
                    } else if (exerciseData.IsLimitTime === "是") {
                        $("#trExerciseManageLimitTime").show();
                        $divExerciseManageExerciseInfo.find('input[name = LimitTime]').attr("data-verify", "int").attr("data-verify-errormessage", "限制时长必须是正整数，请检查!");
                    }
                }

                var getExerciseManageListDataByExerciseId = function() {
                    var queryData = {};
                    var dataResult = {};
                    queryData.ExerciseId = exerciseId;
                    ajaxRequest({
                        url: "/" + controllerName + "/GetExerciseManageListDataByExerciseId",
                        data: queryData,
                        type: "get",
                        datatype: "json",
                        async: false,
                        success: function(jdata) {
                            dataResult = jdata;
                        }
                    });
                    return dataResult;
                }

                var currentedittype = currentEdittype;

                if (enumEditTypes.insert == currentedittype) {

                    for (var p in exerciseData) {
                        exerciseData[p] = "";
                    }

                    exerciseData.IsRandom = "是";
                    exerciseData.IsOrder = "是";
                    exerciseData.IsShowRightAnswer = "是";
                    exerciseData.IsLimitTime = "是";
                    exerciseData.IsSaveAnswer = "是";
                    exerciseData.LimitTime = 0;
                    isShow(exerciseData);

                } else if (enumEditTypes.update == currentedittype || enumEditTypes.view == currentedittype) {
                    exerciseData = getExerciseManageListDataByExerciseId();
                    isShow(exerciseData);
                }

                setJson($divExerciseManageExerciseInfo, exerciseData);

                if (enumEditTypes.view == currentedittype) {
                    for (var p in exerciseData) {
                        $divExerciseManageExerciseInfo.find("[name='" + p + "']").prop("disabled", true);
                    }
                    $mdlExerciseManageExerciseInfo.find("[name='saveExerciseManage_ExerciseInfoConfirm']").addClass("hidden");
                } else {
                    for (var p in exerciseData) {
                        $divExerciseManageExerciseInfo.find("[name='" + p + "']").prop("disabled", false);
                    }
                    $mdlExerciseManageExerciseInfo.find("[name='saveExerciseManage_ExerciseInfoConfirm']").removeClass("hidden");
                    $divExerciseManageExerciseInfo.find("[name='exerciseId']").val(exerciseId);
                }

                initRadioCheck();

            });
        }

        var initExerciseManageSaveButton = function () {
            $mdlExerciseManageExerciseInfo.find("[name='saveExerciseManage_ExerciseInfoConfirm']").on("click", function () {
                var checkResult = verifyForm($divExerciseManageExerciseInfo);
                if (!checkResult) {
                    return false;
                }

                var exerciseData = getJson($divExerciseManageExerciseInfo);

                var functionName = "";
                if (currentEdittype === enumEditTypes.update) {
                    functionName = "/UpdateExerciseManager";
                } else if (currentEdittype === enumEditTypes.insert) {
                    functionName = "/InsertExerciseManager";
                }

                $divExerciseManageExerciseInfo.ajaxSubmit({
                    url: "/" + controllerName + functionName,
                    type: "post",
                    date: exerciseData,
                    beforeSubmit: function(formArray, jqForm) {
                    },
                    success: function (jdata) {
                        if (jdata.IsSuccess !=null) {
                            alert(jdata.ErrorMessage);
                            return false;
                        }

                        alert("操作成功！");
                        $gridExerciseManageMain.trigger("reloadGrid");
                        $mdlExerciseManageExerciseInfo.modal("toggle");
                        return true;
                    }
                });
                return false;
            });
        }

        iniExerciseManageInfoModel();
        initExerciseManageSaveButton();
    }

    //页面加载时运行
    $(document).ready(function() {
        initQueryArea();
        initExerciseManageGrid();
        initButtonArea();
        initExerciseManageModel();
    });
})