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
exports.PaymentApi = void 0;
/// <reference path="./common.d.ts" />
var wsapi_1 = require("./wsapi");
var PaymentApi = /** @class */ (function (_super) {
    __extends(PaymentApi, _super);
    function PaymentApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    PaymentApi.prototype.getPaymentMethod = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/pay/GetPaymentMethod", {}, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        });
    };
    ///获取PayPal支付方式的对象
    PaymentApi.prototype.getMPGSInfo = function (orderId, payType, callback) {
        var _this = this;
        WSGet(this.apiPath + "/pay/GetMPGSInfo", { orderId: orderId, payType: payType }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback) {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                    else {
                        _this.log(data.Message);
                    }
                }
            }
        });
    };
    ///获取PayPal支付方式的对象
    PaymentApi.prototype.getPayPalInfo = function (orderId, callback) {
        var _this = this;
        WSGet(this.apiPath + "/pay/GetPayPalInfo", { orderId: orderId }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback) {
                    callback(data);
                }
            }
        });
    };
    ///获取PayPal支付方式的对象
    PaymentApi.prototype.getStripeInfo = function (orderId, callback) {
        var _this = this;
        WSGet(this.apiPath + "/pay/GetSripeInfo", { orderId: orderId }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback) {
                    callback(data);
                }
            }
        });
    };
    //支付完成后更新order的支付狀態
    PaymentApi.prototype.updateOrderPaymentStatus = function (orderId, type, paykey, callback) {
        var _this = this;
        WSGet(this.apiPath + "/pay/UpdateOrderPaymentStatus", { orderId: orderId, type: type, paymentKey: paykey }, function (data) {
            _this.log(data);
            //if (status === "success") {
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
                else {
                    _this.log(data.Message);
                }
            }
            //}
        });
    };
    return PaymentApi;
}(wsapi_1.WSAPI));
exports.PaymentApi = PaymentApi;
//# sourceMappingURL=paymentApi.js.map