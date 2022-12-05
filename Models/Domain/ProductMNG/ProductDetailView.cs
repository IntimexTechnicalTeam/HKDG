namespace Domain
{
    public class ProductDetailView : ProductInfo
    {
        /// <summary>
        /// 当前客户是否收藏
        /// </summary>
        public bool IsFavorite { get; set; }
        /// <summary>
        /// 沒貨，售罄
        /// </summary>
        public bool IsSellout { get; set; }


        public bool HasDiscount
        {
            get
            {
                return this.SalePrice < this.OriginalPrice;
            }
        }

        public List<string> EventCodes { get; set; }

        public bool IsEventProduct
        {

            get
            {
                if (EventCodes == null)
                {
                    return false;
                }
                else
                {
                    if (EventCodes.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            set
            { }
        }

        public ProductType IconType { get; set; }

        public DateTime CreateDate { get; set; }

        public string IconString { get; set; }

        public bool IsShowAdult { get; set; }
    }
}
