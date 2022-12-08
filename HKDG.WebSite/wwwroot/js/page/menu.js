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
            if (n.Type < 0) return;

            let link;
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
                    link = '/CMS/content/' + n.Value.Id;
                    break;
                case 4: // 產品目錄
                    link = '/Product/Category/' + n.Value.Id;
                    break;
            }

            if (link) {
                window.location.href = link;
            }
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