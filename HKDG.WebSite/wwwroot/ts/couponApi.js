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
exports.CouponApi = void 0;
/// <reference path="./common.d.ts" />
var wsapi_1 = require("./wsapi");
var CouponApi = /** @class */ (function (_super) {
    __extends(CouponApi, _super);
    function CouponApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    CouponApi.prototype.getCoupons = function (cond, callback) {
        var _this = this;
        WSPost(this.apiPath + "/Coupon/GetCouponGroup", cond, function (data) {
            if (callback) {
                callback(data);
                //if (data.Succeeded) {
                //    callback(data);
                //}
            }
        }, function () { });
    };
    ;
    CouponApi.prototype.getRules = function (callback) {
        var _this_1 = this;
        var path = this.apiPath + "/Coupon/CheckHasGroupOrRuleDiscount";
        WSGet(path, {}, function (data, status) {
            _this_1.log(data);
            if (callback) {
                callback(data);
                //if (data.Succeeded) {
                //    callback(data);
                //} else {
                //    console.log(data.Message);
                //}
            }
        });
    };
    ;
    CouponApi.prototype.getPromotionCode = function (merchantId, code, price, callback) {
        var _this_1 = this;
        var path = this.apiPath + "/Coupon/GetPromotionCodeCoupon";
        WSGet(path, { merchantId: merchantId, code: code, price: price }, function (data, status) {
            _this_1.log(data);
            if (callback) {
                callback(data);
            }
        });
    };
    ;
    CouponApi.prototype.getMallFun = function (callback) {
        var _this_1 = this;
        var path = this.apiPath + "/Coupon/GetMallFun";
        WSGet(path, {}, function (data, status) {
            _this_1.log(data);
            if (callback) {
                callback(data);
            }
        });
    };
    ;
    CouponApi.prototype.getMemberCoupon = function (cond, callback) {
        var path = this.apiPath + "/Coupon/GetMemberCoupon";
        WSPost(path, cond, function (data, status) {
            if (callback) {
                callback(data);
            }
        });
    };
    ;
    CouponApi.prototype.getPromotionCodeV2 = function (cond, callback) {
        var path = this.apiPath + "/Coupon/GetPromotionCodeCouponV2";
        WSPost(path, cond, function (data, status) {
            if (callback) {
                callback(data);
            }
        });
    };
    ;
    return CouponApi;
}(wsapi_1.WSAPI));
exports.CouponApi = CouponApi;
//# sourceMappingURL=couponApi.js.map