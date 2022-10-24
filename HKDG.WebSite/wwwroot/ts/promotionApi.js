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
exports.PromotionApi = void 0;
/// <reference path="./common.d.ts" />
var wsapi_1 = require("./wsapi");
var PromotionApi = /** @class */ (function (_super) {
    __extends(PromotionApi, _super);
    function PromotionApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    PromotionApi.prototype.getLatestProduct = function (callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Product/GetLatest", {}, function (result, status) {
            _this_1.log(result);
            if (callback) {
                if (result.Succeeded) {
                    callback(result.ReturnValue);
                }
                else {
                    _this_1.log(result.Message);
                }
            }
        });
    };
    ;
    PromotionApi.prototype.getHeaderBanner = function (data, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Banner/GetHeaderBanner", {
            from: data.from,
            page: data.page,
        }, function (data, status) {
            _this.log(data);
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        });
    };
    ;
    PromotionApi.prototype.getPromotion = function (data, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Banner/GetPromotion", {
            from: data.from,
            page: data.page,
            position: data.position,
        }, function (data, status) {
            _this.log(data);
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        });
    };
    ;
    ///获取推广商家
    ///data.From
    ///data.Page
    ///data.Position 
    ///data.ShowBanner//是否获取推广banner
    ///data.ShowProduct是否获取推广产品
    ///data.ShowMerchant是否获取推广商家
    ///data.ProductSize//获取推广产品的数量
    ///data.MerchantSize 获取推广商家的数量
    ///data.MerchantProductSize 如果传0则不获取推广商家的产品
    PromotionApi.prototype.getPromotionByCond = function (data, callback) {
        var _this = this;
        WSPost(this.apiPath + "/Promotion/GetPromotionByCondition", data, function (data, status) {
            _this.log(data);
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        });
    };
    ;
    PromotionApi.prototype.getHomePromotionList = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/Promotion/GetHomePromotionList", {}, function (data, status) {
            if (callback) {
                callback(data);
            }
        });
    };
    ;
    PromotionApi.prototype.getPromotionList = function (data, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Banner/GetPromotionList", {
            from: data.from,
            page: data.page,
            position: data.position,
        }, function (data, status) {
            _this.log(data);
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        });
    };
    ;
    PromotionApi.prototype.getHotSearchKeyList = function (qty, callback) {
        var _this = this;
        WSGet(this.apiPath + "/SearchKey/GetHotSearchKeys", {
            qty: qty
        }, function (data, status) {
            _this.log(data);
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        });
    };
    ;
    //獲取最新的系統公告
    PromotionApi.prototype.getLatestSysAnnouncement = function (qty, callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Message/GetLatestSysAnnouncement", { qty: qty }, function (data, status) {
            _this_1.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    PromotionApi.prototype.getHeaderPrmtList = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/Promotion/GetHeaderPrmtList", {}, function (data, status) {
            _this.log(data);
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        });
    };
    ;
    return PromotionApi;
}(wsapi_1.WSAPI));
exports.PromotionApi = PromotionApi;
//# sourceMappingURL=promotionApi.js.map