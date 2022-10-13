namespace Domain
{
    public class CouponDiscountInfo
    {
        public DiscountType DiscountType { get; set; }
        public string DiscountTypeName
        {
            get
            {
                string name = string.Empty;
                switch (DiscountType)
                {
                    case DiscountType.Coupon:
                        name = HKDG.Resources.Message.Coupon;
                        break;
                    case DiscountType.MemberGroup:
                        name = HKDG.Resources.Message.MemberGroup;
                        break;
                    case DiscountType.PromotionCode:
                        name = HKDG.Resources.Message.PromotionCode;
                        break;
                    case DiscountType.PromotionRule:
                        name = HKDG.Resources.Message.PromotionRule;
                        break;
                    case DiscountType.MallFun:
                        name = HKDG.Resources.Label.MallFun;
                        break;

                }
                return name;
            }
            set
            {

            }
        }
        public decimal DiscountVaule { get; set; }
        public CouponUsage DiscountUsage { get; set; }
        public string DiscountUsageName
        {
            get
            {
                string name = string.Empty;
                switch (DiscountUsage)
                {
                    case CouponUsage.DeliveryCharge:
                        name = HKDG.Resources.Label.CouponTypeDeliveryCharge;
                        break;
                    case CouponUsage.Price:
                        name = HKDG.Resources.Label.CouponTypeGoodsPrice;
                        break;
                }
                return name;
            }
            set
            {

            }
        }
        public string Code { get; set; }

        public decimal DiscountPrice { get; set; }
    }
}
