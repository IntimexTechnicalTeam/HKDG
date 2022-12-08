ajaxPage( "ProItem", "/js/components/ProItem.js" ); 
ajaxPage( "ProItem2", "/js/components/ProItem2.js" );   

var tempStr = '<div class="pro-list-box">\
    <div class="pro-list" :class="[declass, {\'grid\': grid}]">\
        <pro-item v-for="item in data.Data" :data="item" v-if="grid"></pro-item>\
        <pro-item2 v-for="item in data.Data" :data="item" v-else></pro-item2>\
    </div>\
</div>';

// 定义一个產品列表组件
var ProList1 = {
    components: {
        'pro-item': ProItem,
        'pro-item2': ProItem2
    },
    props: {
        declass: String, // 自定义类名
        data: {    // 產品列表數據
            type: Object,
            default: () => {
                return [];
            }
        },
        grid: {    // 设置產品是否左右排版
            type: Boolean,
            default: true
        } 
    },
    data: function () {
        return {

        }
    },
    template: tempStr,
    methods: {
        loadPage: function() {
            var _this = this;
            var page = 1;
            $('.pro-list-box').dropload({
                scrollArea : window,
                autoLoad:false,
                threshold:200,
                domDown : {
                    domClass   : 'dropload-down',
                    domRefresh : '<div class="dropload-refresh"></div>',
                    domLoad    : '<div class="dropload-load"><span class="loading"></span></div>',
                    domNoData  : '<div class="dropload-noData"></div>'
                },
                loadDownFn : function(me){
                    page++;
                    // _this.pager.Page = page;

                    console.log(page,'page');
                    // InstoreSdk.api.product.search(_this.pager, function(result) {
                    //     if(result.Data.length){
                    //         for(var i =0;i < result.Data.length;i++){
                    //             _this.result.Data.push(result.Data[i]);
                    //             _this.result.Data[i].OriginalPrice = _this.priceFormat(_this.result.Data[i].OriginalPrice.toString(),2);
                    //             _this.result.Data[i].SalePrice = _this.priceFormat(_this.result.Data[i].SalePrice.toString(),2);
                    //             _this.result.Data[i].OriginalPrice2 = _this.priceFormat(_this.result.Data[i].OriginalPrice2.toString(),2);
                    //             _this.result.Data[i].SalePrice2 = _this.priceFormat(_this.result.Data[i].SalePrice2.toString(),2);
                                
                    //         }
                    //     }
                    //     else{
                    //         // 锁定
                    //         me.lock();
                    //         // 无数据
                    //         me.noData();   
                    //     }
                    //     me.resetload();
                    // })
                }
            });
        }
    },
    mounted() {
        this.loadPage();
        console.log(this.grid,'grid')
    }
}