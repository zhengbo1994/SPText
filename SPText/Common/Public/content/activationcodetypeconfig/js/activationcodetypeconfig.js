'use strict'
$(function () {
    var controllerName = "ActivationCodeTypeConfig";
    var $queryActivationCodeTypeConfigArea = $('#divActivationCodeTypeConfig_QueryArea');
    var $gridActivationCodeTypeConfigMain = $("#gridActivationCodeTypeConfig_main");
    var $pagerActivationCodeTypeConfigMain = $("#pagerActivationCodeTypeConfig_main");
    var $btnAddActivationCodeType = $("#btnAddActivationCodeType");
    var $mdlActivationCodeTypeConfig = $("#mdlActivationCodeTypeConfig_ActivetionCodeTypeConfigInfo");
    var $fromActivationCodeTypeConfigInfo = $("#fromActivationCodeTypeConfigInfo");
    var enumEditTypes = { insert: 0, update: 1, view: 2 }
    var currentEdittype;
    var activatiionCodeTypeId = 0;

    var initQueryArea = function () {

        //课程类型下拉框
        var initSelectLessonTypeName = function () {
            var initSelectLessonType = function () {
                var $SelectLessonType = $queryActivationCodeTypeConfigArea.find("[name='LessonTypeId']");
                var getSelectLessonTypeList = function () {
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

                var lessonTypeList = getSelectLessonTypeList();
                $SelectLessonType.empty();
                var $optionAll = $("<option>");

                $optionAll.val("");
                $optionAll.text("请选择");
                $SelectLessonType.append($optionAll);
                for (var i = 0; i < lessonTypeList.length; i++) {
                    var lessonTypeInfo = lessonTypeList[i];
                    var $option = $("<option>");
                    $option.val(lessonTypeInfo.LessonTypeId);
                    $option.text(lessonTypeInfo.LessonTypeName);
                    $SelectLessonType.append($option);
                }
            }

            initSelectLessonType();
        }

        var initQueryButton = function () {
            $("#btnActivationCodeTypeConfigQuery").on("click", function() {
                
                var queryData = {};
                queryData = getJson($queryActivationCodeTypeConfigArea);
                $gridActivationCodeTypeConfigMain.jqGrid("setGridParam", { page: 1, postData: queryData }).trigger("reloadGrid");
            });
        }

        initQueryButton();
        initSelectLessonTypeName();
    }

    var initActivationCodeTypeConfigGrid = function () {

        //修改激活码类型信息
        window.UpdateActivationCodeType = function (rowId) {
            var rowData = $gridActivationCodeTypeConfigMain.jqGrid("getRowData", rowId);
            var activeTypeId = rowData.ActiveTypeId;
            currentEdittype = enumEditTypes.update;
            activatiionCodeTypeId = activeTypeId;
            $mdlActivationCodeTypeConfig.modal("show");
        }


        var queryData = {};
        queryData = getJson($queryActivationCodeTypeConfigArea);
        $gridActivationCodeTypeConfigMain.jqGrid({
            url: "/" + controllerName + "/GetActivationCodeTypeListForJqgrid",
            mtype: "post",
            datatype: "json",
            postData: queryData,
            colNames: ["Id", "激活码类型", "课程类型","备注", "操作"],
            colModel: [
                { name: "ActiveTypeId", index: "ActiveTypeId", align: "center", width: 30 , hidden: true },
                { name: "ActiveTypeName", index: "ActiveTypeName", align: "center", width: 120},
                { name: "LessonTypeName", index: "LessonTypeName", align: "center", width: 120},
                { name: "Remark", index: "Remark", align: "center", width: 200 },
                {
                    name: "操作", index: "操作", key: true, width: 110, align: "center", formatter: function(cellvalue, options, rowobj) {
                        var buttons = '';
                        buttons += '<a href="#" title="修改" onclick="UpdateActivationCodeType(' + options.rowId + ')" style="padding: 7px;line-height: 1em;">'
                            + '<i class="ace-icon  fa fa-edit"></i> 修改</a>';
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
            pager: $pagerActivationCodeTypeConfigMain,
            loadComplete: function () {
                var table = this;
                updatePagerIcons(table);
                jqGridAutoWidth();
                setGridHeight($gridActivationCodeTypeConfigMain.selector);
            }
        });
    }

    var initButtonArea = function () {

        var initAddButton = function () {
            $btnAddActivationCodeType.on("click", function () {
                currentEdittype = enumEditTypes.insert;
                $mdlActivationCodeTypeConfig.modal("show");
                $gridActivationCodeTypeConfigMain.jqGrid().trigger("reloadGrid");
            });
        }
        initAddButton();
    }

    var initModal = function () {

        var activatioinCodeTypeModal = function () {

            //获取激活码类型相关信息
            var getActivatioinCodeTypeInfo = function() {
                var dataResult = {};
                ajaxRequest({
                    url: "/" + controllerName + "/GetActivatioinCodeTypeInfoById",
                    type: "post",
                    datatype: "json",
                    data: { "ActivationCodeTypeId": activatiionCodeTypeId },
                    async: false,
                    cache: false,
                    success: function (jdata) {
                        dataResult = jdata;
                    }
                });
                return dataResult;
            }

            //培训学校下拉框
            var initSelectLessonTypeName = function () {
                var initSelectLessonType = function () {
                    var $SelectLessonType = $fromActivationCodeTypeConfigInfo.find("[name='LessonTypeId']");
                    var getSelectLessonTypeList = function () {
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

                    var lessonTypeList = getSelectLessonTypeList();
                    $SelectLessonType.empty();
                    var $optionAll = $("<option>");

                    $optionAll.val("");
                    $optionAll.text("请选择");
                    $SelectLessonType.append($optionAll);
                    for (var i = 0; i < lessonTypeList.length; i++) {
                        var lessonTypeInfo = lessonTypeList[i];
                        var $option = $("<option>");
                        $option.val(lessonTypeInfo.LessonTypeId);
                        $option.text(lessonTypeInfo.LessonTypeName);
                        $SelectLessonType.append($option);
                    }
                   
                    $SelectLessonType.chosen({
                        no_results_text: "未找到",
                        allow_single_deselect: true,
                        disable_search: false,
                        search_contains: true
                    });
                }


                initSelectLessonType();
            }  

            initSelectLessonTypeName();

            $mdlActivationCodeTypeConfig.on("show.bs.modal", function () {

                var $SelectLessonType = $fromActivationCodeTypeConfigInfo.find("[name='LessonTypeId']");

                var resultData = "";
                var defaultData = { ActivationCodeTypeName: "", Remark: "",Percent:"" };
                setJson($fromActivationCodeTypeConfigInfo, defaultData);

                if (enumEditTypes.update === currentEdittype || enumEditTypes.view === currentEdittype) {
                    resultData = getActivatioinCodeTypeInfo();
                }

                setJson($fromActivationCodeTypeConfigInfo, resultData);

                $SelectLessonType.trigger("chosen:updated");
              
            });

            //保存
            $mdlActivationCodeTypeConfig.find("[name='btnSave']").on("click", function () {

                var verifyResult = verifyForm($fromActivationCodeTypeConfigInfo);
                if (!verifyResult) {
                    return false;
                }

                var postData = {};

                postData = getJson($fromActivationCodeTypeConfigInfo);

                var functionName = "";

                if (enumEditTypes.insert === currentEdittype) {
                    functionName = "/SaveActivationCodeTypeInfo";
                } else if (enumEditTypes.update === currentEdittype) {
                    functionName = "/UpdateActivationCodeTypeInfo";
                    postData.ActivationCodeTypeId = activatiionCodeTypeId;
                    if (postData.ActivationCodeTypeName.indexOf("(") > -1) {
                        postData.ActivationCodeTypeName = postData.ActivationCodeTypeName.substring(0, postData.ActivationCodeTypeName.indexOf("("));
                    }
                }

                var ajaxOpt = {
                    url: "/" + controllerName + functionName,
                    data: postData,
                    type: "post",
                    async: false,
                    success: function (jdata) {
                        if (jdata.IsSuccess != null) {
                            alert(jdata.ErrorMessage);
                            return false;
                        }

                        alert("保存成功！");
                        $gridActivationCodeTypeConfigMain.jqGrid().trigger("reloadGrid");
                        $mdlActivationCodeTypeConfig.modal("toggle");
                        return true;
                    }
                };
                ajaxRequest(ajaxOpt);

            });

        }

        activatioinCodeTypeModal();
    }

    $(document).ready(function() {
        initQueryArea();
        initActivationCodeTypeConfigGrid();
        initButtonArea();
        initModal();
    });
})