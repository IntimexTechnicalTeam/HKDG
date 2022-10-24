/// <reference path="./common.d.ts" />
import { WSAPI } from "./wsapi"
export class CouponApi extends WSAPI {
    
    getCoupons(cond: any, callback: Function) {
        var _this = this;
        WSPost(this.apiPath + "/Coupon/GetCouponGroup", cond, function (data: any) {
            if (callback) {
                callback(data);
                //if (data.Succeeded) {
                //    callback(data);
                //}
            }
        }, function () { });
    };

    getRules(callback: Function) {
        var path = this.apiPath + "/Coupon/CheckHasGroupOrRuleDiscount";
        WSGet(path, {}, (data: any, status: any) => {
            this.log(data);
            if (callback) {
                callback(data);
                //if (data.Succeeded) {
                //    callback(data);
                //} else {
                //    console.log(data.Message);
                //}
            }
        });
    };

    getPromotionCode(merchantId: string, code: string,price:number, callback: Function) {
        var path = this.apiPath + "/Coupon/GetPromotionCodeCoupon";
        WSGet(path, { merchantId: merchantId, code: code,price:price }, (data: any, status: any) => {
            this.log(data);
            if (callback) {
                callback(data);
            }
        });
    };

    getMallFun(callback: Function) {
        var path = this.apiPath + "/Coupon/GetMallFun";
        WSGet(path, {}, (data: any, status: any) => {
            this.log(data);
            if (callback) {
                callback(data);
            }
        });
    };

    getMemberCoupon(cond: any,callback: Function) {
        var path = this.apiPath + "/Coupon/GetMemberCoupon";
        WSPost(path, cond, (data: any, status: any) => {
            if (callback) {
                callback(data);
            }
        });
    };

    getPromotionCodeV2(cond: any, callback: Function) {
        var path = this.apiPath + "/Coupon/GetPromotionCodeCouponV2";
        WSPost(path, cond, (data: any, status: any) => {
            if (callback) {
                callback(data);
            }
        });
    };
}