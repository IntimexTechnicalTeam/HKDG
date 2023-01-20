createApp({
  components: {
    'star-rating': StarRating,
    'pro-list': ProList,
    'bd-dialog': BdDialog
  },
  data() {
  		return {
  			proData: [{
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
        isGrid: true, // 產品佈局類型 (grid => true, block => false)
        merchantData: MerchantDetail, // 商家數據
        merchantId: MerchantDetail.MerchantId, // 商家Id
        merProData: MerchantProdData.Data, // 商家產品數據
        totalRecord: MerchantProdData.TotalRecord || 0,  // 當前商家產品總數量
        totalPage: MerchantProdData.TotalPage || 1, // 當前商家產品總頁數
        termsDtls: '', // 當前查閱條款內容
        sortType: [{  // 排序類型數據
          name: jsMessage.NewArrival,
          value: 'New'
        }, {
          name: jsMessage.CLowPrice,
          value: 'HighPrice'
        }, {
          name: jsMessage.CHighPrice,
          value: 'LowPrice'
        }, {
          name: jsMessage.BestSeller,
          value: 'HotSale'
        }],
        pager: { // 分頁數據
          Page: 1,
          PageSize: 10,
          MerchantId: MerchantDetail.MerchantId,
          Key: '',
          OrderBy: 'New' // 排序
        }
  		}
	},
	methods: {
    // 關注商店
    addFavMerchant: function () {
      let _this = this;
      if (this.merchantData.IsFavorite) {
        InstoreSdk.api.member.removeFavMerchant(this.merchantData.Code, function (result) {
            _this.merchantData.IsFavorite = false;
            addtocartS(result.Message, '/imgs/icons/heart.png')
        });
      } else {
        InstoreSdk.api.member.addFavMerchant(this.merchantData.Code,
            function (result) {
                _this.merchantData.IsFavorite = true;
                addtocartS(result.Message, '/imgs/icons/heart2.png')
            },
            function () {
               showLoginDialog(window.location.href);
            });
      }
    },
    // 獲取商家產品
    getMerchantPro: function (value,dropload) {
      let _this = this;
      if (value)  this.pager.Page = value;
      
      InstoreSdk.api.merchant.getProducts(this.pager, function (result) {
        console.log(result,'獲取商家產品');

        if (result.Succeeded) {
          _this.merProData = _this.merProData.concat(result.ReturnValue.Data);
          _this.totalPage = result.ReturnValue.TotalPage;
          _this.totalRecord = result.ReturnValue.TotalRecord;

          if (dropload) {
            _this.$nextTick(() => {
              dropload.resetload();
            });
          }
        } else {
          addtocartS(data.Message, '/imgs/warn-icon.png');
        }
      });
    },
		// 閱讀相關條款 type 類型 (退貨及退款條款 -> 1, 運送及運費條款 -> 2)
    readTerms: function (type) {
      this.termsDtls = type === 1 ? this.merchantData.TandC : this.merchantData.ReturnTerms;
      this.$refs.terms.openDialog();
    },
    // 更改查詢條件
    changeQuery: function () {
      this.merProData = [];
      this.pager.Page = 1;
      this.totalPage = 1;
      this.totalRecord = 0;

      this.getMerchantPro();
    },
    // 聯繫商家
    contactMerchant: function () {
      location.href = 'mailto:'+ this.merchantData.ContactEmail;
    }
	},
	created() {
	},
	mounted() {
    console.log(MerchantDetail,MerchantProdData);
	}
}).mount('#container')