var tempStr = '<div class="nav-tab">\
  <a href="javascript:;" v-for="item in navList" :class="{\'active\': location.pathname === item.link}">{{item.text}}</a>\
</div>';

// 定义一个Nav Tab组件
var NavTab = {
    data: function () {
        return {
            navList: [{
                text: '個人資訊',
                link: ''
            }, {
                text: '優惠券',
                link: ''
            }, {
                text: '訊息',
                link: ''
            }, {
                text: '行程表',
                link: ''
            }, {
                text: '我的訂單',
                link: ''
            }, {
                text: '喜愛清單',
                link: ''
            }]
        }
    },
    template: tempStr,
    methods: {

    }
}