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
	  	proList: [{
	  		Currency: {
	  			Code: 'HKD'
	  		},
	  		Currency2: {
				Code: 'USD'
	  		},
	  		Code: 'Code',
	  		Name: 'Name',
	  		Imgs: [
	  		'https://img2.baidu.com/it/u=1904742041,1902077588&fm=253&app=138&size=w931&n=0&f=JPEG&fmt=auto?sec=1666458000&t=5b1c7ebe20152288cc841bca0e24d7ca', 
	  		'https://img2.baidu.com/it/u=3936366904,3589428500&fm=253&app=138&size=w931&n=0&f=JPEG&fmt=auto?sec=1666458000&t=a52bc0c2c2e9ef7d17c05f2f0a2e3d5d',
	  		'https://img2.baidu.com/it/u=3936366904,3589428500&fm=253&app=138&size=w931&n=0&f=JPEG&fmt=auto?sec=1666458000&t=a52bc0c2c2e9ef7d17c05f2f0a2e3d5d'
	  		],
	  		MerchantName: 'Traveler’s Art Journal',
	  		OriginalPrice: 1000,
	  		SalePrice: 888,
	  		SalePrice2: 777,
	  		HasDiscount: true,
	  		IsEventProduct: false,
	  		IconType: 0
	  	}],
	  	category: category, // 商品分類
	  	reOption: {
            autoHeight: true, //高度随内容变化
            slidesPerView : 2.2,  
            spaceBetween: 10,
            scrollbar: {
				el: '.swiper-scrollbar',
			}
        },
	  	ftOption: {
            slidesPerView: 2.2,
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
		        slidesPerView : 9,  
                spaceBetween: 10
		    });
		},
		initBrandSwiper: function () {
			var swiper = new Swiper(".brand .proSwiper", {
				autoHeight: true,
		        slidesPerView : 3.5,  
                spaceBetween: 15
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