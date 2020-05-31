'use strict';
$(function () {
    var controllerName = "ActivationCodeUseingQuery";
    var $divActiveCodeUseQueryQueryArea = $('#divActiveCodeUseQuery_QueryArea');
    var $gridActiveCodeUseQueryMain = $("#gridActiveCodeUseQuery_Main");
    var $pagerActiveCodeUseQueryMain = $("#pagerActiveCodeUseQuery_Main");


    var initQueryArea = function () {
        var initQueryButton = function () {
            $("#btnActiveCodeUseQuery_Query").on("click", function () {
                var queryData = {};
                var divQueryArea = $divActiveCodeUseQueryQueryArea;
                queryData = getJson(divQueryArea);
                $gridActiveCodeUseQueryMain.jqGrid("setGridParam", { postData: queryData }).trigger("reloadGrid");
            });
        }

        var initSelectActiveCodeTypeName = function () {
            var $SelectActiveCodeType = $divActiveCodeUseQueryQueryArea.find("[name='ActiveCodeTypeId']");
            var getSelectActiveCodeTypeList = function () {
                var dataResult = {};
                ajaxRequest({
                    url: "/" + controllerName + "/GetActiveCodeTypeList",
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

            var activeCodeTypeList = getSelectActiveCodeTypeList();
            $SelectActiveCodeType.empty();
            var $optionAll = $("<option>");

            $optionAll.val("");
            $optionAll.text("请选择");
            $SelectActiveCodeType.append($optionAll);
            for (var i = 0; i < activeCodeTypeList.length; i++) {
                var activeCodeType = activeCodeTypeList[i];
                var $option = $("<option>");
                $option.val(activeCodeType.ActiveCodeTypeId);
                $option.text(activeCodeType.ActiveCodeTypeName);
                $SelectActiveCodeType.append($option);
            }
        }

        initQueryButton();
        initSelectActiveCodeTypeName();
    }


    var initJqGrid = function () {
        var queryData = {};
        var divQueryArea = $divActiveCodeUseQueryQueryArea;
        queryData = getJson(divQueryArea);
        $gridActiveCodeUseQueryMain.jqGrid({
            url: "/" + controllerName + "/GetActiveCodeListForJqGrid",
            datatype: "json",
            mtype: "post",
            postData: queryData,
            colNames: ["ActiveCodeId", "激活码类型", "激活码", "激活码使用寿命(天)", "备注", "创建人", "创建时间", "发给机构的管理员", "发送时间", "接收的机构", "发给企业的机构", "发送时间", "接收的企业", "发给个人的企业", "发送时间", "接收的人","激活人","激活时间"],
            colModel: [
                { name: "ActiveCodeId", index: "ActiveCodeId", width: 50, hidden: true },
                { name: "ActiveTypeName", index: "ActiveTypeName", align: "center", width: 150 },
                { name: "ActiveCodeNumber", index: "ActiveCodeNumber", align: "center", width: 250 },
                { name: "UseDays", index: "UseDays", align: "center", width: 125 },
                { name: "Remark", index: "Remark", align: "center", width: 200 },
                { name: "CreateBy", index: "CreateBy", align: "center", width: 70 },
                { name: "CreateDate", index: "CreateDate", align: "center", width: 120 },

                { name: "TrainingInstitutionCreateBy", index: "TrainingInstitutionCreateBy", align: "center", width: 100 },
                { name: "TrainingInstitutionDate", index: "TrainingInstitutionDate", align: "center", width: 120 },
                { name: "TrainingInstitutionName", index: "TrainingInstitutionId", align: "center", width: 130 },

                { name: "EnterpriseCreateBy", index: "EnterpriseCreateBy", align: "center", width: 100 },
                { name: "EnterpriseDate", index: "EnterpriseDate", align: "center", width: 120 },
                { name: "EnterpriseName", index: "EnterpriseId", align: "center", width: 130 },

                { name: "CustomerCreateBy", index: "CustomerCreateBy", align: "center", width: 100 },
                { name: "CustomerDate", index: "CustomerDate", align: "center", width: 120 },
                { name: "CustomerName", index: "CustomerId", align: "center", width: 130 },

                { name: "UserName", index: "UserName", align: "center", width: 70 },
                { name: "UserDate", index: "UserDate", align: "center", width: 120 }

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
            pager: $pagerActiveCodeUseQueryMain,
            loadComplete: function () {
                var table = this;
                updatePagerIcons(table);
                jqGridAutoWidth();
                setGridHeight($gridActiveCodeUseQueryMain.selector);
            }
        });
    }


    $(document).ready(function() {
        initQueryArea();
        initJqGrid();
    });
})