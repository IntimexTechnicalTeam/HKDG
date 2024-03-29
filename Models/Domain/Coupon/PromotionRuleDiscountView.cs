﻿namespace Domain
{
    public class PromotionRuleDiscountView
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 优惠的价钱
        /// </summary>
        public decimal DiscountPrice { get; set; }

        /// <summary>
        /// 单件产品优惠的价钱
        /// </summary>
        public decimal SingleDiscountPrice { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public Guid ProductId { get; set; }

    }
}
