createApp({
	data() {
	  return {
	  	nowYear: new Date().getFullYear(),
	  	email: '', // 用戶郵箱
	  	// 網站追蹤列表
        traceList: [{
            logo: '/imgs/icons/fb.png',
            link: 'https://www.facebook.com/BuyDong.hk/'
        }, {
            logo: '/imgs/icons/ytb.png',
            link: 'https://www.youtube.com/channel/UC-59RIcYf9q3dhrcmrbvKMw/featured'
        }, {
            logo: '/imgs/icons/ins.png',
            link: 'https://www.instagram.com/buydong.hk/'
        }]
	  }
	},
	methods: {
		subscribe: function() {	// 訂閱最新消息
			console.log('訂閱最新消息');
		}
	},
	created() {
	},
	mounted() {

	}
}).mount('#footer')