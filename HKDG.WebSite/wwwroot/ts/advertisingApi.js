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
exports.AdvertisingApi = void 0;
var wsapi_1 = require("./wsapi");
var AdvertisingApi = /** @class */ (function (_super) {
    __extends(AdvertisingApi, _super);
    function AdvertisingApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    AdvertisingApi.prototype.getAdViewingDetail = function (cond, callback) {
        var _this = this;
        WSPost(_this.apiPath + "/Advertising/GetAdViewingDetail", cond, function (data) {
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    ;
    AdvertisingApi.prototype.finishAdViewing = function (cond, callback) {
        var _this = this;
        WSPost(_this.apiPath + "/Advertising/FinishAdViewing", cond, function (data) {
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    ;
    return AdvertisingApi;
}(wsapi_1.WSAPI));
exports.AdvertisingApi = AdvertisingApi;
//# sourceMappingURL=advertisingApi.js.map