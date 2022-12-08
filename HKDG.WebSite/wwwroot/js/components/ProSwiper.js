ajaxPage( "ProItem", "/js/components/ProItem.js" );  

var tempStr = '<div class="swiper proSwiper scrollbar" :class="declass" v-if="data.length">\
    <div class="swiper-wrapper">\
        <div class="swiper-slide" v-for="item in data">\
            <pro-item :data="item"></pro-item>\
        </div>\
    </div>\
    <div class="swiper-scrollbar"></div>\
</div>';

// 定义一个產品輪播组件
var ProSwiper = {
    components: {
        'pro-item': ProItem
    },
    props: {
        declass: String, // 自定义类名,區分多個Swiper
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
        data: { // 產品數據
            type: Array,
            default: () => {
                return [];
            }
        }
    },
    data: function () {
        return {

        }
    },
    template: tempStr,
    methods: {
        initProSwiper: function () {
            let domClass = this.declass ? '.proSwiper' + '.' + this.declass : '.proSwiper';
            var swiper = new Swiper(domClass, this.options);
        }
    },
    mounted() {
        this.$nextTick(() => {
            this.initProSwiper();
        })
    }
}