var tempStr = '<div class="nav-tab">\
  <a :href="item.link" v-for="item in navList" :class="{\'active\': pathname.toLowerCase() === item.link.toLowerCase()}">{{item.text}}</a>\
</div>';

// 定义一个Nav Tab组件
var NavTab = {
    data: function () {
        return {
            IspType: IspType,
            pathname: '',
            navList: [{
                text: comStr.PersonalInfo,
                link: '/Account/MemberInfo'
            }, {
                text: comStr.Coupon,
                link: '/Account/MyCoupon'
            }, {
                text: comStr.Message,
                link: '/Account/MyMessage'
            }, {
                text: comStr.MyOrder,
                link: '/Order/List'
            }, {
                text: comStr.MyFavorite,
                link: '/Account/MyFavorite'
            }]
        }
    },
    template: tempStr,
    methods: {

    },
    created() {
        this.pathname = location.pathname;
    }
}