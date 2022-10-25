ajaxPage( "proItem", "/js/components/proItem.js" );  

var tempStr = '<div class="pmt-one" :class="declass">\
    <div class="util-title">\
        {{promotion.Name}}\
        <a href="javascript:;" class="more-btn">更多 ></a>\
    </div>\
    <div class="swiper pmtSwiper scrollbar">\
        <div class="swiper-wrapper">\
            <div class="swiper-slide" v-for="item in promotion.PrmtProductList">\
                <pro-item :data="item"></pro-item>\
            </div>\
        </div>\
        <div class="swiper-scrollbar"></div>\
    </div>\
</div>';

// 定义一个推廣產品輪播组件
var pmtSwiper = {
    components: {
        'pro-item': ProItem
    },
    props: {
        declass: String, // Swiper自定义类名
        options: { // swiper設置參數
            type: Object,
            default: {
                autoHeight: true, //高度随内容变化
                slidesPerView : 2.2,  
                spaceBetween: 10,
                scrollbar: {
                    el: '.swiper-scrollbar',
                }
            }
        },
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
        initPmtBanner: function () {
            var swiper = new Swiper("." + this.declass + ' .pmtSwiper', this.options);
        }
    },
    mounted() {
        this.$nextTick(() => {
            this.initPmtBanner();
        })
        
    }
}