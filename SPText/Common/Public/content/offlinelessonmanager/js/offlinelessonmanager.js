'use strict';
$(function () {
    var controllerName = "OffLineLessonManager";
    var $divOffLineLessonManagerQueryArea = $("#divOffLineLessonManager_QueryArea");
    var $gridOffLineLessonManagerMain = $("#gridOffLineLessonManager_Main");
    var $pagerOffLineLessonManagerMain = $("#pagerOffLineLessonManager_Main");
    var $divOffLineLessonManagerOffLineLessonInfo = $("#divOffLineLessonManager_OffLineLessonInfo");
    var $mdlOffLineLessonManagerOffLineLessonInfo = $("#mdlOffLineLessonManager_OffLineLessonInfo");
    var $mdlOffLineLessonManagerLessonTypeInfo = $("#mdlOffLineLessonManager_LessonTypeInfo");
    var $gridOffLineLessonManagerLessonTypeMain = $("#gridOffLineLessonManager_LessonTypeMain");
    var $pagerOffLineLessonManagerLessonTypeMain = $("#pagerOffLineLessonManager_LessonTypeMain");
    var $divOffLineLessonManagerLessonTypeQueryArea = $("#divOffLineLessonManager_LessonTypeQueryArea");

    var $mdlOffLineLessonManagerTrainingTeacherInfo = $("#mdlOffLineLessonManager_TrainingTeacherInfo");
    var $gridOffLineLessonManagerTrainingTeacherMain = $("#gridOffLineLessonManager_TrainingTeacherMain");
    var $pagerOffLineLessonManagerTrainingTeacherMain = $("#pagerOffLineLessonManager_TrainingTeacherMain");
    var $divOffLineLessonManagerTrainingTeacherQueryArea = $("#divOffLineLessonManager_TrainingTeacherQueryArea");

    var enumEditTypes = { insert: 0, update: 1, view: 2 }
    var currentEdittype;
    var offLineLessonId = 0;

    var initChooseLessonTypeName = function () {
        var rowId = $gridOffLineLessonManagerLessonTypeMain.jqGrid("getGridParam", "selrow");
        if (isNull(rowId)) {
            alert("请选择数据！");
            return false;
        }

        var rowData = $gridOffLineLessonManagerLessonTypeMain.jqGrid("getRowData", rowId);

        $divOffLineLessonManagerOffLineLessonInfo.find("[name='LessonTypeId']").val(rowData.LessonTypeId);
        $divOffLineLessonManagerOffLineLessonInfo.find("[name='LessonTypeName']").val(rowData.LessonTypeNmae);


        $mdlOffLineLessonManagerLessonTypeInfo.modal("hide");
        $mdlOffLineLessonManagerOffLineLessonInfo.modal("show");
    }

    var initChooseInstitutionTeacher = function () {

        var rowId = $gridOffLineLessonManagerTrainingTeacherMain.jqGrid("getGridParam", "selrow");
        if (isNull(rowId)) {
            alert("请选择数据！");
            return false;
        }

        var rowData = $gridOffLineLessonManagerTrainingTeacherMain.jqGrid("getRowData", rowId);


        $divOffLineLessonManagerOffLineLessonInfo.find("[name=TeacherId]").val(rowData.TeacherId);
        $divOffLineLessonManagerOffLineLessonInfo.find("[name=TeacherName]").val(rowData.TeacherName);

        $mdlOffLineLessonManagerTrainingTeacherInfo.modal("hide");
        $mdlOffLineLessonManagerOffLineLessonInfo.modal("show");
    }

    var initQueryArea = function () {
        var initQueryButton = function () {
            $("#btnOffLineLessonManager_Query").on("click", function () {
                var queryData = {};
                var divQueryArea = $divOffLineLessonManagerQueryArea;
                queryData = getJson(divQueryArea);
                $gridOffLineLessonManagerMain.jqGrid("setGridParam", { postData: queryData }).trigger("reloadGrid");
            });
        }

        var initLessonTypeQueryButton = function () {
            $("#btnOffLineLessonManager_LessonTypeQuery").on("click", function () {
                var queryData = {};
                var divQueryArea = $divOffLineLessonManagerLessonTypeQueryArea;
                queryData = getJson(divQueryArea);
                $gridOffLineLessonManagerLessonTypeMain.jqGrid("setGridParam", { postData: queryData }).trigger("reloadGrid");
            });
        }

        var initTrainingTeacherQueryButton = function () {
            $("#btnOffLineLessonManager_TrainingTeacherQuery").on("click", function () {
                var queryData = {};
                var divQueryArea = $divOffLineLessonManagerTrainingTeacherQueryArea;
                queryData = getJson(divQueryArea);
                $gridOffLineLessonManagerTrainingTeacherMain.jqGrid("setGridParam", { postData: queryData }).trigger("reloadGrid");
            });
        }

        setInputAsDatetimePlug($divOffLineLessonManagerQueryArea.find("[name='LessonBeginTime']"), "yyyy-mm-dd hh:ii");
        initQueryButton();
        initLessonTypeQueryButton();
        initTrainingTeacherQueryButton();
    }

    var initJqGrid = function () {

        var initOffLineLessonManagerGrid = function () {
            var queryData = {};
            var divQueryArea = $divOffLineLessonManagerQueryArea;
            queryData = getJson(divQueryArea);
            $gridOffLineLessonManagerMain.jqGrid({
                url: "/" + controllerName + "/GetOffLineLessonListForJqgrid",
                datatype: "json",
                mtype: "post",
                postData: queryData,
                colNames: ["OffLineLessonId", "课程名称", "课程类型", "开课时间", "开课时长", "培训讲师", "关联培训学校", "是否报名", "是否打卡", "是否审核", "是否免费", "是否直播","是否公开", "报名人数", "打卡人数", "已审核人数", "备注"],
                colModel: [
                    { name: "OffLineLessonId", index: "OffLineLessonId", width: 60, hidden: true },
                    { name: "LessonName", index: "LessonName", align: "center", width: 150 },
                    { name: "LessonTypeName", index: "LessonTypeName", align: "center", width: 100 },
                    { name: "LessonBeginTime", index: "LessonBeginTime", align: "center", width: 100 },
                    { name: "Time", index: "Time", align: "center", width: 100 },
                    { name: "TeacherName", index: "TeacherName", align: "center", width: 100 },
                    { name: "TrainingInstitutionName", index: "TrainingInstitutionName", align: "center", width: 120 },
                    { name: "NeedRegister", index: "NeedRegister", align: "center", width: 100 },
                    { name: "IsPunchClock", index: "IsPunchClock", align: "center", width: 130 },
                    { name: "NeedCheck", index: "NeedCheck", align: "center", width: 100 },
                    { name: "IsFree", index: "IsFree", align: "center", width: 100 },
                    { name: "IsLive", index: "IsLive", align: "center", width: 100 },
                    { name: "IsOvert", index: "IsOvert", align: "center", width: 100 },
                    { name: "RegisterCount", index: "RegisterCount", align: "center", width: 80 },
                    { name: "PunchTheClockCount", index: "PunchTheClockCount", align: "center", width: 80 },
                    { name: "ApproveCount", index: "ApproveCount", align: "center", width: 90 },
                    { name: "Remark", index: "Remark", align: "center", width: 120 }
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
                pager: $pagerOffLineLessonManagerMain,
                subGrid: true,
                subGridOptions: {
                    "plusicon": "ace-icon fa fa-plus",
                    "minusicon": "ace-icon fa fa-minus",
                    "openicon": "ace-icon fa fa-share"
                },
                subGridRowExpanded: function (subgrid_id, row_id) {
                    var subgrid_table_id, pager_id;
                    var rowData = $gridOffLineLessonManagerMain.jqGrid("getRowData", row_id);
                    subgrid_table_id = subgrid_id + "_t";
                    pager_id = "p_" + subgrid_table_id;
                    $("#" + subgrid_id).html("<div style='width:100%;overflow:auto'><table id='" + subgrid_table_id + "' class='scroll' ></table></div>");
                    var subGridQueryData = {};

                    subGridQueryData.OffLineLessonId = rowData.OffLineLessonId;
                    $("#" + subgrid_table_id).jqGrid({
                        url: "/" + controllerName + "/GetUserRegisterInfoListForJqGrid",
                        datatype: "json",
                        postData: subGridQueryData,
                        rownumbers: true,
                        rowNum: 10,
                        colNames: ["RelUserRegisterId", "姓名", "身份证号", "报名时间", "打卡状态", "打卡时间", "审核状态", "审核时间", "备注"],
                        colModel: [
                            { name: "RelUserRegisterId", index: "RelUserRegisterId", width: "10", align: "center", hidden: true },
                            { name: "UserName", index: "UserName", align: "center", width: 100 },
                            { name: "UserCode", index: "UserCode", align: "center", width: 150 },
                            { name: "RegisterTime", index: "RegisterTime", align: "center", width: 150 },
                            { name: "IsPunchClock", index: "IsPunchClock", align: "center", width: 100 },
                            { name: "PunchClockTime", index: "PunchClockTime", align: "center", width: 150 },
                            { name: "ApproveStatus", index: "ApproveStatus", align: "center", width: 100 },
                            { name: "ApproveTime", index: "ApproveTime", align: "center", width: 150 },
                            { name: "Remark", index: "Remark", align: "center", width: 120 }
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
                    setGridHeight($gridOffLineLessonManagerMain.selector);
                }
            });
        }

        var initLessonTypeGrid = function () {
            var $queryData = {};
            $queryData = getJson($divOffLineLessonManagerLessonTypeQueryArea);
            $gridOffLineLessonManagerLessonTypeMain.jqGrid({
                url: "/" + controllerName + "/GetLessonTypeForJqgrid",
                datatype: "json",
                postData: $queryData,
                colNames: ["LessonTypeId", "培训类型名称", "课程类型名称"],
                colModel: [
                    { name: "LessonTypeId", index: "LessonTypeId", width: 60, hidden: true },
                    { name: "TrainingTypeName", index: "TrainingTypeName", align: "center", width: 150 },
                    { name: "LessonTypeNmae", index: "LessonTypeNmae", align: "center", width: 150 }
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
                pager: $pagerOffLineLessonManagerLessonTypeMain,
                //JqGrid双击事件
                ondblClickRow: function (rowid, iRow, iCol, e) {
                    initChooseLessonTypeName();
                },
                loadComplete: function () {
                    var table = this;
                    updatePagerIcons(table);
                    jqGridAutoWidth();
                }
            });
        }

        var initTrainingTeacherGrid = function () {
            var $queryData = {};
            $queryData = getJson($divOffLineLessonManagerTrainingTeacherQueryArea);
            $gridOffLineLessonManagerTrainingTeacherMain.jqGrid({
                url: "/" + controllerName + "/GetTrainingTeacherForJqgrid",
                datatype: "json",
                postData: $queryData,
                colNames: ["TeacherId", "培训讲师名称"],
                colModel: [
                    { name: "TeacherId", index: "TeacherId", width: 60, hidden: true },
                    { name: "TeacherName", index: "TeacherName", align: "center", width: 150 }
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
                pager: $pagerOffLineLessonManagerTrainingTeacherMain,
                //JqGrid双击事件
                ondblClickRow: function (rowid, iRow, iCol, e) {
                    initChooseInstitutionTeacher();
                },
                loadComplete: function () {
                    var table = this;
                    updatePagerIcons(table);
                    jqGridAutoWidth();
                }
            });
        }

        initOffLineLessonManagerGrid();
        initLessonTypeGrid();
        initTrainingTeacherGrid();
    }

    var initButtonArea = function () {

        //新增
        $("#btnInsert").on("click", function () {
            currentEdittype = enumEditTypes.insert;
            $mdlOffLineLessonManagerOffLineLessonInfo.modal("show");

            var offLineLessonData = {};

            offLineLessonData = getJson($divOffLineLessonManagerOffLineLessonInfo);

            for (var p in offLineLessonData) {
                if (offLineLessonData.hasOwnProperty(p)) {
                    offLineLessonData[p] = "";
                }
            }

            offLineLessonData.NeedRegister = "是";
            offLineLessonData.IsFree = "是";
            offLineLessonData.IsPunchClock = "是";
            offLineLessonData.IsLive = "是";
            offLineLessonData.NeedCheck = "是";
            offLineLessonData.IsOvert = "是";


            setJson($divOffLineLessonManagerOffLineLessonInfo, offLineLessonData);
        });

        //修改
        $("#btnUpdate").on("click", function () {

            var rowId = $gridOffLineLessonManagerMain.jqGrid("getGridParam", "selrow");
            if (isNull(rowId)) {
                alert("请选择数据！");
                return false;
            }

            var rowData = $gridOffLineLessonManagerMain.jqGrid("getRowData", rowId);

            offLineLessonId = rowData.OffLineLessonId;
            currentEdittype = enumEditTypes.update;
            $mdlOffLineLessonManagerOffLineLessonInfo.modal("show");

            var getOffLineLessonManagerListDataByExerciseId = function () {
                var queryData = {};
                var dataResult = {};
                queryData.OffLineLessonId = offLineLessonId;
                ajaxRequest({
                    url: "/" + controllerName + "/GetOffLineLessonListById",
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

            var offLineLessonData = {};

            offLineLessonData = getOffLineLessonManagerListDataByExerciseId();
            $divOffLineLessonManagerOffLineLessonInfo.find("[name='offLineLessonId']").val(offLineLessonId);

            setJson($divOffLineLessonManagerOffLineLessonInfo, offLineLessonData);

            return false;
        });

        //删除
        $("#btnDelete").on("click", function () {

            var rowId = $gridOffLineLessonManagerMain.jqGrid("getGridParam", "selrow");
            if (isNull(rowId)) {
                alert("请选择数据！");
                return false;
            }

            var rowData = $gridOffLineLessonManagerMain.jqGrid("getRowData", rowId);

            var offLineLessonId = rowData.OffLineLessonId;
            if (!confirm("确定要删除数据吗？")) {
                return false;
            }

            ajaxRequest({
                url: "/" + controllerName + "/DeleteOffLineLesson",
                data: { "OffLineLessonId": offLineLessonId },
                type: "post",
                ansyc: false,
                success: function (jdata) {
                    if (jdata.IsSuccess != null) {
                        alert(jdata.ErrorMessage);
                        return false;
                    }

                    alert("删除成功！");
                    $gridOffLineLessonManagerMain.trigger("reloadGrid");
                    return true;
                }
            });
            return false;
        });

        //添加课程类型
        $("#btnOffLineLessonManager_LessonTypeId").on("click", function() {
            $mdlOffLineLessonManagerOffLineLessonInfo.modal("toggle");
            $mdlOffLineLessonManagerLessonTypeInfo.modal("show");
        });

        //添加培训讲师
        $("#btnOffLineLessonManager_TeacherId").on("click", function () {
            $mdlOffLineLessonManagerOffLineLessonInfo.modal("toggle");
            $mdlOffLineLessonManagerTrainingTeacherInfo.modal("show");
        });

    }

    var initModel = function () {

        var iniOffLineLessonManagerInfoModel = function () {

            setInputAsDatetimePlug($divOffLineLessonManagerOffLineLessonInfo.find("[name='LessonBeginTime']"), "yyyy-mm-dd hh:ii");

            $divOffLineLessonManagerOffLineLessonInfo.find('input[name=OffLineLessonImagePath]').change(function () {
                $divOffLineLessonManagerOffLineLessonInfo.find('input[name=OffLineLessonImageFile]').val($(this).val());
            });

            $divOffLineLessonManagerOffLineLessonInfo.find('input[name=LessonSynopsisImagePath]').change(function () {
                $divOffLineLessonManagerOffLineLessonInfo.find('input[name=LessonSynopsisImageFile]').val($(this).val());
            });

            var initOffLineLessonManagerSaveButton = function() {
                $("#btnOffLineLessonManager_SaveInfo").on("click", function() {
                    var checkResult = verifyForm($divOffLineLessonManagerOffLineLessonInfo);
                    if (!checkResult) {
                        return false;
                    }

                    var functionName = "";
                    if (currentEdittype === enumEditTypes.update) {
                        functionName = "/UpdateOffLineLesson";
                    } else if (currentEdittype === enumEditTypes.insert) {
                        functionName = "/InsertOffLineLesson";
                    }

                    $divOffLineLessonManagerOffLineLessonInfo.ajaxSubmit({
                        url: "/" + controllerName + functionName,
                        type: "post",
                        beforeSubmit: function(formArray, jqForm) {
                        },
                        success: function(jdata) {
                            if (jdata.IsSuccess != null) {
                                alert(jdata.ErrorMessage);
                                return false;
                            }

                            alert("操作成功！");
                            $gridOffLineLessonManagerMain.trigger("reloadGrid");
                            $mdlOffLineLessonManagerOffLineLessonInfo.modal("toggle");
                            return true;
                        }
                    });
                    return false;
                });
            }

            initOffLineLessonManagerSaveButton();
        }

        var initLessonTypeModal = function () {


            $mdlOffLineLessonManagerLessonTypeInfo.on('show.bs.modal', function(e) {
                $mdlOffLineLessonManagerLessonTypeInfo.find("[name='LessonTypeName']").val();

            });

            $mdlOffLineLessonManagerLessonTypeInfo.on('hide.bs.modal', function(e) {

                $mdlOffLineLessonManagerOffLineLessonInfo.modal("toggle");
            });

            $("#btnOffLineLessonManager_LessonTypeConfirm").on("click", function() {
                initChooseLessonTypeName();
            });
        }

        var initTrainingTeacherModal = function() {
            $mdlOffLineLessonManagerTrainingTeacherInfo.on('show.bs.modal', function(e) {
                $mdlOffLineLessonManagerLessonTypeInfo.find("[name='InstitutionTeacherName']").val();

            });

            $mdlOffLineLessonManagerTrainingTeacherInfo.on('hide.bs.modal', function(e) {

                $mdlOffLineLessonManagerOffLineLessonInfo.modal("toggle");
            });


            $("#btnOffLineLessonManager_InstitutionTeacherConfirm").on("click", function() {
                initChooseInstitutionTeacher();
            });

        }

        iniOffLineLessonManagerInfoModel();
        initLessonTypeModal();
        initTrainingTeacherModal();
    }


    //页面加载时运行
    $(document).ready(function () {
        initQueryArea();
        initJqGrid();
        initButtonArea();
        initModel();
    });
})