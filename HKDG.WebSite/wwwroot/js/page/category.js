createApp({
  components: {
    'sys-switch': SysSwitch
  },
  data() {
		return {
			category: category
		}
	},
	methods: {
	    // 返回
	    backPage: function() {
	      location.href="/default/menu";
	    },
	    // 跳轉目錄產品頁
	    pushPage: function(item) {
	    	InstoreSdk.api.member.setUILanguage('C', function(data) {
                if (data.Succeeded) {

                    var href = window.location.href;
                    if (href.indexOf("?") === -1) {
                        if (href.indexOf("?lang=") !== -1 || href.indexOf("&lang=") !== -1) {
                            href = setUrlParam(href, ["lang"], [e]);
                        } else {
                            href += "?lang=" + e;
                        }

                    } else {
                        if (href.indexOf("?lang=") !== -1 || href.indexOf("&lang=") !== -1) {
                            href = setUrlParam(href, ["lang"], [e]);
                        } else {
                            href += "&lang=" + e;
                        }
                    }
                    window.location.href = href;
                } else {
                    console.log(data);
                }
            });
	    }
	},
	created() {
	},
	mounted() {

	}
}).mount('#container')