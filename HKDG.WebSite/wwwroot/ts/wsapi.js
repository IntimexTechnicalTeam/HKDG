"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.WSAPI = void 0;
var WSAPI = /** @class */ (function () {
    function WSAPI() {
        this.debug = true;
        this.apiHost = getPMHost(); //$.cookie("PMServer");
        this.apiPath = this.apiHost + "/API";
    }
    //controller: Array<object>;
    WSAPI.prototype.log = function (arg1, arg2) {
        if (this.debug) {
            console.log(arg1);
            if (arg2 != undefined) {
                console.log(arg2);
            }
        }
    };
    ;
    return WSAPI;
}());
exports.WSAPI = WSAPI;
//# sourceMappingURL=wsapi.js.map