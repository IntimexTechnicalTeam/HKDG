var tempStr = '<transition name="fade">\
<div class="bd-dialog" v-show="show">\
    <div class="dlg-mask"></div>\
    <div class="dlg-content-box">\
        <div class="dlg-body">\
            <slot></slot>\
        </div>\
        <div class="dlg-footer">\
            <button @click="quitDialog" v-if="showcancelbtn">{{canceltext || comStr.Close}}</button>\
            <button @click="confirm" v-if="type===\'sumbit\'">{{confirmtext || comStr.Confirm}}</button>\
        </div>\
    </div>\
</div>\
</transition>';

// 定义一个彈窗组件
var BdDialog = {
    props: {
        type: { // 彈窗類型 (sumbit => 操作提交)
            type: String,
            default: ''
        },
        confirmtext: String, // 确定按钮的文本内容
        canceltext: String, // 取消按钮的文本内容
        showcancelbtn: { // 是否顯示取消按鈕
            type: Boolean,
            default: true
        } 
    },
    data: function () {
        return {
            show: false,
            comStr: comStr
        }
    },
    template: tempStr,
    methods: {
        openDialog: function () {
            document.body.style.overflow = 'hidden';
            this.show = true;
        },
        confirm: function () {
            this.$emit('confirm');
        },
        quitDialog: function () {
            document.body.style.overflow = 'auto';
            this.show = false;
        }
    },
    created() {
        
    }
}