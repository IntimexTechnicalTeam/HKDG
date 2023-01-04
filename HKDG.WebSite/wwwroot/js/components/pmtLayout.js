ajaxPage( "ProItem", "/js/components/ProItem.js" );  

var tempStr = '<div class="pmt-one grid" :class="declass">\
    <div class="util-title">\
        {{promotion.Name}}\
    </div>\
    <div class="pro-list">\
        <pro-item v-for="item in promotion.PrmtProductList.slice(0,limit)" :data="item"></pro-item>\
    </div>\
    <a href="/Product/Category" class="more-btn">{{comStr.SeeMore}}</a>\
</div>';

// 定义一个推廣產品輪播组件
var PmtLayout = {
    components: {
        'pro-item': ProItem
    },
    props: {
        declass: String, // Swiper自定义类名
        limit: {    // 顯示的產品數量
            type: Number,
            default: IsMobile ? 6 : 8
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
            IspType: IspType,
            comStr: comStr
        }
    },
    template: tempStr,
    methods: {

    },
    mounted() {

    }
}