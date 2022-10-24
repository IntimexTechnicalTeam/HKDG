/// <reference path="./common.d.ts" />
import { WSAPI } from "./wsapi"

class Message {

    private _content!: string;
    public get Content(): string {
        return this._content;
    }
    public set Content(v: string) {
        this._content = v;
    }

    private _sendDate!: string;
    public get SendDate(): string {
        return this._sendDate;
    }
    public set SendDate(v: string) {
        this._sendDate = v;
    }

    private _sender!: string;
    public get Sender(): string {
        return this._sender;
    }
    public set Sender(v: string) {
        this._sender = v;
    }

    private _type!: number;
    public get Type(): number {
        return this._type;
    }
    public set Type(v: number) {
        this._type = v;
    }


    private _seq!: number;
    public get Seq(): number {
        return this._seq;
    }
    public set Seq(v: number) {
        this._seq = v;
    }

}
class CustomerServiceApi extends WSAPI {
    getInquiryType(callback: Function) {

        WSGet(
            this.apiPath + "/CS/GetInquiryType",
            {},
            (data: any, status: any) => {
                this.log(data);
                if (status === "success") {
                    if (callback) {
                        callback(data);
                    }
                }
            }
        );
    };

    inquiry(model: object, callback: Function) {

        WSPost(
            this.apiPath + "/CS/Inquiry",
            model,
            (data: any, status: any) => {
                this.log(data);
                if (status === "success") {
                    if (data.Succeeded) {
                        if (callback) {
                            callback(data.Message);
                        } else {
                            showInfo(data.Message);
                        }
                    } else {
                        showSystemError(data.ReturnValue);
                    }
                }
            }
        );

    };
    /**
     * 打开一个对话
     * @param prodCode 产品编号
     * @param mchId  商家ID
     */
    openChat(mchId: string, prodId: string, prodCode: string, callback: Function) {
        WSGet(
            this.apiPath + "/CS/OpenChat",
            { prodCode: prodCode, prodId: prodId, mchId: mchId },
            (data: any, status: any) => {
                this.log(data);
                if (status === "success") {
                    if (callback) {
                        callback(data);
                    }
                }
            }
        );
    };
    getData(cid: string, seq: number, callback: Function) {
        // var data = new Array();
        // if (seq == 0) {

        //     var m1 = new Message();
        //     m1.Content = "hello justin";
        //     m1.SendDate = "2018-08-22 16:28:20";
        //     m1.Sender = "Justin";
        //     m1.Type = 0;
        //     m1.Seq = 1;
        //     data.push(m1);

        //     var m2 = new Message();
        //     m2.Content = "hello ella";
        //     m2.SendDate = "2018-08-22 16:30:20";
        //     m2.Sender = "STP ADMIN";
        //     m2.Type = 1;
        //     m2.Seq = 2;
        //     data.push(m2);
        // } else {
        //     var m2 = new Message();
        //     m2.Content = "good bye";
        //     m2.SendDate = "2018-08-22 16:40:20";
        //     m2.Sender = "STP ADMIN";
        //     m2.Type = 1;
        //     m2.Seq = 4;
        //     data.push(m2);
        // }
        // return data;
        WSGet(
            this.apiPath + "/CS/GetMessage",
            { chatId: cid, seq: seq },
            (data: any, status: any) => {
                this.log(data);
                if (status === "success") {
                    if (callback) {
                        callback(data);
                    }
                }
            }
        );
    }
    send(cid: string, msg: Message, callback: Function) {
        WSPost(
            this.apiPath + "/CS/Send",
            { Content: msg, ChatId: cid },
            (data: any, status: any) => {
                this.log(data);
                if (status === "success") {
                    if (callback) {
                        callback(data);
                    }
                }
            }
        );
    }

    sendImage(cid: string, bigImage: string, smallImg: string,callback: Function) {
        var content = "<image class='csimage' src='" + smallImg + "'/>";
        WSPost(
            this.apiPath + "/CS/Send",
            { Content: content, ChatId: cid,BigImage:bigImage, SmallImage:smallImg, },
            (data: any, status: any) => {
                this.log(data);
                if (status === "success") {
                    if (callback) {
                        callback(data);
                    }
                }
            }
        );
    }
}

export { CustomerServiceApi, Message }

