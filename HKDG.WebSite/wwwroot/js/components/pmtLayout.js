ajaxPage( "proItem", "/js/components/proItem.js" );  

var tempStr = '<div class="pmt-one grid" :class="declass">\
    <div class="util-title">\
        {{promotion.Name}}\
    </div>\
    <div class="pro-list">\
        <pro-item v-for="item in promotion.PrmtProductList.slice(0,limit)" :data="item"></pro-item>\
    </div>\
    <button class="more-btn">看更多</button>\
</div>';

// 定义一个推廣產品輪播组件
var pmtLayout = {
    components: {
        'pro-item': ProItem
    },
    props: {
        declass: String, // Swiper自定义类名
        limit: {    // 顯示的產品數量
            type: Number,
            default: 6
        },
        promotion: {    //推廣數據
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