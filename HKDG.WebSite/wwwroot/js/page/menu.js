createApp({
  components: {
    'sys-switch': SysSwitch
  },
  data() {
  		return {
        
  		}
	},
	methods: {
		// 跳轉產品目錄頁
	    pushPage: function() {
	    	location.href = "/product/category?IspType=BD";
	    }
	},
	created() {
	},
	mounted() {

	}
}).mount('#container')