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
        }],
        CookiesTips: msgFooter.CookiesTips,
        FooterMenus: menu.FooterMenus
	  }
	},
	methods: {
		// 驗證訂閱
		subVerify: function() {	// 訂閱最新消息
			console.log('訂閱最新消息');

			var reg = /^([a-zA-Z0-9_-]|\.)+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$/;
			let _this = this;

		    if (!this.email.match(reg)) {
		        addtocartS(msgFooter.EnterEmailForSubscribe, '/imgs/warn-icon.png');
		        return;
		    } else {
		        if (localStorage.getItem("logined") == 0) {
		            InstoreSdk.api.member.checkIsRegister(this.email, function (data) {
		                if (data.Succeeded == true) {
		                    if (data.ReturnValue == true) {
		                        localStorage.setItem('subscribeEmail', _this.email);
		                        window.location.href = "/Account/Login";
		                    }
		                    else {
		                        localStorage.setItem('subscribeEmail', _this.email);
		                        window.location.href = "/Account/Login";
		                    }
		                    fbpSubscribe();
		                }
		            });
		        }
		        else {
		            _this.subscribe();
		        }
		    }
		},
		// 訂閱
		subscribe: function () {
            var _this = this;
            InstoreSdk.api.member.subscribe(this.email, true, function (data) {
                if (data.Succeeded == true) {
                    if (data.ReturnValue == true) {
                        addtocartS(msgFooter.ThanksForSubscribe, '/imgs/tick-icon.png');
                        localStorage.setItem("subscribeEmail", '');
                    }
                    else {
                        addtocartS(msgFooter.SubscribeFail, '/imgs/warn-icon.png');
                    }

                }
                else {
                    if (data.ReturnValue == "3002") {
                        window.location.href = "/Account/Login";
                    }
                }
            });
        },
        // 導航跳轉邏輯處理
        toUrl: function (n) {
            toUrl(n);
        }
	},
	created() {
	},
	mounted() {
        
	}
}).mount('#footer')