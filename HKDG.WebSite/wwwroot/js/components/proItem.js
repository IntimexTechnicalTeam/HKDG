var tempStr = '<div class="pro-item">\
    <a v-bind:href="\'/product/detail/\' + data.Code" v-bind:title="data.Name">\
        <div class="img-box">\
            <img v-bind:src="data.Imgs[2] || \'/imgs/default-pro.png\'" v-bind:title="data.Name" v-bind:alt="data.Name" />\
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
                <p class="now-price currency2" v-if="data.Currency2">{{data.Currency2.Code}} {{data.SalePrice2}}</p>\
            </div>\
            <span class="fav-btn" :class="{\'fav\': data.IsFavorite}" @click.prevent="addToFavorite"></span>\
        </div>\
    </a>\
</div>';

// 定义一个產品组件
var ProItem = {
    props: ['data'],
    data: function () {
        return {
            IspType: IspType,
            comStr: comStr
        }
    },
    template: tempStr,
    methods: {
        // 加入/取消收藏
        addToFavorite: function () {
            console.log('addToFavorite');
            let _this = this;
            if (this.data.IsFavorite) {
                InstoreSdk.api.member.removeFavorite(this.data.ProductId, function(result) {
                    _this.data.IsFavorite = false;
                    addtocartS(result.Message, '/imgs/icons/heart.png');
                });
            } else {
              InstoreSdk.api.member.addFavorite(this.data.ProductId, function(result) {
                  _this.data.IsFavorite = true;
                  addtocartS(result.Message, '/imgs/icons/heart2.png');
                  if (typeof(fbpAddToWishlist) === "function") {
                    _this.$nextTick(fbpAddToWishlist);
                  }
              },
              function(data) {
                  if (data.Code == 1000) {
                    window.location.href = "/Account/Login?returnUrl=" + window.location.href;
                  }
              });
            }
        }
    }
}