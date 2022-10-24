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
exports.MessageApi = void 0;
var wsapi_1 = require("./wsapi");
var MessageApi = /** @class */ (function (_super) {
    __extends(MessageApi, _super);
    function MessageApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    //獲取未讀消息數量
    MessageApi.prototype.getUnreadMsgQty = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/Message/GetMbrUnreadMsgQty", null, function (data, status) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        });
    };
    //獲取未讀消息列表
    MessageApi.prototype.getUnreadMsgList = function (pager, callback) {
        var _this_1 = this;
        WSPost(this.apiPath + "/Message/GetMemberUnreadMsg", pager, function (data, status) {
            _this_1.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    //獲取全部消息列表
    MessageApi.prototype.getMessage = function (pager, callback) {
        var _this_1 = this;
        WSPost(this.apiPath + "/Message/getMessage", pager, function (data, status) {
            _this_1.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    //回復消息（需傳遞被回復記錄的ID）
    MessageApi.prototype.replyMsg = function (message, callback) {
        var _this_1 = this;
        WSPost(this.apiPath + "/Message/MbrReplyMessage", message, function (data, status) {
            _this_1.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    //發送會員的售前咨詢信息
    MessageApi.prototype.sendPreSalesMsg = function (message, callback) {
        var _this_1 = this;
        WSPost(this.apiPath + "/Message/SendMemberPreSalesMsg", message, function (data, status) {
            _this_1.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    //發送會員的售後咨詢信息
    MessageApi.prototype.sendAfterSalesMsg = function (message, callback) {
        var _this_1 = this;
        WSPost(this.apiPath + "/Message/SendMemberAfterSalesMsg", message, function (data, status) {
            _this_1.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    //標記指定的消息為已讀狀態
    MessageApi.prototype.markedMessageAsRead = function (msgIdLst, callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Message/MarkedMessageAsRead", { msgIdLst: msgIdLst }, function (data, status) {
            _this_1.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    //標記所有消息為已讀狀態
    MessageApi.prototype.markedMbrAllMsgAsRead = function (callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Message/MarkedMbrAllMsgAsRead", null, function (data, status) {
            _this_1.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    //刪除指定的信息
    MessageApi.prototype.deleteMsg = function (msgIdLst, callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Message/DeleteMsg", { msgIdLst: msgIdLst }, function (data, status) {
            _this_1.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    //刪除所有信息
    MessageApi.prototype.deleteAllMsg = function (callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Message/DeleteAllMsg", null, function (data, status) {
            _this_1.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    //新增到貨通知
    MessageApi.prototype.addArrivalNotify = function (cond, callback) {
        var _this_1 = this;
        WSPost(this.apiPath + "/Message/AddArrivalNotify", cond, function (data, status) {
            _this_1.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    //取消到貨通知
    MessageApi.prototype.cancelArrivalNotify = function (cond, callback) {
        var _this_1 = this;
        WSPost(this.apiPath + "/Message/CancelArrivalNotify", cond, function (data, status) {
            _this_1.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    //是否已有到貨通知
    MessageApi.prototype.checkExsitArrivalNotify = function (cond, callback) {
        var _this_1 = this;
        WSPost(this.apiPath + "/Message/CheckExsitArrivalNotify", cond, function (data, status) {
            _this_1.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    return MessageApi;
}(wsapi_1.WSAPI));
exports.MessageApi = MessageApi;
//# sourceMappingURL=messageApi.js.map