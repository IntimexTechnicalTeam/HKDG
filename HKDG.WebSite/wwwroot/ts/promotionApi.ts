/// <reference path="./common.d.ts" />
import { WSAPI } from "./wsapi"

export class PromotionApi extends WSAPI {
    getLatestProduct(callback: Function) { 
        WSGet(this.apiPath + "/Product/GetLatest",
            {},
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

    getHeaderBanner(data: any, callback: Function) {
        var _this = this;
        WSGet(this.apiPath + "/Banner/GetHeaderBanner",
            { 
                from: data.from,
                page: data.page,
            },
            function (data: any, status: any) {
                _this.log(data);
                if (callback) {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }                    
                }
            });
    };

    getPromotion(data: any, callback: Function) {
        var _this = this;
        WSGet(this.apiPath + "/Banner/GetPromotion",
            {
                from: data.from,
                page: data.page,
                position: data.position,
            },
            function (data: any, status: any) {
                _this.log(data);
                if (callback) {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                }
            });

    };

    ///获取推广商家
    ///data.From
    ///data.Page
    ///data.Position 
    ///data.ShowBanner//是否获取推广banner
    ///data.ShowProduct是否获取推广产品
    ///data.ShowMerchant是否获取推广商家
    ///data.ProductSize//获取推广产品的数量
    ///data.MerchantSize 获取推广商家的数量
    ///data.MerchantProductSize 如果传0则不获取推广商家的产品
    getPromotionByCond(data: any, callback: Function) {
        var _this = this;
        WSPost(this.apiPath + "/Promotion/GetPromotionByCondition",data,function (data: any, status: any) {
                _this.log(data);
                if (callback) {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                }
            });

    };
    
    
    getHomePromotionList(callback: Function) {
        var _this = this;
        WSGet(this.apiPath + "/Promotion/GetHomePromotionList",
            {
             
            },
            function (data: any, status: any) {
                if (callback) {
                    callback(data);
                }
            });

    };
    getPromotionList(data: any, callback: Function) {
        var _this = this;
        WSGet(this.apiPath + "/Banner/GetPromotionList",
            {
                from: data.from,
                page: data.page,
                position: data.position,
            },
            function (data: any, status: any) {
                _this.log(data);
                if (callback) {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                }
            });

    };

    getHotSearchKeyList(qty: string, callback: Function) {
        var _this = this;
        WSGet(this.apiPath + "/SearchKey/GetHotSearchKeys",
            {
                qty: qty
            },
            function (data: any, status: any) {
                _this.log(data);
                if (callback) {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                }
            });

    };

       //獲取最新的系統公告
    getLatestSysAnnouncement(qty: string, callback: Function) {
        WSGet(this.apiPath + "/Message/GetLatestSysAnnouncement",
            { qty: qty },
            (data: any, status: any) => {
                this.log(data);
                if (callback) {
                    callback(data);
                }
            }, function () { });
    }

      getHeaderPrmtList(callback: Function) {
        var _this = this;
        WSGet(this.apiPath + "/Promotion/GetHeaderPrmtList",
            {
             
            },
            function (data: any, status: any) {
                _this.log(data);
                if (callback) {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                }
            });

    };
}



