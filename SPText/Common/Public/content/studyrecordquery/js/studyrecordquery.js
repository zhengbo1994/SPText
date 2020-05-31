'use strict';
$(function() {
    var controllerName = "StudyRecordQuery";
    var $divStudyRecordQueryQueryArea = $('#divStudyRecordQuery_QueryArea');
    var $gridStudyRecordQueryMain = $("#gridStudyRecordQuery_Main");
    var $pagerStudyRecordQueryMain = $("#pagerStudyRecordQuery_Main");

    var initQueryArea = function() {
        var initQueryButton = function() {
            $("#btnStudyRecordQuery_Query").on("click", function() {
                var queryData = {};
                var divQueryArea = $divStudyRecordQueryQueryArea;
                queryData = getJson(divQueryArea);
                $gridStudyRecordQueryMain.jqGrid("setGridParam", { postData: queryData }).trigger("reloadGrid");
            });
        }
        initQueryButton();
    }

    var initJqGrid = function() {
        var queryData = {};
        var divQueryArea = $gridStudyRecordQueryMain;
        queryData = getJson(divQueryArea);
        $gridStudyRecordQueryMain.jqGrid({
            url: "/" + controllerName + "/GetActiveCodeUseRecordUserListForJqGrid",
            datatype: "json",
            mtype: "post",
            postData: queryData,
            colNames: ["VideoPlayCompleteRecordId", "姓名", "证件编号", "视频名称", "完成时间"],
            colModel: [
                { name: "VideoPlayCompleteRecordId", index: "VideoPlayCompleteRecordId", width: 10, hidden: true },
                { name: "UserName", index: "UserName", align: "center", width: 180 },
                { name: "UserCode", index: "UserCode", align: "center", width: 220 },
                { name: "CompleteVideo", index: "CompleteVideo", align: "center", width: 350 },
                { name: "CompleteDate", index: "CompleteDate", align: "center", width: 205 }
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
            pager: $pagerStudyRecordQueryMain,
            loadComplete: function() {
                var table = this;
                updatePagerIcons(table);
                jqGridAutoWidth();
                setGridHeight($gridStudyRecordQueryMain.selector);
            }
        });
    }


    $(document).ready(function() {
        initQueryArea();
        initJqGrid();
    });
});