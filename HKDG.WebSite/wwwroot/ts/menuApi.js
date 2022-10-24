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
exports.MenuApi = void 0;
/// <reference path="./common.d.ts" />
var wsapi_1 = require("./wsapi");
var MenuApi = /** @class */ (function (_super) {
    __extends(MenuApi, _super);
    function MenuApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    /**
     * 获取自定义菜单
     * @param callback
     */
    MenuApi.prototype.GetMenuBar = function (callback) {
        WSGet(this.apiPath + "/Menu/GetMenuBar", {}, function (data, status) {
            if (callback) {
                callback(data);
            }
        });
    };
    ;
    return MenuApi;
}(wsapi_1.WSAPI));
exports.MenuApi = MenuApi;
//# sourceMappingURL=menuApi.js.map