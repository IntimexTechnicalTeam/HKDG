ajaxPage( "ProItem", "/js/components/ProItem.js" ); 
ajaxPage( "ProItem2", "/js/components/ProItem2.js" );   

var tempStr = '<div class="pro-list-box">\
    <div class="pro-list" :class="[declass, {\'grid\': grid}]">\
        <pro-item v-for="item in data" :data="item" v-if="grid"></pro-item>\
        <pro-item2 v-for="item in data" :data="item" v-else></pro-item2>\
    </div>\
</div>';

// 定义一个產品列表组件
var ProList = {
    components: {
        'pro-item': ProItem,
        'pro-item2': ProItem2
    },
    props: {
        declass: String, // 自定义类名
        data: {    // 產品列表數據
            type: Array,
            default: () => {
                return [];
            }
        },
        totalpage: {    // 分頁總頁數
            type: Number,
            default: 1
        },
        grid: {    // 设置產品是否左右排版
            type: Boolean,
            default: true
        } 
    },
    data: function () {
        return {
            page: 1
        }
    },
    template: tempStr,
    methods: {
        loadPage: function() {
            var _this = this;
            var page = 1;
            $('.pro-list-box').dropload({
                scrollArea : window,
                autoLoad: false,
                domDown : {
                    domClass   : 'dropload-down',
                    domRefresh : '<div class="dropload-refresh"></div>',
                    domLoad    : '<div class="dropload-load"><span class="loading"></span></div>',
                    domNoData  : '<div class="dropload-noData">我是有底線的~</div>'
                },
                loadDownFn: function(me){
                    page++;

                    if (page > _this.totalpage) {
                        // 锁定
                        me.lock();
                        // 无数据
                        me.noData();
                    } else {
                        _this.$emit('load', page);
                    }

                    me.resetload();
                }
            });
        }
    },
    mounted() {
        // this.loadPage();

        let _this = this;
        this.$nextTick(() => {
            loadPage($('.pro-list-box'), _this.totalpage, (val,me) => _this.$emit('load', val, me));
        });
    }
}