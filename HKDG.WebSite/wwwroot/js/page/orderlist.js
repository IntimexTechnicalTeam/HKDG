createApp({
	components: {
		'star-rating': StarRating
	},
  	data() {
  		return {
  			// 0 => 待付款, 1 => 處理中（已確認付款）, 4 => 已完成, 5 => 已取消
  			status: 0,
  			rating: 3
  		}
	},
	methods: {
		
	},
	created() {
	},
	mounted() {

	}
}).mount('#container')