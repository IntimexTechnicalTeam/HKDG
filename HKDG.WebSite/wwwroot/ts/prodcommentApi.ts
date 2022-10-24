/// <reference path="./common.d.ts" />
import { WSAPI } from "./wsapi"


export class ProdCommentApi extends WSAPI {

    ///获取买家的所有评论
    getShopperComments(cond: object, callback: Function) {
        var _this = this;
        WSPost(this.apiPath + "/ProductComment/GetShopperComments", cond, function (data: any) {
            if (callback) {
                callback(data);
            }
        }, function () { });
    }

    ///获取产品的所有评论
    getProductComments(cond: object, callback: Function) {
        var _this = this;
        WSPost(this.apiPath + "/ProductComment/GetProductComments", cond, function (data: any) {
            if (callback) {
                if (data && data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        }, function () { });
    }
    ///获取评论
    getCommentById(commentId: string, callback: Function) {
        var _this = this;
        WSGet(this.apiPath + "/ProductComment/GetCommentById", { commentId: commentId }, function (data: any) {
            if (callback) {
                callback(data);
            }
        }, function () { });
    }

    ///根據訂單獲取評論
    getSubOrderComments(Id: string, callback: Function) {
        var _this = this;
        WSGet(this.apiPath + "/ProductComment/GetSubOrderComments", { subOrderId: Id }, function (data: any) {
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        }, function () { });
    }

    ///保存评论
    saveComments(comments: any, callback: Function) {
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
            success: function (data: any) {
                if (callback) {
                    callback(data);
                }
            },
            error: function () {

            }
        })
        //WSPost(this.apiPath + "/ProductComment/SaveComments", comments, function (data: any) {
        //    if (callback) {
        //        callback(data);
        //    }
        //}, function () { });
    }

    ///删除评论
    deleteComment(commentId: string, callback: Function) {
        var _this = this;
        WSGet(this.apiPath + "/ProductComment/DeleteComment", { commentId: commentId }, function (data: any) {
            if (callback) {
                callback(data);
            }
        }, function () { });
    }
    ///檢查當前用戶是否有該產品的評論權限
    checkAuthorityForProduct(sku: string, callback: Function) {
        WSGet(this.apiPath + "/ProductComment/CheckAuthorityOfProduct", { sku: sku }, function (data: any) {
            if (callback) {
                callback(data);
            }
        }, function () { });
    }
    //editComment(comment: object, callback: Function) {
    //    var _this = this;
    //    WSPost(this.apiPath + "/ProductComment/SaveComment", comment, function (data: any) {
    //        if (callback) {
    //            callback(data);
    //        }
    //    }, function () { });
    //}

}