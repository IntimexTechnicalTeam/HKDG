namespace Domain
{

    public class ProdSearchCond  
    {
        public PageInfo PageInfo { get; set; } = new PageInfo();

        public Guid MerchantId { get; set; } = Guid.Empty;
        public string Key { get; set; }

        public string ProductCode { get; set; }
        public string KeyWordType { get; set; }
        public Guid Category { get; set; } = Guid.Empty;

        public Guid Attribute { get; set; } = Guid.Empty;
        public Guid AttributeValue { get; set; } = Guid.Empty;

        public int IsActive { get; set; } = 1;

        public int IsDeleted { get; set; } = 0;
        public int IsApprove { get; set; } = 1;

        public string ApproveStatus { get; set; }
     
        /// <summary>
        /// 
        /// </summary>
        public ProductSearchType ProductSearchType { get; set; }

        /// <summary>
        /// 价格范围
        /// </summary>
        public List<decimal> Prices { get; set; }

        
    }
}
