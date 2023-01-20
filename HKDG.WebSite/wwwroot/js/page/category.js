createApp({
  components: {
    'sys-switch': SysSwitch
  },
  data() {
		return {
            catId: getQueryString("catId") || '', // 父級目錄Id
			category: category
		}
	},
	methods: {
	    // 返回
	    backPage: function() {
	      location.href="/Default/Menu";
	    },
        // 加盟
        joinIn: function() {
        	window.open('https://hkdg.buydong.hk/');
        },
	    // 跳轉目錄產品頁
	    pushPage: function(item) {
            location.href = '/Product/CatProduct/' + item.Id;
	    }
	},
	created() {
        let _this = this;
        if (this.catId) {
            this.category = this.category.find(function(i) {
                return i.Id === _this.catId;
            }).Children;
        }
	},
	mounted() {
        console.log(this.category,'category');
	}
}).mount('#container')