/// <reference path="./common.d.ts" />
import { WSAPI } from "./wsapi"

export class MerchantApi extends WSAPI {
    getMerchantInfo(merchantId: string, callback: Function) {
        var path = this.apiPath + "/Merchant/GetMerchantInfo";
        WSGet(path, { merchID: merchantId }, (result: any, status: any) => {
            if (result.Succeeded) {
                if (callback) {
                    callback(result);
                }
            }
        });
    };

    getMerchantList(key:string,page:number,pageSize:number,callback: Function) {
        var path = this.apiPath + "/Merchant/Search";
        WSGet(path,{key:key,page:page,pageSize:pageSize}, (result: any, status: any) => {
            if (result.Succeeded) {
                if (callback) {
                    callback(result);
                }
            }
        });
    };

    getProducts(cond: object, callback: Function) {
        var path = this.apiPath + "/Merchant/GetProducts";
        WSPost(path, cond, (result: any, status: any) => {
            if (result.Succeeded) {
                if (callback) {
                    callback(result);
                }
            }
        });
    };

    countBannerClick(merchantId:string,bannerLink:string,callback:Function){
        var path = this.apiPath + "/Merchant/CountBannerClick";
        WSGet(path,{merchantId:merchantId,bannerLink:bannerLink}, (result: any, status: any) => {
            if (callback) {
                callback(result);
            }
        });
    };
}