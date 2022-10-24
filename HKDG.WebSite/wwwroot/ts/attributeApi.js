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
exports.AttributeApi = void 0;
/// <reference path="./common.d.ts" />
var wsapi_1 = require("./wsapi");
var AttributeApi = /** @class */ (function (_super) {
    __extends(AttributeApi, _super);
    function AttributeApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    /**
     * 根据传入的参数获取属性
     * Cond.Type(0获取所有属性,1获取库存属性,2获取非库存属性)
     */
    AttributeApi.prototype.getAttrs = function (cond, success, error) {
        WSPost(this.apiPath + "/Attribute/GetFrontAttribute", cond, function (data) {
            if (success) {
                success(data);
            }
        }, function () { });
    };
    return AttributeApi;
}(wsapi_1.WSAPI));
exports.AttributeApi = AttributeApi;
//# sourceMappingURL=attributeApi.js.map