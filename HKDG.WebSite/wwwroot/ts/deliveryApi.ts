/// <reference path="./common.d.ts" />
import { WSAPI } from "./wsapi"

export class DeliveryApi extends WSAPI {

    getCountry(callback: Function) {
        var path = this.apiPath + "/Address/Country";
        WSGet(path, {}, (data: any, status: any) => {
            this.log(data);
            if (status === "success") {
                if (callback) {
                    if (data.Succeeded) {
                        callback(data.ReturnValue);
                    } else {
                        console.log(data.Message);
                    }
                }
            } else {
                showInfo(status);
            }
        });

    };
    getProvince(countryId: string, callback: Function) {
        var path = this.apiPath + "/Address/Province";
        WSGet(path, { countryId: countryId }, (data: any, status: any) => {
            this.log(path, data);
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                } else {
                    console.log(data.Message);
                }
            }
        });

    };

    getExpress(addressId: string, weight: number, callback: Function) {
        WSGet(
            this.apiPath + "/Express/GetExpress",
            { deliveryAddrId: addressId, totalWeight: weight },
            (data: any, status: any) => {
                this.log(data);
                if (status === "success") {
                    if (callback) {
                        if (data.Succeeded) {
                            var key = data.Message;
                            callback(data.ReturnValue, key);
                        }
                    }
                } else {
                    showInfo(status);
                }
            }
        );
    };
    getExpressWithDiscount(exCond: any, callback: Function) {
        var _this = this;
        WSPost(this.apiPath + "/Express/GetExpressWithDiscount", exCond, function (data: any) {
            if (callback) {
                // if (data.Succeeded) {
                //     var key = data.Message;
                //     callback(data.ReturnValue, key);
                // }
                callback(data);
            }
        }, function () { });
    };

    getIPostStationExpress(exCond: any, callback: Function) {
        var _this = this;
        WSPost(this.apiPath + "/Express/GetIPostStationExpress", exCond, function (data: any) {
            if (callback) {
                // if (data.Succeeded) {
                //     var key = data.Message;
                //     callback(data.ReturnValue, key);
                // }
                callback(data);
            }
        }, function () { });
    };
    getIPostStationList(callback: Function) {
        var _this = this;
        WSGet(this.apiPath + "/Express/GetIPostStationList", {}, function (data: any) {
            if (callback) {
                if (data.Succeeded) {
                    //var key = data.Message;
                    // callback(data.ReturnValue);
                    callback(data);
                }
            }
        }, function () { });
    }
    getCollectionList(exCond: any, callback: Function) {
        var _this = this;
        WSPost(this.apiPath + "/Express/GetCollectionOfficeList", exCond, function (data: any) {
            if (callback) {
                // if (data.Succeeded) {
                //     var key = data.Message;
                //     callback(data.ReturnValue, key);
                // }
                callback(data);
            }
        }, function () { });
    }
    getIPostStationByMCNCode(code: string, merchId: string, callback: Function) {
        var _this = this;
        WSGet(this.apiPath + "/Express/GetIPostStationByMCNCode", { mcnCode: code, merchId: merchId }, function (data: any) {
            if (callback) {
                //if (data.Succeeded) {
                callback(data);
                //}
            }
        }, function () { });
    }

    getDeliveryActiveInfo(mercId: string, callback: Function) {
        var _this = this;
        WSGet(this.apiPath + "/Express/GetDeliveryActiveInfo", { merchantId: mercId }, function (data: any) {
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        }, function () { });
    }

    checkCountryIsValid(info: any, callback: Function) {
        var _this = this;
        WSPost(this.apiPath + "/Express/CheckCountryIsVaild", info, function (data: any) {
            if (callback) {
                if (data.Succeeded) {
                    callback(data.ReturnValue);
                }
            }
        }, function () { });
    }

    getAddress(callback: any) {
        WSGet(
            this.apiPath + "/Address/GetAddresses",
            {},
            (data: any, status: any) => {
                this.log(data);
                if (status === "success") {
                    if (callback) {
                        if (data.Succeeded) {
                            callback(data.ReturnValue);
                        }
                    }
                }
            }
        );
    };
    getDefaultAddr(callback: any) {
        WSGet(
            this.apiPath + "/Address/GetDefaultAddr",
            {},
            (data: any, status: any) => {
                this.log(data);
                if (status === "success") {
                    if (callback && typeof (callback) === "function") {
                        if (data.Succeeded) {
                            callback(data.ReturnValue);
                        }
                    } else {
                        showInfo(data.Message);
                    }
                }
            }
        );
    };
    getPickupAddress(callback: any) {
        WSGet(
            this.apiPath + "/Address/GetPickupAddress",
            {},
            (data: any, status: any) => {
                this.log(data);
                if (status === "success") {
                    if (callback && typeof (callback) === "function") {
                        callback(data);
                    } else {
                        showInfo(data.Message);
                    }
                }
            }
        );
    };

    saveAddress(address: any, callback: Function) {
        var action = "/Address/Add";
        console.log(address);
        if (address.Id && address.Id != "") {
            action = "/Address/Update";
        }
        WSAjaxSP({
            type: "post",
            url: this.apiPath + action,
            data: address,
            success: (data: any, status: any) => {
                this.log(data);
                if (status === "success") {
                    if (callback && typeof (callback) === "function") {
                        callback(data);
                    } else {
                        showInfo(data.Message);
                    }
                }
            }
        });
    };
    removeAddress(id: string, callback: Function) {
        WSGet(this.apiPath + "/address/Remove",
            { id: id },
            (data: any, status: any) => {
                this.log(data);
                if (status === "success") {
                    if (callback && typeof (callback) === "function") {
                        callback(data);
                    } else {
                        showInfo(data.Message);
                    }
                }
            });
    };
    gtPaymentType(callback: any) {
        WSGet(
            this.apiPath + "/pay/GetPaymentTypes",
            {},
            (data: any, status: any) => {
                this.log(data);
                if (status === "success") {
                    if (callback && typeof (callback) === "function") {
                        callback(data);
                    } else {
                        showInfo(data.Message);
                    }
                }
            });
    };
    GetExpressList(exCond: any, callback: Function) {
        var _this = this;
        WSPost(this.apiPath + "/Express/GetExpressList", exCond, function (data: any) {
            if (callback) {
                // if (data.Succeeded) {
                //     var key = data.Message;
                //     callback(data.ReturnValue, key);
                // }
                callback(data);
            }
        }, function () { });
    };
    GetExpressChargeListByCode(exCond: any, callback: Function) {
        var _this = this;
        WSPost(this.apiPath + "/Express/GetExpressChargeListByCode", exCond, function (data: any) {
            if (callback) {
                // if (data.Succeeded) {
                //     var key = data.Message;
                //     callback(data.ReturnValue, key);
                // }
                callback(data);
            }
        }, function () { });
    };

}



