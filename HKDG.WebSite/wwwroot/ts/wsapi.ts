

class WSAPI {
    debug: boolean = true;
    apiHost: string = getPMHost();//$.cookie("PMServer");
    apiPath: string = this.apiHost + "/API";
    //controller: Array<object>;


    log(arg1: any, arg2?: any) {
        if (this.debug) {
            console.log(arg1);
            if (arg2 != undefined) {
                console.log(arg2);
            }
        }
    };
}
export   {WSAPI}

