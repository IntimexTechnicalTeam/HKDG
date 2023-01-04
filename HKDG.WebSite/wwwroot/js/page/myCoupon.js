var testData = {"Succeeded":true,"Message":null,"ReturnValue":{"TotalPage":1,"TotalRecord":6,"Data":[{"MerchantName":"Edtoys 天才教材網店","CouponStatus":3,"IsUsed":false,"UsedDate":null,"Id":"60e8d27a-2dda-4d24-89dc-d7ad99c8f238","Title":"D 30-10","Remark":"D 30-10","MerchantId":"e3220fa6-570b-45a1-9f57-9115cc2eece6","Code":"","IsPercent":false,"CouponType":2,"DiscountRange":30.0,"DiscountValue":10.0,"EffectDateFrom":"2022-10-10 00:00:00","EffectDateTo":"2023-05-26 00:00:00","IsActive":true,"DiscountType":3,"Image":"","Rules":null},{"MerchantName":"Edtoys 天才教材網店","CouponStatus":3,"IsUsed":false,"UsedDate":null,"Id":"f8f2add9-3a5e-4160-9cd1-14d729be937e","Title":"100-30","Remark":"100-30","MerchantId":"e3220fa6-570b-45a1-9f57-9115cc2eece6","Code":"","IsPercent":false,"CouponType":1,"DiscountRange":100.0,"DiscountValue":30.0,"EffectDateFrom":"2022-10-10 00:00:00","EffectDateTo":"2023-03-25 00:00:00","IsActive":true,"DiscountType":3,"Image":"","Rules":null},{"MerchantName":"","CouponStatus":3,"IsUsed":false,"UsedDate":null,"Id":"6a60dec8-ad6d-4a4f-8dbd-223cb6fbbb3c","Title":"20-10","Remark":"","MerchantId":"00000000-0000-0000-0000-000000000000","Code":"","IsPercent":false,"CouponType":2,"DiscountRange":20.0,"DiscountValue":10.0,"EffectDateFrom":"2022-08-17 00:00:00","EffectDateTo":"2025-04-16 00:00:00","IsActive":true,"DiscountType":3,"Image":"","Rules":null},{"MerchantName":"","CouponStatus":3,"IsUsed":false,"UsedDate":null,"Id":"c08ba6dd-6175-4341-b654-066ab41d3b34","Title":"4132","Remark":"4123","MerchantId":"00000000-0000-0000-0000-000000000000","Code":"","IsPercent":false,"CouponType":1,"DiscountRange":1000.0,"DiscountValue":0.0,"EffectDateFrom":"2022-09-27 00:00:00","EffectDateTo":"2027-09-27 00:00:00","IsActive":true,"DiscountType":3,"Image":"","Rules":null},{"MerchantName":"八善有限公司","CouponStatus":3,"IsUsed":false,"UsedDate":null,"Id":"532596bf-3041-438d-95b7-c54849abb72a","Title":"1111","Remark":"我枯ek","MerchantId":"dfc430f2-b767-4852-a1ec-f93e7407610c","Code":"","IsPercent":false,"CouponType":1,"DiscountRange":100.0,"DiscountValue":0.0,"EffectDateFrom":"2022-08-28 00:00:00","EffectDateTo":"2029-09-26 00:00:00","IsActive":true,"DiscountType":3,"Image":"","Rules":null},{"MerchantName":"","CouponStatus":3,"IsUsed":false,"UsedDate":null,"Id":"99c87f77-b2aa-4699-a59a-4434f230c336","Title":"-10","Remark":"滿50-10","MerchantId":"00000000-0000-0000-0000-000000000000","Code":"","IsPercent":false,"CouponType":1,"DiscountRange":50.0,"DiscountValue":10.0,"EffectDateFrom":"2022-08-17 00:00:00","EffectDateTo":"2025-04-16 00:00:00","IsActive":true,"DiscountType":3,"Image":"/ClientResources/47de6d2e-4091-49bc-a422-491d95e8608b/00000000-0000-0000-0000-000000000000/CouponImage/f09b8918-b38b-444f-acec-4d01799e69e6.jpg","Rules":null}],"Page":1,"PageSize":10,"Offset":0,"SortName":"","SortOrder":""}}

createApp({
	components: {
		'nav-tab': NavTab
	},
	data() {
		return {
            coupons: [], // 優惠券數據
            pager: {
                page: 1,
                pageSize: 10,
                status : 3
            }
		}
	},
	methods: {
        getMemberCoupon: function (value, dropload) {
            if (value)  this.pager.page = value;

            let _this = this;
            InstoreSdk.api.coupon.getMemberCoupon(this.pager, function(result){
                if (result.Succeeded) {
                    _this.coupons = _this.coupons.concat(result.ReturnValue.Data);

                    if (_this.pager.page === 1 && _this.coupons.length) {
                        _this.$nextTick(() => {
                          loadPage($('.coupon-box'), result.ReturnValue.TotalPage, (val,me) => _this.getMemberCoupon(val, me));
                        });
                    }

                    if (dropload) {
                        _this.$nextTick(() => {
                          dropload.resetload();
                        });
                    }
                } else {
                    addtocartS(data.Message, '/imgs/warn-icon.png');
                }

                console.log(_this.coupons, '优惠券数据');
            });
        },
        // 去使用優惠券
        useCoupon: function (item) {
            if (item.MerchantId !== "00000000-0000-0000-0000-000000000000") {
                location.href = "/Merchant/Detail/" + item.MerchantId;
            } else {
                location.href = "/Default/Index";
            }
        }
	},
	created() {
		this.getMemberCoupon();
	},
	mounted() {

	}
}).mount('#container')