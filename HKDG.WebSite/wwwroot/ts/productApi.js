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
exports.ProductApi = void 0;
/// <reference path="./common.d.ts" />
var wsapi_1 = require("./wsapi");
var memberApi_1 = require("./memberApi");
var ProductApi = /** @class */ (function (_super) {
    __extends(ProductApi, _super);
    function ProductApi() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ProductApi.prototype.getProduct = function (sku, callback) {
        var _this = this;
        WSGet(this.apiPath + "/Product/GetById", { id: sku }, function (data, status) {
            _this.log(data);
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        }, function () { });
    };
    ;
    /**
     *  获取目录的产品分页列表
     * @param pager  data sample: {CatId:10,Page:1,PageSize=10}
     * @param callback
     */
    ProductApi.prototype.getCatProduct = function (pager, callback) {
        var _this_1 = this;
        WSPost(this.apiPath + "/product/GetCatProdByPage", pager, function (data, status) {
            _this_1.log(data);
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        }, function () { });
    };
    ;
    /**
     * 搜寻产品
     * @param {object} cond ，例子：  {Key:"name or desc",PageInfo:{Page:1,PageSize:10}}
     * @param {Function} callback
     */
    ProductApi.prototype.search = function (cond, callback) {
        var _this_1 = this;
        WSPost(this.apiPath + "/product/Search", cond, function (data, status) {
            _this_1.log(data);
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        }, function () { });
    };
    ;
    /**
     *
     * @param callback
     */
    ProductApi.prototype.getCatalogs = function (callback) {
        var _this_1 = this;
        var path = this.apiPath + "/product/GetCatalogs";
        WSAjaxSP({
            type: "get",
            data: {},
            url: path,
            success: function (result) {
                _this_1.log(path, result);
                if (callback) {
                    if (result.Succeeded) {
                        callback(result.ReturnValue);
                    }
                    else {
                        _this_1.log(result.Message);
                    }
                }
            }
        });
    };
    ;
    ProductApi.prototype.getCatalog = function (id, callback) {
        var _this_1 = this;
        var path = this.apiPath + "/Product/GetCatalog";
        WSAjaxSP({
            type: "get",
            data: { cid: id },
            url: path,
            success: function (data) {
                _this_1.log(path, data);
                if (callback) {
                    if (data.Succeeded) {
                        callback(data);
                    }
                }
            }
        });
    };
    ;
    ProductApi.prototype.getAttrImage = function (sku, imageType, attr1, attr2, attr3, callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/product/GetAttrImage", {
            sku: sku,
            attrType: imageType,
            attr1: attr1,
            attr2: attr2,
            attr3: attr3
        }, function (data, status) {
            _this_1.log(data);
            if (callback) {
                callback(data);
            }
        });
    };
    ;
    /**
     * 获取相关产品
     */
    ProductApi.prototype.getRltProduct = function (sku, callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Product/GetRelatedProduct", { id: sku }, function (result, status) {
            _this_1.log(result);
            if (callback) {
                if (result.Succeeded) {
                    callback(result.ReturnValue);
                }
                else {
                    _this_1.log(result.Message);
                }
            }
        });
    };
    ;
    ProductApi.prototype.checkSaleOut = function (code, attr1, attr2, attr3, callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Product/CheckSaleOut", {
            code: code,
            attr1: attr1,
            attr2: attr2,
            attr3: attr3
        }, function (result, status) {
            _this_1.log(result);
            if (callback) {
                callback(result);
            }
        });
    };
    ;
    ProductApi.prototype.check = function (code, attr1, attr2, attr3, saleTime, callback) {
        WSGet(this.apiPath + "/Product/Check", {
            code: code,
            attr1: attr1,
            attr2: attr2,
            attr3: attr3,
            saleTime: saleTime
        }, function (result, status) {
            if (callback) {
                callback(result);
            }
        });
    };
    ProductApi.prototype.countClick = function (code, isSearch, callback) {
        WSGet(this.apiPath + "/Product/CountClick", {
            code: code,
            isSearch: isSearch
        }, function (result, status) {
            if (callback) {
                callback(result);
            }
        });
    };
    ProductApi.prototype.checkOnSale = function (code, callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Product/checkOnSale", {
            code: code
        }, function (result, status) {
            _this_1.log(result);
            if (callback) {
                callback(result);
            }
        });
    };
    ;
    ProductApi.prototype.addFavorite = function (sku, success, fail) {
        var memberApi = new memberApi_1.MemberApi();
        memberApi.addFavorite(sku, success, fail);
    };
    ;
    ProductApi.prototype.removeFavorite = function (sku, success, fail) {
        var memberApi = new memberApi_1.MemberApi();
        memberApi.removeFavorite(sku, success, fail);
    };
    ;
    ProductApi.prototype.checkProductSaleQuota = function (count, code, callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Product/CheckProductSaleQuota", {
            count: count,
            code: code
        }, function (result, status) {
            _this_1.log(result);
            if (callback) {
                callback(result);
            }
        });
    };
    ;
    ProductApi.prototype.saveClickRecord = function (code, callback) {
        WSGet(this.apiPath + "/Product/SaveClickRecord", {
            code: code
        }, function (result, status) {
            if (callback) {
                callback(result);
            }
        });
    };
    ;
    ProductApi.prototype.saveSearchClickRecord = function (code, callback) {
        WSGet(this.apiPath + "/Product/SaveSearchClickRecord", {
            code: code
        }, function (result, status) {
            if (callback) {
                callback(result);
            }
        });
    };
    ;
    /**
     * 获取用户偏好
     */
    ProductApi.prototype.getPreferenceProduct = function (qty, callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Product/GetPreferenceProduct", { qty: qty }, function (result, status) {
            _this_1.log(result);
            if (callback) {
                if (result.Succeeded) {
                    callback(result.ReturnValue);
                }
                else {
                    _this_1.log(result.Message);
                }
            }
        });
    };
    ;
    ProductApi.prototype.getPreferenceCatalog = function (qty, callback) {
        var _this_1 = this;
        WSGet(this.apiPath + "/Product/GetPreferenceCatalog", { qty: qty }, function (result, status) {
            _this_1.log(result);
            if (callback) {
                if (result.Succeeded) {
                    callback(result.ReturnValue);
                }
                else {
                    _this_1.log(result.Message);
                }
            }
        });
    };
    ;
    /**
 *  獲取推薦產品的清單
 * @param pager  data sample: {Page:1,PageSize=10}
 * @param callback
 */
    ProductApi.prototype.getRcmdProdByPage = function (pager, callback) {
        var _this_1 = this;
        WSPost(this.apiPath + "/product/GetRcmdProdByPage", pager, function (data, status) {
            _this_1.log(data);
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        }, function () { });
    };
    ;
    return ProductApi;
}(wsapi_1.WSAPI));
exports.ProductApi = ProductApi;
//# sourceMappingURL=productApi.js.map