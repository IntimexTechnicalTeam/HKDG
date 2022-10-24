import { WSAPI } from "./wsapi"

export class MessageApi extends WSAPI {

    //獲取未讀消息數量
    getUnreadMsgQty(callback: Function) {
        var _this = this;
        WSGet(this.apiPath + "/Message/GetMbrUnreadMsgQty", null,
            function (data: any, status: any) {
                _this.log(data);
                if (callback) {
                    callback(data);
                }
            });
    }

    //獲取未讀消息列表
    getUnreadMsgList(pager: object, callback: Function) {
        WSPost(this.apiPath + "/Message/GetMemberUnreadMsg",
            pager,
            (data: any, status: any) => {
                this.log(data);
                if (callback) {
                    callback(data);
                }
            }, function () { });
    }

    //獲取全部消息列表
    getMessage(pager: object, callback: Function) {
        WSPost(this.apiPath + "/Message/getMessage",
            pager,
            (data: any, status: any) => {
                this.log(data);
                if (callback) {
                    callback(data);
                }
            }, function () { });
    }

    //回復消息（需傳遞被回復記錄的ID）
    replyMsg(message: object, callback: Function) {
        WSPost(this.apiPath + "/Message/MbrReplyMessage",
            message,
            (data: any, status: any) => {
                this.log(data);
                if (callback) {
                    callback(data);
                }
            }, function () { });
    }

    //發送會員的售前咨詢信息
    sendPreSalesMsg(message: object, callback: Function) {
        WSPost(this.apiPath + "/Message/SendMemberPreSalesMsg",
            message,
            (data: any, status: any) => {
                this.log(data);
                if (callback) {
                    callback(data);
                }
            }, function () { });
    }

    //發送會員的售後咨詢信息
    sendAfterSalesMsg(message: object, callback: Function) {
        WSPost(this.apiPath + "/Message/SendMemberAfterSalesMsg",
            message,
            (data: any, status: any) => {
                this.log(data);
                if (callback) {
                    callback(data);
                }
            }, function () { });
    }

    //標記指定的消息為已讀狀態
    markedMessageAsRead(msgIdLst: string, callback: Function) {
        WSGet(this.apiPath + "/Message/MarkedMessageAsRead",
            { msgIdLst: msgIdLst },
            (data: any, status: any) => {
                this.log(data);
                if (callback) {
                    callback(data);
                }
            }, function () { });
    }

    //標記所有消息為已讀狀態
    markedMbrAllMsgAsRead(callback: Function) {
        WSGet(this.apiPath + "/Message/MarkedMbrAllMsgAsRead",
            null,
            (data: any, status: any) => {
                this.log(data);
                if (callback) {
                    callback(data);
                }
            }, function () { });
    }

    //刪除指定的信息
    deleteMsg(msgIdLst: string, callback: Function) {
        WSGet(this.apiPath + "/Message/DeleteMsg",
            { msgIdLst: msgIdLst },
            (data: any, status: any) => {
                this.log(data);
                if (callback) {
                    callback(data);
                }
            }, function () { });
    }

    //刪除所有信息
    deleteAllMsg(callback: Function) {
        WSGet(this.apiPath + "/Message/DeleteAllMsg",
            null,
            (data: any, status: any) => {
                this.log(data);
                if (callback) {
                    callback(data);
                }
            }, function () { });
    }

 

    //新增到貨通知
    addArrivalNotify(cond: object, callback: Function) {
        WSPost(this.apiPath + "/Message/AddArrivalNotify",
            cond,
            (data: any, status: any) => {
                this.log(data);
                if (callback) {
                    callback(data);
                }
            }, function () { });
    }

    //取消到貨通知
    cancelArrivalNotify(cond: object, callback: Function) {
        WSPost(this.apiPath + "/Message/CancelArrivalNotify",
            cond,
            (data: any, status: any) => {
                this.log(data);
                if (callback) {
                    callback(data);
                }
            }, function () { });
    }

    //是否已有到貨通知
    checkExsitArrivalNotify(cond: object, callback: Function) {
        WSPost(this.apiPath + "/Message/CheckExsitArrivalNotify",
            cond,
            (data: any, status: any) => {
                this.log(data);
                if (callback) {
                    callback(data);
                }
            }, function () { });
    }
}