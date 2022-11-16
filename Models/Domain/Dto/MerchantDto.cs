namespace Domain
{
    public class MerchantDto
    {
        public Guid Id => MchId;

        /// <summary>
        /// 商家Id
        /// </summary>
        public Guid MchId { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 商家編碼
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 評分
        /// </summary>
        public decimal Score { get; set; }
        /// <summary>
        /// 是否被收藏
        /// </summary>
        public bool IsFavorite { get; set; }
        /// <summary>
        /// 小圖標
        /// </summary>
        public string LogoS { get; set; }
        /// <summary>
        /// 大圖標
        /// </summary>
        public string LogoB { get; set; }

        public List<ProductDto> Products { get; set; } = new List<ProductDto>();

        /// <summary>
        /// 是否香港品牌
        /// </summary>
        public bool IsHongKong { get; set; }

        public int Seq { get; set; }
    }
}
