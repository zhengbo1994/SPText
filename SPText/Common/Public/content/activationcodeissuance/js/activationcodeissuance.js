'use strict'
$(function () {
    var controllerName = "ActivationCodeIssuance";
    var $divActivationCodeIssuanceQueryArea = $("#divActivationCodeIssuance_QueryArea");
    var $gridActivationCodeIssuanceMain = $("#gridActivationCodeIssuance_Main");
    var $pagerActivationCodeIssuanceMain = $("#pagerActivationCodeIssuance_Main");
    var $btnActivationCodeIssuanceInsert = $("#btnActivationCodeIssuanceInsert");
    var $mdlActivationCodeIssuance = $("#mdlActivationCodeIssuance_ActiveCodeInfo");
    var $formActivationCodeIssuance = $("#formActivationCodeIssuance_ActiveCodeInfo");


    var initQueryArea = function () {
        var initQueryButton = function () {
            $("#btnActivationCodeIssuance_Query").on("click", function () {
                var queryData = {};
                var divQueryArea = $divActivationCodeIssuanceQueryArea;
                queryData = getJson(divQueryArea);
                $gridActivationCodeIssuanceMain.jqGrid("setGridParam", { postData: queryData }).trigger("reloadGrid");
            });
        }

        var initSelectTrainingInstitutionName = function () {
            var initSelectTrainingInstitution = function () {
                var $SelectTrainingInstitution = $divActivationCodeIssuanceQueryArea.find("[name='TrainingInstitutionId']");
                var getSelectTrainingInstitutionList = function () {
                    var dataResult = {};
                    ajaxRequest({
                        url: "/" + controllerName + "/GetSelectTrainingInstitutionList",
                        type: "post",
                        datatype: "json",
                        async: false,
                        success: function (jdata) {
                            dataResult = jdata;
                        },
                        error: function () {
                            dataResult = null;
                        }
                    });
                    return dataResult;
                }

                var listTrainingInstitutionList = getSelectTrainingInstitutionList();
                $SelectTrainingInstitution.empty();
                var $optionAll = $("<option>");

                $optionAll.val("");
                $optionAll.text("请选择");
                $SelectTrainingInstitution.append($optionAll);
                for (var i = 0; i < listTrainingInstitutionList.length; i++) {
                    var trainingInstitutions = listTrainingInstitutionList[i];
                    var $option = $("<option>");
                    $option.val(trainingInstitutions.TrainingInstitutionId);
                    $option.text(trainingInstitutions.TrainingInstitutionName);
                    $SelectTrainingInstitution.append($option);
                }
            }

            initSelectTrainingInstitution();
        }

        initQueryButton();
        initSelectTrainingInstitutionName();
    }

    var initActivationCodeIssuanceGrid = function () {

        //导出激活码记录
        window.ActivationCodeExport = function (rowId) {

            var rowData = $gridActivationCodeIssuanceMain.jqGrid("getRowData", rowId);
            var activeCodeIssuanceRecordId = rowData.ActiveCodeIssuanceRecordId;

            if (!activeCodeIssuanceRecordId) {
                return false;
            }

            window.location.href = "/" + controllerName + "/ExportActivationCodeList?ActiveCodeIssuanceRecordId=" + activeCodeIssuanceRecordId;
            return true;
        }

        var queryData = {};
        var divQueryArea = $divActivationCodeIssuanceQueryArea;
        queryData = getJson(divQueryArea);
        $gridActivationCodeIssuanceMain.jqGrid({
            url: "/" + controllerName + "/GetActivationCodeIssuanceListForJqGrid",
            datatype: "json",
            mtype: "post",
            postData: queryData,
            colNames: ["Id", "激活码类型", "发放数量", "培训学校", "发放单位/人", "使用天数", "操作人", "操作日期", "备注", "操作"],
            colModel: [
                { name: "ActiveCodeIssuanceRecordId", index: "ActiveCodeIssuanceRecordId", hidden: true },
                { name: "ActivationCodeTypeName", index: "ActivationCodeTypeName", align: "center", width: 120 },
                { name: "ActivationCodeCount", index: "ActivationCodeCount", align: "center", width: 50 },
                { name: "TrainingInstitutionName", index: "TrainingInstitutionName", align: "center", width: 120 },
                { name: "IssuanceUnitName", index: "IssuanceUnitName", align: "center", width: 120 },
                { name: "UseDays", index: "UseDays", align: "center", width: 60 },
                { name: "UserName", index: "UserName", align: "center", width: 80 },
                { name: "CreateDate", index: "CreateDate", align: "center", width: 80 },
                { name: "Remark", index: "Remark", align: "center", width: 200 },
                {
                    name: '操作', index: '操作', width: 50, align: "center", formatter: function (cellvalue, options, rowobj) {
                        var buttons = ''
                            + '<a href="#" title="导出" onclick="ActivationCodeExport(' + options.rowId + ')" style="padding: 7px;line-height: 1em;">'
                            + '<i class="aace-icon fa fa-download "></i> 导出</a>';
                        return buttons;
                    }
                }
            ],
            multiselect: false,
            multiboxonly: true,
            autowidth: true,
            altRows: true,
            pgbuttons: true,
            viewrecords: true,
            shrinkToFit: true,
            pginput: true,
            rowList: [10, 20, 30, 50, 70, 100],
            pager: $pagerActivationCodeIssuanceMain,
            ondblClickRow: function (rowid, iRow, iCol, e) {
                $gridActivationCodeIssuanceMain.jqGrid("toggleSubGridRow", rowid);
            },
            subGrid: true,
            subGridOptions: {
                "plusicon": "ace-icon fa fa-plus",
                "minusicon": "ace-icon fa fa-minus",
                "openicon": "ace-icon fa fa-share"
            },
            subGridRowExpanded: function (subgrid_id, row_id) {
                var subgrid_table_id, pager_id;
                var rowData = $gridActivationCodeIssuanceMain.jqGrid("getRowData", row_id);
                subgrid_table_id = subgrid_id + "_t";
                pager_id = "p_" + subgrid_table_id;
                $("#" + subgrid_id).html("<div style='width:100%;overflow:auto'><table id='" + subgrid_table_id + "' class='scroll' ></table></div>");
                var subGridQueryData = {};

                subGridQueryData.ActiveCodeIssuanceRecordId = rowData.ActiveCodeIssuanceRecordId; //激活发放Id
                $("#" + subgrid_table_id).jqGrid({
                    url: "/" + controllerName + "/GetActivationCodeListForJqgrid",
                    datatype: "json",
                    postData: subGridQueryData,
                    colNames: ["Id", "激活码"],
                    colModel: [
                        { name: "ActiveCodeId", index: "ActiveCodeId", width: "10", align: "center", hidden: true },
                        { name: "ActiveCodeNumber", index: "ActiveCodeNumber", width: "200", align: "center" }
                    ],
                    rownumbers: true,
                    rowNum: 9999,
                    autoWidth: false,
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
                setGridHeight($gridActivationCodeIssuanceMain.selector);
            }
        });
    }

    var initButtonArea = function () {

        var initAddActivationCode = function () {
            $btnActivationCodeIssuanceInsert.on("click", function () {
                $mdlActivationCodeIssuance.modal("show");
                $gridActivationCodeIssuanceMain.jqGrid().trigger("reloadGrid");
            });
        }

        initAddActivationCode();


    }

    var initModal = function () {

        var addActivationCodeModal = function () {

            var initSelectActivationCodeTypeName = function () {

                var initSelectActivationCodeType = function () {
                    var $SelectActivationCodeType = $formActivationCodeIssuance.find("[name='ActivationCodeTypeId']");
                    var getSelectActivationCodeTypeList = function () {
                        var dataResult = {};
                        ajaxRequest({
                            url: "/" + controllerName + "/GetSelectActivationCodeTypeList",
                            type: "post",
                            datatype: "json",
                            async: false,
                            success: function (jdata) {
                                dataResult = jdata;
                            }
                        });
                        return dataResult;
                    }


                    var activationCodeTypeList = getSelectActivationCodeTypeList();
                    $SelectActivationCodeType.empty();
                    var $optionAll = $("<option>");

                    $optionAll.val("");
                    $optionAll.text("请选择");
                    $SelectActivationCodeType.append($optionAll);
                    for (var i = 0; i < activationCodeTypeList.length; i++) {
                        var activationCodeTypeInfo = activationCodeTypeList[i];
                        var $option = $("<option>");
                        $option.val(activationCodeTypeInfo.ActivationCodeTypeId);
                        $option.text(activationCodeTypeInfo.ActivationCodeTypeName + "(" + activationCodeTypeInfo. Percent + "%)");
                        $SelectActivationCodeType.append($option);
                    }

                    $SelectActivationCodeType.chosen({
                        no_results_text: "未找到",
                        allow_single_deselect: true,
                        disable_search: false,
                        search_contains: true
                    });

                }

                initSelectActivationCodeType();
            }

            var initSelectTrainingInstitutionName = function () {

                var initSelectTrainingInstitution = function () {
                    var $SelectTrainingInstitution = $formActivationCodeIssuance.find("[name='TrainingInstitutionId']");
                    var getSelectTrainingInstitutionList = function () {
                        var dataResult = {};
                        ajaxRequest({
                            url: "/" + controllerName + "/GetSelectTrainingInstitutionList",
                            type: "post",
                            datatype: "json",
                            async: false,
                            success: function (jdata) {
                                dataResult = jdata;
                            }
                        });
                        return dataResult;
                    }

                    var listTrainingInstitutionList = getSelectTrainingInstitutionList();
                    $SelectTrainingInstitution.empty();
                    var $optionAll = $("<option>");

                    $optionAll.val("");
                    $optionAll.text("请选择");
                    $SelectTrainingInstitution.append($optionAll);
                    for (var i = 0; i < listTrainingInstitutionList.length; i++) {
                        var trainingInstitutions = listTrainingInstitutionList[i];
                        var $option = $("<option>");
                        $option.val(trainingInstitutions.TrainingInstitutionId);
                        $option.text(trainingInstitutions.TrainingInstitutionName);
                        $SelectTrainingInstitution.append($option);
                    }

                    $SelectTrainingInstitution.chosen({
                        no_results_text: "未找到",
                        allow_single_deselect: true,
                        disable_search: false,
                        search_contains: true
                    });
                }

                initSelectTrainingInstitution();
            }

            initSelectActivationCodeTypeName();
            initSelectTrainingInstitutionName();


            $mdlActivationCodeIssuance.on("show.bs.modal", function () {

                var defaultData = { Count: "", IssuanceUnitName: "", Remark: "" };
                setJson($formActivationCodeIssuance, defaultData);
            });


            $mdlActivationCodeIssuance.find("[name='btnSave']").on("click", function () {
                var verifyResult = verifyForm($formActivationCodeIssuance);
                if (!verifyResult) {
                    return false;
                }

                var postData = {};

                postData = getJson($formActivationCodeIssuance);

                var ajaxOpt = {
                    url: "/" + controllerName + "/InsertActivationCodeInfo",
                    data: postData,
                    type: "post",
                    async: false,
                    success: function (jdata) {
                        if (jdata.IsSuccess != null) {
                            alert(jdata.ErrorMessage);
                            return false;
                        }

                        alert("保存成功！");
                        $gridActivationCodeIssuanceMain.jqGrid().trigger("reloadGrid");
                        $mdlActivationCodeIssuance.modal("toggle");
                        return true;
                    }
                };
                ajaxRequest(ajaxOpt);

            });
        }

        addActivationCodeModal();
    }


    //页面加载时运行
    $(document).ready(function () {
        initQueryArea();
        initActivationCodeIssuanceGrid();
        initButtonArea();
        initModal();
    });
});