'use strict'
$(function () {
    var controllerName = "SubjectConfig";

    //章节列表
    var $divSubjectConfigSubjectQueryArea = $("#divSubjectConfig_SubjectQueryArea");
    var $gridSubjectConfigSubjectListMain = $("#gridSubjectConfig_SubjectListMain");
    var $mdlSubjectConfigSubjectListInfo = $("#mdlSubjectConfig_SubjectListInfo");
    var $divSubjectConfigSubjectListInfo = $("#divSubjectConfig_SubjectListInfo");
    var $divSubjectConfigSubjectList = $("#divSubjectConfig_SubjectList");
    var $pagerSubjectConfigSubjectListMain = $("#pagerSubjectConfig_SubjectListMain");

    //练习列表
    var $gridSubjectConfigExerciseListMain = $("#gridSubjectConfig_ExerciseListMain");
    var $mdlSubjectConfigExerciseListInfo = $("#mdlSubjectConfig_ExerciseListInfo");
    var $divSubjectConfigExerciseQueryArea = $("#divSubjectConfig_ExerciseQueryArea");
    var $gridSubjectConfigNextExerciseListMain = $("#gridSubjectConfig_NextExerciseListMain");
    var $pagerExerciseManageMain = $("#pagerExerciseManage_NextExerciseListMain");
    var $divSubjectConfigExerciseList = $("#divSubjectConfig_ExerciseList");
    var $pagerSubjectConfigExerciseListMain = $("#pagerSubjectConfig_ExerciseListMain");

    //视频列表
    var $gridSubjectConfigVideoListMain = $("#gridSubjectConfig_VideoListMain");
    var $mdlSubjectConfigVideoListInfo = $("#mdlSubjectConfig_VideoListInfo");
    var $divSubjectConfigVideoQueryArea = $("#divSubjectConfig_VideoQueryArea");
    var $gridSubjectConfigNextVideoListMain = $("#gridSubjectConfig_NextVideoListMain");
    var $pagerVideoManageMain = $("#pagerVideoManage_NextVideoListMain");
    var $divSubjectConfigVideoList = $("#divSubjectConfig_VideoList");
    var $pagerSubjectConfigVideoListMain = $("#pagerSubjectConfig_VideoListMain");

    var enumEditTypes = { insert: 0, update: 1 }
    var currentEditType;

    //调用onSelectRow事件时，保存的编号
    var selectedSubjectId = -1;

    //做查、删、改自身的编号
    var subjectId = 0;

    //初始化查询区域
    var initQueryArea = function () {

        //章节
        var initSubjectQueryButton = function () {
            $("#btnSubjectConfig_SubjectQuery").on("click", function () {
                var queryData = {};
                var divQueryArea = $divSubjectConfigSubjectQueryArea;
                queryData = getJson(divQueryArea);
                $gridSubjectConfigSubjectListMain.jqGrid("setGridParam", { page: 1, postData: queryData }).trigger("reloadGrid");
            });
        }

        //新增练习
        var initExerciseQueryButton = function () {
            $("#btnSubjectConfig_ExerciseQuery").on("click", function () {
                var queryData = {};
                var divQueryArea = $divSubjectConfigExerciseQueryArea;
                queryData = getJson(divQueryArea);
                $gridSubjectConfigNextExerciseListMain.jqGrid("setGridParam", { page: 1, postData: queryData }).trigger("reloadGrid");
            });
        }

        //新增视频
        var initVideoQueryButton = function () {
            $("#btnSubjectConfig_VideoQuery").on("click", function () {
                var queryData = {};
                var divQueryArea = $divSubjectConfigVideoQueryArea;
                queryData = getJson(divQueryArea);
                $gridSubjectConfigNextVideoListMain.jqGrid("setGridParam", { page: 1, postData: queryData }).trigger("reloadGrid");
            });
        }

        initSubjectQueryButton();
        initExerciseQueryButton();
        initVideoQueryButton();
    }


    //初始化所有列表
    var initGrid = function () {

        //章节列表
        var initSubjectGrid = function () {
            var queryData = {};
            var divQueryArea = $divSubjectConfigSubjectQueryArea;
            queryData = getJson(divQueryArea);
            $gridSubjectConfigSubjectListMain.jqGrid({
                url: "/" + controllerName + "/GetSubjectListForJqgrid",
                datatype: "json",
                mtype: "post",
                postData: queryData,
                height: window.innerHeight - $("#divSubjectConfig_SubjectListMain").offset().top - 50,
                colNames: ['SubjectId', '章节名称', '课程类型', "序号"],
                colModel: [
                   { name: "SubjectId", index: "SubjectId", width: 10, hidden: true },
                   { name: "SubjectName", index: "SubjectName", align: "center", width: 190 },
                   { name: "LessonTypeName", index: "LessonTypeName", align: "center", width: 130 },
                   { name: "Seq", index: "Seq", align: "center", width: 90 }
                ],
                multiselect: false,
                autowidth: true,
                rowNum: 999,
                altRows: true,
                pgbuttons: false,
                viewrecords: true,
                shrinkToFit: false,
                pginput: false,
                pager: $pagerSubjectConfigSubjectListMain,
                onSelectRow: function (rowId) {
                    var subjectId = $gridSubjectConfigSubjectListMain.jqGrid("getRowData", rowId).SubjectId;
                    if (isNull(subjectId)) {
                        subjectId = parseInt(subjectId);
                    }

                    var queryData = {};
                    queryData.subjectId = selectedSubjectId = subjectId;

                    //加载练习列表
                    $gridSubjectConfigExerciseListMain.jqGrid("setGridParam", { page: 1, postData: queryData }).trigger("reloadGrid");
                    //加载视频列表
                    $gridSubjectConfigVideoListMain.jqGrid("setGridParam", { page: 1, postData: queryData }).trigger("reloadGrid");


                },
                loadComplete: function () {
                    jqGridAutoWidth($("#divSubjectConfig_SubjectListMain"));
                    setGridHeight($gridSubjectConfigSubjectListMain.selector);
                }
            });
        }

        //章节练习关联列表
        var initSubjectExerciseGrid = function () {
            var postData = {};
            $gridSubjectConfigExerciseListMain.jqGrid({
                url: "/" + controllerName + "/GetRelSubjectExerciseListForJqGrid",
                postData: postData,
                datatype: "json",
                mtype: "post",
                height: window.innerHeight - $("#divSubjectConfig_ExerciseListMain").offset().top - 50,
                colNames: ['RelSubjectExerciseId', '练习名称', '试卷编码', '序号'],
                colModel: [
                    { name: "RelSubjectExerciseId", index: "RelSubjectExerciseId", width: 10, hidden: true },
                    { name: "ExerciseName", index: "ExerciseName", align: "center", width: 170 },
                    { name: "PaperCode", index: "PaperCode", align: "center", width: 140 },
                    { name: "Seq", index: "Seq", align: "center", width: 70 }
                ],
                multiselect: true,
                autowidth: true,
                rowNum: 50,
                altRows: true,
                pgbuttons: false,
                viewrecords: true,
                shrinkToFit: false,
                pginput: false,
                pager: $pagerSubjectConfigExerciseListMain,
                loadComplete: function (XMLHttpRequest) {
                    setGridHeight($gridSubjectConfigExerciseListMain.selector);

                    var getExerciseListBySubjectId = function () {
                        var queryData = {};
                        var dataResult = {};
                        queryData.subjectId = selectedSubjectId;

                        ajaxRequest({
                            url: "/" + controllerName + "/GetExerciseListBySubjectId",
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

                    getExerciseListBySubjectId();

                }
            });
        }

        //章节视频关联列表
        var initSubjectVideoGrid = function () {
            var postData = {};
            $gridSubjectConfigVideoListMain.jqGrid({
                url: "/" + controllerName + "/GetRelSubjectVideoListForJqGrid",
                postData: postData,
                datatype: "json",
                mtype: "post",
                height: window.innerHeight - $("#divSubjectConfig_VideoListMain").offset().top - 50,
                colNames: ['RelSubjectVideoId', '视频名称', '视频编码', '序号'],
                colModel: [
                    { name: "RelSubjectVideoId", index: "RelSubjectVideoId", width: 10, hidden: true },
                    { name: "VideoName", index: "VideoName", align: "center", width: 150 },
                    { name: "VideoCode", index: "VideoCode", align: "center", width: 150 },
                    { name: "Seq", index: "Seq", align: "center", width: 70 }
                ],
                multiselect: true,
                autowidth: true,
                rowNum: 50,
                altRows: true,
                pgbuttons: false,
                viewrecords: true,
                shrinkToFit: false,
                pginput: false,
                pager: $pagerSubjectConfigVideoListMain,
                loadComplete: function (XMLHttpRequest) {
                    setGridHeight($gridSubjectConfigVideoListMain.selector);

                    var getVideoListBySubjectId = function () {
                        var queryData = {};
                        var dataResult = {};
                        queryData.subjectId = selectedSubjectId;

                        ajaxRequest({
                            url: "/" + controllerName + "/GetVideoListBySubjectId",
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
                    getVideoListBySubjectId();
                }
            });
        }

        //练习列表
        var initExerciseGrid = function () {
            var queryData = {};
            var divQueryArea = $divSubjectConfigExerciseQueryArea;
            queryData = getJson(divQueryArea);
            $gridSubjectConfigNextExerciseListMain.jqGrid({
                url: "/" + controllerName + "/GetNextExerciseListForJqGrid",
                datatype: "json",
                mtype: "post",
                postData: queryData,
                colNames: ["ExerciseId", "练习名称", "试卷编号"],
                colModel: [
                    { name: "ExerciseId", index: "ExerciseId", width: 20, hidden: true },
                    { name: "ExerciseName", index: "ExerciseName", align: "center", width: 200 },
                    { name: "PaperCode", index: "PaperCode", align: "center", width: 155 }
                ],
                multiselect: true,
                autowidth: true,
                rowNum: 20,
                altRows: true,
                pgbuttons: true,
                viewrecords: true,
                width: "100%",
                shrinkToFit: false,
                pginput: true,
                rowList: [10, 20, 30, 50, 70, 100],
                pager: $pagerExerciseManageMain,
                loadComplete: function () {
                    var table = this;
                    updatePagerIcons(table);
                    jqGridAutoWidth();

                }
            });
        }

        //视频列表
        var initVideoGrid = function () {
            var queryData = {};
            var divQueryArea = $divSubjectConfigVideoQueryArea;
            queryData = getJson(divQueryArea);
            $gridSubjectConfigNextVideoListMain.jqGrid({
                url: "/" + controllerName + "/GetNextVideoListForJqGrid",
                datatype: "json",
                mtype: "post",
                postData: queryData,
                colNames: ["VideoId", "视频名称", "视频编码"],
                colModel: [
                    { name: "VideoId", index: "VideoId", width: 20, hidden: true },
                    { name: "VideoName", index: "VideoName", align: "center", width: 200 },
                    { name: "VideoCode", index: "VideoCode", align: "center", width: 155 }
                ],
                multiselect: true,
                autowidth: true,
                rowNum: 20,
                altRows: true,
                pgbuttons: true,
                viewrecords: true,
                width: "100%",
                shrinkToFit: false,
                pginput: true,
                rowList: [10, 20, 30, 50, 70, 100],
                pager: $pagerVideoManageMain,
                loadComplete: function () {
                    var table = this;
                    updatePagerIcons(table);
                    jqGridAutoWidth();
                }
            });
        }

        initSubjectGrid();
        initSubjectExerciseGrid();
        initSubjectVideoGrid();
       initExerciseGrid();
        initVideoGrid();
    }


    //初始化按钮区域
    var initButtonArea = function () {

        //章节
        var initSubjectButtonArea = function () {

            //新增
            $divSubjectConfigSubjectList.find("[name='btnSubjectInsert']").on("click", function () {
                currentEditType = enumEditTypes.insert;
                $mdlSubjectConfigSubjectListInfo.modal("show");
            });

            //修改
            $divSubjectConfigSubjectList.find("[name='btnSubjectUpdate']").on("click", function () {

                var rowId = $gridSubjectConfigSubjectListMain.jqGrid("getGridParam", "selrow");
                if (isNull(rowId)) {
                    alert("请选择数据！");
                    return false;
                }

                var rowData = $gridSubjectConfigSubjectListMain.jqGrid("getRowData", rowId);
                subjectId = rowData.SubjectId;
                currentEditType = enumEditTypes.update;
                $mdlSubjectConfigSubjectListInfo.modal("show");
                return false;
            });

            //删除
            $divSubjectConfigSubjectList.find("[name='btnSubjectDelete']").on("click", function () {

                var rowId = $gridSubjectConfigSubjectListMain.jqGrid("getGridParam", "selrow");
                if (isNull(rowId)) {
                    alert("请选择数据！");
                    return false;
                }

                var rowData = $gridSubjectConfigSubjectListMain.jqGrid("getRowData", rowId);

                var subjectId = rowData.SubjectId;
                if (!confirm("确定要删除数据吗？")) {
                    return false;
                }

                ajaxRequest({
                    url: "/" + controllerName + "/DeleteSubjectById",
                    data: { "SubjectId": subjectId },
                    type: "post",
                    ansyc: false,
                    success: function (jdata) {
                        if (jdata.isSuccess != null) {
                            alert(jdata.ErrorMessage);
                            return false;
                        }

                        alert("删除成功！");
                        $gridSubjectConfigSubjectListMain.trigger("reloadGrid");
                        var queryData = {};
                        queryData.subjectId = 0;
                        //加载练习列表
                        $gridSubjectConfigExerciseListMain.jqGrid("setGridParam", { page: 1, postData: queryData }).trigger("reloadGrid");
                        //加列视频列表
                        $gridSubjectConfigVideoListMain.jqGrid("setGridParam", { page: 1, postData: queryData }).trigger("reloadGrid");
                        return true;
                    }
                });
                return false;
            });
        }

        //章节练习关联列表
        var initExerciseButtonArea = function () {

            //新增
            $divSubjectConfigExerciseList.find("[name='btnExerciseInsert']").on("click", function () {
                if (selectedSubjectId === -1) {
                    alert("请选择章节！");
                    return false;
                }

                currentEditType = enumEditTypes.insert;
                $mdlSubjectConfigExerciseListInfo.modal("show");
                return true;
            });

            //删除
            $divSubjectConfigExerciseList.find("[name='btnExerciseDelete']").on("click", function () {

                var rowId = $gridSubjectConfigExerciseListMain.jqGrid("getGridParam", "selrow");
                if (isNull(rowId)) {
                    alert("请选择数据！");
                    return false;
                }

                if (!confirm("确定要删除数据吗？")) {
                    return false;
                }
                var arrRowid = $gridSubjectConfigExerciseListMain.jqGrid("getGridParam", "selarrrow");
                var relSubjectExerciseIdList = [];
                for (var i = 0; i < arrRowid.length; i++) {
                    var relSubjectExerciseId = $gridSubjectConfigExerciseListMain.jqGrid("getRowData", arrRowid[i]).RelSubjectExerciseId;
                    relSubjectExerciseIdList.push(relSubjectExerciseId);
                }

                var postData = {};
                postData.RelSubjectExerciseIdList = relSubjectExerciseIdList;
                var strData = JSON.stringify(postData);
                ajaxRequest({
                    url: "/" + controllerName + "/DelRelSubjectExercise",
                    data: { "strData": strData },
                    type: "post",
                    ansyc: false,
                    success: function (jdata) {
                        if (jdata.IsSuccess != null) {
                            alert(jdata.ErrorMessage);
                            return false;
                        }

                        alert("删除成功！");
                        $gridSubjectConfigExerciseListMain.trigger("reloadGrid");
                        return true;
                    }
                });
                return false;
            });
        }

        //章节视频关联列表
        var initVideoButtonArea = function () {

            //新增
            $divSubjectConfigVideoList.find("[name='btnVideoInsert']").on("click", function () {
                if (selectedSubjectId === -1) {
                    alert("请选择章节！");
                    return false;
                }
                currentEditType = enumEditTypes.insert;
                $mdlSubjectConfigVideoListInfo.modal("show");
                return true;
            });

            //删除
            $divSubjectConfigVideoList.find("[name='btnVideoDelete']").on("click", function () {
                var rowId = $gridSubjectConfigVideoListMain.jqGrid("getGridParam", "selrow");
                if (isNull(rowId)) {
                    alert("请选择数据！");
                    return false;
                }

                if (!confirm("确定要删除数据吗？")) {
                    return false;
                }
                var arrRowid = $gridSubjectConfigVideoListMain.jqGrid("getGridParam", "selarrrow");
                var relSubjectVideoIdList = [];
                for (var i = 0; i < arrRowid.length; i++) {
                    var relSubjectVideoId = $gridSubjectConfigVideoListMain.jqGrid("getRowData", arrRowid[i]).RelSubjectVideoId;
                    relSubjectVideoIdList.push(relSubjectVideoId);
                }

                var postData = {};
                postData.RelSubjectVideoIdList = relSubjectVideoIdList;
                var strData = JSON.stringify(postData);
                ajaxRequest({
                    url: "/" + controllerName + "/DelRelSubjectVideo",
                    data: { "strData": strData },
                    type: "post",
                    ansyc: false,
                    success: function (jdata) {
                        if (jdata.isSuccess != null) {
                            alert(jdata.ErrorMessage);
                            return false;
                        }

                        alert("删除成功！");
                        $gridSubjectConfigVideoListMain.trigger("reloadGrid");
                        return true;
                    }
                });
                return false;
            });
        }

        initSubjectButtonArea();
        initExerciseButtonArea();
        initVideoButtonArea();
    }

    //初始化Model
    var initModel = function () {

        //初始化章节Model
        var initSubjectModel = function () {

            $divSubjectConfigSubjectListInfo.find('input[name=SubjectConfigImagePath]').change(function () {
                $divSubjectConfigSubjectListInfo.find('input[name=ImageFile]').val($(this).val());
            });

            var initSubjectInfoModel = function () {

                var getSubjectListBySubjectId = function () {
                    var dataResult = {};
                    ajaxRequest({
                        url: "/" + controllerName + "/GetSubjectListBySubjectId",
                        data: {"SubjectId":subjectId},
                        type: "get",
                        datatype: "json",
                        async: false,
                        success: function (jdata) {
                            dataResult = jdata;
                        }
                    });
                    return dataResult;
                }

                $mdlSubjectConfigSubjectListInfo.on('show.bs.modal', function () {

                    var subjectData = {};

                    if (enumEditTypes.insert == currentEditType) {
                        subjectData = getJson($divSubjectConfigSubjectListInfo);
                        for (var s in subjectData) {
                            subjectData[s] = "";
                        }

                    } else if (enumEditTypes.update == currentEditType) {
                        subjectData = getSubjectListBySubjectId();
                    }

                    setJson($divSubjectConfigSubjectListInfo, subjectData);
                    $divSubjectConfigSubjectListInfo.find("[name='SubjectId']").val(subjectId);
                });
            }

            //保存
            var initSubjectSaveButton = function () {
                $mdlSubjectConfigSubjectListInfo.find("[name='saveSubjectConfig_SubjectSave']").on("click", function () {
                    var checkResult = verifyForm($divSubjectConfigSubjectListInfo);
                    if (!checkResult) {
                        return false;
                    }
                    var subjectData = getJson($divSubjectConfigSubjectListInfo);
                    var functionName = "";
                    if (currentEditType === enumEditTypes.update) {
                        functionName = "/UpdateSubject";
                    } else if (currentEditType === enumEditTypes.insert) {
                        functionName = "/InsertSubject";
                    }

                    $divSubjectConfigSubjectListInfo.ajaxSubmit({
                        url: "/" + controllerName + functionName,
                        type: "post",
                        date: subjectData,
                        beforeSubmit: function (formArray, jqForm) {

                        },
                        success: function (jdata) {
                            if (jdata.IsSuccess != null) {
                                alert(jdata.ErrorMessage);
                                return false;
                            }

                            alert("保存成功！");
                            $gridSubjectConfigSubjectListMain.trigger("reloadGrid");
                            $mdlSubjectConfigSubjectListInfo.modal("toggle");
                            return true;
                        }
                    });
                    return false;
                });
            }

            //课程类型下拉框绑定
            var initSelectLessonTypeQueryButton = function () {
                var $SelectLessonTypeDetails = $divSubjectConfigSubjectListInfo.find("[name='LessonTypeId']");
                var getLessonTypeDetailsList = function () {
                    var dataResult = {};
                    ajaxRequest({
                        url: "/" + controllerName + "/GetLessonTypeList",
                        type: "post",
                        datatype: "json",
                        async: false,
                        success: function (jdata) {
                            dataResult = jdata;
                        }
                    });
                    return dataResult;
                }

                var lessonTypeList = getLessonTypeDetailsList();
                var $optionAll = $("<option>");

                $optionAll.val("");

                $optionAll.text("全部");
                $SelectLessonTypeDetails.append($optionAll);

                for (var i = 0; i < lessonTypeList.length; i++) {
                    var lessonTypeInfo = lessonTypeList[i];
                    var $option = $("<option>");
                    $option.val(lessonTypeInfo.LessonTypeId);
                    $option.text(lessonTypeInfo.LessonTypeName);
                    $SelectLessonTypeDetails.append($option);
                }
            }

            initSubjectInfoModel();
            initSubjectSaveButton();
            initSelectLessonTypeQueryButton();
        }

        //初始化新增练习Model
        var initNextExerciseModel = function () {

            //保存
            var initExerciseSaveButton = function () {
                $mdlSubjectConfigExerciseListInfo.find("[name='saveSubjectConfig_ExerciseInfoConfirm']").on("click", function () {
                    var rowId = $gridSubjectConfigNextExerciseListMain.jqGrid("getGridParam", "selrow");
                    if (isNull(rowId)) {
                        alert("请选择数据！");
                        return false;
                    }

                    var arrRowid = $gridSubjectConfigNextExerciseListMain.jqGrid("getGridParam", "selarrrow");
                    var exerciseIdList = [];
                    for (var i = 0; i < arrRowid.length; i++) {
                        var exerciseId = $gridSubjectConfigNextExerciseListMain.jqGrid("getRowData", arrRowid[i]).ExerciseId;
                        exerciseIdList.push(exerciseId);
                    }
                    var subjectId = selectedSubjectId;
                    var postData = {};
                    postData.ExerciseIdList = [];
                    for (var j = 0; j < exerciseIdList.length; j++) {
                        postData.ExerciseIdList.push(exerciseIdList[j]);
                    }
                    postData.SubjectId = subjectId;
                    var strData = JSON.stringify(postData);
                    var ajaxOpt = {
                        url: "/" + controllerName + "/InsertRelSubjectExercise",
                        data: { "strData": strData },
                        type: "post",
                        async: false,
                        success: function (jdata) {
                            if (jdata.isSuccess != null) {
                                alert(jdata.ErrorMessage);
                                return false;
                            }

                            alert("保存成功！");
                            $gridSubjectConfigExerciseListMain.trigger("reloadGrid");
                            $mdlSubjectConfigExerciseListInfo.modal("toggle");
                            return true;
                        }
                    };
                    ajaxRequest(ajaxOpt);
                });
            }

            initExerciseSaveButton();
        }

        //初始化新增视频Model
        var initNextVideoModel = function () {

            //保存
            var initVideoSaveButton = function () {
                $mdlSubjectConfigVideoListInfo.find("[name='saveSubjectConfig_VideoInfoConfirm']").on("click", function () {

                    var rowId = $gridSubjectConfigNextVideoListMain.jqGrid("getGridParam", "selrow");
                    if (isNull(rowId)) {
                        alert("请选择数据！");
                        return false;
                    }

                    var arrRowid = $gridSubjectConfigNextVideoListMain.jqGrid("getGridParam", "selarrrow");
                    var videoIdList = [];
                    for (var i = 0; i < arrRowid.length; i++) {
                        var videoId = $gridSubjectConfigNextVideoListMain.jqGrid("getRowData", arrRowid[i]).VideoId;
                        videoIdList.push(videoId);
                    }
                    var subjectId = selectedSubjectId;
                    var postData = {};
                    postData.VideoIdList = [];
                    for (var j = 0; j < videoIdList.length; j++) {
                        postData.VideoIdList.push(videoIdList[j]);
                    }
                    postData.SubjectId = subjectId;
                    var strData = JSON.stringify(postData);
                    var ajaxOpt = {
                        url: "/" + controllerName + "/InsertRelSubjectVideo",
                        data: { "strData": strData },
                        type: "post",
                        async: false,
                        success: function (jdata) {
                            if (jdata.isSuccess != null) {
                                alert(jdata.ErrorMessage);
                                return false;
                            }

                            alert("保存成功！");
                            $gridSubjectConfigVideoListMain.trigger("reloadGrid");
                            $mdlSubjectConfigVideoListInfo.modal("toggle");
                            return true;
                        }
                    };
                    ajaxRequest(ajaxOpt);
                });
            }

            initVideoSaveButton();
        }

        initSubjectModel();
        initNextExerciseModel();
        initNextVideoModel();
    }


    $(document).ready(function () {
        initQueryArea();
        initGrid();
        initButtonArea();
        initModel();
    });
});