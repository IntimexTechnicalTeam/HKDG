createApp({
	data() {
	  return {
	    message: 'Hello Vue!',
	    show: true, // 控制公告是否顯示
	    sysAnnounce: [{
	    	Content: '新增順豐速遞服務，方便又快捷！'
	    },{
	    	Content: '好煩呀！'
	    }],
	    searchType: 0, // 搜索類型
	    searchKey: '',   // 搜索關鍵詞
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
	        // $.cookie('closeSAC', true, { expires: 1 });
	        // this.show = false;
	    },
	    // 跳轉首頁，清除搜索詞信息
        goHome: function(){
            window.location.href = "/";
            localStorage.removeItem("searchKeytext");
        },
        // 產品/商家 搜索
        goSearch: function () {
            localStorage.setItem("searchKeytext", this.searchKey);

            let searchUrl = this.searchType ? '/merchant/list' : '/product/search';

            window.location.href = searchUrl + "?key=" + this.searchKey;
            
            if(typeof(fbpSearch) === "function"){
                this.$nextTick(fbpSearch);
            }
        },
	},
	created() {
	    if ($.cookie('closeSAC')) {
	        this.show = false;
	    }
	},
	mounted() {
		let _this = this;
		this.$nextTick(function () {
            _this.initAnnouncement();
        });

		// 手機版處理Function
		if (plat_flag === 'M') {
			sizeChange();
		}
	}
}).mount('#header')