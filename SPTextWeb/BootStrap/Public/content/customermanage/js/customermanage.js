'use strict';
$(function () {
    var controllerName = "CustomerManage";
    var $divCustomerManageQueryArea = $("#divCustomerManage_QueryArea");
    var $gridCustomerManageMain = $("#gridCustomerManage_Main");
    var $pagerCustomerManageMain = $("#pagerCustomerManage_Main");


    var getGridSelectedRowData = function ($gridCustomerManageMain, noSelectionCallback) {
        var rowId = $gridCustomerManageMain.jqGrid("getGridParam", "selrow");
        if (!rowId) {
            if ("function" == typeof (noSelectionCallback)) {
                noSelectionCallback();
            }
            return undefined;
        }
        var rowData = $gridCustomerManageMain.jqGrid("getRowData", rowId);
        return rowData;
    };

    var initQueryArea = function () {

        var initQueryButton = function () {
            $("#btnCustomerManage_Query").on("click", function () {
                var queryData = {};
                var divQueryArea = $divCustomerManageQueryArea;
                queryData = getJson(divQueryArea);
                $gridCustomerManageMain.jqGrid("setGridParam", { page: 1, postData: queryData }).trigger("reloadGrid");
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


                var $SelectProvince = $divCustomerManageQueryArea.find("[name='Province']");
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

            var initSelectArea = function () {

                var cityName = $divCustomerManageQueryArea.find("[name='City']").val();;
                var $SelectArea = $divCustomerManageQueryArea.find("[name='Area']");

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

            var initSelectCity = function () {
                var getSelectCityList = function (province) {
                    var dataResult = {};
                    ajaxRequest({
                        url: "/" + controllerName + "/GetCityListByProvinceName",
                        data: { "provinceName": province },
                        type: "post",
                        datatype: "json",
                        async: false,
                        success: function (jdata) {
                            dataResult = jdata;
                        }
                    });
                    return dataResult;
                }

                var province = $divCustomerManageQueryArea.find("[name='Province']").val();
               
                var cityList = getSelectCityList(province);

                var $SelectCity = $divCustomerManageQueryArea.find("[name='City']");
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

            $divCustomerManageQueryArea.find("[name='Province']").on("change", function () {
                initSelectCity();
            });

            $divCustomerManageQueryArea.find("[name='City']").on("change", function () {
                initSelectArea();
            });
        }

        initProvinceCityAreaAction();

        initQueryButton();
    }

    var initJqGrid = function () {
        var initCustomerManageJqGrid = function () {
            var queryData = {};
            var divQueryArea = $divCustomerManageQueryArea;
            queryData = getJson(divQueryArea);
            $gridCustomerManageMain.jqGrid({
                url: "/" + controllerName + "/GetCustomerManageListForJqGrid",
                datatype: "json",
                mtype: "post",
                postData: queryData,
                colNames: ["CustomerId","学员姓名", "身份证号", "联系电话", "所属企业", "所在省份", "所在城市", "所在区域"],
                colModel: [
                    { name: "CustomerId", index: "CustomerId", width: 50, hidden: true },
                    { name: "CustomerName", index: "CustomerName", align: "center", width: 100 },
                    { name: "IdNumber", index: "IdNumber", align: "center", width: 200 },
                    { name: "PhoneNumber", index: "PhoneNumber", align: "center", width: 150 },
                    { name: "EnterpriseName", index: "EnterpriseName", align: "center", width: 330 },
                    { name: "Province", index: "Province", align: "center", width: 150 },
                    { name: "City", index: "City", align: "center", width: 150 },
                    { name: "Area", index: "Area", align: "center", width: 150 }
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
                pager: $pagerCustomerManageMain,
                loadComplete: function () {
                    var table = this;
                    updatePagerIcons(table);
                    jqGridAutoWidth();
                    setGridHeight($gridCustomerManageMain.selector);
                }
            });
        }

        initCustomerManageJqGrid();
    }

    var initButtoArea = function () {

        var resetPassword = function () {
            $("#btnCustomerManage_ResetPassword").on("click", function () {
               
                var rowData = getGridSelectedRowData($gridCustomerManageMain, function () {
                    alert("请选择数据！");
                    
                });

                if (rowData == undefined) {
                    return false;
                }

                var arrRowid = $gridCustomerManageMain.jqGrid("getGridParam", "selarrrow");

                if (!confirm("确认重置【" + arrRowid.length + "】行密码吗？")) {
                    return false;
                }

                var customerIdList = [];
                for (var i = 0; i < arrRowid.length; i++) {
                    var customerId = $gridCustomerManageMain.jqGrid("getRowData", arrRowid[i]).CustomerId;
                    customerIdList.push(customerId);
                }

                var postData = {};
                postData.CustomerIdList = [];
                for (var j = 0; j < customerIdList.length; j++) {
                    postData.CustomerIdList.push(customerIdList[j]);
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

        resetPassword();

    }

    $(document).ready(function() {
        initQueryArea();
        initJqGrid();
        initButtoArea();
    });
})