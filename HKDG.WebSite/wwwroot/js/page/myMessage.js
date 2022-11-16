createApp({
  data() {
  		return {

  		}
	},
	methods: {
    // 查看消息詳情
    viewDtls:function (e) {
      if ($(e.target).closest('.msg-item').hasClass('open')) {
        $(e.target).closest('.msg-item').removeClass('open');
      } else {
        $(e.target).closest('.msg-item').addClass('open');
      }

      $(e.target).closest('.msg-item').find('.content').slideToggle();
    }
	},
	created() {
	},
	mounted() {

	}
}).mount('#container')