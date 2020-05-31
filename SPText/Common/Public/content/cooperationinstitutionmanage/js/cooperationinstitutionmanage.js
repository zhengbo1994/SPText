'use strict';
$(function() {
    var controllerName = "CooperationInstitutionManage";
    //合作机构
    var $divCooperationInstitutionManageQueryArea = $("#divCooperationInstitutionManage_QueryArea");
    var $gridCooperationInstitutionManageMain = $("#gridCooperationInstitutionManage_Main");
    var $pagerCooperationInstitutionManageMain = $("#pagerCooperationInstitutionManage_Main");
    var $mdlCooperationInstitutionManageInfo = $("#mdlCooperationInstitutionManage_Info");
    var $divCooperationInstitutionManageInfo = $("#divCooperationInstitutionManage_Info");

    var $mdlCooperationInstitutionRelatedLinkInfo = $("#mdlCooperationInstitutionManage_CooperationInstitutionRelatedLinkInfo");
    var $divCooperationInstitutionRelatedLinkInfo = $("#divCooperationInstitutionManage_CooperationInstitutionRelatedLinkInfo");

    //培训机构
    var $mdlTrainingInstitutionInfo = $("#mdlTrainingInstitution_Info");
    var $divTrainingInstitutionQueryArea = $("#divTrainingInstitution_QueryArea");
    var $gridTrainingInstitutionMain = $("#gridTrainingInstitution_Main");
    var $pagerTrainingInstitutionMain = $("#pagerTrainingInstitution_Main");

    //课程合作机构关联
    var $gridCooperationInstitutionManageLessonTypeMain = $("#gridCooperationInstitutionManageLessonType_Main");
    var $pagerCooperationInstitutionManageLessonTypeMain = $("#pagerCooperationInstitutionManageLessonType_Main");


    var enum_EditTypes = { insert: 0, update: 1, view: 2 }
    var currentEditType;
    var institutionId = 0;
    //调用onSelectRow事件时，保存的编号
    var selectedInstitutionId = -1;

    //合作机构
    var getCooperationInstitutionGridSelectedRowData = function($gridCooperationInstitutionManageMain, noSelectionCallback) {
        var rowId = $gridCooperationInstitutionManageMain.jqGrid("getGridParam", "selrow");
        if (!rowId) {
            if ("function" == typeof (noSelectionCallback)) {
                noSelectionCallback();
            }
            return undefined;
        }
        var rowData = $gridCooperationInstitutionManageMain.jqGrid("getRowData", rowId);
        return rowData;
    }

    //培训机构
    var getTrainingInstitutionGridSelectedRowData = function($gridTrainingInstitutionMain, noSelectionCallback) {
        var rowId = $gridTrainingInstitutionMain.jqGrid("getGridParam", "selrow");
        if (!rowId) {
            if ("function" == typeof (noSelectionCallback)) {
                noSelectionCallback();
            }
            return undefined;
        }
        var rowData = $gridTrainingInstitutionMain.jqGrid("getRowData", rowId);
        return rowData;
    }

    //添加培训机构
    var chooseInstitution = function() {
        var rowData = getTrainingInstitutionGridSelectedRowData($gridTrainingInstitutionMain, function() {
            alert("请选择数据！");
        });
        if (rowData == undefined) {
            return false;
        }

        $divCooperationInstitutionManageInfo.find("[name='TrainingInstitutionId']").val(rowData.InstitutionId);
        $divCooperationInstitutionManageInfo.find("[name='TrainingInstitutionName']").val(rowData.InstitutionName);


        $mdlTrainingInstitutionInfo.modal("toggle");
        $mdlCooperationInstitutionManageInfo.modal("show");
    }

    var initQueryArea = function() {

        var initCooperationInstitutionQuery = function() {
            $("#btnCooperationInstitutionManage_Query").on("click", function() {
                var queryData = {};
                var divQueryArea = $divCooperationInstitutionManageQueryArea;
                queryData = getJson(divQueryArea);
                $gridCooperationInstitutionManageMain.jqGrid("setGridParam", { postData: queryData }).trigger("reloadGrid");
            });
        }

        var initTrainingInstitutionQuery = function() {
            var initQueryButton = function() {
                $("#btnTrainingInstitution_Query").on("click", function() {
                    var queryData = {};
                    var divQueryArea = $divTrainingInstitutionQueryArea;
                    queryData = getJson(divQueryArea);
                    $gridTrainingInstitutionMain.jqGrid("setGridParam", { postData: queryData }).trigger("reloadGrid");
                });
            }

            initQueryButton();
        }


        initCooperationInstitutionQuery();
        initTrainingInstitutionQuery();

    }

    var initJqGrid = function() {

        var initCooperationInstitutionGrid = function () {


            window.CooperationInstitutionManage_AddCooperationInstitutionRelatedLink = function (cooperationInstitutionId) {

                var fromData = getJson($divCooperationInstitutionRelatedLinkInfo);
                for (var p in fromData) {
                    fromData[p] = "";
                }
                fromData.CooperationInstitutionId = cooperationInstitutionId;
                setJson($divCooperationInstitutionRelatedLinkInfo, fromData);

                $mdlCooperationInstitutionRelatedLinkInfo.modal('show');
            }

            window.CooperationInstitutionManage_EditCooperationInstitutionRelatedLink = function (linkId) {

                var getCooperationInstitutionRelatedLink = function (linkId) {
                    var dataResult = {};
                    ajaxRequest({
                        url: "/" + controllerName + "/GetCooperationInstitutionRelatedLinkById",
                        data: { "linkId": linkId },
                        type: "post",
                        async: false,
                        success: function (jdata) {
                            dataResult = jdata;
                        }
                    });
                    return dataResult;
                }
                var fromData = getCooperationInstitutionRelatedLink(linkId);
                setJson($divCooperationInstitutionRelatedLinkInfo, fromData);
                $mdlCooperationInstitutionRelatedLinkInfo.modal('show');
            }

            window.CooperationInstitutionManage_DeleteCooperationInstitutionRelatedLink = function (linkId, subJqgridId) {
                if (!confirm("确定删除？")) {
                    return false;
                }
                ajaxRequest({
                    url: "/" + controllerName + "/DeleteCooperationInstitutionRelatedLinkById",
                    data: { "linkId": linkId },
                    type: "post",
                    async: true,
                    success: function (jdata) {
                        $("#" + subJqgridId).jqGrid().trigger("reloadGrid");
                    }
                });
            }

            var $queryData = {};
            $queryData = getJson($divCooperationInstitutionManageQueryArea);
            $gridCooperationInstitutionManageMain.jqGrid({
                url: "/" + controllerName + "/GetCooperationInstitutionInfoListForJqgrid",
                datatype: "json",
                postData: $queryData,
                colNames: ["InstitutionId", "合作机构名称", "合作机构编码", "所属培训机构", "TrainingInstitutionId", "首页链接", "联系电话", "服务电话", "所在地址", "操作"],
                colModel: [
                    { name: "InstitutionId", index: "InstitutionId", width: 10, hidden: true },
                    { name: "InstitutionName", index: "InstitutionName", align: "center", width: 150 },
                    { name: "InstitutionCode", index: "InstitutionCode", align: "center", width: 80 },
                    { name: "TrainingInstitutionName", index: "TrainingInstitutionName", align: "center", width: 150 },
                    { name: "TrainingInstitutionId", index: "TrainingInstitutionId", width: 10, hidden: true },
                    { name: "HomeLink", index: "HomeLink", align: "center", width: 100 },
                    { name: "LinkPhone", index: "LinkPhone", align: "center", width: 100 },
                    { name: "CustomerServicePhone", index: "CustomerServicePhone", align: "center", width: 100 },
                    { name: "Address", index: "Address", align: "center", width: 200 },
                    {
                        name: "操作",
                        index: "操作",
                        align: "center",
                        width: 200,
                        formatter: function(cellvalue, options, rowobj) {
                            var buttons = ''
                                + '<a href="javascript:void(0);" onclick="CooperationInstitutionManage_AddCooperationInstitutionRelatedLink(' + rowobj.InstitutionId + ')" title="添加相关链接" style="padding: 7px;line-height: 1em;">'
                                + '<i class="fa fa-plus "></i> 添加相关链接</a>';
                            return buttons;
                        }
                    }
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
                pager: $pagerCooperationInstitutionManageMain,
                onSelectRow: function(rowId) {
                    var institutionId = $gridCooperationInstitutionManageMain.jqGrid("getRowData", rowId).InstitutionId;
                    if (isNull(institutionId)) {
                        institutionId = parseInt(institutionId);
                    }

                    var queryData = {};
                    queryData.institutionId = selectedInstitutionId = institutionId;

                    //加载课程列表
                    $gridCooperationInstitutionManageLessonTypeMain.jqGrid("setGridParam", { page: 1, postData: queryData }).trigger("reloadGrid");
                },
                loadComplete: function() {
                    var table = this;
                    updatePagerIcons(table);
                    jqGridAutoWidth();
                    setGridHeight($gridCooperationInstitutionManageMain.selector);
                },
                subGrid: true,
                subGridOptions: {
                    "plusicon": "ace-icon fa fa-plus",
                    "minusicon": "ace-icon fa fa-minus",
                    "openicon": "ace-icon fa fa-share"
                },
                subGridRowExpanded: function(subgrid_id, row_id) {
                    var subgrid_table_id, pager_id;
                    var rowData = $gridCooperationInstitutionManageMain.jqGrid("getRowData", row_id);
                    subgrid_table_id = subgrid_id + "_t";
                    pager_id = "p_" + subgrid_table_id;
                    $("#" + subgrid_id).html("<div style='width:100%;overflow:auto'><table id='" + subgrid_table_id + "' class='scroll' ></table></div>");
                    var subGridQueryData = {};

                    subGridQueryData.cooperationInstitutionId = rowData.InstitutionId;
                    $("#" + subgrid_table_id).jqGrid({
                        url: "/" + controllerName + "/GetCooperationInstitutionRelatedLinkListForJqgrid",
                        mtype: 'post',
                        datatype: "json",
                        postData: subGridQueryData,
                        colNames: ["LinkId", "CooperationInstitutionId", "链接名称", "链接地址", "序号", "操作"],
                        colModel: [
                            { name: "LinkId", index: "LinkId", width: "10", align: "center", hidden: true },
                            { name: "CooperationInstitutionId", index: "CooperationInstitutionId", width: "10", align: "center", hidden: true },
                            { name: "LinkName", index: "LinkName", align: "center", width: 150 },
                            { name: "LinkAddress", index: "LinkAddress", align: "center", width: 150 },
                            { name: "Seq", index: "Seq", align: "center", width: 50 },
                            {
                                name: "操作",
                                index: "操作",
                                align: "center",
                                width: 200,
                                formatter: function(cellvalue, options, rowobj) {
                                    var buttons = ''
                                        + '<a href="javascript:void(0);" onclick="CooperationInstitutionManage_DeleteCooperationInstitutionRelatedLink(' + rowobj.LinkId + ',\'' + subgrid_table_id + '\')" title="删除" style="padding: 7px;line-height: 1em;">'
                                        + '<i class="fa fa-trash "></i> 删除</a>'
                                        + '<a href="javascript:void(0);" onclick="CooperationInstitutionManage_EditCooperationInstitutionRelatedLink(' + rowobj.LinkId + ')" title="编辑" style="padding: 7px;line-height: 1em;">'
                                        + '<i class="fa fa-edit "></i> 编辑</a>';
                                    return buttons;
                                }
                            }
                        ],
                        rownumbers: true,
                        autoWidth: true,
                        rowNum: 9999,
                        ondblClickRow: function(rowid, iRow, iCol, e) {
                            return false;
                        }

                    });
                }
            });
        }

        var initTrainingInstitutionGrid = function() {
            var $queryData = {};
            $queryData = getJson($divTrainingInstitutionQueryArea);
            $gridTrainingInstitutionMain.jqGrid({
                url: "/" + controllerName + "/GetTrainingInstitutionInfoListForJqgrid",
                datatype: "json",
                postData: $queryData,
                colNames: ["InstitutionId", "培训机构名称", "社会信用代码"],
                colModel: [
                    { name: "InstitutionId", index: "InstitutionId", width: 10, hidden: true },
                    { name: "InstitutionName", index: "InstitutionName", align: "center", width: 250 },
                    { name: "SocialCreditCode", index: "SocialCreditCode", align: "center", width: 350 }
                ],
                multiselect: false,
                autowidth: true,
                rowNum: 20,
                altRows: true,
                pgbuttons: true,
                viewrecords: true,
                width: "100%",
                shrinkToFit: false,
                pginput: true,
                rowList: [10, 20, 30, 50, 70, 100],
                pager: $pagerTrainingInstitutionMain,
                ondblClickRow: function(rowid, iRow, iCol, e) {
                    chooseInstitution();
                },
                loadComplete: function() {
                    var table = this;
                    updatePagerIcons(table);
                    jqGridAutoWidth();
                }
            });
        }

        var initLessonGrid = function() {
            var postData = {};
            $gridCooperationInstitutionManageLessonTypeMain.jqGrid({
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
                pager: $pagerCooperationInstitutionManageLessonTypeMain,
                loadComplete: function(XMLHttpRequest) {
                    var table = this;
                    updatePagerIcons(table);
                    jqGridAutoWidth();
                    setGridHeight($gridCooperationInstitutionManageLessonTypeMain.selector);

                    var getLessonTypeListByInstitutionId = function() {
                        var queryData = {};
                        var dataResult = {};
                        queryData.institutionId = selectedInstitutionId;

                        ajaxRequest({
                            url: "/" + controllerName + "/GetLessonTypeListByInstitutionId",
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

                    var lessonTypeIdList = getLessonTypeListByInstitutionId();

                    if (lessonTypeIdList.length != 0) {
                        for (var i = 0; i < lessonTypeIdList.length; i++) {
                            for (var j = 0; j < XMLHttpRequest.length; j++) {
                                if (lessonTypeIdList[i].LessonTypeId == XMLHttpRequest[j].LessonTypeId) {
                                    $gridCooperationInstitutionManageLessonTypeMain.jqGrid('setSelection', j + 1);
                                }
                            }
                        }
                    }
                }

            });
        }


        initCooperationInstitutionGrid();
        initTrainingInstitutionGrid();
        initLessonGrid();
    }

    var initButtonArea = function() {

        var initCooperationInstitutionButtonArea = function() {

            //新增
            $("#btnCooperationInstitutionManage_Insert").on("click", function() {
                currentEditType = enum_EditTypes.insert;
                $mdlCooperationInstitutionManageInfo.modal("show");

                var institutionData = {};
                if (enum_EditTypes.insert == currentEditType) {
                    $divCooperationInstitutionManageInfo.find("[name='TrainingInstitutionName']").prop("disabled", true);
                    institutionData = getJson($divCooperationInstitutionManageInfo);
                    for (var p in institutionData) {
                        institutionData[p] = "";
                    }
                }
                setJson($divCooperationInstitutionManageInfo, institutionData);
            });

            //修改
            $("#btnCooperationInstitutionManage_Update").on("click", function() {
                var rowData = getCooperationInstitutionGridSelectedRowData($gridCooperationInstitutionManageMain, function() {
                    alert("请选择数据！");
                });
                if (rowData == undefined) {
                    return false;
                }
                institutionId = rowData.InstitutionId;
                var trainingInstitutionName = rowData.TrainingInstitutionName;
                currentEditType = enum_EditTypes.update;
                $mdlCooperationInstitutionManageInfo.modal("show");

                //根据Id获取合作机构信息
                var getCooperationInstitutionManageInfoById = function() {
                    var dataResult = {};
                    ajaxRequest({
                        url: "/" + controllerName + "/GetCooperationInstitutionManageInfoById",
                        data: { "institutionId": institutionId },
                        type: "get",
                        datatype: "json",
                        async: false,
                        success: function(jdata) {
                            dataResult = jdata;
                        }
                    });
                    return dataResult;
                }

                var institutionData = {};
                if (enum_EditTypes.update == currentEditType) {
                    $divCooperationInstitutionManageInfo.find("[name='TrainingInstitutionName']").prop("disabled", true);
                    institutionData = getCooperationInstitutionManageInfoById();
                    institutionData.TrainingInstitutionName = trainingInstitutionName;
                }
                setJson($divCooperationInstitutionManageInfo, institutionData);
                $divCooperationInstitutionManageInfo.find("[name='institutionId']").val(institutionId);
                return false;
            });

            //删除
            $("#btnCooperationInstitutionManage_Delete").on("click", function() {
                var rowData = getCooperationInstitutionGridSelectedRowData($gridCooperationInstitutionManageMain, function() {
                    alert("请选择数据！");
                });
                if (rowData == undefined) {
                    return false;
                }
                institutionId = rowData.InstitutionId;
                if (!confirm("确定要删除数据吗？")) {
                    return false;
                }
                ajaxRequest({
                    url: "/" + controllerName + "/DeleteCooperationInstitutionManage",
                    data: { "institutionId": institutionId },
                    type: "post",
                    datatype: "Json",
                    ansyc: false,
                    success: function(jdata) {
                        if (jdata.isSuccess != null) {
                            alert(jdata.ErrorMessage);
                            return false;
                        } else {
                            alert("删除成功！");
                            $gridCooperationInstitutionManageMain.trigger("reloadGrid");
                            return true;
                        }
                    }
                });
                return false;
            });

            //添加用户
            $("#btnCooperationInstitutionManage_Choose").on("click", function() {
                $mdlCooperationInstitutionManageInfo.modal("toggle");
                $mdlTrainingInstitutionInfo.modal("show");
            });

            //清空培训机构
            $("#btnCooperationInstitutionManage_Clear").on("click", function() {
                $divCooperationInstitutionManageInfo.find("[name='TrainingInstitutionId']").val("");
                $divCooperationInstitutionManageInfo.find("[name='TrainingInstitutionName']").val("");
            });
        }

        var initLessonTypeButtonArea = function() {

            //保存
            $("#btnLessonType_Save").on("click", function() {
                if (selectedInstitutionId === -1) {
                    alert("请选择合作机构！");
                    return false;
                }

                var arrRowid = $gridCooperationInstitutionManageLessonTypeMain.jqGrid("getGridParam", "selarrrow");
                var lessonTypeIdList = [];

                for (var i = 0; i < arrRowid.length; i++) {
                    var lessonTypeId = $gridCooperationInstitutionManageLessonTypeMain.jqGrid("getRowData", arrRowid[i]).LessonTypeId;
                    lessonTypeIdList.push(lessonTypeId);
                }

                var institutionId = selectedInstitutionId;

                var postData = {};
                postData.lessonTypeIdList = [];
                for (var j = 0; j < lessonTypeIdList.length; j++) {
                    postData.lessonTypeIdList.push(lessonTypeIdList[j]);
                }

                postData.institutionId = institutionId;
                var strData = JSON.stringify(postData);
                var ajaxOpt = {
                    url: "/" + controllerName + "/InsertRelLessonTypeCooperationInstitution",
                    data: { "strData": strData },
                    type: "post",
                    async: false,
                    success: function(jdata) {
                        if (jdata.isSuccess != null) {
                            alert(jdata.ErrorMessage);
                            return false;
                        }
                        alert("保存成功！");
                        $gridCooperationInstitutionManageLessonTypeMain.trigger("reloadGrid");
                        return true;
                    }
                };
                ajaxRequest(ajaxOpt);

            });
        }

        initCooperationInstitutionButtonArea();
        initLessonTypeButtonArea();
    }

    var initModel = function() {

        var initCooperationInstitutionModel = function() {

            $divCooperationInstitutionManageInfo.find('input[name=LogoPath]').change(function() {
                $divCooperationInstitutionManageInfo.find('input[name=LogoFile]').val($(this).val());
            });

            $divCooperationInstitutionManageInfo.find('input[name=QRCodePath]').change(function() {
                $divCooperationInstitutionManageInfo.find('input[name=QRCodeFile]').val($(this).val());
            });

            var initCooperationInstitutionSaveButton = function() {
                $("#saveCooperationInstitutionManage_Confirm").on("click", function() {
                    var checkResult = verifyForm($divCooperationInstitutionManageInfo);
                    if (!checkResult) {
                        return false;
                    }
                    var institutionData = getJson($divCooperationInstitutionManageInfo);

                    var functionName = "";
                    if (currentEditType === enum_EditTypes.update) {
                        functionName = "/UpdateCooperationInstitutionManage";
                    } else if (currentEditType === enum_EditTypes.insert) {
                        functionName = "/InsertCooperationInstitutionManage";
                    }
                    $divCooperationInstitutionManageInfo.ajaxSubmit({
                        url: "/" + controllerName + functionName,
                        type: "post",
                        date: institutionData,
                        beforeSubmit: function(formArray, jqForm) {
                        },
                        success: function(jdata) {
                            if (jdata.isSuccess != null) {
                                alert(jdata.ErrorMessage);
                                return false;
                            }
                            alert("保存成功！");
                            $gridCooperationInstitutionManageMain.trigger("reloadGrid");
                            $mdlCooperationInstitutionManageInfo.modal("toggle");
                            return true;
                        }
                    });
                    return false;
                });
            }

            initCooperationInstitutionSaveButton();
        }

        var initTrainingInstitutionModel = function() {

            var initTrainingInstitutionSaveButton = function() {
                $("#saveTrainingInstitution_Confirm").on("click", function() {
                    chooseInstitution();
                });
            }

            var initTrainingInstitutionCloseButton = function() {
                $("#closeTrainingInstitution_Confirm").on("click", function() {
                    $mdlTrainingInstitutionInfo.modal("toggle");
                    $mdlCooperationInstitutionManageInfo.modal("show");
                });
            }

            initTrainingInstitutionSaveButton();
            initTrainingInstitutionCloseButton();
        }

        var initCooperationInstitutionRelatedLinkModal = function() {
            $mdlCooperationInstitutionRelatedLinkInfo.find("[name='btnSave']").on("click", function() {

                var thisButton = $(this);
                thisButton.prop('disabled', true);
                var fromData = getJson($divCooperationInstitutionRelatedLinkInfo);
                ajaxRequest({
                    url: "/" + controllerName + "/SaveCooperationInstitutionRelatedLink",
                    data: fromData,
                    type: "post",
                    async: true,
                    success: function(jdata) {
                        thisButton.prop('disabled', false);
                        if (fromData.LinkId && fromData.LinkId != 0) {
                            alert('保存成功');
                        } else {
                            alert('新增成功');
                        }
                    }
                });
            });
        }

        initCooperationInstitutionRelatedLinkModal();
        initCooperationInstitutionModel();
        initTrainingInstitutionModel();
    }

    $(document).ready(function() {
        initQueryArea();
        initJqGrid();
        initButtonArea();
        initModel();
    });
});