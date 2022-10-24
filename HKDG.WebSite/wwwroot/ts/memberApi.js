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
exports.MemberApi = void 0;
/// <reference path="./common.d.ts" />
var wsapi_1 = require("./wsapi");
var MemberApi = /** @class */ (function (_super) {
    __extends(MemberApi, _super);
    function MemberApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    /**
     *  登入系統
     * @param account
     * @param password
     * @param rememberMe
     * @param callback
     */
    MemberApi.prototype.login = function (account, password, rememberMe, callback) {
        var _this = this;
        WSPost(
        //this.apiPath + "/Member/Login",
        "/account/Login", { Account: account, Password: password, RememberMe: rememberMe }, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (data.Succeeded) {
                    WSGet(addTimeStamp(_this.apiPath + "/member/GetMemberInfo"), {}, function () {
                        if (callback) {
                            callback(data);
                        }
                    }, function (e) {
                        alert(e);
                    });
                }
                else {
                    if (callback) {
                        callback(data);
                    }
                }
            }
        });
    };
    ;
    MemberApi.prototype.logout = function (callback) {
        // setCookie("access_token", "", "/", -1);
        // setCookie("uid", "", "/", -1); 
        var _this_1 = this;
        WSGet(this.apiPath + "/Member/Logout", {}, function (data, status) {
            _this_1.log(data);
            if (status === "success") {
                if (callback) {
                    callback(data);
                }
                else {
                    window.location.href = "/";
                }
            }
        });
    };
    ;
    /**
     * 第三方關聯登入系統
     * @param model
     * @param callback
     */
    MemberApi.prototype.fbLogin = function (model, callback) {
        var _this = this;
        WSPost("/account/FBLogin", model, function (data, status) {
            _this.log(data);
            if (status === "success") {
                if (data.Succeeded) {
                    WSGet(addTimeStamp(_this.apiPath + "/member/GetMemberInfo"), {}, function () {
                        if (callback) {
                            callback(data);
                        }
                    }, function (e) {
                        alert(e);
                    });
                }
                if (callback) {
                    callback(data);
                }
            }
        });
    };
    ;
    MemberApi.prototype.updatePassword = function (oldpwd, newpwd, callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Member/UpdatePassword", { oldpwd: oldpwd, newpwd: newpwd }, function (data, status) {
            _this_1.log(data);
            if (status === "success") {
                if (data.Succeeded) {
                }
                if (callback) {
                    callback(data);
                }
            }
        });
    };
    ;
    MemberApi.prototype.getMallFun = function (callback) {
        WSGet(this.apiPath + "/Member/GetMemberMallFun", {}, function (data, status) {
            if (callback) {
                callback(data);
            }
        });
    };
    ;
    /**
     * 注册会员
     * @param data
     * @param callback
     */
    MemberApi.prototype.register = function (data, callback) {
        WSAjaxSP({
            type: "Post",
            url: this.apiPath + "/Member/Register",
            data: data,
            success: callback,
            error: function (e) {
            },
            complete: function () {
            }
        });
    };
    ;
    MemberApi.prototype.joinAccount = function (data, callback) {
        WSAjaxSP({
            type: "Post",
            url: this.apiPath + "/Member/AssociatedThirdPartyAccount ",
            data: data,
            success: callback,
            error: function (e) {
            },
            complete: function () {
            }
        });
    };
    ;
    /**
    * 获取会员信息
    * @param callback
    */
    MemberApi.prototype.getProfile = function (callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Member/getProfile", {}, function (data, status) {
            _this_1.log(data);
            if (status === "success") {
                // setCookie("uid", data.ReturnValue.Id, "/", null);
                if (data.Succeeded) {
                    localStorage.setItem("logined", "1");
                    if (callback) {
                        callback(data.ReturnValue);
                    }
                    // setInterval(() => {
                    //     var token = getCookie("access_token", "/");
                    //     if (!token) {
                    //         window.location.href = window.location.href;
                    //         setCookie("logined", "0", "/", -1);
                    //     } else {
                    //         this.log("token valid");
                    //     }
                    // }, 5000);
                }
                else {
                    localStorage.setItem("logined", "0");
                    if (data.ReturnValue) {
                        // var l = getCookie("uLanguage", "/");
                        // if (!l) {
                        //     setCookie("uLanguage", data.ReturnValue.LanguageCode, "/", null);
                        //     window.location.reload(true);
                        // }
                    }
                    if (callback) {
                        callback();
                    }
                    // setInterval(() => {
                    //     var token = getCookie("access_token", "/");
                    //     if (!token) {
                    //         window.location.href = window.location.href;
                    //     } else {
                    //         this.log("token valid");
                    //     }
                    // }, 5000);
                }
            }
        });
    };
    ;
    /**
     * 获取会员信息
     * @param callback
     */
    MemberApi.prototype.getMemberInfo = function (callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Member/getMemberInfo", {}, function (data, status) {
            _this_1.log(data);
            if (status === "success") {
                if (data.Succeeded) {
                    // setCookie("uid", data.ReturnValue.Id, "/", null);
                    localStorage.setItem("logined", "1");
                    if (callback) {
                        callback(data.ReturnValue);
                    }
                    // setInterval(() => {
                    //     var token = getCookie("access_token", "/");
                    //     if (!token) {
                    //         // window.location.href = window.location.href;
                    //         setCookie("logined", "0", "/", -1);
                    //     } else {
                    //         this.log("token valid");
                    //     }
                    // }, 5000);
                }
                else {
                    localStorage.setItem("logined", "0");
                    if (data.ReturnValue) {
                        // var l = getCookie("uLanguage", "/");
                        // if (!l) {
                        //     setCookie("uLanguage", data.ReturnValue.LanguageCode, "/", null);
                        //     window.location.reload(true);
                        // }
                    }
                    if (callback) {
                        callback(data.ReturnValue);
                    }
                    // setInterval(() => {
                    //     var token = getCookie("access_token", "/");
                    //     if (!token) {
                    //         // window.location.href = window.location.href;
                    //     } else {
                    //         this.log("token valid");
                    //     }
                    // }, 5000);
                }
            }
        });
    };
    ;
    /**
     * 更新會員資料
     * @param obj
     * @param callback
     */
    MemberApi.prototype.updateProfile = function (model, callback) {
        var _this_1 = this;
        WSPost(this.apiPath + "/Member/UpdateProfile", model, function (data, status) {
            _this_1.log(data);
            if (status === "success") {
                if (callback) {
                    if (data.Succeeded) {
                        var lang = "C";
                        if (model.Language == 0) {
                            lang = "E";
                        }
                        else if (model.Language == 1) {
                            lang = "C";
                        }
                        else {
                            lang = "S";
                        }
                        setCookie("uLanguage", lang, "/", null);
                    }
                    callback(data);
                }
            }
        });
    };
    ;
    MemberApi.prototype.activate = function (mid, tid, callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Member/Activate", { mid: mid, tid: tid }, function (data, status) {
            _this_1.log(data);
            if (status === "success") {
                if (callback) {
                    callback(data);
                }
            }
        });
    };
    ;
    /**
     *
     * @param prodId
     * @param success
     * @param fail
     */
    MemberApi.prototype.addFavorite = function (prodId, success, fail) {
        var path = this.apiPath + "/member/AddFavorite";
        WSGet(path, { productId: prodId }, function (result, status) {
            if (result) {
                if (result.Succeeded) {
                    if (success) {
                        success(result);
                    }
                }
                else {
                    if (fail) {
                        fail(result.ReturnValue);
                    }
                    else {
                        showSystemError(result.ReturnValue);
                    }
                }
            }
            else {
                showMessage("Please try again later.");
            }
        });
    };
    ;
    /**
     *
     * @param prodId
     * @param success
     * @param fail
     */
    MemberApi.prototype.removeFavorite = function (prodId, success, fail) {
        var _this_1 = this;
        WSGet(this.apiPath + "/member/RemoveFavorite", { productId: prodId }, function (data, status) {
            _this_1.log(data);
            if (status === "success") {
                if (data.Succeeded) {
                    if (success) {
                        success(data);
                    }
                }
                else {
                    if (fail) {
                        fail(data.ReturnValue);
                    }
                }
            }
        });
    };
    ;
    /**
  *
  * @param prodCode
  * @param success
  * @param fail
  */
    MemberApi.prototype.checkFavorite = function (prodCode, success, fail) {
        var _this_1 = this;
        WSGet(this.apiPath + "/member/CheckFavorite", { code: prodCode }, function (data, status) {
            _this_1.log(data);
            if (status === "success") {
                if (data) {
                    if (data.Succeeded) {
                        if (success) {
                            success(data);
                        }
                    }
                    else {
                        if (fail) {
                            fail(data.ReturnValue);
                        }
                    }
                }
            }
        });
    };
    ;
    /**
       * 获取用户的喜爱清单合计
       * @param callback
       */
    MemberApi.prototype.getFavoriteCollection = function (callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Member/GetFavoriteCollection", {}, function (data, status) {
            _this_1.log(data);
            if (status === "success") {
                if (callback) {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                }
            }
        });
    };
    ;
    /**
     *
     * @param callback
     */
    MemberApi.prototype.getFavorite = function (callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Member/GetFavorite", {}, function (data, status) {
            _this_1.log(data);
            if (status === "success") {
                if (callback) {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                }
            }
        });
    };
    ;
    /**
     *
     * @param code
     * @param callback
     */
    MemberApi.prototype.setUILanguage = function (code, callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Member/SaveUILang", { lang: code }, function (data, status) {
            _this_1.log(data);
            if (status === "success") {
                if (callback) {
                    callback(data);
                }
            }
        });
    };
    ;
    MemberApi.prototype.setCurrency = function (cur, callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Member/SetCurrency", { currency: cur }, function (data, status) {
            _this_1.log(data);
            if (data.Succeeded) {
                if (callback) {
                    callback(data);
                }
                else {
                    window.location.reload();
                }
            }
            else {
                alert(data.Message);
            }
        });
    };
    ;
    MemberApi.prototype.getCurrency = function (callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Member/GetCurrency", {}, function (data, status) {
            _this_1.log(data);
            if (callback) {
                callback(data);
            }
        });
    };
    ;
    /**
     *
     * @param account 發送重置密碼郵件
     * @param callback
     */
    MemberApi.prototype.resetPassword = function (account, callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Member/resetPassword", { account: account }, function (data, status) {
            _this_1.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    ;
    MemberApi.prototype.forgetPassword = function (account, callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Member/ForgetPassword", { account: account }, function (data, status) {
            _this_1.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    ;
    MemberApi.prototype.resendActive = function (account, callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Member/ResendActive", { account: account }, function (data, status) {
            _this_1.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    ;
    /**
     * 從重置密碼郵件更新密碼
     * @param mid
     * @param code
     * @param pwd
     * @param callback
     */
    MemberApi.prototype.updatePwdFM = function (mid, code, pwd, callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Member/UpdatePwdFM", { id: mid, code: code, password: pwd }, function (data, status) {
            _this_1.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    ;
    /**
     * 检查邮件是否注册
     * @param email
     * @param callback
     */
    MemberApi.prototype.checkIsRegister = function (email, callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Member/CheckMemberIsRegister", { email: email }, function (data, status) {
            _this_1.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    ;
    /**
     * 订阅
     * @param email
     * @param callback
     */
    MemberApi.prototype.subscribe = function (email, status, callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Member/Subscribe", { email: email, status: status }, function (data, status) {
            _this_1.log(data);
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
    ;
    /**
 * 獲取喜愛商戶清單
 * @param pager 翻頁條件
 * @param callback 回調函數
 */
    MemberApi.prototype.getFavMerchants = function (pager, callback) {
        var _this_1 = this;
        var path = this.apiPath + "/Member/GetFavMerchants";
        WSPost(path, pager, function (result, status) {
            _this_1.log(result);
            if (result.Succeeded) {
                if (callback) {
                    callback(result);
                }
            }
        });
    };
    ;
    /**
* 檢查會員是否已收藏指定商家
* @param merchCode 商家編號
* @param success 成功回調函數
* @param fail 失敗回調函數
*/
    MemberApi.prototype.checkFavoriteMerch = function (merchCode, success, fail) {
        var _this_1 = this;
        WSGet(this.apiPath + "/member/CheckFavoriteMerch", { merchCode: merchCode }, function (data, status) {
            _this_1.log(data);
            if (status === "success") {
                if (data) {
                    if (data.Succeeded) {
                        if (success) {
                            success(data);
                        }
                    }
                    else {
                        if (fail) {
                            fail(data);
                        }
                    }
                }
            }
        });
    };
    ;
    /**
 * 增加商家到喜愛清單
 * @param merchCode
 * @param success
 * @param fail
 */
    MemberApi.prototype.addFavMerchant = function (merchCode, success, fail) {
        var path = this.apiPath + "/member/AddFavMerchant";
        WSGet(path, { merchCode: merchCode }, function (result, status) {
            if (result) {
                if (result.Succeeded) {
                    if (success) {
                        success(result);
                    }
                }
                else {
                    if (fail) {
                        fail(result.ReturnValue);
                    }
                    else {
                        showSystemError(result.Message);
                    }
                }
            }
            else {
                showMessage("Please try again later.");
            }
        });
    };
    ;
    /**
 * 將指定商家移除出喜愛清單
 * @param merchCode
 * @param success
 * @param fail
 */
    MemberApi.prototype.removeFavMerchant = function (merchCode, success, fail) {
        var path = this.apiPath + "/member/RemoveFavMerchant";
        WSGet(path, { merchCode: merchCode }, function (result, status) {
            if (result) {
                if (result.Succeeded) {
                    if (success) {
                        success(result);
                    }
                }
                else {
                    if (fail) {
                        fail(result.ReturnValue);
                    }
                    else {
                        showSystemError(result.Message);
                    }
                }
            }
            else {
                showMessage("Please try again later.");
            }
        });
    };
    ;
    /**
*   獲取已收藏的商家ID清單
* @param callback
*/
    MemberApi.prototype.getFavMerchantCodes = function (callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Member/GetFavMerchantCodes", {}, function (data, status) {
            _this_1.log(data);
            if (status === "success") {
                if (callback) {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                }
            }
        });
    };
    ;
    /**
     * 根據會員ID獲取會員瀏覽產品記錄
     * @param cond {object} cond ，例子：  {MemberId:會員ID,PageInfo:{Page:1,PageSize:10}}
     * @param callback
     */
    MemberApi.prototype.getProdBrowsingHistory = function (cond, callback) {
        var _this_1 = this;
        var path = this.apiPath + "/Member/GetProdBrowsingHistory";
        WSPost(path, cond, function (result, status) {
            _this_1.log(result);
            if (result.Succeeded) {
                if (callback) {
                    callback(result);
                }
            }
        });
    };
    ;
    /**
     * 根據會員ID獲取會員瀏覽商家記錄
     * @param cond {object} cond ，例子：  {MemberId:會員ID,PageInfo:{Page:1,PageSize:10}}
     * @param callback
     */
    MemberApi.prototype.getMerchBrowsingHistory = function (cond, callback) {
        var _this_1 = this;
        var path = this.apiPath + "/Member/GetMerchBrowsingHistory";
        WSPost(path, cond, function (result, status) {
            _this_1.log(result);
            if (result.Succeeded) {
                if (callback) {
                    callback(result);
                }
            }
        });
    };
    ;
    /**
 * facebook關聯登入
 * @param model {object} model ，例子：  {MemberId:會員ID,PageInfo:{Page:1,PageSize:10}}
 * @param callback
 */
    MemberApi.prototype.fBLogin = function (model, callback) {
        var _this_1 = this;
        var path = this.apiPath + "/Member/FBLogin";
        WSPost(path, model, function (result, status) {
            _this_1.log(result);
            if (result.Succeeded) {
                if (callback) {
                    callback(result);
                }
            }
        });
    };
    ;
    return MemberApi;
}(wsapi_1.WSAPI));
exports.MemberApi = MemberApi;
//# sourceMappingURL=memberApi.js.map