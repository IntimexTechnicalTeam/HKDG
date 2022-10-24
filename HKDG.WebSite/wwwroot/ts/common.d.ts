

declare function WSGet(url: string, data: any, success: Function, fail?: Function): void;
declare function WSPost(url: string, data: any, success: Function, fail?: Function): void;
declare function WSAjaxSP(obj: Object): void;
declare function WSAjax(url: string, data: any, success: Function, fail?: Function): void;

declare function getPMHost():string;
declare function getCustUILanguage():string; 

declare function showInfo(msg:string):void;
declare function showMessage(msg:string):void;
declare function showSystemError(msg:any):void;

declare function setCookie(key:string,value:string,path:string,expiredays:any):void;
declare function getCookie(key:string,path:string):string;
declare function addTimeStamp(url:string):string;




