/// <reference path="./common.d.ts" />
import { WSAPI } from "./wsapi"

export class ShoppingCartApi extends WSAPI {
    loadData(callback: Function) {
        WSGet(this.apiPath + "/ShoppingCart/GetShopCart",
            {},
            (data: any, status: any) => {
                this.log(data);
                if (callback) {
                    callback(data);
                }
            });
    };
    loadCheckoutData(callback: Function) {
        WSGet(this.apiPath + "/ShoppingCart/GetMallCart",
            {},
            (data: any, status: any) => {
                this.log(data);
                if (callback) {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    } else {
                        this.log(data.Message);
                    }
                }
            });
    };
    addItem(prodId: string,prodCode:string, attr1: string, attr2: string, attr3: string, qty: number, callback: Function, loginFun: Function) {
        WSPost(this.apiPath + "/ShoppingCart/AddItem", { ProductId: prodId, ProdCode:prodCode,Attr1: attr1, Attr2: attr2, Attr3: attr3, qty: qty }, (data: any, status: any) => {
            this.log(data);
            if (status === "success") {
                if (callback) {
                    if (!data.Succeeded) {
                        if (data.ReturnValue == "3002") {//LOGIN
                            if (loginFun && typeof (loginFun) === "function") {
                                loginFun(data);
                            } else {
                                window.location.href = "/account/login?returnUrl=" + window.location.href;
                            }
                            return;
                        }
                    }

                    callback(data);
                }
            }
        })
    };
    removeItem(id: string, callback: Function) {
        var _this = this;
        WSGet(this.apiPath + "/ShoppingCart/RemoveItem", { id: id }, function (data: any, status: any) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        })
    };


    updateItemQty(itemId: string, qty: number, callback: Function) {
        var _this = this;
        WSGet(this.apiPath + "/ShoppingCart/UpdateItemQty", { id: itemId, qty: qty }, function (data: any, status: any) {
            _this.log(data);
            if (status === "success") {
                callback(data);
            }
        })
    };
    saveDelivery(addressId: string, expressId: string, callback: Function) {
        var _this = this;
        WSGet(this.apiPath + "/Pay/SaveDelivery",
            { deliveryAddressId: addressId, expressCompanyId: expressId },
            function (data: any, status: any) {
                _this.log(data);
                if (status === "success") {
                    callback(data);
                }
            });
    };
    savePickupInfo(addressId: string, pickupDate: string, pickupTime: string, pickupAddress: string, callback: Function) {

        WSGet(this.apiPath + "/Pay/SavePickupInfo",
            { addressId: addressId, date: pickupDate, time: pickupTime, address: pickupAddress },
            (data: any, status: any) => {
                this.log(data);
                if (status === "success") {
                    callback(data);
                }
            });
    };
    savePayMethod(methodId: string, callback: Function) {
        var _this = this;
        WSGet(this.apiPath + "/pay/SavePayMethod", { payMethodType: methodId }, function (data: any, status: any) {
            _this.log(data);
            if (status === "success") {
                if (callback) {
                    callback(data);
                }

            }
        });
    };

    getFreeProduct(ruleId: string, skuId: string, qty: string, callback: Function) {
        var _this = this;
        WSGet(this.apiPath + "/ShoppingCart/GetFreeProduct", { ruleId: ruleId, skuId: skuId, qty: qty }, function (data: any, status: any) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        });
    };

    getSetPrice(ruleId: string, skuId: string, qty: string, callback: Function) {
        var _this = this;
        WSGet(this.apiPath + "/ShoppingCart/GetSetPrice", { ruleId: ruleId, skuId: skuId, qty: qty }, function (data: any, status: any) {
            _this.log(data);
            if (callback) {
                callback(data);
            }
        });
    };


}



