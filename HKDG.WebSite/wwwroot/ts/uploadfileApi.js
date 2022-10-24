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
exports.UploadFileApi = void 0;
/// <reference path="./common.d.ts" />
var wsapi_1 = require("./wsapi");
var UploadFileApi = /** @class */ (function (_super) {
    __extends(UploadFileApi, _super);
    function UploadFileApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    UploadFileApi.prototype.uploadFiles = function (files, callback) {
        var _this = this;
        WSAjaxSP({
            url: this.apiPath + "/FileUpload/UploadFile",
            type: 'post',
            dataType: 'json',
            cache: false,
            data: files,
            processData: false,
            contentType: false,
            success: function (data) {
                if (callback) {
                    callback(data);
                }
            },
            error: function () {
            }
        });
    };
    UploadFileApi.prototype.uploadCSImage = function (chatId, files, callback) {
        var _this = this;
        WSAjaxSP({
            url: this.apiPath + "/FileUpload/UploadCSImage?chatId=" + chatId,
            type: 'post',
            dataType: 'json',
            cache: false,
            data: files,
            processData: false,
            contentType: false,
            success: function (data) {
                if (callback) {
                    callback(data);
                }
            },
            error: function () {
            }
        });
    };
    return UploadFileApi;
}(wsapi_1.WSAPI));
exports.UploadFileApi = UploadFileApi;
//# sourceMappingURL=uploadfileApi.js.map