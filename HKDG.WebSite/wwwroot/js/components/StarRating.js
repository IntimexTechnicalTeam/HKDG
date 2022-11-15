var tempStr = '<div class="rating" v-bind:class="{\'disabled\': disabled}">\
    <span v-for="(item,index) in series" v-bind:class="{\'active\': modelValue===5-index}" @click="change(index)">☆</span>\
</div>';

// 定义一个五星評級组件
var StarRating = {
    props: {
        modelValue: Number,
        disabled: Boolean  // 设置 disabled 属性表示组件为只读
    },
    emits: ['update:modelValue'],
    data: function () {
        return {
            series: new Array(5)
        }
    },
    template: tempStr,
    methods: {
        change: function (index) {
            if(!this.disabled) {
                this.$emit('update:modelValue', 5-index);
            }
        }
    },
    mounted() {

    }
}