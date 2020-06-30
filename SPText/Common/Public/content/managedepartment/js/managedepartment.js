'use strict';
$(function () {
    var controllerName = "ManageDepartment";

    //省管理部门
    var $divManageDepartmentQueryArea = $("#divManageDepartment_QueryArea");
    var $gridManageDepartmentMain = $("#gridManageDepartment_Main");
    var $pagerManageDepartmentMmain = $("#pagerManageDepartment_Mmain");
    var $mdlManageDepartmentInfo = $("#mdlManageDepartment_Info");
    var $divManageDepartmentInfo = $("#divManageDepartment_Info");

    var enumEditTypes = { insert: 0, update: 1, view: 2 }
    var currentEdittype;
    var manageDepartmentId = 0;

    var getGridSelectedRowData = function ($gridManageDepartmentMain, noSelectionCallback) {
        var rowId = $gridManageDepartmentMain.jqGrid("getGridParam", "selrow");
        if (!rowId) {
            if ("function" == typeof (noSelectionCallback)) {
                noSelectionCallback();
            }
            return undefined;
        }
        var rowData = $gridManageDepartmentMain.jqGrid("getRowData", rowId);
        return rowData;
    };

    var initQueryArea = function () {

        var initManageDepartmentQueryArea = function () {
            var initManageDepartmentQueryButton = function () {
                $("#btnManageDepartment_Query").on("click", function () {
                    var queryData = {};
                    var divQueryArea = $divManageDepartmentQueryArea;
                    queryData = getJson(divQueryArea);
                    $gridManageDepartmentMain.jqGrid("setGridParam", { pagt: 1, postData: queryData }).trigger("reloadGrid");
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


                    var $SelectProvince = $divManageDepartmentQueryArea.find("[name='Province']");
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

                    var provinceName = $divManageDepartmentQueryArea.find("[name='Province']").val();
                    var $SelectCity = $divManageDepartmentQueryArea.find("[name='City']");

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

                    var $SelectCity = $divManageDepartmentQueryArea.find("[name='City']");
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



                initSelectProvince();

                $divManageDepartmentQueryArea.find("[name='Province']").on("change", function () {
                    initSelectCity();
                });
            }


            initManageDepartmentQueryButton();
            initProvinceCityAreaAction();
        }


        initManageDepartmentQueryArea();
    }

    var initJqGrid = function () {

        var initManageDepartmentJqGrid = function () {

            //重置密码
            window.btnManageDepartment_ResetPassword = function (rowId) {
                var rowData = $gridManageDepartmentMain.jqGrid("getRowData", rowId);
                var accountName = rowData.AccountName;
                ajaxRequest({
                    url: "/" + controllerName + "/ResetPassword",
                    data: { "AccountName": accountName },
                    type: "post",
                    ansyc: false,
                    success: function (jdata) {
                        if (jdata.isSuccess != null) {
                            alert(jdata.ErrorMessage);
                            return false;
                        } else {
                            alert("密码重置成功！");
                            return true;
                        }
                    }
                });
            }

            var $queryData = {};
            $queryData = getJson($divManageDepartmentQueryArea);
            $gridManageDepartmentMain.jqGrid({
                url: "/" + controllerName + "/GetManageDepartmentListForJqGrid",
                datatype: "json",
                postData: $queryData,
                colNames: ["ManageDepartmentId", "账号", "管理部门名称", "所在省份", "所在城市", "创建者", "创建时间","操作"],
                colModel: [
                    { name: "ManageDepartmentId", index: "ManageDepartmentId", width: 10, hidden: true },
                    { name: "AccountName", index: "AccountName", align: "center", width: 100 },
                    { name: "ManageDepartmentName", index: "ManageDepartmentName", align: "center", width: 150 },
                    { name: "Province", index: "Province", align: "center", width: 80 },
                    { name: "City", index: "City", align: "center", width: 80 },
                    { name: "CreateBy", index: "CreateBy", align: "center", width: 80 },
                    { name: "CreateDate", index: "CreateDate", align: "center", width: 120 },
                    {
                        name: '操作', index: '操作', width: 100, align: "center", formatter: function (cellvalue, options, rowobj) {
                            var buttons = '';
                            buttons += '<a href="#" title="重置密码" onclick="btnManageDepartment_ResetPassword(' + options.rowId + ')" style="padding: 7px;line-height: 1em;">'
                                + '<i class="aace-icon fa fa-refresh"></i> 重置密码</a>';
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
                pager: $pagerManageDepartmentMmain,
                loadComplete: function () {
                    var table = this;
                    updatePagerIcons(table);
                    jqGridAutoWidth();
                    setGridHeight($gridManageDepartmentMain.selector);
                }
            });
        }


        initManageDepartmentJqGrid();
    }

    var initButtonArea = function () {

        //新增
        $("#btnManageDepartment_Insert").on("click", function () {
            currentEdittype = enumEditTypes.insert;
            $mdlManageDepartmentInfo.modal("show");
        });

        //修改
        $("#btnManageDepartment_Update").on("click", function () {
            var rowData = getGridSelectedRowData($gridManageDepartmentMain, function () {
                alert("请选择数据！");
            });
            if (rowData == undefined) {
                return false;
            }

            manageDepartmentId = rowData.ManageDepartmentId;
            currentEdittype = enumEditTypes.update;
            $mdlManageDepartmentInfo.modal("show");
            return false;
        });
    }

    var initModel = function () {

        var iniManageDepartmentInfoModel = function () {
            $mdlManageDepartmentInfo.on('show.bs.modal', function (e) {
                var getManageDepartmentInfo = function () {
                    var queryData = {};
                    var dataResult = {};
                    queryData.ManageDepartmentId = manageDepartmentId;
                    ajaxRequest({
                        url: "/" + controllerName + "/GetManageDepartmentListById",
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

                var manageDepartmentData = {};

                if (enumEditTypes.insert == currentEdittype) {
                    manageDepartmentData = getJson($divManageDepartmentInfo);
                    for (var p in manageDepartmentData) {
                        manageDepartmentData[p] = "";
                    }

                    $divManageDepartmentInfo.find("[name='AccountName']").removeAttr("readonly", "readonly");

                } else if (enumEditTypes.update == currentEdittype) {
                    manageDepartmentData = getManageDepartmentInfo();
                    $divManageDepartmentInfo.find("[name='AccountName']").attr("readonly", "readonly");
                    $divManageDepartmentInfo.find("[name='Province']").change();
                    $divManageDepartmentInfo.find("[name='City']").val(manageDepartmentData.City);
                }
                setJson($divManageDepartmentInfo, manageDepartmentData);
                $divManageDepartmentInfo.find("[name='ManageDepartmentId']").val(manageDepartmentId);
            });
        }

        var initManageDepartmentSaveButton = function () {
            $mdlManageDepartmentInfo.find("[id='saveManageDepartment_Confirm']").on("click", function() {
                var checkResult = verifyForm($divManageDepartmentInfo);
                if (!checkResult) {
                    return false;
                }
                var manageDepartmentData = getJson($divManageDepartmentInfo);

                var functionName = "";
                if (currentEdittype === enumEditTypes.update) {
                    functionName = "/UpdateManageDepartment";
                } else if (currentEdittype === enumEditTypes.insert) {
                    functionName = "/InsertManageDepartment";
                }

                $divManageDepartmentInfo.ajaxSubmit({
                    url: "/" + controllerName + functionName,
                    type: "post",
                    date: manageDepartmentData,
                    beforeSubmit: function(formArray, jqForm) {
                    },
                    success: function(jdata) {
                        if (jdata.isSuccess != null) {
                            alert(jdata.ErrorMessage);
                            return false;
                        }
                        alert("保存成功！");
                        $gridManageDepartmentMain.trigger("reloadGrid");
                        $mdlManageDepartmentInfo.modal("toggle");
                        return true;
                    }
                });
                return false;
            });
        }

        //省、市、区信息
        var initProvinceCityAreaAction = function () {

            var intiQueryProvinceInput = function () {
                var getProvinceList = function () {
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

                var provinceList = getProvinceList();


                var $SelectProvince = $divManageDepartmentInfo.find("[name='Province']");
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

            var initQueryCityInput = function () {
                var getCityList = function (province) {
                    var dataResult = {};
                    ajaxRequest({
                        url: "/" + controllerName + "/GetCityListByProvinceName",
                        data: { "province": province },
                        type: "post",
                        datatype: "json",
                        async: false,
                        success: function (jdata) {
                            dataResult = jdata;
                        }
                    });
                    return dataResult;
                }

                var province = $divManageDepartmentInfo.find("[name='Province']").val();
                var cityList = getCityList(province);

                var $txtQueryCityInput = $divManageDepartmentInfo.find("[name='City']");
                $txtQueryCityInput.empty();
                var $optionCityAll = $("<option>");
                $optionCityAll.text("请选择");
                $optionCityAll.val("");
                $txtQueryCityInput.append($optionCityAll);
                for (var i = 0; i < cityList.length; i++) {
                    var city = cityList[i];
                    var $option = $("<option>");
                    $option.val(city);
                    $option.text(city);
                    $txtQueryCityInput.append($option);
                }
            }

            intiQueryProvinceInput();

            $divManageDepartmentInfo.find("[name='Province']").on("change", function () {
                initQueryCityInput();
            });
        }

        initProvinceCityAreaAction();
        iniManageDepartmentInfoModel();
        initManageDepartmentSaveButton();
    }

    $(document).ready(function () {
        initQueryArea();
        initJqGrid();
        initButtonArea();
        initModel();
    });
})