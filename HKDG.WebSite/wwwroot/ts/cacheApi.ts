/// <reference path="./common.d.ts" />
import { WSAPI } from "./wsapi"
export class CacheApi extends WSAPI {
    
    getHeaderCache(callback: Function) {
        var _this = this;
        WSGet(this.apiPath + "/cache/GetHeaderCache", null, function (data: any) {
            if (callback) {
                callback(data);
            }
        }, function () { });
    };

    getHomeBodyCache(cond:any, callback: Function) {
        var _this = this;
        WSPost(this.apiPath + "/cache/GetHomeBodyCache", cond, function (data: any) {
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
}