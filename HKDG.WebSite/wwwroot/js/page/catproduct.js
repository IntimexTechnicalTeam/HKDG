createApp({
  components: {
    'pro-list': ProList
  },
  data() {
  		return {
        isMobile: platform === 'D' ? false : true,
        isGrid: true, // 產品佈局類型 (grid => true, block => false)
        catId: catId, // 當前目錄Id
        catalog: catalog, // 當前目錄數據
        catProducts: catProducts.Data || [], // 當前目錄產品數據
        totalRecord: catProducts.TotalRecord || 0,  // 當前目錄產品總數量
        totalPage: catProducts.TotalPage || 1, // 當前目錄產品總頁數
        // 查詢條件
        subCatId: '', // 商品分類
        sort: '', // 排序
        // -------
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
          page: 1,
          pageSize: 10
        }
  		}
	},
	methods: {
    // 獲取對應目錄產品數據
    getCatProduct: function (value,dropload) {
      let _this = this;
      if (value)  this.pager.page = value;

      let param = {
        CatId: this.subCatId || this.catId,
        Page: this.pager.page,
        PageSize: this.pager.pageSize,
        OrderBy: this.sort || 'New'
      };

      InstoreSdk.api.product.getCatProduct(param, function (result) {
        _this.catProducts = _this.catProducts.concat(result.Data);
        _this.totalPage = result.TotalPage;
        _this.totalRecord = result.TotalRecord;

        if (dropload) {
          _this.$nextTick(() => {
            dropload.resetload();
          });
        }
      });
    },
    // 更改查詢條件
    changeQuery: function () {
      this.catProducts = [];
      this.pager = {
        page: 1,
        pageSize: 10
      };
      this.totalPage = 1;
      this.totalRecord = 0;

      this.getCatProduct();
    }
	},
	created() {

	},
	mounted() {
    console.log(this.catId,'catId');
    console.log(this.catalog,'catalog');
    console.log(this.catProducts,'catProducts');
	}
}).mount('#container')