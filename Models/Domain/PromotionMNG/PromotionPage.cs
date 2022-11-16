namespace Domain
{
    public class PromotionPage
    {
        /// <summary>
        /// 推廣記錄ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 語言版本
        /// </summary>
        public string Lang { get; set; }

        /// <summary>
        /// 關鍵字名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 推廣關鍵字圖片描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 推廣關鍵字圖片名稱
        /// </summary>
        public string ImgName { get; set; }

        /// <summary>
        /// 推廣關鍵字圖片
        /// </summary>
        public string ImgPath { get; set; }

        /// <summary>
        /// 序號
        /// </summary>
        public int Seq { get; set; }

        /// <summary>
        /// 橫幅列表
        /// </summary>
        public List<Banner> BannerList { get; set; } = new List<Banner>();

        /// <summary>
        /// 推廣產品列表
        /// </summary>
        public List<ProductDto> PrmtProductList { get; set; } = new List<ProductDto>();

        /// <summary>
        /// 推薦商家列表
        /// </summary>
        public List<MerchantDto> MerchantList { get; set; } = new List<MerchantDto>();

        /// <summary>
        /// 推广类型
        /// </summary>
        public int Type => (int)PrmtLstType;

        public string Page { get; set; }

        public PrmtLayoutType PrmtLstType { get; set; } = PrmtLayoutType.None;

        public ClientSideType ClientType { get; set; } = ClientSideType.Mobile;
    }
}
