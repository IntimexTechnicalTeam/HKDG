/// <reference path="./common.d.ts" />
import { WSAPI } from "./wsapi"



export class ReportApi extends WSAPI {

    GetHotSale(pageSize: number, page: number, callback: Function) {

        WSPost(this.apiPath + "/report/GetHotSale", { Page: page, PageSize: pageSize }, (data: any, status: any) => {
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
        });
    };

    GetHighScore(pageSize: number, page: number, callback: Function) {

        WSPost(this.apiPath + "/report/GetHighScore", { Page: page, PageSize: pageSize }, (data: any, status: any) => {
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
        });
    };
    GetHotStore(pageSize: number, page: number, callback: Function) {

        WSPost(this.apiPath + "/report/GetHotStore", { Page: page, PageSize: pageSize }, (data: any, status: any) => {
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
        });
    };

    getRecommendProducts(userId:string,lang:string,page: number, pageSize: number, callback: Function) {

        WSPost(this.apiPath + "/report/GetReCommendProducts", {MemberId:userId,Language:lang ,Page: page, PageSize: pageSize }, (data: any, status: any) => {
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
}