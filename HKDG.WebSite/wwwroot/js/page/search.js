createApp({
  components: {
    'pro-list': ProList
  },
  data() {
  		return {
        isMobile: platform === 'D' ? false : true,
        isGrid: true, // 產品佈局類型 (grid => true, block => false)
        proData: [],
        totalRecord: 0,  // 搜索產品總數量
        totalPage: 1, // 搜索產品總頁數
        pager: { // 分頁數據
          Key: getQueryString('key') || '',
          Page: 1,
          PageSize: 2,
          SortName:'UpdateDateString',
          SortOrder:'DESC'
        }
  		}
	},
	methods: {
    // 搜索結果
    search: function(value, dropload) {
      if (value)  this.pager.Page = value;

      var _this = this;
      InstoreSdk.api.product.search(this.pager, function(result) {
        _this.proData = result.Data;
        _this.totalPage = result.TotalPage;
        _this.totalRecord = result.TotalRecord;

        if (dropload) {
          _this.$nextTick(() => {
            dropload.resetload();
          });
        }
      });
    },
	},
	created() {
    this.search();
	},
	mounted() {
    
	}
}).mount('#container')