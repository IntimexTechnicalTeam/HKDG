namespace Domain
{
    public class DiscountView
    {
        public Guid Id { get; set; }

        public bool IsPercent { get; set; }

        public decimal DiscountValue { get; set; }

        public decimal DiscountPrice { get; set; }

        public DiscountType DiscountType { get; set; }

        public CouponUsage CouponType { get; set; }

        public string Code { get; set; }

        public Guid ProductId { get; set; }

        public string DiscountTypeName
        {
            get
            {
                var result = "";
                switch (DiscountType)
                {
                    case DiscountType.Coupon:
                        switch (CouponType)
                        {
                            case CouponUsage.Price:
                                result = Label.CashCoupon;
                                break;
                            case CouponUsage.DeliveryCharge:
                                result = Label.DeliveryChargeCoupon;
                                break;
                        }
                        break;
                    case DiscountType.PromotionCode:
                        result = Label.PromotionCode;
                        break;
                    case DiscountType.PromotionRule:
                        result = Label.PromotionRule;
                        break;
                    case DiscountType.MemberGroup:
                        result = Label.MemberGroupDiscount;
                        break;
                    case DiscountType.MallFun:
                        result = "MallFun";
                        break;
                    case DiscountType.FreeChargeProduct:
                        result = Label.FreeChargeProduct;
                        break;

                }
                return result;
            }
            set { }
        }
    }
}

