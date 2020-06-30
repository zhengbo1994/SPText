'use strict'
$(function () {
    var controllerName = "RoomManage";
    var $divRoomManageQueryArea = $('#divRoomManage_QueryArea');
    var $gridRoomManageMain = $("#gridRoomManage_Main");
    var $pagerRoomManageMain = $("#pagerRoomManage_Main");


    var initQueryArea = function () {
        var initQueryButton = function () {
            $("#btnRoomManage_Query").on("click", function () {
                var queryData = {};
                var divQueryArea = $divRoomManageQueryArea;
                queryData = getJson(divQueryArea);
                $gridRoomManageMain.jqGrid("setGridParam", { postData: queryData }).trigger("reloadGrid");
            });
        }


        initQueryButton();
    }

    var initJqGrid = function () {
        var queryData = {};
        var divQueryArea = $divRoomManageQueryArea;
        queryData = getJson(divQueryArea);
        $gridRoomManageMain.jqGrid({
            url: "/" + controllerName + "/GetRoomRecordListForJqGrid",
            datatype: "json",
            mtype: "post",
            postData: queryData,
            colNames: ["RoomRecordId", "房间名称", "房间号码", "最大人数", "注册人数", "开始时间", "结束时间", "创建人", "创建日期"],
            colModel: [
                { name: "RoomRecordId", index: "RoomRecordId", width: 10, hidden: true },
                { name: "RoomName", index: "RoomName", align: "center", width: 200 },
                { name: "RoomNumber", index: "RoomNumber", align: "center", width: 100 },
                { name: "PersonCountLimit", index: "PersonCountLimit", align: "center", width: 100 },
                { name: "RegisteredNumber", index: "RegisteredNumber", align: "center", width: 100 },
                { name: "StartDate", index: "StartDate", align: "center", width: 130 },
                { name: "EndDate", index: "EndDate", align: "center", width: 130 },
                { name: "CreateBy", index: "CreateBy", align: "center", width: 100 },
                { name: "CreateDate", index: "CreateDate", align: "center", width: 130 }
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
            pager: $pagerRoomManageMain,
            ondblClickRow: function (rowid, iRow, iCol, e) {
                $gridRoomManageMain.jqGrid("toggleSubGridRow", rowid);
            },
            subGrid: true,
            subGridOptions: {
                "plusicon": "ace-icon fa fa-plus",
                "minusicon": "ace-icon fa fa-minus",
                "openicon": "ace-icon fa fa-share"
            },
            subGridRowExpanded: function (subgrid_id, row_id) {
                var subgrid_table_id, pager_id;
                var rowData = $gridRoomManageMain.jqGrid("getRowData", row_id);
                subgrid_table_id = subgrid_id + "_t";
                pager_id = "p_" + subgrid_table_id;
                $("#" + subgrid_id).html("<div style='width:100%;overflow:auto'><table id='" + subgrid_table_id + "' class='scroll' ></table></div>");
                var subGridQueryData = {};

                subGridQueryData.RoomRecordId = rowData.RoomRecordId;
                $("#" + subgrid_table_id).jqGrid({
                    url: "/" + controllerName + "/GetRelRoomUserListByRoomRecordId",
                    datatype: "json",
                    postData: subGridQueryData,
                    rownumbers: true,
                    rowNum: 10,
                    colNames: ["RelRoomUserId", "姓名", "证件编号", "注册时间", "总学时", "练习情况"],
                    colModel: [
                        { name: "RelRoomUserId", index: "RelRoomUserId", width: "10", align: "center", hidden: true },
                        { name: "UserName", index: "UserName", align: "center", width: 100 },
                        { name: "IdNumber", index: "IdNumber", align: "center", width: 150 },
                        { name: "CreateDate", index: "CreateDate", align: "center", width: 150 },
                        { name: "VideoCount", index: "VideoCount", align: "center", width: 100 },
                        { name: "ExerciseInfo", index: "ExerciseInfo", align: "center", width: 200 }
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
                setGridHeight($gridRoomManageMain.selector);
            }
        });


    }




    //页面加载时运行
    $(document).ready(function () {
        initQueryArea();
        initJqGrid();
    })
})