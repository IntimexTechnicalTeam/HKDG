/// <reference path="./common.d.ts" />
import { WSAPI } from "./wsapi"

export class MenuApi extends WSAPI {
    
    /**
     * 获取自定义菜单
     * @param callback 
     */
    GetMenuBar(callback: Function) {
        WSGet(
            this.apiPath + "/Menu/GetMenuBar",
            {},
            (data: any, status: any) => {
                if (callback) {
                    callback(data);
                }
            }
        );
    };


}