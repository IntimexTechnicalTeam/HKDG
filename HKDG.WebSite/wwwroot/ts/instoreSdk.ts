import { ProductApi } from "./productApi"
import { PromotionApi } from "./promotionApi"
import { CustomerServiceApi } from "./customerServiceApi"
import { DeliveryApi } from "./deliveryApi"
import { MemberApi } from "./memberApi"
import { ShoppingCartApi } from "./ShoppingCartApi"
import { CMSApi } from "./cmsApi"
import { OrderApi } from "./orderApi"
import { PaymentApi } from "./paymentApi"
import { MessageApi } from "./messageApi"
import { ProdCommentApi } from "./prodcommentApi"
import { UploadFileApi } from "./uploadfileApi"
import { EnMessage } from "./lang/en";
import { HKMessage } from "./lang/zh-HK";
import { CNMessage } from "./lang/zh-CN";
import { MerchantApi } from "./merchantApi";
import { ReportApi } from "./reportApi";
import { CouponApi } from "./couponApi";
import { MenuApi } from "./menuApi";
import {AttributeApi} from "./attributeApi";
import { CacheApi } from "./cacheApi";
import { AdvertisingApi } from "./advertisingApi";

export class Api {
    product: ProductApi;
    promotion: PromotionApi;
    cs: CustomerServiceApi;
    delivery: DeliveryApi;
    member: MemberApi;
    shoppingCart: ShoppingCartApi;
    cms: CMSApi;
    order: OrderApi;
    payment: PaymentApi;
    message: MessageApi;
    prodcomment: ProdCommentApi;
    uploadfile: UploadFileApi;
    merchant: MerchantApi;
    report: ReportApi;
    coupon: CouponApi;
    menu: MenuApi;
    attribute:AttributeApi;
    cache: CacheApi;
    advertising: AdvertisingApi;

    constructor() {
        this.product = new ProductApi();
        this.promotion = new PromotionApi();
        this.cs = new CustomerServiceApi();
        this.member = new MemberApi();
        this.merchant = new MerchantApi();
        this.delivery = new DeliveryApi();
        this.shoppingCart = new ShoppingCartApi();
        this.cms = new CMSApi();
        this.order = new OrderApi();
        this.payment = new PaymentApi();
        this.message = new MessageApi();
        this.prodcomment = new ProdCommentApi();
        this.uploadfile = new UploadFileApi();
        this.report = new ReportApi();
        this.coupon = new CouponApi();
        this.menu = new MenuApi();
        this.attribute = new AttributeApi();
        this.cache = new CacheApi();
        this.advertising = new AdvertisingApi();
    }
}


class InstoreSdk {
    ver: string;
    api: Api;
    message: Message;

    constructor() {
        this.ver = "2.0";
        this.api = new Api();

        this.message = this.createMessage();
        console.log("InstoreSdk mall inited");
    };
    isLogin(): boolean {
        var logined = localStorage.getItem("logined");
        return logined == "1";
    };

    private createMessage(): Message {
        var lang = getCookie("uLanguage", "/");
        switch (lang) {
            case "E":
                return new EnMessage();
            case "C":
                return new HKMessage();
            case "S":
                return new CNMessage();
            default:
                return new EnMessage();
        }

    }
}


export { InstoreSdk }