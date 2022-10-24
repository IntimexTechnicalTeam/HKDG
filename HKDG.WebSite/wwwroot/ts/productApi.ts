/// <reference path="./common.d.ts" />
import { WSAPI } from "./wsapi"
import { MemberApi } from "./memberApi"


export class ProductApi extends WSAPI {

    getProduct(sku: string, callback: Function) {
        var _this = this;
        WSGet(this.apiPath + "/Product/GetById",
            { id: sku },
            function (data: any, status: any) {
                _this.log(data);
                if (callback) {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                }
            }, function () { });
    };
    /**
     *  获取目录的产品分页列表
     * @param pager  data sample: {CatId:10,Page:1,PageSize=10}
     * @param callback 
     */
    getCatProduct(pager: object, callback: Function) {
        WSPost(this.apiPath + "/product/GetCatProdByPage", pager, (data: any, status: any) => {
            this.log(data);
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        }, function () { });
    };
    /**
     * 搜寻产品
     * @param {object} cond ，例子：  {Key:"name or desc",PageInfo:{Page:1,PageSize:10}}
     * @param {Function} callback 
     */
    search(cond: object, callback: Function) {

        WSPost(this.apiPath + "/product/Search",
            cond,
            (data: any, status: any) => {
                this.log(data);
                if (callback) {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                }
            }, function () { });
    };
    /**
     * 
     * @param callback 
     */
    getCatalogs(callback: Function) {
        var path = this.apiPath + "/product/GetCatalogs";
        WSAjaxSP({
            type: "get",
            data: {},
            url: path,
            success: (result: any) => {
                this.log(path, result);
                if (callback) {
                    if (result.Succeeded) {
                        callback(result.ReturnValue);
                    } else {
                        this.log(result.Message);
                    }
                }
            }
        });
    };

    getCatalog(id: string, callback: Function) {
        var path = this.apiPath + "/Product/GetCatalog";
        WSAjaxSP({
            type: "get",
            data: { cid: id },
            url: path,
            success: (data: any) => {
                this.log(path, data);
                if (callback) {
                    if (data.Succeeded) {
                        callback(data);
                    }
                }
            }
        });
    };
    getAttrImage(sku: string, imageType: string, attr1: string, attr2: string, attr3: string, callback: Function) {
        WSGet(this.apiPath + "/product/GetAttrImage",
            {
                sku: sku,
                attrType: imageType,
                attr1: attr1,
                attr2: attr2,
                attr3: attr3
            },
            (data: any, status: any) => {
                this.log(data);
                if (callback) {
                    callback(data);
                }
            });
    };
    /**
     * 获取相关产品
     */
    getRltProduct(sku: string, callback: Function) {

        WSGet(this.apiPath + "/Product/GetRelatedProduct",
            { id: sku },
            (result: any, status: any) => {
                this.log(result);
                if (callback) {
                    if (result.Succeeded) {
                        callback(result.ReturnValue);
                    } else {
                        this.log(result.Message);
                    }
                }
            });
    };

    checkSaleOut(code: string, attr1: string, attr2: string, attr3: string, callback: Function) {

        WSGet(this.apiPath + "/Product/CheckSaleOut",
            {
                code: code,
                attr1: attr1,
                attr2: attr2,
                attr3: attr3
            },
            (result: any, status: any) => {
                this.log(result);
                if (callback) {
                    callback(result);
                }
            });
    };

    check(code:string,attr1: string, attr2: string, attr3: string,saleTime:string,callback:Function)
    {
        WSGet(this.apiPath + "/Product/Check",
        {
            code: code,
            attr1: attr1,
            attr2: attr2,
            attr3: attr3,
            saleTime: saleTime

        },
        (result: any, status: any) => {
            if (callback) {
                callback(result);
            }
        });
    }

    countClick(code:string,isSearch: string,callback:Function)
    {
        WSGet(this.apiPath + "/Product/CountClick",
        {
            code: code,
            isSearch: isSearch
        },
        (result: any, status: any) => {
            if (callback) {
                callback(result);
            }
        });
    }

    checkOnSale(code: string, callback: Function) {

        WSGet(this.apiPath + "/Product/checkOnSale",
            {
                code: code                
            },
            (result: any, status: any) => {
                this.log(result);
                if (callback) {
                    callback(result);
                }
            });
    };


    addFavorite(sku: string, success: Function, fail: Function) {
        var memberApi = new MemberApi();
        memberApi.addFavorite(sku, success, fail);
    };
    removeFavorite(sku: string, success: Function, fail: Function) {
        var memberApi = new MemberApi();
        memberApi.removeFavorite(sku, success, fail);
    };

    checkProductSaleQuota(count: number, code: string, callback: Function) {
        WSGet(this.apiPath + "/Product/CheckProductSaleQuota",
            {
                count: count,
                code: code
            },
            (result: any, status: any) => {
                this.log(result);
                if (callback) {
                    callback(result);
                }
            });
    };

    saveClickRecord(code: string, callback: Function) {
        WSGet(this.apiPath + "/Product/SaveClickRecord",
            {
                code: code
            },
            (result: any, status: any) => {
                if (callback) {
                    callback(result);
                }
            });
    };
    saveSearchClickRecord(code: string, callback: Function) {
        WSGet(this.apiPath + "/Product/SaveSearchClickRecord",
            {
                code: code
            },
            (result: any, status: any) => {
                if (callback) {
                    callback(result);
                }
            });
    };
    /**
     * 获取用户偏好
     */
    getPreferenceProduct(qty: number, callback: Function) {

        WSGet(this.apiPath + "/Product/GetPreferenceProduct",
            { qty: qty },
            (result: any, status: any) => {
                this.log(result);
                if (callback) {
                    if (result.Succeeded) {
                        callback(result.ReturnValue);
                    } else {
                        this.log(result.Message);
                    }
                }
            });
    };
    getPreferenceCatalog(qty: number, callback: Function) {

        WSGet(this.apiPath + "/Product/GetPreferenceCatalog",
            { qty: qty },
            (result: any, status: any) => {
                this.log(result);
                if (callback) {
                    if (result.Succeeded) {
                        callback(result.ReturnValue);
                    } else {
                        this.log(result.Message);
                    }
                }
            });
    };
    /**
 *  獲取推薦產品的清單
 * @param pager  data sample: {Page:1,PageSize=10}
 * @param callback 
 */
    getRcmdProdByPage(pager: object, callback: Function) {
        WSPost(this.apiPath + "/product/GetRcmdProdByPage", pager, (data: any, status: any) => {
            this.log(data);
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        }, function () { });
    };
}