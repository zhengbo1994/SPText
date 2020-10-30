'use strict';
$(function() {
    var controllerName = "TeacherPermissionManage";

    //培训讲师列表
    var $divTrainingTeacherQueryArea = $("#divTrainingTeacher_QueryArea");
    var $gridTrainingTeacherMain = $("#gridTrainingTeacher_Main");
    var $pagerTrainingTeacherMain = $("#pagerTrainingTeacher_Main");

    //讲师课程关联列表
    var $gridTrainingTeacherLessonTypeMain = $("#gridTrainingTeacherLessonType_Main");
    var $pagerTrainingTeacherLessonTypeMain = $("#pagerTrainingTeacherLessonType_Main");

    //调用onSelectRow事件时，保存的编号
    var selectedTeacherId = -1;

    var initQueryArea = function() {
        var initTrainingTeacherQuery = function() {
            $("#btnTrainingTeacher_Query").on("click", function() {
                var queryData = {};
                var divQueryArea = $divTrainingTeacherQueryArea;
                queryData = getJson(divQueryArea);
                $gridTrainingTeacherMain.jqGrid("setGridParam", { postData: queryData }).trigger("reloadGrid");
            });
        }


        initTrainingTeacherQuery();
    }

    var initJqGrid = function() {

        var initTrainingTeacherGrid = function() {
            var $queryData = {};
            $queryData = getJson($divTrainingTeacherQueryArea);
            $gridTrainingTeacherMain.jqGrid({
                url: "/" + controllerName + "/GetTrainingTeacherInfoListForJqgrid",
                datatype: "json",
                postData: $queryData,
                colNames: ["TeacherId", "讲师姓名", "身份证号", "联系电话", "所属培训机构", "InstitutionId", "讲师介绍"],
                colModel: [
                    { name: "TeacherId", index: "TeacherId", width: 10, hidden: true },
                    { name: "TeacherName", index: "TeacherName", align: "center", width: 150 },
                    { name: "IdNumber", index: "IdNumber", align: "center", width: 200 },
                    { name: "ContactNumber", index: "ContactNumber", align: "center", width: 150 },
                    { name: "InstitutionName", index: "InstitutionName", align: "center", width: 210 },
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
                pager: $pagerTrainingTeacherMain,
                onSelectRow: function(rowId) {
                    var teacherId = $gridTrainingTeacherMain.jqGrid("getRowData", rowId).TeacherId;
                    if (isNull(teacherId)) {
                        teacherId = parseInt(teacherId);
                    }

                    var queryData = {};
                    queryData.teacherId = selectedTeacherId = teacherId;

                    //加载课程列表
                    $gridTrainingTeacherLessonTypeMain.jqGrid("setGridParam", { page: 1, postData: queryData }).trigger("reloadGrid");
                },
                loadComplete: function() {
                    var table = this;
                    updatePagerIcons(table);
                    jqGridAutoWidth();
                    setGridHeight($gridTrainingTeacherMain.selector);
                }
            });
        }

        var initLessonGrid = function() {
            var postData = {};
            $gridTrainingTeacherLessonTypeMain.jqGrid({
                url: "/" + controllerName + "/GetLessonTypeListForJqGrid",
                postData: postData,
                datatype: "json",
                mtype: "post",
                colNames: ["LessonTypeId", "课程名称"],
                colModel: [
                    { name: "LessonTypeId", index: "LessonTypeId", width: 50, hidden: true },
                    { name: "LessonTypeName", index: "LessonTypeName", align: "center", width: 200 }
                ],
                multiselect: true,
                autowidth: true,
                rowNum: 50,
                altRows: true,
                pgbuttons: false,
                viewrecords: true,
                shrinkToFit: false,
                pginput: false,
                pager: $pagerTrainingTeacherLessonTypeMain,
                loadComplete: function(XMLHttpRequest) {
                    var table = this;
                    updatePagerIcons(table);
                    jqGridAutoWidth();
                    setGridHeight($gridTrainingTeacherLessonTypeMain.selector);

                    var getLessonTypeListByTrainingTeacherId = function() {
                        var queryData = {};
                        var dataResult = {};
                        queryData.teacherId = selectedTeacherId;

                        ajaxRequest({
                            url: "/" + controllerName + "/GetLessonTypeIdListByTrainingTeacherId",
                            data: queryData,
                            type: "get",
                            datatype: "json",
                            async: false,
                            success: function(jdata) {
                                dataResult = jdata;
                            },
                            error: function() {
                                dataResult = null;
                            }
                        });
                        return dataResult;
                    }

                    var lessonTypeIdList = getLessonTypeListByTrainingTeacherId();

                    if (lessonTypeIdList.length != 0) {
                        for (var i = 0; i < lessonTypeIdList.length; i++) {
                            for (var j = 0; j < XMLHttpRequest.length; j++) {
                                if (lessonTypeIdList[i].LessonTypeId == XMLHttpRequest[j].LessonTypeId) {
                                    $gridTrainingTeacherLessonTypeMain.jqGrid('setSelection', j + 1);
                                }
                            }
                        }
                    }
                }
            });
        }

        initTrainingTeacherGrid();
        initLessonGrid();
    }

    var initButtonArea = function() {

        //保存
        $("#btnTrainingTeacherLessonType_Save").on("click", function() {
            if (selectedTeacherId === -1) {
                alert("请选择培训讲师！");
                return false;
            }

            var arrRowid = $gridTrainingTeacherLessonTypeMain.jqGrid("getGridParam", "selarrrow");
            var lessonTypeIdList = [];

            for (var i = 0; i < arrRowid.length; i++) {
                var lessonTypeId = $gridTrainingTeacherLessonTypeMain.jqGrid("getRowData", arrRowid[i]).LessonTypeId;
                lessonTypeIdList.push(lessonTypeId);
            }

            var teacherId = selectedTeacherId;

            var postData = {};
            postData.lessonTypeIdList = [];
            for (var j = 0; j < lessonTypeIdList.length; j++) {
                postData.lessonTypeIdList.push(lessonTypeIdList[j]);
            }

            postData.teacherId = teacherId;
            var strData = JSON.stringify(postData);
            var ajaxOpt = {
                url: "/" + controllerName + "/InsertRelTrainingTeacherLessonType",
                data: { "strData": strData },
                type: "post",
                async: false,
                success: function(jdata) {
                    if (jdata.isSuccess != null) {
                        alert(jdata.ErrorMessage);
                        return false;
                    }
                    alert("保存成功！");
                    $gridTrainingTeacherLessonTypeMain.trigger("reloadGrid");
                    return true;
                }
            };
            ajaxRequest(ajaxOpt);

        });
    }

    $(document).ready(function() {
        initQueryArea();
        initJqGrid();
        initButtonArea();
    });
});