createApp({
  components: {
    'star-rating': StarRating
  },
  data() {
  		return {
        banner: [{
          src: 'https://img0.baidu.com/it/u=2564065540,1877968420&fm=253&fmt=auto&app=138&f=JPEG?w=500&h=313'
        }, {
          src: 'https://img1.baidu.com/it/u=1740954870,1509955191&fm=253&fmt=auto&app=138&f=JPEG?w=800&h=500'
        }, {
          src: 'https://img2.baidu.com/it/u=72117843,339688063&fm=253&fmt=auto&app=138&f=JPEG?w=751&h=500'
        }],
        rating: 3,
        buyQty: 1
  		}
	},
	methods: {
    initPreview: function () {
      var swiper = new Swiper(".proSwiper", {
          navigation: {
            nextEl: '.swiper-button-next',
            prevEl: '.swiper-button-prev',
          },
            pagination: {
              el: '.swiper-pagination',
              type: 'fraction'
            },
        });
    }
	},
	created() {
	},
	mounted() {
    this.initPreview();
	}
}).mount('#container')