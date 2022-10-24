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
exports.Message = exports.CustomerServiceApi = void 0;
/// <reference path="./common.d.ts" />
var wsapi_1 = require("./wsapi");
var Message = /** @class */ (function () {
    function Message() {
    }
    Object.defineProperty(Message.prototype, "Content", {
        get: function () {
            return this._content;
        },
        set: function (v) {
            this._content = v;
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(Message.prototype, "SendDate", {
        get: function () {
            return this._sendDate;
        },
        set: function (v) {
            this._sendDate = v;
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(Message.prototype, "Sender", {
        get: function () {
            return this._sender;
        },
        set: function (v) {
            this._sender = v;
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(Message.prototype, "Type", {
        get: function () {
            return this._type;
        },
        set: function (v) {
            this._type = v;
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(Message.prototype, "Seq", {
        get: function () {
            return this._seq;
        },
        set: function (v) {
            this._seq = v;
        },
        enumerable: false,
        configurable: true
    });
    return Message;
}());
exports.Message = Message;
var CustomerServiceApi = /** @class */ (function (_super) {
    __extends(CustomerServiceApi, _super);
    function CustomerServiceApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    CustomerServiceApi.prototype.getInquiryType = function (callback) {
        var _this = this;
        WSGet(this.apiPath + "/CS/GetInquiryType", {}, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback) {
                    callback(data);
                }
            }
        });
    };
    ;
    CustomerServiceApi.prototype.inquiry = function (model, callback) {
        var _this = this;
        WSPost(this.apiPath + "/CS/Inquiry", model, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (data.Succeeded) {
                    if (callback) {
                        callback(data.Message);
                    }
                    else {
                        showInfo(data.Message);
                    }
                }
                else {
                    showSystemError(data.ReturnValue);
                }
            }
        });
    };
    ;
    /**
     * 打开一个对话
     * @param prodCode 产品编号
     * @param mchId  商家ID
     */
    CustomerServiceApi.prototype.openChat = function (mchId, prodId, prodCode, callback) {
        var _this = this;
        WSGet(this.apiPath + "/CS/OpenChat", { prodCode: prodCode, prodId: prodId, mchId: mchId }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback) {
                    callback(data);
                }
            }
        });
    };
    ;
    CustomerServiceApi.prototype.getData = function (cid, seq, callback) {
        // var data = new Array();
        // if (seq == 0) {
        var _this = this;
        //     var m1 = new Message();
        //     m1.Content = "hello justin";
        //     m1.SendDate = "2018-08-22 16:28:20";
        //     m1.Sender = "Justin";
        //     m1.Type = 0;
        //     m1.Seq = 1;
        //     data.push(m1);
        //     var m2 = new Message();
        //     m2.Content = "hello ella";
        //     m2.SendDate = "2018-08-22 16:30:20";
        //     m2.Sender = "STP ADMIN";
        //     m2.Type = 1;
        //     m2.Seq = 2;
        //     data.push(m2);
        // } else {
        //     var m2 = new Message();
        //     m2.Content = "good bye";
        //     m2.SendDate = "2018-08-22 16:40:20";
        //     m2.Sender = "STP ADMIN";
        //     m2.Type = 1;
        //     m2.Seq = 4;
        //     data.push(m2);
        // }
        // return data;
        WSGet(this.apiPath + "/CS/GetMessage", { chatId: cid, seq: seq }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback) {
                    callback(data);
                }
            }
        });
    };
    CustomerServiceApi.prototype.send = function (cid, msg, callback) {
        var _this = this;
        WSPost(this.apiPath + "/CS/Send", { Content: msg, ChatId: cid }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback) {
                    callback(data);
                }
            }
        });
    };
    CustomerServiceApi.prototype.sendImage = function (cid, bigImage, smallImg, callback) {
        var _this = this;
        var content = "<image class='csimage' src='" + smallImg + "'/>";
        WSPost(this.apiPath + "/CS/Send", { Content: content, ChatId: cid, BigImage: bigImage, SmallImage: smallImg, }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (callback) {
                    callback(data);
                }
            }
        });
    };
    return CustomerServiceApi;
}(wsapi_1.WSAPI));
exports.CustomerServiceApi = CustomerServiceApi;
//# sourceMappingURL=customerServiceApi.js.map