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
exports.CacheApi = void 0;
/// <reference path="./common.d.ts" />
var wsapi_1 = require("./wsapi");
var CacheApi = /** @class */ (function (_super) {
    __extends(CacheApi, _super);
    function CacheApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    CacheApi.prototype.getHeaderCache = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/cache/GetHeaderCache", null, function (data) {
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    ;
    CacheApi.prototype.getHomeBodyCache = function (cond, callback) {
        var _this = this;
        WSPost(this.apiPath + "/cache/GetHomeBodyCache", cond, function (data) {
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    ;
    return CacheApi;
}(wsapi_1.WSAPI));
exports.CacheApi = CacheApi;
//# sourceMappingURL=cacheApi.js.map