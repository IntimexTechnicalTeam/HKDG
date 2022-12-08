ajaxPage( "ProItem2", "/js/components/ProItem2.js" );  

var tempStr = '<div class="pro-list" :class="declass">\
    <pro-item v-for="item in data" :data="item"></pro-item>\
</div>';

// 定义一个產品列表组件
var ProList2 = {
    components: {
        'pro-item': ProItem2
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