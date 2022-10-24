/// <reference path="./common.d.ts" />
import { WSAPI } from "./wsapi"

export class CMSApi extends WSAPI {
    getContent(cId: string, success: Function, error: Function) {
        WSGet(this.apiPath + "/cms/GetContent", { cid: cId }, function (data: any) {
            if (success) {
                success(data);
            }
        }, function () { });
    }

    getContentsByCatId(catId: string, success: Function, error: Function) {
        WSGet(this.apiPath + "/cms/GetContentsByCatId", { catId: catId }, function (data: any) {
            if (success) {
                success(data);
            }
        }, function () { });
    }

    getCategoryTree(success: Function, error: Function) {
        WSGet(this.apiPath + "/cms/GetCategoryTree", {}, function (data: any) {
            if (success) {
                success(data);
            }
        }, function () { });
    }
}