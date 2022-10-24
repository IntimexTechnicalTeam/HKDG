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
exports.ProdCommentApi = void 0;
/// <reference path="./common.d.ts" />
var wsapi_1 = require("./wsapi");
var ProdCommentApi = /** @class */ (function (_super) {
    __extends(ProdCommentApi, _super);
    function ProdCommentApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ///获取买家的所有评论
    ProdCommentApi.prototype.getShopperComments = function (cond, callback) {
        var _this = this;
        WSPost(this.apiPath + "/ProductComment/GetShopperComments", cond, function (data) {
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    ///获取产品的所有评论
    ProdCommentApi.prototype.getProductComments = function (cond, callback) {
        var _this = this;
        WSPost(this.apiPath + "/ProductComment/GetProductComments", cond, function (data) {
            if (callback) {
                if (data && data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        }, function () { });
    };
    ///获取评论
    ProdCommentApi.prototype.getCommentById = function (commentId, callback) {
        var _this = this;
        WSGet(this.apiPath + "/ProductComment/GetCommentById", { commentId: commentId }, function (data) {
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    ///根據訂單獲取評論
    ProdCommentApi.prototype.getSubOrderComments = function (Id, callback) {
        var _this = this;
        WSGet(this.apiPath + "/ProductComment/GetSubOrderComments", { subOrderId: Id }, function (data) {
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        }, function () { });
    };
    ///保存评论
    ProdCommentApi.prototype.saveComments = function (comments, callback) {
        var _this = this;
        console.log(comments);
        WSAjaxSP({
            url: this.apiPath + "/ProductComment/SaveComments",
            type: 'post',
            dataType: 'json',
            cache: false,
            processData: false,
            contentType: 'application/json',
            data: JSON.stringify(comments),
            success: function (data) {
                if (callback) {
                    callback(data);
                }
            },
            error: function () {
            }
        });
        //WSPost(this.apiPath + "/ProductComment/SaveComments", comments, function (data: any) {
        //    if (callback) {
        //        callback(data);
        //    }
        //}, function () { });
    };
    ///删除评论
    ProdCommentApi.prototype.deleteComment = function (commentId, callback) {
        var _this = this;
        WSGet(this.apiPath + "/ProductComment/DeleteComment", { commentId: commentId }, function (data) {
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    ///檢查當前用戶是否有該產品的評論權限
    ProdCommentApi.prototype.checkAuthorityForProduct = function (sku, callback) {
        WSGet(this.apiPath + "/ProductComment/CheckAuthorityOfProduct", { sku: sku }, function (data) {
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    return ProdCommentApi;
}(wsapi_1.WSAPI));
exports.ProdCommentApi = ProdCommentApi;
//# sourceMappingURL=prodcommentApi.js.map