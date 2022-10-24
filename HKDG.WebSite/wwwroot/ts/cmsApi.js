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
exports.CMSApi = void 0;
/// <reference path="./common.d.ts" />
var wsapi_1 = require("./wsapi");
var CMSApi = /** @class */ (function (_super) {
    __extends(CMSApi, _super);
    function CMSApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    CMSApi.prototype.getContent = function (cId, success, error) {
        WSGet(this.apiPath + "/cms/GetContent", { cid: cId }, function (data) {
            if (success) {
                success(data);
            }
        }, function () { });
    };
    CMSApi.prototype.getContentsByCatId = function (catId, success, error) {
        WSGet(this.apiPath + "/cms/GetContentsByCatId", { catId: catId }, function (data) {
            if (success) {
                success(data);
            }
        }, function () { });
    };
    CMSApi.prototype.getCategoryTree = function (success, error) {
        WSGet(this.apiPath + "/cms/GetCategoryTree", {}, function (data) {
            if (success) {
                success(data);
            }
        }, function () { });
    };
    return CMSApi;
}(wsapi_1.WSAPI));
exports.CMSApi = CMSApi;
//# sourceMappingURL=cmsApi.js.map