createApp({
	data() {
	  return {
	  	nowYear: new Date().getFullYear(),
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
	},
	created() {
	},
	mounted() {

	}
}).mount('#footer')