ajaxPage( "ProSwiper", "/js/components/ProSwiper.js" );  

var tempStr = '<div class="pmt-one" :class="declass">\
    <div class="util-title">\
        {{promotion.Name}}\
        <a :href="link || \'/Product/Category\'" class="more-btn" v-if="more">{{comStr.More}} ></a>\
    </div>\
    <pro-swiper :declass="declass" :data="promotion.PrmtProductList" :options="options"></pro-swiper>\
</div>';

// 定义一个推廣產品輪播组件
var PmtSwiper = {
    components: {
        'pro-swiper': ProSwiper
    },
    props: {
        declass: String, // 自定义类名
        options: Object, // swiper設置參數
        promotion: { //推廣數據
            type: Object,
            default: () => {
                return {};
            }
        },
        more: Boolean,  // 设置是否顯示更多按鈕
        link: String // 設置更多按鈕的跳轉鏈接
    },
    data: function () {
        return {
            IspType: IspType,
            comStr: comStr
        }
    },
    template: tempStr,
    methods: {

    },
    created() {
        
    },
    mounted() {

    }
}