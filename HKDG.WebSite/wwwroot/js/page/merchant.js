createApp({
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
        }]
  		}
	},
	methods: {
		
	},
	created() {
	},
	mounted() {

	}
}).mount('#container')