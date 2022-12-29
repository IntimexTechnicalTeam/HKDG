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
        CookiesTips: jsMessage.CookiesTips,
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
		        addtocartS(jsMessage.EnterEmailForSubscribe, '/imgs/warn-icon.png');
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
                        addtocartS(jsMessage.ThanksForSubscribe, '/imgs/tick-icon.png');
                        localStorage.setItem("subscribeEmail", '');
                    }
                    else {
                        addtocartS(jsMessage.SubscribeFail, '/imgs/warn-icon.png');
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
            if (n.Type < 0) return;

            let link;
            let flag;
            switch (n.Type) {
                case 0: // 鏈接 => 0
                    if (!n.IsNewWin && n.Url) {
                        window.location.href = n.Url;
                    } else if (n.IsNewWin && n.Url) {
                        window.open(n.Url);
                    }
                    break;
                case 1: // cms目錄
                    link = '/product/CatProduct?catId=' + n.Value.Id;
                    break;
                case 2: // cms內容
                    // link = '/CMS/content/' + n.Value.Id;
                    transitBD('/CMS/content/' + n.Value.Id);
                    flag = true;
                    break;
                case 4: // 產品目錄
                    link = '/product/Category?catId=' + n.Value.Id;
                    break;
            }

            if (link && !flag) {
                window.location.href = link;
            }
        }
	},
	created() {
	},
	mounted() {
		console.log(menu,'menu')
	}
}).mount('#footer')