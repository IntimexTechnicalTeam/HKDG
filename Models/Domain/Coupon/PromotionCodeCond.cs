﻿namespace Domain
{
    public class PromotionCodeCond
    {
        /// <summary>
        /// 商家ID
        /// </summary>
        public Guid MerchantId { get; set; }
        /// <summary>
        /// 推廣碼編號
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 產品清單
        /// </summary>
        public List<PromotionCodeProdCond> ProductList { get; set; }
    }
}
