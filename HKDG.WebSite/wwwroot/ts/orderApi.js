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
exports.OrderApi = void 0;
/// <reference path="./common.d.ts" />
var wsapi_1 = require("./wsapi");
var OrderApi = /** @class */ (function (_super) {
    __extends(OrderApi, _super);
    function OrderApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    OrderApi.prototype.getPaymentType = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/pay/GetPaymentMethod", {}, function (data, status) {
            _this.log(data);
            if (status === "success") {
                callback(data);
            }
        });
    };
    ;
    OrderApi.prototype.getPageOrder = function (pageSize, page, status, orderBy, callback) {
        var _this = this;
        if (status === void 0) { status = -1; }
        if (orderBy === void 0) { orderBy = "CreateDate"; }
        WSPost(this.apiPath + "/Order/SearchOrder", { Page: page, PageSize: pageSize, Status: status, OrderBy: orderBy }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                // if (callback && typeof (callback) === "function") {
                //     if (data.Succeeded) {
                //         callback(data.ReturnValue);
                //     }
                // } else {
                //     showInfo(data.Message);
                // }
                callback(data);
            }
        });
    };
    ;
    OrderApi.prototype.getPageOldOrder = function (pageSize, page, status, orderBy, callback) {
        var _this = this;
        if (status === void 0) { status = -1; }
        if (orderBy === void 0) { orderBy = "CreateDate"; }
        WSPost(this.apiPath + "/Order/SearchOldOrder", { Page: page, PageSize: pageSize, Status: status, OrderBy: orderBy }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                // if (callback && typeof (callback) === "function") {
                //     if (data.Succeeded) {
                //         callback(data.ReturnValue);
                //     }
                // } else {
                //     showInfo(data.Message);
                // }
                callback(data);
            }
        });
    };
    ;
    OrderApi.prototype.getOrder = function (id, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Order/GetOrder", { id: id }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    if (data.Succeeded) {
                        callback(data);
                    }
                    else {
                        showInfo(data.Message);
                    }
                }
                else {
                    showInfo(data.Message);
                }
            }
        });
    };
    ;
    OrderApi.prototype.getOldOrder = function (id, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Order/GetOldOrder", { id: id }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    if (data.Succeeded) {
                        callback(data);
                    }
                    else {
                        showInfo(data.Message);
                    }
                }
                else {
                    showInfo(data.Message);
                }
            }
        });
    };
    ;
    OrderApi.prototype.getOrderStatusList = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/Order/GetOrderStatus", {}, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    if (data.Succeeded) {
                        callback(data);
                    }
                    else {
                        showInfo(data.Message);
                    }
                }
                else {
                    showInfo(data.Message);
                }
            }
        });
    };
    ;
    OrderApi.prototype.cancel = function (id, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Order/Cancel", { id: id }, function (data, status) {
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
    OrderApi.prototype.createOrder = function (order, callback) {
        WSPost(this.apiPath + "/order/Create", order, function (data, status) {
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    callback(data);
                }
                else {
                    showInfo(data.Message);
                }
            }
        }, function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
            console.log(textStatus);
            console.log(errorThrown);
        });
    };
    ;
    OrderApi.prototype.getReturnOrderTypeList = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/order/GetReturnType", {}, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    if (data.Succeeded) {
                        callback(data);
                    }
                    else {
                        showInfo(data.Message);
                    }
                }
                else {
                    showInfo(data.Message);
                }
            }
        });
    };
    ;
    OrderApi.prototype.createReturnOrder = function (returnorder, callback) {
        WSPost(this.apiPath + "/order/CreateReturnOrder", returnorder, function (data, status) {
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    callback(data);
                }
                else {
                    showInfo(data.Message);
                }
            }
        }, function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
            console.log(textStatus);
            console.log(errorThrown);
        });
    };
    ;
    OrderApi.prototype.getReturnOrders = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/order/GetReturnOrders", {}, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    if (data.Succeeded) {
                        callback(data);
                    }
                    else {
                        showInfo(data.Message);
                    }
                }
                else {
                    showInfo(data.Message);
                }
            }
        });
    };
    ;
    OrderApi.prototype.getOrderTrackingInfo = function (trackingNo, callback) {
        var _this = this;
        WSGet(this.apiPath + "/order/GetOrderMailTrackingInfo", { trackingNo: trackingNo }, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        });
    };
    ;
    OrderApi.prototype.getReturnOrder = function (id, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Order/getReturnOrder", { id: id }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    if (data.Succeeded) {
                        callback(data);
                    }
                    else {
                        showInfo(data.Message);
                    }
                }
                else {
                    showInfo(data.Message);
                }
            }
        });
    };
    ;
    return OrderApi;
}(wsapi_1.WSAPI));
exports.OrderApi = OrderApi;
//# sourceMappingURL=orderApi.js.map