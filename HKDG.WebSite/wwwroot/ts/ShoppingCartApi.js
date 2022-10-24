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
exports.ShoppingCartApi = void 0;
/// <reference path="./common.d.ts" />
var wsapi_1 = require("./wsapi");
var ShoppingCartApi = /** @class */ (function (_super) {
    __extends(ShoppingCartApi, _super);
    function ShoppingCartApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ShoppingCartApi.prototype.loadData = function (callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/ShoppingCart/GetShopCart", {}, function (data, status) {
            _this_1.log(data);
            if (callback) {
                callback(data);
            }
        });
    };
    ;
    ShoppingCartApi.prototype.loadCheckoutData = function (callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/ShoppingCart/GetMallCart", {}, function (data, status) {
            _this_1.log(data);
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
                else {
                    _this_1.log(data.Message);
                }
            }
        });
    };
    ;
    ShoppingCartApi.prototype.addItem = function (prodId, prodCode, attr1, attr2, attr3, qty, callback, loginFun) {
        var _this_1 = this;
        WSPost(this.apiPath + "/ShoppingCart/AddItem", { ProductId: prodId, ProdCode: prodCode, Attr1: attr1, Attr2: attr2, Attr3: attr3, qty: qty }, function (data, status) {
            _this_1.log(data);
            if (status === "success") {
                if (callback) {
                    if (!data.Succeeded) {
                        if (data.ReturnValue == "3002") { //LOGIN
                            if (loginFun && typeof (loginFun) === "function") {
                                loginFun(data);
                            }
                            else {
                                window.location.href = "/account/login?returnUrl=" + window.location.href;
                            }
                            return;
                        }
                    }
                    callback(data);
                }
            }
        });
    };
    ;
    ShoppingCartApi.prototype.removeItem = function (id, callback) {
        var _this = this;
        WSGet(this.apiPath + "/ShoppingCart/RemoveItem", { id: id }, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        });
    };
    ;
    ShoppingCartApi.prototype.updateItemQty = function (itemId, qty, callback) {
        var _this = this;
        WSGet(this.apiPath + "/ShoppingCart/UpdateItemQty", { id: itemId, qty: qty }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                callback(data);
            }
        });
    };
    ;
    ShoppingCartApi.prototype.saveDelivery = function (addressId, expressId, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Pay/SaveDelivery", { deliveryAddressId: addressId, expressCompanyId: expressId }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                callback(data);
            }
        });
    };
    ;
    ShoppingCartApi.prototype.savePickupInfo = function (addressId, pickupDate, pickupTime, pickupAddress, callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Pay/SavePickupInfo", { addressId: addressId, date: pickupDate, time: pickupTime, address: pickupAddress }, function (data, status) {
            _this_1.log(data);
            if (status === "success") {
                callback(data);
            }
        });
    };
    ;
    ShoppingCartApi.prototype.savePayMethod = function (methodId, callback) {
        var _this = this;
        WSGet(this.apiPath + "/pay/SavePayMethod", { payMethodType: methodId }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback) {
                    callback(data);
                }
            }
        });
    };
    ;
    ShoppingCartApi.prototype.getFreeProduct = function (ruleId, skuId, qty, callback) {
        var _this = this;
        WSGet(this.apiPath + "/ShoppingCart/GetFreeProduct", { ruleId: ruleId, skuId: skuId, qty: qty }, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        });
    };
    ;
    ShoppingCartApi.prototype.getSetPrice = function (ruleId, skuId, qty, callback) {
        var _this = this;
        WSGet(this.apiPath + "/ShoppingCart/GetSetPrice", { ruleId: ruleId, skuId: skuId, qty: qty }, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        });
    };
    ;
    return ShoppingCartApi;
}(wsapi_1.WSAPI));
exports.ShoppingCartApi = ShoppingCartApi;
//# sourceMappingURL=ShoppingCartApi.js.map