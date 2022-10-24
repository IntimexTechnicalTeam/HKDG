/// <reference path="./common.d.ts" />
import { WSAPI } from "./wsapi"

export class PaymentApi extends WSAPI {
    getPaymentMethod(callback: Function) {
        WSGet(
            this.apiPath + "/pay/GetPaymentMethod",
            {},
            (data: any, status: any) => {
                this.log(data);
                if (status === "success") {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    }
                }
            });
    }
    ///获取PayPal支付方式的对象
    getMPGSInfo(orderId: string,payType:string, callback: Function) {
        WSGet(
            this.apiPath + "/pay/GetMPGSInfo",
            { orderId: orderId, payType:payType },
            (data: any, status: any) => {
                this.log(data);
                if (status === "success") {
                    if (callback) {
                        if (data.Succeeded) {
                            callback(data.ReturnValue);
                        } else {
                            this.log(data.Message);
                        }
                    }

                }
            });
    }
    ///获取PayPal支付方式的对象
    getPayPalInfo(orderId: string, callback: Function) {
        WSGet(
            this.apiPath + "/pay/GetPayPalInfo",
            { orderId: orderId },
            (data: any, status: any) => {
                this.log(data);
                if (status === "success") {
                    if (callback) {
                        callback(data);
                    }

                }
            });
    }

    ///获取PayPal支付方式的对象
    getStripeInfo(orderId: string, callback: Function) {
        WSGet(
            this.apiPath + "/pay/GetSripeInfo",
            { orderId: orderId },
            (data: any, status: any) => {
                this.log(data);
                if (status === "success") {
                    if (callback) {
                        callback(data);
                    }

                }
            });
    }
    //支付完成后更新order的支付狀態
    updateOrderPaymentStatus(orderId: string, type: string, paykey: string, callback: Function) {
        WSGet(
            this.apiPath + "/pay/UpdateOrderPaymentStatus",
            { orderId: orderId, type: type, paymentKey: paykey },
            (data: any) => {
                this.log(data);
                //if (status === "success") {
                if (callback) {
                      if (data.Succeeded) {
                            callback(data.ReturnValue);
                        } else {
                            this.log(data.Message);
                        }
                }
                //}
            });
    }
}



