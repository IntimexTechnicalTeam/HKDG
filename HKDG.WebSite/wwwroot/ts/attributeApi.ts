/// <reference path="./common.d.ts" />
import { WSAPI } from "./wsapi"

export class AttributeApi extends WSAPI {
    /**
     * 根据传入的参数获取属性
     * Cond.Type(0获取所有属性,1获取库存属性,2获取非库存属性)
     */
    getAttrs(cond: object,success: Function, error: Function) {
        WSPost(this.apiPath + "/Attribute/GetFrontAttribute", cond, function (data: any) {
            if (success) {
                success(data);
            }
        }, function () { });
    }
}