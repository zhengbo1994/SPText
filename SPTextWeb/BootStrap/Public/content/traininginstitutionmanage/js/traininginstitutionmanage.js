'use strict';
$(function () {
    var controllerName = "TrainingInstitutionManage";
    var $divTrainingInstitutionManageQueryArea = $("#divTrainingInstitutionManage_QueryArea");
    var $gridTrainingInstitutionManagemain = $("#gridTrainingInstitutionManage_main");
    var $pagerTrainingInstitutionManagemain = $("#pagerTrainingInstitutionManage_main");
    var $mdlTrainingInstitutionManageInfo = $("#mdlTrainingInstitutionManage_Info");
    var $divTrainingInstitutionManageInfo = $("#divTrainingInstitutionManage_Info");

    var enum_EditTypes = { insert: 0, update: 1, view: 2 }
    var currentEdittype;
    var institutionId = 0;


    var getGridSelectedRowData = function ($gridTrainingInstitutionManagemain, noSelectionCallback) {
        var rowId = $gridTrainingInstitutionManagemain.jqGrid("getGridParam", "selrow");
        if (!rowId) {
            if ("function" == typeof (noSelectionCallback)) {
                noSelectionCallback();
            }
            return undefined;
        }
        var rowData = $gridTrainingInstitutionManagemain.jqGrid("getRowData", rowId);
        return rowData;
    }

    var initQueryArea = function () {

        var initQueryButton = function () {
            $("#btnTrainingInstitutionManage_Query").on("click", function () {
                var queryData = {};
                var divQueryArea = $divTrainingInstitutionManageQueryArea;
                queryData = getJson(divQueryArea);
                $gridTrainingInstitutionManagemain.jqGrid("setGridParam", { page: 1, postData: queryData }).trigger("reloadGrid");
            });
        }

        //省、市、区信息
        var initProvinceCityAreaAction = function () {

            var initSelectProvince = function () {

                var getSelectProvinceList = function () {
                    var dataResult = {};
                    ajaxRequest({
                        url: "/" + controllerName + "/GetProvinceList",
                        type: "post",
                        datatype: "json",
                        async: false,
                        success: function (jdata) {
                            dataResult = jdata;
                        }
                    });
                    return dataResult;
                }

                var provinceList = getSelectProvinceList();


                var $SelectProvince = $divTrainingInstitutionManageQueryArea.find("[name='Province']");
                $SelectProvince.empty();
                var $optionAll = $("<option>");
                $optionAll.val("");
                $optionAll.text("请选择");
                $SelectProvince.append($optionAll);

                for (var i = 0; i < provinceList.length; i++) {
                    var province = provinceList[i];
                    var $option = $("<option>");
                    $option.val(province);
                    $option.text(province);
                    $SelectProvince.append($option);
                }
            }

            var initSelectCity = function () {

                var provinceName = $divTrainingInstitutionManageQueryArea.find("[name='Province']").val();
                var $selectCity = $divTrainingInstitutionManageQueryArea.find("[name='City']");

                var getSelectCityList = function () {
                    var dataResult = {};
                    ajaxRequest({
                        url: "/" + controllerName + "/GetCityListByProvinceName",
                        data: { "provinceName": provinceName },
                        type: "post",
                        datatype: "json",
                        async: false,
                        success: function (jdata) {
                            dataResult = jdata;
                        }
                    });
                    return dataResult;
                }
                var cityList = getSelectCityList();

                $selectCity.empty();
                var $optionAll = $("<option>");
                $optionAll.val("");
                $optionAll.text("请选择");
                $selectCity.append($optionAll);
                for (var i = 0; i < cityList.length; i++) {
                    var city = cityList[i];
                    var $option = $("<option>");
                    $option.val(city);
                    $option.text(city);
                    $selectCity.append($option);
                }
            }

            var initSelectArea = function () {

                var cityName = $divTrainingInstitutionManageQueryArea.find("[name='City']").val();
                var $SelectArea = $divTrainingInstitutionManageQueryArea.find("[name='Area']");

                var getSelectAreaList = function () {
                    var dataResult = {};
                    ajaxRequest({
                        url: "/" + controllerName + "/GetAreaListByCityName",
                        type: "post",
                        datatype: "json",
                        data: { "cityName": cityName },
                        async: false,
                        success: function (jdata) {
                            dataResult = jdata;
                        }
                    });
                    return dataResult;
                }

                var areaList = getSelectAreaList();

                $SelectArea.empty();
                var $optionAll = $("<option>");

                $optionAll.val("");
                $optionAll.text("请选择");
                $SelectArea.append($optionAll);
                for (var i = 0; i < areaList.length; i++) {
                    var area = areaList[i];
                    var $option = $("<option>");
                    $option.val(area);
                    $option.text(area);
                    $SelectArea.append($option);
                }
            }


            initSelectProvince();

            $divTrainingInstitutionManageQueryArea.find("[name='Province']").on("change", function () {
                initSelectCity();
            });

            $divTrainingInstitutionManageQueryArea.find("[name='City']").on("change", function () {
                initSelectArea();
            });
        }

        initQueryButton();
        initProvinceCityAreaAction();
    }

    var initJqGrid = function () {

        var initTrainingInstitutionGrid = function () {
            var queryData = {};
            var divQueryArea = $divTrainingInstitutionManageQueryArea;
            queryData = getJson(divQueryArea);
            $gridTrainingInstitutionManagemain.jqGrid({
                url: "/" + controllerName + "/GetTrainingInstitutionListForJqgrid",
                datatype: "json",
                postData: queryData,
                colNames: ["InstitutionId", "培训机构名称", "社会信用代码", "联系人", "联系人电话", "所在省份", "所在城市", "所在区域", "培训机构地址"],
                colModel: [
                    { name: "InstitutionId", index: "InstitutionId", width: 30, hidden: true },
                    { name: "InstitutionName", index: "InstitutionName", align: "center", width: 200 },
                    { name: "SocialCreditCode", index: "SocialCreditCode", align: "center", width: 180 },
                    { name: "ContactPerson", index: "ContactPerson", align: "center", width: 100 },
                    { name: "ContactNumber", index: "ContactNumber", align: "center", width: 120 },
                    { name: "Province", index: "Province", align: "center", width: 80 },
                    { name: "City", index: "City", align: "center", width: 80 },
                    { name: "Area", index: "Area", align: "center", width: 100 },
                    { name: "Address", index: "Address", align: "center", width: 200 }
                ],
                multiselect: true,
                autowidth: true,
                rowNum: 20,
                altRows: true,
                pgbuttons: true,
                viewrecords: true,
                shrinkToFit: false,
                pginput: true,
                rowList: [10, 20, 30, 50, 70, 100],
                pager: $pagerTrainingInstitutionManagemain,
                loadComplete: function () {
                    var table = this;
                    updatePagerIcons(table);
                    jqGridAutoWidth();
                    setGridHeight($gridTrainingInstitutionManagemain.selector);
                }
            });
        }

        initTrainingInstitutionGrid();
    }

    var initButtonArea = function () {

        //新增
        $("#btnTrainingInstitutionManage_Insert").on("click", function () {
            currentEdittype = enum_EditTypes.insert;
            $mdlTrainingInstitutionManageInfo.modal("show");
        });

        //修改
        $("#btnTrainingInstitutionManage_Update").on("click", function () {
            var rowData = getGridSelectedRowData($gridTrainingInstitutionManagemain, function () {
                alert("请选择数据！");
            });
            if (rowData == undefined) {
                return false;
            }
            currentEdittype = enum_EditTypes.update;
            institutionId = rowData.InstitutionId;
            $mdlTrainingInstitutionManageInfo.modal("show");
            return false;
        });

        //重置密码
        $("#btnTrainingInstitutionManage_ResetPassword").on("click", function () {
            var rowData = getGridSelectedRowData($gridTrainingInstitutionManagemain, function () {
                alert("请选择数据！");
            });
            if (rowData == undefined) {
                return false;
            }

            var arrRowid = $gridTrainingInstitutionManagemain.jqGrid("getGridParam", "selarrrow");
            if (!confirm("确认重置【" + arrRowid.length + "】行密码吗？")) {
                return false;
            }

            var trainingInstitutionIdList = [];
            for (var i = 0; i < arrRowid.length; i++) {
                var trainingInstitutionId = $gridTrainingInstitutionManagemain.jqGrid("getRowData", arrRowid[i]).InstitutionId;
                trainingInstitutionIdList.push(trainingInstitutionId);
            }

            var postData = {};
            postData.TrainingInstitutionIdList = [];
            for (var j = 0; j < trainingInstitutionIdList.length; j++) {
                postData.TrainingInstitutionIdList.push(trainingInstitutionIdList[j]);
            }
            var strData = JSON.stringify(postData);
            ajaxRequest({
                url: "/" + controllerName + "/ResetPassword",
                data: { "strData": strData },
                type: "post",
                ansyc: false,
                success: function (jdata) {
                    if (jdata.isSuccess != null) {
                        alert(jdata.ErrorMessage);
                        return false;
                    } else {
                        alert("重置密码成功！");
                        return true;
                    }
                }
            });
        });
    }

    var initModel = function () {
        var iniTrainingInstitutionManageInfoModel = function () {

            $divTrainingInstitutionManageInfo.find('input[name=TrainingInstitutionImagePath]').change(function () {
                $divTrainingInstitutionManageInfo.find('input[name=TrainingInstitutionImageFile]').val($(this).val());
            });

            $divTrainingInstitutionManageInfo.find('input[name=TrainingInstitutionSealImagePath]').change(function () {
                $divTrainingInstitutionManageInfo.find('input[name=TrainingInstitutionSealImageFile]').val($(this).val());
            });

            $mdlTrainingInstitutionManageInfo.on('show.bs.modal', function (e) {
                var getTrainingInstitutionListByInstitutionId = function () {
                    var queryData = {};
                    var dataResult = {};
                    queryData.institutionId = institutionId;
                    ajaxRequest({
                        url: "/" + controllerName + "/GetTrainingInstitutionListByInstitutionId",
                        data: queryData,
                        type: "get",
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

                var institutionData = {};
         
                if (enum_EditTypes.insert == currentEdittype) {
                    institutionData = getJson($divTrainingInstitutionManageInfo);
                    for (var p in institutionData) {
                        institutionData[p] = "";
                    }
                
                    $divTrainingInstitutionManageInfo.find('[name=SocialCreditCode]').removeAttr("readonly");

                } else if (enum_EditTypes.update == currentEdittype) {
                    institutionData = getTrainingInstitutionListByInstitutionId();
                    $divTrainingInstitutionManageInfo.find('[name=SocialCreditCode]').attr("readonly", "readonly");
                    $divTrainingInstitutionManageInfo.find("[name='Province']").change();
                    $divTrainingInstitutionManageInfo.find("[name='City']").val(institutionData.City);
                    $divTrainingInstitutionManageInfo.find("[name='City']").change();
                    $divTrainingInstitutionManageInfo.find("[name='Area']").val(institutionData.Area);
                }
                setJson($divTrainingInstitutionManageInfo, institutionData);
         
                $divTrainingInstitutionManageInfo.find("[name='institutionId']").val(institutionId);
            });
        }

        var initTrainingInstitutionManageSaveButton = function () {
            $mdlTrainingInstitutionManageInfo.find("[id='saveTrainingInstitutionManage_Confirm']").on("click", function () {
                var checkResult = verifyForm($divTrainingInstitutionManageInfo);
                if (!checkResult) {
                    return false;
                }
                var institutionData = getJson($divTrainingInstitutionManageInfo);

                var functionName = "";
                if (currentEdittype === enum_EditTypes.update) {
                    functionName = "/UpdateTrainingInstitutionManage";
                } else if (currentEdittype === enum_EditTypes.insert) {
                    functionName = "/InsertTrainingInstitutionManage";
                }

                $divTrainingInstitutionManageInfo.ajaxSubmit({
                    url: "/" + controllerName + functionName,
                    type: "post",
                    date: institutionData,
                    beforeSubmit: function (formArray, jqForm) {
                    },
                    success: function (jdata) {

                        if (jdata.isSuccess != null) {
                            alert(jdata.ErrorMessage);
                            return false;
                        }

                        alert("保存成功！");
                        $gridTrainingInstitutionManagemain.trigger("reloadGrid");
                        $mdlTrainingInstitutionManageInfo.modal("toggle");
                        return true;
                    }
                });
                return false;
            });
        }

        //省、市、区信息
        var initProvinceCityAreaAction = function () {

            var initSelectProvince = function () {
                var getSelectProvinceList = function () {
                    var dataResult = {};
                    ajaxRequest({
                        url: "/" + controllerName + "/GetProvinceList",
                        type: "post",
                        datatype: "json",
                        async: false,
                        success: function (jdata) {
                            dataResult = jdata;
                        }
                    });
                    return dataResult;
                }

                var provinceList = getSelectProvinceList();


                var $SelectProvince = $divTrainingInstitutionManageInfo.find("[name='Province']");
                $SelectProvince.empty();
                var $optionAll = $("<option>");
                $optionAll.val("");
                $optionAll.text("请选择");
                $SelectProvince.append($optionAll);

                for (var i = 0; i < provinceList.length; i++) {
                    var province = provinceList[i];
                    var $option = $("<option>");
                    $option.val(province);
                    $option.text(province);
                    $SelectProvince.append($option);
                }
            }

            var initSelectCity = function () {

                var provinceName = $divTrainingInstitutionManageInfo.find("[name='Province']").val();
                var getSelectCityList = function () {
                    var dataResult = {};
                    ajaxRequest({
                        url: "/" + controllerName + "/GetCityListByProvinceName",
                        data: { "provinceName": provinceName },
                        type: "post",
                        datatype: "json",
                        async: false,
                        success: function (jdata) {
                            dataResult = jdata;
                        }
                    });
                    return dataResult;
                }
                var cityList = getSelectCityList();


                var $SelectCity = $divTrainingInstitutionManageInfo.find("[name='City']");
                $SelectCity.empty();
                var $optionAll = $("<option>");
                $optionAll.val("");
                $optionAll.text("请选择");
                $SelectCity.append($optionAll);
                for (var i = 0; i < cityList.length; i++) {
                    var city = cityList[i];
                    var $option = $("<option>");
                    $option.val(city);
                    $option.text(city);
                    $SelectCity.append($option);
                }
            }

            var initSelectArea = function () {

                var cityName = $divTrainingInstitutionManageInfo.find("[name='City']").val();
                var $SelectArea = $divTrainingInstitutionManageInfo.find("[name='Area']");

                var getSelectAreaList = function () {
                    var dataResult = {};
                    ajaxRequest({
                        url: "/" + controllerName + "/GetAreaListByCityName",
                        type: "post",
                        datatype: "json",
                        data: { "cityName": cityName },
                        async: false,
                        success: function (jdata) {
                            dataResult = jdata;
                        }
                    });
                    return dataResult;
                }

                var areaList = getSelectAreaList();

                $SelectArea.empty();
                var $optionAll = $("<option>");

                $optionAll.val("");
                $optionAll.text("请选择");
                $SelectArea.append($optionAll);
                for (var i = 0; i < areaList.length; i++) {
                    var area = areaList[i];
                    var $option = $("<option>");
                    $option.val(area);
                    $option.text(area);
                    $SelectArea.append($option);
                }
            }


            initSelectProvince();

            $divTrainingInstitutionManageInfo.find("[name='Province']").on("change", function () {
                initSelectCity();
            });

            $divTrainingInstitutionManageInfo.find("[name='City']").on("change", function () {
                initSelectArea();
            });
        }

        initProvinceCityAreaAction();
        iniTrainingInstitutionManageInfoModel();
        initTrainingInstitutionManageSaveButton();
    }


    $(document).ready(function () {
        initQueryArea();
        initJqGrid();
        initButtonArea();
        initModel();
    });
});