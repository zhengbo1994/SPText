'use strict';
$(function() {
    var controllerName = "TrainingRecordQuery";
    var $divTrainingRecordQueryQueryArea = $('#divTrainingRecordQuery_QueryArea');
    var $gridTrainingRecordQueryMain = $("#gridTrainingRecordQuery_Main");
    var $pagerTrainingRecordQueryMain = $("#pagerTrainingRecordQuery_Main");


    var initQueryArea = function() {
        var initQueryButton = function() {
            $("#btnTrainingRecordQuery_Query").on("click", function() {
                var queryData = {};
                var divQueryArea = $divTrainingRecordQueryQueryArea;
                queryData = getJson(divQueryArea);
                $gridTrainingRecordQueryMain.jqGrid("setGridParam", { postData: queryData }).trigger("reloadGrid");
            });
        }


        initQueryButton();
    }

    var initJqGrid = function() {
        var queryData = {};
        var divQueryArea = $divTrainingRecordQueryQueryArea;
        queryData = getJson(divQueryArea);
        $gridTrainingRecordQueryMain.jqGrid({
            url: "/" + controllerName + "/GetActiveCodeUseRecordUserListForJqGrid",
            datatype: "json",
            mtype: "post",
            postData: queryData,
            colNames: ["UserId", "姓名", "证件编号"],
            colModel: [
                { name: "UserId", index: "UserId", width: 10, hidden: true },
                { name: "UserName", index: "UserName", align: "center", width: 455 },
                { name: "UserCode", index: "UserCode", align: "center", width: 500 }
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
            pager: $pagerTrainingRecordQueryMain,
            subGrid: true,
            subGridOptions: {
                "plusicon": "ace-icon fa fa-plus",
                "minusicon": "ace-icon fa fa-minus",
                "openicon": "ace-icon fa fa-share"
            },
            subGridRowExpanded: function(subgrid_id, row_id) {
                var subgrid_table_id, pager_id;
                var rowData = $gridTrainingRecordQueryMain.jqGrid("getRowData", row_id);
                subgrid_table_id = subgrid_id + "_t";
                pager_id = "p_" + subgrid_table_id;
                $("#" + subgrid_id).html("<div style='width:100%;overflow:auto'><table id='" + subgrid_table_id + "' class='scroll' ></table></div>");
                var subGridQueryData = {};

                subGridQueryData.UserId = rowData.UserId;
                $("#" + subgrid_table_id).jqGrid({
                    url: "/" + controllerName + "/GetUserStudyRecordListByUserId",
                    datatype: "json",
                    postData: subGridQueryData,
                    rownumbers: true,
                    rowNum: 10,
                    colNames: ["StudyRecordId", "课程名称", "总学时", "已完成学时", "练习情况"],
                    colModel: [
                        { name: "StudyRecordId", index: "StudyRecordId", width: "10", align: "center", hidden: true },
                        { name: "LessonTypeName", index: "LessonTypeName", align: "center", width: 150 },
                        { name: "TotalHours", index: "TotalHours", align: "center", width: 100 },
                        { name: "CompleteHours", index: "CompleteHours", align: "center", width: 100 },
                        { name: "ExerciseInfo", index: "ExerciseInfo", align: "center", width: 300 }
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
                setGridHeight($gridTrainingRecordQueryMain.selector);
            }
        });
    }

    $(document).ready(function() {
        initQueryArea();
        initJqGrid();
    });
});