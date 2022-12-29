createApp({
  components: {
    // 'pro-item': ProItem,
    'pmt-swiper': PmtSwiper,
    'pmt-layout': PmtLayout
  },
  data() {
	  return {
	  	banner: null,
	  	promotionList: [],
	  	category: category, // 商品分類
	  	reOption: {
            autoHeight: true, //高度随内容变化
            slidesPerView : IsMobile ? 2.2 : 4,  
            spaceBetween: 10,
            scrollbar: {
				el: '.swiper-scrollbar',
			}
        },
	  	ftOption: {
            slidesPerView: IsMobile ? 2.2 : 4,
	        grid: {
	          rows: 2,
	        },
	        spaceBetween: 10,
            scrollbar: {
				el: '.swiper-scrollbar',
			}
        }
	  }
	},
	methods: {
		initBanner: function () {
			var swiper = new Swiper(".bannerSwiper", {
		        spaceBetween: 10,
		        slidesPerView: IsMobile ? 1 : 3,
		        loop: true,
		        loopAdditionalSlides: 2,
		        autoplay: {
		        	delay: 3000,
			    	pauseOnMouseEnter: true,
			  	},
		        navigation: {
				    nextEl: '.swiper-button-next',
				    prevEl: '.swiper-button-prev',
				},
		        pagination: {
		          el: ".swiper-pagination",
		          clickable: true,
		        }
		    });
		},
		initCatSwiper: function () {
			var swiper = new Swiper(".category .proSwiper", {
		        slidesPerView : IsMobile ? 9 : 12,  
                spaceBetween: IsMobile ? 10 : 20,
                scrollbar: IsMobile ? false : {
                    el: '.swiper-scrollbar'
                }
		    });
		},
		initBrandSwiper: function () {
			var swiper = new Swiper(".brand .proSwiper", {
				autoHeight: true,
		        slidesPerView :  IsMobile ? 3.5 : 6,  
                spaceBetween: 15,
                scrollbar: IsMobile ? false : {
                    el: '.swiper-scrollbar'
                }
		    });
		}
	},
	created() {
		let index = promotion.findIndex(function(i) {
            return !i.Seq && i.Type === 1;
        });

        this.banner = index === -1 ? {} : promotion.splice(index,1)[0];
        this.promotionList = promotion;
	},
	mounted() {
		this.initBanner();
		this.initCatSwiper();
		this.initBrandSwiper();

		console.log(this.promotionList,'promotionList');
		console.log(category,'category');
	}
}).mount('#container')