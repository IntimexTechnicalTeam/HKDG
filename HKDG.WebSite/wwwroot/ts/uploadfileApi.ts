/// <reference path="./common.d.ts" />
import { WSAPI } from "./wsapi"


export class UploadFileApi extends WSAPI {

    uploadFiles(files: any, callback: Function) {
        var _this = this;

        WSAjaxSP({
            url: this.apiPath + "/FileUpload/UploadFile",
            type: 'post',
            dataType: 'json',
            cache: false,
            data: files,
            processData: false,
            contentType: false,
            success: function (data: any) {
                if (callback) {
                    callback(data);
                }
            },
            error: function () {

            }
        }); 
    }
    uploadCSImage(chatId:string,files: any, callback: Function) {
        var _this = this;

        WSAjaxSP({
            url: this.apiPath + "/FileUpload/UploadCSImage?chatId="+chatId,
            type: 'post',
            dataType: 'json',
            cache: false,
            data: files,
            processData: false,
            contentType: false,
            success: function (data: any) {
                if (callback) {
                    callback(data);
                }
            },
            error: function () {

            }
        }); 
    }
}