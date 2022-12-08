createApp({
	data() {
	  return {
	  	BuyDong: BuyDong,
	    show: true, // 控制公告是否顯示
	    sysAnnounce: LatesNotice.SystemNotice,
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
				loop: true,
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
                    _this.totalNum = result.ReturnValue.TotalQty;
                } else {
                    addtocartS(result.Message, '/imgs/warn-icon.png');
                }
            });
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