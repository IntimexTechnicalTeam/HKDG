ajaxPage('SysSwitch', '/js/components/SysSwitch.js');

createApp({
    components: {
        'sys-switch': SysSwitch
    },
	data() {
	  return {
	    show: true, // 控制公告是否顯示
	    sysAnnounce: LatesNotice.SystemNotice,
        menu: menu.HeaderMenus, // Header Menu
        category: category,
        isSubCat: false, // 子菜單是否為產品全目錄
	    searchType: 0, // 搜索類型
	    searchKey: getQueryString('key') || '',   // 搜索關鍵詞
	    IspType: IspType,
	    logined: localStorage.getItem("logined") === '1' ? true : false,
	    totalNum: 0 // 購物車商品數量
	  }
	},
	methods: {
		// 初始化系統公告
		initAnnouncement: function () {
		    var announcementSwiper = new Swiper('#announcement', {
		        direction: 'vertical',
				// loop: true,
		        autoplay: {
		            delay: 3000,
		            disableOnInteraction: false,
		        }
		    })
		},
		// 關閉系統公告
	    closeAnnounce: function () {
	    	console.log('close');
	        $.cookie('closeSAC', true, { expires: 1 });
	        this.show = false;
	    },
	    // 跳轉首頁，清除搜索詞信息
        goHome: function(){
            window.location.href = "/";
        },
        // 產品/商家 搜索
        goSearch: function () {
        	if (!this.searchKey) return;

            let searchUrl = this.searchType ? '/merchant/list' : '/product/search';

            window.location.href = searchUrl + "?key=" + this.searchKey;
            
            if(typeof(fbpSearch) === "function"){
                this.$nextTick(fbpSearch);
            }
        },
        // 獲取購物車信息
        loadItems: function () {
            let _this = this;
            InstoreSdk.api.shoppingCart.getShopCartAsync(function (result) {
                if (result.Succeeded) {
                    _this.totalNum = result.ReturnValue ? result.ReturnValue.TotalQty : 0;
                } else {
                    addtocartS(result.Message, '/imgs/warn-icon.png');
                }
            });
        },
        // 跳轉購物車
        pushCart: function () {
            if (this.logined) {
                transitBD('/account/shoppingcart');
            } else {
                location.href = '/Account/Login';
            }
        },
        // 加盟
        joinIn: function() {
            window.open('https://hkdg.buydong.hk/');
        },
        // 導航跳轉邏輯處理
        toUrl: function (item) {
            toUrl(item);
        },
        // 獲取菜單子目錄數據
        checkSub: function (item) {
            if (item.Type === 4 && guidEmpty(item.Value.Id)) {
                this.isSubCat = true;
            } else {
                this.isSubCat = false;
            }
        }
	},
	created() {
		// 手機版處理Function
		if (platform === 'M') sizeChange();

	    if ($.cookie('closeSAC')) this.show = false;

	    if (this.logined) this.loadItems();

	    window['loadItems'] = this.loadItems;
	},
	mounted() {
		let _this = this;
		this.$nextTick(function () {
            _this.initAnnouncement();

            if (platform === 'M') FixedHeader();
        });
	}
}).mount('#header')

//公共初始化函数
$(function () {
    var link = "https://connect.facebook.net/zh_CN/sdk.js";
    if (getUILanguage() === "E") {
        link = "https://connect.facebook.net/en_US/sdk.js";
    } else if (getUILanguage() === "S") {
        link = "https://connect.facebook.net/zh_CN/sdk.js";
    } else if (getUILanguage() === "C") {
        link = "https://connect.facebook.net/zh_HK/sdk.js";
    }
    else {
        link = "https://connect.facebook.net/en_US/sdk.js";
    }
    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) { return; }
        js = d.createElement(s); js.id = id;
        js.src = link;
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));
})
