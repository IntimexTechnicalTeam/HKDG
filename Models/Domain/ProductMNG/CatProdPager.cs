using System.ComponentModel;

namespace Domain
{ /// <summary>
  ///  产品目录产品列表分页器
  /// </summary>
    public class CatProdPager : PageInfo
    {
        /// <summary>
        ///  产品目录Id
        /// </summary>
        public Guid CatId { get; set; }

        [DefaultValue("New")]
        public string OrderBy { get; set; } = "New";

        /// <summary>
        /// 显示的货币类型
        /// </summary>
        public SimpleCurrency DisplayCurrency { get; set; }


    }
}