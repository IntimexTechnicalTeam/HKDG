ajaxPage( "ProSwiper", "/js/components/ProSwiper.js" );  

var tempStr = '<div class="pmt-one" :class="declass">\
    <div class="util-title">\
        {{promotion.Name}}\
        <a href="javascript:;" class="more-btn">更多 ></a>\
    </div>\
    <pro-swiper :declass="declass" :data="promotion.PrmtProductList" :options="options"></pro-swiper>\
</div>';

// 定义一个推廣產品輪播组件
var pmtSwiper = {
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
        }
    },
    data: function () {
        return {

        }
    },
    template: tempStr,
    methods: {

    },
    mounted() {
        
    }
}