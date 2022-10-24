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
exports.ReportApi = void 0;
/// <reference path="./common.d.ts" />
var wsapi_1 = require("./wsapi");
var ReportApi = /** @class */ (function (_super) {
    __extends(ReportApi, _super);
    function ReportApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ReportApi.prototype.GetHotSale = function (pageSize, page, callback) {
        var _this = this;
        WSPost(this.apiPath + "/report/GetHotSale", { Page: page, PageSize: pageSize }, function (data, status) {
            _this.log(data);
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
    ReportApi.prototype.GetHighScore = function (pageSize, page, callback) {
        var _this = this;
        WSPost(this.apiPath + "/report/GetHighScore", { Page: page, PageSize: pageSize }, function (data, status) {
            _this.log(data);
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
    ReportApi.prototype.GetHotStore = function (pageSize, page, callback) {
        var _this = this;
        WSPost(this.apiPath + "/report/GetHotStore", { Page: page, PageSize: pageSize }, function (data, status) {
            _this.log(data);
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
    ReportApi.prototype.getRecommendProducts = function (userId, lang, page, pageSize, callback) {
        var _this = this;
        WSPost(this.apiPath + "/report/GetReCommendProducts", { MemberId: userId, Language: lang, Page: page, PageSize: pageSize }, function (data, status) {
            _this.log(data);
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
    return ReportApi;
}(wsapi_1.WSAPI));
exports.ReportApi = ReportApi;
//# sourceMappingURL=reportApi.js.map