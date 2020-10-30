'use strict';
$(function () {
    var controllerName = "VideoConfig";
    var $divVideoConfigQueryArea = $("#divVideoConfig_QueryArea");
    var $gridVideoConfigMain = $("#gridVideoConfig_Main");
    var $pagerVideoConfigMain = $("#pagerVideoConfig_Main");
    var $mdlVideoConfigVideoConfigInfo = $("#mdlVideoConfig_VideoConfigInfo");
    var $divVideoConfigVideoConfigInfo = $("#divVideoConfig_VideoConfigInfo");
    var enumEditTypes = { insert: 0, update: 1, view: 2 }
    var currentEditType;
    var videoId = 0;

    var initQueryArea = function () {
        var initQueryButton = function () {
            $("#btnVideoConfig_Query").on("click", function () {
                var queryData = {};
                var divQueryArea = $divVideoConfigQueryArea;
                queryData = getJson(divQueryArea);
                $gridVideoConfigMain.jqGrid("setGridParam", { page: 1, postData: queryData }).trigger("reloadGrid");
            });
        }

        initQueryButton();
    }

    var initVideoConfig = function () {
        var queryData = {};
        var divQueryArea = $divVideoConfigQueryArea;
        queryData = getJson(divQueryArea);
        $gridVideoConfigMain.jqGrid({
            url: "/" + controllerName + "/GetVideoConfigListForJqGrid",
            datatype: "json",
            mtype: "post",
            postData: queryData,
            colNames: ["VideoId", "视频名称", "视频编码", "描述", "是否有练习", "练习名称", "出题间隔(分钟)", "老师姓名","创建人", "创建时间"],
            colModel: [
                { name: "VideoId", index: "VideoId", width: 20, hidden: true },
                { name: "VideoName", index: "VideoName", align: "center", width: 200 },
                { name: "VideoCode", index: "VideoCode", align: "center", width: 150 },
                { name: "Description", index: "Description", align: "center", width: 150 },
                { name: "IsExercise", index: "IsExercise", align: "center", width: 70 },
                { name: "ExerciseId", index: "ExerciseId", align: "center", width: 200 },
                { name: "ExerciseInterval", index: "ExerciseInterval", align: "center", width: 100 },
                { name: "TeacherName", index: "TeacherName", align: "center", width: 100 },
                { name: "CreateBy", index: "CreateBy", align: "center", width: 130 },
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
            pager: $pagerVideoConfigMain,
            loadComplete: function () {
                var table = this;
                updatePagerIcons(table);
                jqGridAutoWidth();
                setGridHeight($gridVideoConfigMain.selector);
            }
        });
    }

    var initButtonArea = function () {

        //新增
        $("#btnVideoConfig_Insert").on("click", function () {
            currentEditType = enumEditTypes.insert;
            $mdlVideoConfigVideoConfigInfo.modal("show");
        });

        //修改
        $("#btnVideoConfig_Update").on("click", function () {
            var rowId = $gridVideoConfigMain.jqGrid("getGridParam", "selrow");
            if (isNull(rowId)) {
                alert("请选择数据！");
                return false;
            }

            var rowData = $gridVideoConfigMain.jqGrid("getRowData", rowId);

            videoId = rowData.VideoId;
            currentEditType = enumEditTypes.update;
            $mdlVideoConfigVideoConfigInfo.modal("show");
            return false;
        });

        //删除
        $("#btnVideoConfig_Delete").on("click", function () {

            var rowId = $gridVideoConfigMain.jqGrid("getGridParam", "selrow");
            if (isNull(rowId)) {
                alert("请选择数据！");
                return false;
            }

            var rowData = $gridVideoConfigMain.jqGrid("getRowData", rowId);

            videoId = rowData.VideoId;
            if (!confirm("确定要删除数据吗？")) {
                return false;
            }

            ajaxRequest({
                url: "/" + controllerName + "/DeleteVideo",
                data: { "videoId": videoId },
                type: "post",
                ansyc: false,
                success: function (jdata) {
                    if (jdata.IsSuccess != null) {
                        alert(jdata.ErrorMessage);
                        return false;
                    }

                    alert("删除成功！");
                    $gridVideoConfigMain.trigger("reloadGrid");
                    return true;
                }
            });
            return false;
        });
    }

    var initVideoConfigModel = function () {

        var initVideoConfigInfoModel = function () {
            $divVideoConfigVideoConfigInfo.find('input[name=VideoConfigImagePath]').change(function () {
                $divVideoConfigVideoConfigInfo.find('input[name=ImageFile]').val($(this).val());
            });

            $mdlVideoConfigVideoConfigInfo.on('show.bs.modal', function (e) {

                var videoData = {};
                videoData = getJson($divVideoConfigVideoConfigInfo);

                var initRadioCheck = function () {
                    $(":radio").change(function () {
                        var checked = $(this).prop('checked');

                        if (!checked) {
                            return false;
                        }

                        var val = $(this).val();
                        videoData.IsExercise = val;
                        isShow(videoData);
                    });
                }

                var isShow = function (videoData) {
                    
                    if (videoData.IsExercise == "否") {
                        $("#trVideoConfigExerciseId").hide();
                        $("#trVideoConfigExerciseInterval").hide();
                        $divVideoConfigVideoConfigInfo.find('input[name = ExerciseInterval]').removeAttr("data-verify", "int").removeAttr("data-verify-errormessage", "出题间隔为正整数,请检查!");
                    } else if (videoData.IsExercise == "是") {
                        $("#trVideoConfigExerciseId").show();
                        $("#trVideoConfigExerciseInterval").show();
                        $divVideoConfigVideoConfigInfo.find('input[name = ExerciseInterval]').attr("data-verify", "int").attr("data-verify-errormessage", "出题间隔为正整数,请检查!");
                    }
                }

                var getVideoConfigListDataByVideoId = function () {
                    var queryData = {};
                    var dataResult = {};
                    queryData.VideoId = videoId;
                    ajaxRequest({
                        url: "/" + controllerName + "/GetVideoInfoByVideoId",
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

                if (enumEditTypes.insert == currentEditType) {
                    for (var p in videoData) {
                        videoData[p] = "";
                    }
                    videoData.IsExercise = "是";
                    videoData.ExerciseInterval = 0;
                    isShow(videoData);

                } else if (enumEditTypes.update == currentEditType) {
                    videoData = getVideoConfigListDataByVideoId();
                    isShow(videoData);
                }

                setJson($divVideoConfigVideoConfigInfo, videoData);
                $divVideoConfigVideoConfigInfo.find("[name='videoId']").val(videoId);

                initRadioCheck();

            });
        }

        var initVideoConfigSaveButton = function () {
            $mdlVideoConfigVideoConfigInfo.find("[name='saveVideoConfig_VideoConfigInfoConfirm']").on("click", function () {

                var videoData = getJson($divVideoConfigVideoConfigInfo);
                if (videoData.IsExercise === "是") {
                    var exerciseid = videoData.ExerciseId;
                    if (isNull(exerciseid)) {
                        alert("练习名称为必填项,请检查!");
                        return false;
                    }
                }

                var checkResult = verifyForm($divVideoConfigVideoConfigInfo);
                if (!checkResult) {
                    return false;
                }

                var functionName = "";
                if (currentEditType === enumEditTypes.update) {
                    functionName = "/UpdateVideoInfo";
                } else if (currentEditType === enumEditTypes.insert) {
                    functionName = "/InsertVideoInfo";
                }
                $divVideoConfigVideoConfigInfo.ajaxSubmit({
                    url: "/" + controllerName + functionName,
                    type: "post",
                    date: videoData,
                    beforeSubmit: function (formArray, jqForm) {
                    },
                    success: function (jdata) {
                        if (jdata.isSuccess != null) {
                            alert(jdata.ErrorMessage);
                            return false;
                        }

                        alert("保存成功！");
                        $gridVideoConfigMain.trigger("reloadGrid");
                        $mdlVideoConfigVideoConfigInfo.modal("toggle");
                        return true;

                    }
                });
                return false;
            });
        }

        var initSelectExerciseNameQueryButton = function () {
            //练习名称下拉框绑定
            var initSelectExerciseName = function () {

                var $SelectExerciseNameDetails = $divVideoConfigVideoConfigInfo.find("[name='ExerciseId']");
                var getExerciseNameDetailsList = function () {
                    var dataResult = {};
                    ajaxRequest({
                        url: "/" + controllerName + "/GetExerciseNameDetailsList",
                        type: "post",
                        datatype: "json",
                        async: false,
                        success: function (jdata) {
                            dataResult = jdata;
                        }
                    });
                    return dataResult;
                }

                var listExerciseNameTypeDetails = getExerciseNameDetailsList();
                $SelectExerciseNameDetails.empty();
                var $optionAll = $("<option>");

                $optionAll.val("");

                $optionAll.text("请选择");
                $SelectExerciseNameDetails.append($optionAll);

                for (var i = 0; i < listExerciseNameTypeDetails.length; i++) {
                    var exerciseNameDetails = listExerciseNameTypeDetails[i];
                    var $option = $("<option>");
                    $option.val(exerciseNameDetails.ExerciseId);
                    $option.text(exerciseNameDetails.ExerciseNameDetails);
                    $SelectExerciseNameDetails.append($option);
                }
            }

            initSelectExerciseName();
        }

       
        initVideoConfigInfoModel();
        initVideoConfigSaveButton();
        initSelectExerciseNameQueryButton();
    }


    $(document).ready(function () {
        initQueryArea();
        initVideoConfig();
        initButtonArea();
        initVideoConfigModel();
    });
})