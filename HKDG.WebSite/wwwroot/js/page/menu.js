createApp({
  components: {
    'sys-switch': SysSwitch
  },
  data() {
  		return {
        	menu: menu.HeaderMenus, // Header Menu
  		}
	},
	methods: {
		// 導航跳轉邏輯處理
        toUrl: function (n) {
            toUrl(n);
        },
		// 跳轉產品目錄頁
	    pushPage: function() {
	    	location.href = "/product/category";
	    }
	},
	created() {
	},
	mounted() {
		console.log(this.menu, '菜單數據');
	}
}).mount('#container')