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
exports.MerchantApi = void 0;
/// <reference path="./common.d.ts" />
var wsapi_1 = require("./wsapi");
var MerchantApi = /** @class */ (function (_super) {
    __extends(MerchantApi, _super);
    function MerchantApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    MerchantApi.prototype.getMerchantInfo = function (merchantId, callback) {
        var path = this.apiPath + "/Merchant/GetMerchantInfo";
        WSGet(path, { merchID: merchantId }, function (result, status) {
            if (result.Succeeded) {
                if (callback) {
                    callback(result);
                }
            }
        });
    };
    ;
    MerchantApi.prototype.getMerchantList = function (key, page, pageSize, callback) {
        var path = this.apiPath + "/Merchant/Search";
        WSGet(path, { key: key, page: page, pageSize: pageSize }, function (result, status) {
            if (result.Succeeded) {
                if (callback) {
                    callback(result);
                }
            }
        });
    };
    ;
    MerchantApi.prototype.getProducts = function (cond, callback) {
        var path = this.apiPath + "/Merchant/GetProducts";
        WSPost(path, cond, function (result, status) {
            if (result.Succeeded) {
                if (callback) {
                    callback(result);
                }
            }
        });
    };
    ;
    MerchantApi.prototype.countBannerClick = function (merchantId, bannerLink, callback) {
        var path = this.apiPath + "/Merchant/CountBannerClick";
        WSGet(path, { merchantId: merchantId, bannerLink: bannerLink }, function (result, status) {
            if (callback) {
                callback(result);
            }
        });
    };
    ;
    return MerchantApi;
}(wsapi_1.WSAPI));
exports.MerchantApi = MerchantApi;
//# sourceMappingURL=merchantApi.js.map