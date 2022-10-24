import { WSAPI } from "./wsapi"
export class AdvertisingApi extends WSAPI {
    
    getAdViewingDetail(cond: any, callback: Function) {
        var _this = this;
        WSPost(_this.apiPath + "/Advertising/GetAdViewingDetail", cond, function (data: any) {
            if (callback) {
                callback(data);
            }
        }, function () { });
    };

    finishAdViewing(cond: any, callback: Function) {
        var _this = this;
        WSPost(_this.apiPath + "/Advertising/FinishAdViewing", cond, function (data: any) {
            if (callback) {
                callback(data);
            }
        }, function () { });
    };
}