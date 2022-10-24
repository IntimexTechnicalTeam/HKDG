"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.InstoreSdk = exports.Api = void 0;
var productApi_1 = require("./productApi");
var promotionApi_1 = require("./promotionApi");
var customerServiceApi_1 = require("./customerServiceApi");
var deliveryApi_1 = require("./deliveryApi");
var memberApi_1 = require("./memberApi");
var ShoppingCartApi_1 = require("./ShoppingCartApi");
var cmsApi_1 = require("./cmsApi");
var orderApi_1 = require("./orderApi");
var paymentApi_1 = require("./paymentApi");
var messageApi_1 = require("./messageApi");
var prodcommentApi_1 = require("./prodcommentApi");
var uploadfileApi_1 = require("./uploadfileApi");
var en_1 = require("./lang/en");
var zh_HK_1 = require("./lang/zh-HK");
var zh_CN_1 = require("./lang/zh-CN");
var merchantApi_1 = require("./merchantApi");
var reportApi_1 = require("./reportApi");
var couponApi_1 = require("./couponApi");
var menuApi_1 = require("./menuApi");
var attributeApi_1 = require("./attributeApi");
var cacheApi_1 = require("./cacheApi");
var advertisingApi_1 = require("./advertisingApi");
var Api = /** @class */ (function () {
    function Api() {
        this.product = new productApi_1.ProductApi();
        this.promotion = new promotionApi_1.PromotionApi();
        this.cs = new customerServiceApi_1.CustomerServiceApi();
        this.member = new memberApi_1.MemberApi();
        this.merchant = new merchantApi_1.MerchantApi();
        this.delivery = new deliveryApi_1.DeliveryApi();
        this.shoppingCart = new ShoppingCartApi_1.ShoppingCartApi();
        this.cms = new cmsApi_1.CMSApi();
        this.order = new orderApi_1.OrderApi();
        this.payment = new paymentApi_1.PaymentApi();
        this.message = new messageApi_1.MessageApi();
        this.prodcomment = new prodcommentApi_1.ProdCommentApi();
        this.uploadfile = new uploadfileApi_1.UploadFileApi();
        this.report = new reportApi_1.ReportApi();
        this.coupon = new couponApi_1.CouponApi();
        this.menu = new menuApi_1.MenuApi();
        this.attribute = new attributeApi_1.AttributeApi();
        this.cache = new cacheApi_1.CacheApi();
        this.advertising = new advertisingApi_1.AdvertisingApi();
    }
    return Api;
}());
exports.Api = Api;
var InstoreSdk = /** @class */ (function () {
    function InstoreSdk() {
        this.ver = "2.0";
        this.api = new Api();
        this.message = this.createMessage();
        console.log("InstoreSdk mall inited");
    }
    ;
    InstoreSdk.prototype.isLogin = function () {
        var logined = localStorage.getItem("logined");
        return logined == "1";
    };
    ;
    InstoreSdk.prototype.createMessage = function () {
        var lang = getCookie("uLanguage", "/");
        switch (lang) {
            case "E":
                return new en_1.EnMessage();
            case "C":
                return new zh_HK_1.HKMessage();
            case "S":
                return new zh_CN_1.CNMessage();
            default:
                return new en_1.EnMessage();
        }
    };
    return InstoreSdk;
}());
exports.InstoreSdk = InstoreSdk;
//# sourceMappingURL=instoreSdk.js.map