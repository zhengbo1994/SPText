'use strict'
$(function () {
    var controllerName = "ExerciseRecordQuery";
    var $divExerciseRecordQueryQueryArea = $('#divExerciseRecordQuery_QueryArea');
    var $gridExerciseRecordQueryMain = $("#gridExerciseRecordQuery_Main");
    var $pagerExerciseRecordQueryMain = $("#pagerExerciseRecordQuery_Main");




    var initQueryArea = function () {
        var initQueryButton = function () {
            $("#btnExerciseRecordQuery_Query").on("click", function () {
                var queryData = {};
                var divQueryArea = $divExerciseRecordQueryQueryArea;
                queryData = getJson(divQueryArea);
                $gridExerciseRecordQueryMain.jqGrid("setGridParam", { postData: queryData }).trigger("reloadGrid");
            });
        }


        initQueryButton();
    }

    var initJqGrid = function () {
        var queryData = {};
        var divQueryArea = $gridExerciseRecordQueryMain;
        queryData = getJson(divQueryArea);
        $gridExerciseRecordQueryMain.jqGrid({
            url: "/" + controllerName + "/GetExerciseRecordQueryListForJqgrid",
            datatype: "json",
            mtype: "post",
            postData: queryData,
            colNames: ["ExerciseRecordQueryId", "姓名", "证件编号", "练习名称", "得分", "开始时间", "交卷时间"],
            colModel: [
                { name: "ExerciseRecordQueryId", index: "ExerciseRecordQueryId", width: 10, hidden: true },
                { name: "UserName", index: "UserName", align: "center", width: 100 },
                { name: "UserCode", index: "UserCode", align: "center", width: 150 },
                { name: "ExerciseName", index: "ExerciseName", align: "center", width: 200 },
                { name: "Score", index: "Score", align: "center", width: 55 },
                { name: "BeginDateTime", index: "BeginDateTime", align: "center", width: 150 },
                { name: "EndDateTime", index: "EndDateTime", align: "center", width: 150 }
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
            pager: $pagerExerciseRecordQueryMain,
            loadComplete: function () {
                var table = this;
                updatePagerIcons(table);
                jqGridAutoWidth();
                setGridHeight($gridExerciseRecordQueryMain.selector);
            }
        });
    }






    $(document).ready(function () {
        initQueryArea();
        initJqGrid();
    })
})