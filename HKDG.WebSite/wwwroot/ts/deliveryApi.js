"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
exports.DeliveryApi = void 0;
/// <reference path="./common.d.ts" />
var wsapi_1 = require("./wsapi");
var DeliveryApi = /** @class */ (function (_super) {
    __extends(DeliveryApi, _super);
    function DeliveryApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    DeliveryApi.prototype.getCountry = function (callback) {
        var _this_1 = this;
        var path = this.apiPath + "/Address/Country";
        WSGet(path, {}, function (data, status) {
            _this_1.log(data);
            if (status === "success") {
                if (callback) {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                    else {
                        console.log(data.Message);
                    }
                }
            }
            else {
                showInfo(status);
            }
        });
    };
    ;
    DeliveryApi.prototype.getProvince = function (countryId, callback) {
        var _this_1 = this;
        var path = this.apiPath + "/Address/Province";
        WSGet(path, { countryId: countryId }, function (data, status) {
            _this_1.log(path, data);
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
                else {
                    console.log(data.Message);
                }
            }
        });
    };
    ;
    DeliveryApi.prototype.getExpress = function (addressId, weight, callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Express/GetExpress", { deliveryAddrId: addressId, totalWeight: weight }, function (data, status) {
            _this_1.log(data);
            if (status === "success") {
                if (callback) {
                    if (data.Succeeded) {
                        var key = data.Message;
                        callback(data.ReturnValue, key);
                    }
                }
            }
            else {
                showInfo(status);
            }
        });
    };
    ;
    DeliveryApi.prototype.getExpressWithDiscount = function (exCond, callback) {
        var _this = this;
        WSPost(this.apiPath + "/Express/GetExpressWithDiscount", exCond, function (data) {
            if (callback) {
                // if (data.Succeeded) {
                //     var key = data.Message;
                //     callback(data.ReturnValue, key);
                // }
                callback(data);
            }
        }, function () { });
    };
    ;
    DeliveryApi.prototype.getIPostStationExpress = function (exCond, callback) {
        var _this = this;
        WSPost(this.apiPath + "/Express/GetIPostStationExpress", exCond, function (data) {
            if (callback) {
                // if (data.Succeeded) {
                //     var key = data.Message;
                //     callback(data.ReturnValue, key);
                // }
                callback(data);
            }
        }, function () { });
    };
    ;
    DeliveryApi.prototype.getIPostStationList = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/Express/GetIPostStationList", {}, function (data) {
            if (callback) {
                if (data.Succeeded) {
                    //var key = data.Message;
                    // callback(data.ReturnValue);
                    callback(data);
                }
            }
        }, function () { });
    };
    DeliveryApi.prototype.getCollectionList = function (exCond, callback) {
        var _this = this;
        WSPost(this.apiPath + "/Express/GetCollectionOfficeList", exCond, function (data) {
            if (callback) {
                // if (data.Succeeded) {
                //     var key = data.Message;
                //     callback(data.ReturnValue, key);
                // }
                callback(data);
            }
        }, function () { });
    };
    DeliveryApi.prototype.getIPostStationByMCNCode = function (code, merchId, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Express/GetIPostStationByMCNCode", { mcnCode: code, merchId: merchId }, function (data) {
            if (callback) {
                //if (data.Succeeded) {
                callback(data);
                //}
            }
        }, function () { });
    };
    DeliveryApi.prototype.getDeliveryActiveInfo = function (mercId, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Express/GetDeliveryActiveInfo", { merchantId: mercId }, function (data) {
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        }, function () { });
    };
    DeliveryApi.prototype.checkCountryIsValid = function (info, callback) {
        var _this = this;
        WSPost(this.apiPath + "/Express/CheckCountryIsVaild", info, function (data) {
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        }, function () { });
    };
    DeliveryApi.prototype.getAddress = function (callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Address/GetAddresses", {}, function (data, status) {
            _this_1.log(data);
            if (status === "success") {
                if (callback) {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                }
            }
        });
    };
    ;
    DeliveryApi.prototype.getDefaultAddr = function (callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Address/GetDefaultAddr", {}, function (data, status) {
            _this_1.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                }
                else {
                    showInfo(data.Message);
                }
            }
        });
    };
    ;
    DeliveryApi.prototype.getPickupAddress = function (callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Address/GetPickupAddress", {}, function (data, status) {
            _this_1.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    callback(data);
                }
                else {
                    showInfo(data.Message);
                }
            }
        });
    };
    ;
    DeliveryApi.prototype.saveAddress = function (address, callback) {
        var _this_1 = this;
        var action = "/Address/Add";
        console.log(address);
        if (address.Id && address.Id != "") {
            action = "/Address/Update";
        }
        WSAjaxSP({
            type: "post",
            url: this.apiPath + action,
            data: address,
            success: function (data, status) {
                _this_1.log(data);
                if (status === "success") {
                    if (callback && typeof (callback) === "function") {
                        callback(data);
                    }
                    else {
                        showInfo(data.Message);
                    }
                }
            }
        });
    };
    ;
    DeliveryApi.prototype.removeAddress = function (id, callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/address/Remove", { id: id }, function (data, status) {
            _this_1.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    callback(data);
                }
                else {
                    showInfo(data.Message);
                }
            }
        });
    };
    ;
    DeliveryApi.prototype.gtPaymentType = function (callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/pay/GetPaymentTypes", {}, function (data, status) {
            _this_1.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    callback(data);
                }
                else {
                    showInfo(data.Message);
                }
            }
        });
    };
    ;
    DeliveryApi.prototype.GetExpressList = function (exCond, callback) {
        var _this = this;
        WSPost(this.apiPath + "/Express/GetExpressList", exCond, function (data) {
            if (callback) {
                // if (data.Succeeded) {
                //     var key = data.Message;
                //     callback(data.ReturnValue, key);
                // }
                callback(data);
            }
        }, function () { });
    };
    ;
    DeliveryApi.prototype.GetExpressChargeListByCode = function (exCond, callback) {
        var _this = this;
        WSPost(this.apiPath + "/Express/GetExpressChargeListByCode", exCond, function (data) {
            if (callback) {
                // if (data.Succeeded) {
                //     var key = data.Message;
                //     callback(data.ReturnValue, key);
                // }
                callback(data);
            }
        }, function () { });
    };
    ;
    return DeliveryApi;
}(wsapi_1.WSAPI));
exports.DeliveryApi = DeliveryApi;
//# sourceMappingURL=deliveryApi.js.map