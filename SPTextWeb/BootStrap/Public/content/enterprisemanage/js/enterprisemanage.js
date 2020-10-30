'use strict';
$(function() {
    var controllerName = "EnterpriseManage";
    var $divEnterpriseManageQueryArea = $("#divEnterpriseManage_QueryArea");
    var $gridEnterpriseManagemain = $("#gridEnterpriseManage_main");
    var $pagerEnterpriseManagemain = $("#pagerEnterpriseManage_main");


    var getGridSelectedRowData = function($gridCustomerManageMain, noSelectionCallback) {
        var rowId = $gridCustomerManageMain.jqGrid("getGridParam", "selrow");
        if (!rowId) {
            if ("function" == typeof (noSelectionCallback)) {
                noSelectionCallback();
            }
            return undefined;
        }
        var rowData = $gridCustomerManageMain.jqGrid("getRowData", rowId);
        return rowData;
    }

    var initQueryArea = function() {

        var initQueryButton = function() {
            $("#btnEnterpriseManage_Query").on("click", function() {
                var queryData = {};
                var divQueryArea = $divEnterpriseManageQueryArea;
                queryData = getJson(divQueryArea);
                $gridEnterpriseManagemain.jqGrid("setGridParam", { page: 1, postData: queryData }).trigger("reloadGrid");
            });
        }

        //省、市、区信息
        var initProvinceCityAreaAction = function() {

            var initSelectProvince = function() {

                var getSelectProvinceList = function() {
                    var dataResult = {};
                    ajaxRequest({
                        url: "/" + controllerName + "/GetProvinceList",
                        type: "post",
                        datatype: "json",
                        async: false,
                        success: function(jdata) {
                            dataResult = jdata;
                        }
                    });
                    return dataResult;
                }

                var provinceList = getSelectProvinceList();


                var $SelectProvince = $divEnterpriseManageQueryArea.find("[name='Province']");
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

            var initSelectArea = function() {

                var cityName = $divEnterpriseManageQueryArea.find("[name='City']").val();;
                var $SelectArea = $divEnterpriseManageQueryArea.find("[name='Area']");

                var getSelectAreaList = function() {
                    var dataResult = {};
                    ajaxRequest({
                        url: "/" + controllerName + "/GetAreaListByCityName",
                        type: "post",
                        datatype: "json",
                        data: { "cityName": cityName },
                        async: false,
                        success: function(jdata) {
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

            var initSelectCity = function() {
                var getSelectCityList = function(province) {
                    var dataResult = {};
                    ajaxRequest({
                        url: "/" + controllerName + "/GetCityListByProvinceName",
                        data: { "provinceName": province },
                        type: "post",
                        datatype: "json",
                        async: false,
                        success: function(jdata) {
                            dataResult = jdata;
                        }
                    });
                    return dataResult;
                }

                var province = $divEnterpriseManageQueryArea.find("[name='Province']").val();
                
                var cityList = getSelectCityList(province);

                var $SelectCity = $divEnterpriseManageQueryArea.find("[name='City']");
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

            $divEnterpriseManageQueryArea.find("[name='Province']").on("change", function() {
                initSelectCity();
            });

            $divEnterpriseManageQueryArea.find("[name='City']").on("change", function() {
                initSelectArea();
            });
        }

        initProvinceCityAreaAction();

        initQueryButton();
    }

    var initJqGrid = function() {

        var initEnterpriseGrid = function() {
            var queryData = {};
            var divQueryArea = $divEnterpriseManageQueryArea;
            queryData = getJson(divQueryArea);
            $gridEnterpriseManagemain.jqGrid({
                url: "/" + controllerName + "/GetEnterpriseListForJqgrid",
                datatype: "json",
                postData: queryData,
                colNames: ["EnterpriseId", "企业名称", "社会信用代码", "联系人", "联系人电话", "所在省份", "所在城市", "所在区域", "企业地址"],
                colModel: [
                    { name: "EnterpriseId", index: "EnterpriseId", width: 30, hidden: true },
                    { name: "EnterpriseName", index: "EnterpriseName", align: "center", width: 200 },
                    { name: "SocialCreditCode", index: "SocialCreditCode", align: "center", width: 180 },
                    { name: "ContactPerson", index: "ContactPerson", align: "center", width: 100 },
                    { name: "ContactNumber", index: "ContactNumber", align: "center", width: 120 },
                    { name: "Province", index: "Province", align: "center", width: 100 },
                    { name: "City", index: "City", align: "center", width: 100 },
                    { name: "Area", index: "Area", align: "center", width: 100 },
                    { name: "EnterpriseAddress", index: "EnterpriseAddress", align: "center", width: 250 }
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
                pager: $pagerEnterpriseManagemain,
                loadComplete: function() {
                    var table = this;
                    updatePagerIcons(table);
                    jqGridAutoWidth();
                    setGridHeight($gridEnterpriseManagemain.selector);

                }
            });
        }

        initEnterpriseGrid();
    }

    var initButtonArea = function () {

        //重置密码
        $("#btnEnterpriseManage_ResetPassword").on("click", function() {
            var rowData = getGridSelectedRowData($gridEnterpriseManagemain, function() {
                alert("请选择数据！");
            });

            if (rowData == undefined) {
                return false;
            }

            var arrRowid = $gridEnterpriseManagemain.jqGrid("getGridParam", "selarrrow");
            if (!confirm("确认重置【" + arrRowid.length + "】行密码吗？")) {
                return false;
            }

            var enterpriseIdList = [];
            for (var i = 0; i < arrRowid.length; i++) {
                var enterpriseId = $gridEnterpriseManagemain.jqGrid("getRowData", arrRowid[i]).EnterpriseId;
                enterpriseIdList.push(enterpriseId);
            }

            var postData = {};
            postData.EnterpriseIdList = [];
            for (var j = 0; j < enterpriseIdList.length; j++) {
                postData.EnterpriseIdList.push(enterpriseIdList[j]);
            }

            var strData = JSON.stringify(postData);
            ajaxRequest({
                url: "/" + controllerName + "/ResetPassword",
                data: { "strData": strData },
                type: "post",
                ansyc: false,
                success: function(jdata) {
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


    $(document).ready(function() {
        initQueryArea();
        initJqGrid();
        initButtonArea();
    });
});