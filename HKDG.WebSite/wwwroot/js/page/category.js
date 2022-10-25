createApp({
  data() {
  		return {
  			// lang: getQueryString("lang") || getUILanguage(), // 當前語言值
  			lang: '',
  			langList: [{    // 系統語言列表
                lang: '繁體',
                value: 'C'
            }, {
                lang: '简体',
                value: 'S'
            }, {
                lang: 'English',
                value: 'E'
            }],
            currencyList: [   // 貨幣列表
                {Code: 'HKD', id: 1},          
                {Code: 'USD', id: 2},
                {Code: 'RMB', id: 3}     
            ],
  		}
	},
	methods: {
		// 確認系統語言/貨幣設置
		confirm: function () {
			this.quitDialog();
		},
		openDialog: function () {
			$('body').css({ 'overflow': 'hidden' });
            $('.bd-dialog').fadeIn();
		},
		quitDialog: function () {
			$('body').css({ 'overflow': 'auto' });
            $('.bd-dialog').fadeOut();
		}
	},
	created() {
	},
	mounted() {

	}
}).mount('#container')