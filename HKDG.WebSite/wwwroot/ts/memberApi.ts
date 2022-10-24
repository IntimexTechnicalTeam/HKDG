
/// <reference path="./common.d.ts" />
import { WSAPI } from "./wsapi"

export class MemberApi extends WSAPI {
    /**
     *  登入系統
     * @param account 
     * @param password 
     * @param rememberMe 
     * @param callback 
     */
    login(account: string, password: string, rememberMe: boolean, callback: Function) {
        var _this = this;
        WSPost(
            //this.apiPath + "/Member/Login",
            "/account/Login",
            { Account: account, Password: password, RememberMe: rememberMe },
            (data: any, status: any) => {
                _this.log(data);
                if (status === "success") {
                    if (data.Succeeded) {
                        WSGet(addTimeStamp(_this.apiPath + "/member/GetMemberInfo"), {}, () => {
                            if (callback) {
                                callback(data);
                            }
                        }, (e: any) => {
                            alert(e);
                        });
                    } else {

                        if (callback) {
                            callback(data);
                        }
                    }

                }
            }
        );
    };
    logout(callback: Function) {
        // setCookie("access_token", "", "/", -1);
        // setCookie("uid", "", "/", -1); 

        WSGet(
            this.apiPath + "/Member/Logout",
            {},
            (data: any, status: any) => {
                this.log(data);
                if (status === "success") {

                    if (callback) {
                        callback(data);
                    } else {
                        window.location.href = "/";
                    }

                }
            }
        );
    };
    /**
     * 第三方關聯登入系統
     * @param model
     * @param callback 
     */
    fbLogin(model: object, callback: Function) {
        var _this = this;
        WSPost(
            "/account/FBLogin",
            model,
            (data: any, status: any) => {
                _this.log(data);
                if (status === "success") {
                    if (data.Succeeded) {
                        WSGet(addTimeStamp(_this.apiPath + "/member/GetMemberInfo"), {}, () => {
                            if (callback) {
                                callback(data);
                            }
                        }, (e: any) => {
                            alert(e);
                        });
                    }
                    if (callback) {
                        callback(data);
                    }
                }
            }
        );
    };
    updatePassword(oldpwd: string, newpwd: string, callback: Function) {
        WSGet(
            this.apiPath + "/Member/UpdatePassword",
            { oldpwd: oldpwd, newpwd: newpwd },
            (data: any, status: any) => {
                this.log(data);
                if (status === "success") {
                    if (data.Succeeded) {

                    }
                    if (callback) {
                        callback(data);
                    }
                }
            }
        );
    };

    getMallFun(callback: Function) {
        WSGet(
            this.apiPath + "/Member/GetMemberMallFun",
            {},
            (data: any, status: any) => {
                if (callback) {
                    callback(data);
                }
            }
        );
    };

    /**
     * 注册会员
     * @param data 
     * @param callback 
     */
    register(data: object, callback: Function) {
        WSAjaxSP({
            type: "Post",
            url: this.apiPath + "/Member/Register",
            data: data,
            success: callback,
            error: function (e: any) {

            },
            complete: function () {

            }
        });
    };

    joinAccount(data: object, callback: Function) {
        WSAjaxSP({
            type: "Post",
            url: this.apiPath + "/Member/AssociatedThirdPartyAccount ",
            data: data,
            success: callback,
            error: function (e: any) {

            },
            complete: function () {

            }
        });
    };
    /**
    * 获取会员信息
    * @param callback 
    */
    getProfile(callback: Function) {
        WSGet(
            this.apiPath + "/Member/getProfile",
            {},
            (data: any, status: any) => {
                this.log(data);
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
                    } else {
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
            }
        );
    };
    /**
     * 获取会员信息
     * @param callback 
     */
    getMemberInfo(callback: Function) {
        WSGet(
            this.apiPath + "/Member/getMemberInfo",
            {},
            (data: any, status: any) => {
                this.log(data);
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
                    } else {
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
            }
        );
    };
    /**
     * 更新會員資料
     * @param obj 
     * @param callback 
     */
    updateProfile(model: any, callback: Function) {
        WSPost(
            this.apiPath + "/Member/UpdateProfile",
            model,
            (data: any, status: any) => {
                this.log(data);
                if (status === "success") {
                    if (callback) {
                        if (data.Succeeded) {
                            var lang = "C";
                            if (model.Language == 0) {
                                lang = "E";
                            } else if (model.Language == 1) {
                                lang = "C";
                            } else {
                                lang = "S";
                            }
                            setCookie("uLanguage", lang, "/", null);
                        }
                        callback(data);
                    }
                }
            }
        );
    };

    activate(mid: string, tid: string, callback: Function) {
        WSGet(
            this.apiPath + "/Member/Activate",
            { mid: mid, tid: tid },
            (data: any, status: any) => {
                this.log(data);
                if (status === "success") {
                    if (callback) {
                        callback(data);
                    }
                }
            }
        );
    };
    /**
     * 
     * @param prodId
     * @param success
     * @param fail
     */
    addFavorite(prodId: string, success: Function, fail: Function) {
        var path = this.apiPath + "/member/AddFavorite";
        WSGet(path, { productId: prodId }, (result: any, status: any) => {
            if (result) {
                if (result.Succeeded) {
                    if (success) {
                        success(result);
                    }
                } else {
                    if (fail) {
                        fail(result.ReturnValue);
                    } else {
                        showSystemError(result.ReturnValue);
                    }
                }
            } else {
                showMessage("Please try again later.");
            }
        });
    };
    /**
     * 
     * @param prodId
     * @param success
     * @param fail
     */
    removeFavorite(prodId: string, success: Function, fail: Function) {
        WSGet(this.apiPath + "/member/RemoveFavorite", { productId: prodId }, (data: any, status: any) => {
            this.log(data);
            if (status === "success") {
                if (data.Succeeded) {
                    if (success) {
                        success(data);
                    }
                } else {
                    if (fail) {
                        fail(data.ReturnValue);
                    }
                }
            }
        });
    };
    /**
  * 
  * @param prodCode
  * @param success
  * @param fail
  */
    checkFavorite(prodCode: string, success: Function, fail: Function) {
        WSGet(this.apiPath + "/member/CheckFavorite", { code: prodCode }, (data: any, status: any) => {
            this.log(data);
            if (status === "success") {
                if (data) {
                    if (data.Succeeded) {
                        if (success) {
                            success(data);
                        }
                    } else {
                        if (fail) {
                            fail(data.ReturnValue);
                        }
                    }
                }
            }
        });
    };


    /**
       * 获取用户的喜爱清单合计
       * @param callback
       */
    getFavoriteCollection(callback: Function) {
        WSGet(this.apiPath + "/Member/GetFavoriteCollection", {}, (data: any, status: any) => {
            this.log(data);
            if (status === "success") {
                if (callback) {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                }
            }
        });
    };

    /**
     * 
     * @param callback
     */
    getFavorite(callback: Function) {
        WSGet(this.apiPath + "/Member/GetFavorite", {}, (data: any, status: any) => {
            this.log(data);
            if (status === "success") {
                if (callback) {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                }
            }
        });
    };
    /**
     * 
     * @param code
     * @param callback
     */
    setUILanguage(code: string, callback: Function) {
        WSGet(this.apiPath + "/Member/SaveUILang", { lang: code }, (data: any, status: any) => {
            this.log(data);
            if (status === "success") {
                if (callback) {
                    callback(data);
                }
            }
        });
    };
    setCurrency(cur: string, callback: Function) {

        WSGet(this.apiPath + "/Member/SetCurrency", { currency: cur }, (data: any, status: any) => {
            this.log(data);
            if (data.Succeeded) {
                if (callback) {
                    callback(data);
                } else {
                    window.location.reload();
                }
            } else {
                alert(data.Message);
            }
        });
    };
    getCurrency(callback: Function) {
        WSGet(this.apiPath + "/Member/GetCurrency", {}, (data: any, status: any) => {
            this.log(data);
            if (callback) {
                callback(data);
            }
        });
    };
    /**
     * 
     * @param account 發送重置密碼郵件
     * @param callback 
     */
    resetPassword(account: string, callback: Function) {
        WSGet(this.apiPath + "/Member/resetPassword",
            { account: account },
            (data: any, status: any) => {
                this.log(data);
                if (callback) {
                    callback(data);
                }
            }, function () { });
    };
    forgetPassword(account: string, callback: Function) {
        WSGet(this.apiPath + "/Member/ForgetPassword",
            { account: account },
            (data: any, status: any) => {
                this.log(data);
                if (callback) {
                    callback(data);
                }
            }, function () { });
    };
    resendActive(account: string, callback: Function) {
        WSGet(this.apiPath + "/Member/ResendActive",
            { account: account },
            (data: any, status: any) => {
                this.log(data);
                if (callback) {
                    callback(data);
                }
            }, function () { });
    };
    /**
     * 從重置密碼郵件更新密碼
     * @param mid 
     * @param code 
     * @param pwd 
     * @param callback 
     */
    updatePwdFM(mid: string, code: string, pwd: string, callback: Function) {
        WSGet(this.apiPath + "/Member/UpdatePwdFM",
            { id: mid, code: code, password: pwd },
            (data: any, status: any) => {
                this.log(data);
                if (callback) {
                    callback(data);
                }
            }, function () { });
    };

    /**
     * 检查邮件是否注册
     * @param email 
     * @param callback 
     */
    checkIsRegister(email: string, callback: Function) {
        WSGet(this.apiPath + "/Member/CheckMemberIsRegister",
            { email: email },
            (data: any, status: any) => {
                this.log(data);
                if (callback) {
                    callback(data);
                }
            }, function () { });
    };
    /**
     * 订阅
     * @param email 
     * @param callback 
     */
    subscribe(email: string, status: boolean, callback: Function) {
        WSGet(this.apiPath + "/Member/Subscribe",
            { email: email, status: status },
            (data: any, status: any) => {
                this.log(data);
                if (callback) {
                    callback(data);
                }
            }, function () { });
    };
    /**
 * 獲取喜愛商戶清單
 * @param pager 翻頁條件
 * @param callback 回調函數
 */
    getFavMerchants(pager: object, callback: Function) {
        var path = this.apiPath + "/Member/GetFavMerchants";
        WSPost(path, pager, (result: any, status: any) => {
            this.log(result);
            if (result.Succeeded) {
                if (callback) {
                    callback(result);
                }
            }
        });
    };
    /**
* 檢查會員是否已收藏指定商家
* @param merchCode 商家編號
* @param success 成功回調函數
* @param fail 失敗回調函數
*/
    checkFavoriteMerch(merchCode: string, success: Function, fail: Function) {
        WSGet(this.apiPath + "/member/CheckFavoriteMerch", { merchCode: merchCode }, (data: any, status: any) => {
            this.log(data);
            if (status === "success") {
                if (data) {
                    if (data.Succeeded) {
                        if (success) {
                            success(data);
                        }
                    } else {
                        if (fail) {
                            fail(data);
                        }
                    }
                }
            }
        });
    };
    /**
 * 增加商家到喜愛清單
 * @param merchCode
 * @param success
 * @param fail
 */
    addFavMerchant(merchCode: string, success: Function, fail: Function) {
        var path = this.apiPath + "/member/AddFavMerchant";
        WSGet(path, { merchCode: merchCode }, (result: any, status: any) => {
            if (result) {
                if (result.Succeeded) {
                    if (success) {
                        success(result);
                    }
                } else {
                    if (fail) {
                        fail(result.ReturnValue);
                    } else {
                        showSystemError(result.Message);
                    }
                }
            } else {
                showMessage("Please try again later.");
            }
        });
    };
    /**
 * 將指定商家移除出喜愛清單
 * @param merchCode
 * @param success
 * @param fail
 */
    removeFavMerchant(merchCode: string, success: Function, fail: Function) {
        var path = this.apiPath + "/member/RemoveFavMerchant";
        WSGet(path, { merchCode: merchCode }, (result: any, status: any) => {
            if (result) {
                if (result.Succeeded) {
                    if (success) {
                        success(result);
                    }
                } else {
                    if (fail) {
                        fail(result.ReturnValue);
                    } else {
                        showSystemError(result.Message);
                    }
                }
            } else {
                showMessage("Please try again later.");
            }
        });
    };
    /**
*   獲取已收藏的商家ID清單
* @param callback
*/
    getFavMerchantCodes(callback: Function) {
        WSGet(this.apiPath + "/Member/GetFavMerchantCodes", {}, (data: any, status: any) => {
            this.log(data);
            if (status === "success") {
                if (callback) {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                }
            }
        });
    };

    /**
     * 根據會員ID獲取會員瀏覽產品記錄
     * @param cond {object} cond ，例子：  {MemberId:會員ID,PageInfo:{Page:1,PageSize:10}}
     * @param callback 
     */
    getProdBrowsingHistory(cond: object, callback: Function) {
        var path = this.apiPath + "/Member/GetProdBrowsingHistory";
        WSPost(path, cond, (result: any, status: any) => {
            this.log(result);
            if (result.Succeeded) {
                if (callback) {
                    callback(result);
                }
            }
        });
    };

    /**
     * 根據會員ID獲取會員瀏覽商家記錄
     * @param cond {object} cond ，例子：  {MemberId:會員ID,PageInfo:{Page:1,PageSize:10}}
     * @param callback 
     */
    getMerchBrowsingHistory(cond: object, callback: Function) {
        var path = this.apiPath + "/Member/GetMerchBrowsingHistory";
        WSPost(path, cond, (result: any, status: any) => {
            this.log(result);
            if (result.Succeeded) {
                if (callback) {
                    callback(result);
                }
            }
        });
    };

    /**
 * facebook關聯登入
 * @param model {object} model ，例子：  {MemberId:會員ID,PageInfo:{Page:1,PageSize:10}}
 * @param callback 
 */
    fBLogin(model: object, callback: Function) {
        var path = this.apiPath + "/Member/FBLogin";
        WSPost(path, model, (result: any, status: any) => {
            this.log(result);
            if (result.Succeeded) {
                if (callback) {
                    callback(result);
                }
            }
        });
    };
}