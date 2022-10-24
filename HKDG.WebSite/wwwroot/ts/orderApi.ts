/// <reference path="./common.d.ts" />
import { WSAPI } from "./wsapi"

export class OrderApi extends WSAPI {
    getPaymentType(callback: Function) {
        WSGet(
            this.apiPath + "/pay/GetPaymentMethod",
            {},
            (data: any, status: any) => {
                this.log(data);
                if (status === "success") {
                    callback(data);
                }
            });
    };
    getPageOrder(pageSize: number, page: number, status: Number = -1, orderBy: string = "CreateDate", callback: Function) {

        WSPost(this.apiPath + "/Order/SearchOrder", { Page: page, PageSize: pageSize, Status: status, OrderBy: orderBy }, (data: any, status: any) => {
            this.log(data);
            if (status === "success") {
                // if (callback && typeof (callback) === "function") {
                //     if (data.Succeeded) {
                //         callback(data.ReturnValue);
                //     }
                // } else {
                //     showInfo(data.Message);
                // }
                callback(data);
            }
        });
    };
    getPageOldOrder(pageSize: number, page: number, status: Number = -1, orderBy: string = "CreateDate", callback: Function) {

        WSPost(this.apiPath + "/Order/SearchOldOrder", { Page: page, PageSize: pageSize, Status: status, OrderBy: orderBy }, (data: any, status: any) => {
            this.log(data);
            if (status === "success") {
                // if (callback && typeof (callback) === "function") {
                //     if (data.Succeeded) {
                //         callback(data.ReturnValue);
                //     }
                // } else {
                //     showInfo(data.Message);
                // }
                callback(data);
            }
        });
    };
    getOrder(id: string, callback: Function) {
        WSGet(this.apiPath + "/Order/GetOrder", { id: id }, (data: any, status: any) => {
            this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    if (data.Succeeded) {
                        callback(data);
                    } else {
                        showInfo(data.Message);
                    }
                } else {
                    showInfo(data.Message);
                }
            }
        });
    };
    getOldOrder(id: string, callback: Function) {
        WSGet(this.apiPath + "/Order/GetOldOrder", { id: id }, (data: any, status: any) => {
            this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    if (data.Succeeded) {
                        callback(data);
                    } else {
                        showInfo(data.Message);
                    }
                } else {
                    showInfo(data.Message);
                }
            }
        });
    };
    getOrderStatusList(callback: Function) {
        WSGet(this.apiPath + "/Order/GetOrderStatus", {}, (data: any, status: any) => {
            this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    if (data.Succeeded) {
                        callback(data);
                    } else {
                        showInfo(data.Message);
                    }
                } else {
                    showInfo(data.Message);
                }
            }
        });
    };

    cancel(id: string, callback: Function) {
        WSGet(this.apiPath + "/Order/Cancel", { id: id }, (data: any, status: any) => {
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
    createOrder(order: any, callback: any) {
        WSPost(
            this.apiPath + "/order/Create",
            order,
            function (data: any, status: any) {
                if (status === "success") {
                    if (callback && typeof (callback) === "function") {
                        callback(data);
                    } else {
                        showInfo(data.Message);
                    }
                }
            }, function (XMLHttpRequest: any, textStatus: any, errorThrown: any) {
                console.log(XMLHttpRequest);
                console.log(textStatus);
                console.log(errorThrown);
            });
    };
    getReturnOrderTypeList(callback: Function) {
        WSGet(this.apiPath + "/order/GetReturnType", {}, (data: any, status: any) => {
            this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    if (data.Succeeded) {
                        callback(data);
                    } else {
                        showInfo(data.Message);
                    }
                } else {
                    showInfo(data.Message);
                }
            }
        });
    };
    createReturnOrder(returnorder: any, callback: any) {
        WSPost(
            this.apiPath + "/order/CreateReturnOrder",
            returnorder,
            function (data: any, status: any) {
                if (status === "success") {
                    if (callback && typeof (callback) === "function") {
                        callback(data);
                    } else {
                        showInfo(data.Message);
                    }
                }
            }, function (XMLHttpRequest: any, textStatus: any, errorThrown: any) {
                console.log(XMLHttpRequest);
                console.log(textStatus);
                console.log(errorThrown);
            });
    };
    getReturnOrders(callback: Function) {
        WSGet(this.apiPath + "/order/GetReturnOrders", {}, (data: any, status: any) => {
            this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    if (data.Succeeded) {
                        callback(data);
                    } else {
                        showInfo(data.Message);
                    }
                } else {
                    showInfo(data.Message);
                }
            }
        });
    };

    getOrderTrackingInfo(trackingNo: string, callback: Function) {
        WSGet(this.apiPath + "/order/GetOrderMailTrackingInfo", { trackingNo: trackingNo }, (data: any, status: any) => {
            this.log(data);
            if (callback) {
                callback(data);
            }
        });
    };


    getReturnOrder(id: string, callback: Function) {
        WSGet(this.apiPath + "/Order/getReturnOrder", { id: id }, (data: any, status: any) => {
            this.log(data);
            if (status === "success") {
                if (callback && typeof (callback) === "function") {
                    if (data.Succeeded) {
                        callback(data);
                    } else {
                        showInfo(data.Message);
                    }
                } else {
                    showInfo(data.Message);
                }
            }
        });
    };
}



