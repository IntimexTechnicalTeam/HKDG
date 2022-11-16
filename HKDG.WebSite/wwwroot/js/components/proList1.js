ajaxPage( "proItem", "/js/components/proItem.js" );  

var tempStr = '<div class="pro-list grid" :class="declass">\
    <pro-item v-for="item in data" :data="item"></pro-item>\
</div>';

// 定义一个產品列表组件
var ProList1 = {
    components: {
        'pro-item': ProItem
    },
    props: {
        declass: String, // 自定义类名
        data: {    //產品列表數據
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

    },
    mounted() {

    }
}