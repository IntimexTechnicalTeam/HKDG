namespace HKDG.Domain
{
    public class MallCartProduct
    {
        /// <summary>
        /// ShoppingCartItem.Id 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// SkuId
        /// </summary>
        public Guid SkuId { get; set; }

        /// <summary>
        /// 产品Code
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public List<string> ProductImage { get; set; } = new List<string>();

        /// <summary>
        /// 产品实际价格 SalePrice + AttrValue1AddPrice+ AttrValue2AddPrice + AttrValue3AddPrice
        /// </summary>
        public decimal RealPrice { get; set; }

        /// <summary>
        /// 商品售价
        /// </summary>
        public decimal SalePrice { get; set; }

        /// <summary>
        /// 实际购物数量（包含买X送Y）,暂时没用
        /// </summary>
        public int RealQty { get; set; }

        /// <summary>
        /// 购物数量
        /// </summary>
        public int BuyQty { get; set; }

        /// <summary>
        /// 商品总价钱 = RealPrice * BuyQty
        /// </summary>
        public decimal ItemAmount => RealPrice * BuyQty;

        public List<KeyValue> AttrList { get; set; } = new List<KeyValue>();

        public Guid CatalogId { get; set; }

        /// <summary>
        /// 推广规则名称
        /// </summary>
        public string RuleTitle { get; set; } = string.Empty;

        /// <summary>
        /// 是否赠品
        /// </summary>
        public bool IsFree { get; set; }

        /// <summary>
        /// 每件优惠的价钱
        /// </summary>
        public decimal SingleDiscountPrice { get; set; }

        /// <summary>
        /// Promotion Rule Price
        /// </summary>
        public decimal GroupSaleDiscountPrice { get; set; }

        public Guid KolId { get; set; }

        public bool IsEventProduct { get; set; }

        public PromotionRuleType RuleType { get; set; }

        /// <summary>
        /// 购物车数据状态，正常，已下架或已删除，库存不足，库存紧张
        /// </summary>
        public CartItemStatus CartItemStatus { get; set; } = CartItemStatus.Normal;

        /// <summary>
        /// 商品的Currency对象
        /// </summary>
        public SimpleCurrency Currency = new SimpleCurrency { };

        public bool IsShowAdult { get; set; }

        /// <summary>
        ///购物限额
        /// </summary>
        public int SaleQuota { get; set; }

        /// <summary>
        /// 是否被选中
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// 产品重量
        /// </summary>
        public decimal Weight { get; set; } = 0;

        /// <summary>
        /// 優惠規則Id
        /// </summary>
        public Guid RuleId { get; set; } = Guid.Empty;
    }

    /// <summary>
    /// 以商家为粒度的MallCartItem
    /// </summary>
    public class MallCartItem
    {
        /// <summary>
        /// 商家名称
        /// </summary>
        public string MchName { get; set; }

        /// <summary>
        /// 商家Id
        /// </summary>
        public Guid MchId { get; set; }

        /// <summary>
        /// KolId
        /// </summary>
        public Guid KolId { get; set; }

        /// <summary>
        /// 购物总数量
        /// </summary>
        public int ItemQty { get; set; }

        /// <summary>
        /// 总金额（含税）
        /// </summary>
        public decimal ItemAmount { get; set; }

        ///// <summary>
        ///// 优惠总金额,备用字段
        ///// </summary>
        //public decimal ItemDicount { get; set; }

        ///// <summary>
        ///// 总税额,备用字段
        ///// </summary>
        //public decimal ItemTax { get; set; }

        //public SimpleCurrency Currency { get; set; } = new SimpleCurrency();

        /// <summary>
        /// 商家店铺地址
        /// </summary>       
        public List<KeyValueAddress> StoreAddress { get; set; } = new List<KeyValueAddress>();

        /// <summary>
        /// 会员联系姓名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 会员联系电话
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public List<MallCartProduct> ProductList { get; set; } = new List<MallCartProduct>();

        /// <summary>
        /// 商家付运方式
        /// </summary>
        public List<ExpressChargeInfo> ExpressChargeList { get; set; } = new List<ExpressChargeInfo>();
    }

    /// <summary>
    /// 购物车中失效的产品
    /// </summary>
    public class DisableProduct
    {

        /// <summary>
        /// ShoppingCartItem.Id 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// SkuId
        /// </summary>
        public Guid SkuId { get; set; }

        /// <summary>
        /// 产品Code
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string ProductImage { get; set; }


    }

    public class MallCartInfo
    {
        /// <summary>
        /// 购物总数量
        /// </summary>
        public int TotalQty { get; set; }

        /// <summary>
        /// 总金额（含税）
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 优惠总金额
        /// </summary>
        public decimal TotalDicount { get; set; }

        /// <summary>
        /// 总税额
        /// </summary>
        public decimal TotalTax { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal DeliveryCharge { get; set; }

        /// <summary>
        /// 是否显示成人选项
        /// </summary>
        public bool IsShowAdult { get; set; }

        /// <summary>
        /// 判斷是否有產品中了推廣規則
        /// </summary>
        public bool HasPromotionRule { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal EventProductPrice { get; set; }

        /// <summary>
        /// 有效的购物车数据
        /// </summary>
        public List<MallCartItem> EnableMallCart { get; set; } = new List<MallCartItem>();

        /// <summary>
        /// 失效的购物车数据
        /// </summary>
        public List<DisableProduct> DisableProductList { get; set; } = new List<DisableProduct>();

        /// <summary>
        /// 购物车默认币种
        /// </summary>
        public SimpleCurrency Currency { get; set; } = new SimpleCurrency();
    }

    /// <summary>
    /// 
    /// </summary>
    public class SelectedMallCart
    {
        /// <summary>
        /// 购物总数量
        /// </summary>
        public int TotalQty { get; set; }

        /// <summary>
        /// 总金额（含税）
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 优惠总金额
        /// </summary>
        public decimal TotalDicount { get; set; }

        /// <summary>
        /// 总税额
        /// </summary>
        public decimal TotalTax { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal DeliveryCharge { get; set; }

        /// <summary>
        /// 判斷是否有產品中了推廣規則
        /// </summary>
        public bool HasPromotionRule { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal EventProductPrice { get; set; }

        /// <summary>
        /// 选中的购物车数据
        /// </summary>
        public List<MallCartItem> EnableMallCart { get; set; } = new List<MallCartItem>();

        /// <summary>
        /// 购物车默认币种
        /// </summary>
        public SimpleCurrency Currency { get; set; } = new SimpleCurrency();

    }
}
