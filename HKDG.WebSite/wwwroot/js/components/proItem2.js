var tempStr = '<div class="pro-item layout2">\
    <a v-bind:href="\'/product/detail/\' + data.Code" v-bind:title="data.Name">\
        <div class="img-box">\
            <img v-bind:src="data.Imgs[2] || \'\'" v-bind:title="data.Name" v-bind:alt="data.Name" />\
        </div>\
        <div class="pro-info">\
            <p class="pro-name">\
                <template v-if="!data.IsEventProduct">\
                    <span class="hot" v-if="data.IconType == 1">{{comStr.HotSale}}</span>\
                    <span class="new" v-else-if="data.IconType == 0">{{comStr.NewLaunch}}</span>\
                    <span class="discount" v-else-if="data.OriginalPrice != data.SalePrice">{{comStr.Discounted}}</span>\
                </template>\
                <template v-else>\
                    <span>\
                        {{data.EventCodes[0]}}\
                    </span>\
                </template>\
                {{data.Name}}\
            </p>\
            <div class="pro-store" v-bind:title="data.MerchantName">\
                <img src="/imgs/icons/store.png">\
                <span>{{data.MerchantName}}</span>\
            </div>\
            <div class="pro-price">\
            	<p class="old-price" v-if="data.HasDiscount">{{data.Currency.Code}} {{data.OriginalPrice}}</p>\
                <p class="now-price">{{data.Currency.Code}} {{data.SalePrice}}</p>\
            </div>\
            <span class="fav-btn"></span>\
        </div>\
    </a>\
</div>';

// 定义一个產品组件2
var ProItem2 = {
    props: ['data'],
    data: function () {
        return {
            IspType: IspType,
            comStr: comStr
        }
    },
    template: tempStr,
    methods: {

    }
}