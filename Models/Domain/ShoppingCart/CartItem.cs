using System.ComponentModel;
using System.Reflection.Emit;

namespace Domain
{
    public class CartItem
    {
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid Sku { get; set; } = Guid.Empty;

        /// <summary>
        /// ///商品Id
        /// </summary>
        [Required(ErrorMessage = "ProductId必填")]
        public Guid ProductId { get; set; }
        
        /// <summary>
        ///商品Code 
        /// </summary>
        [Required(ErrorMessage = "ProdCode必填")]
        public string ProdCode { get; set; } = string.Empty;

        /// <summary>
        /// 所选属性1
        /// </summary>
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid Attr1 { get; set; } = Guid.Empty;   //对应数值为ProductSku.AttrValue1

        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid Attr2 { get; set; } = Guid.Empty;

        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid Attr3 { get; set; } = Guid.Empty;


        public string AttrName1 { get; set; }

        public string AttrName2 { get; set; }

        public string AttrName3 { get; set; }

        public string AttrTypeName1 { get; set; }

        public string AttrTypeName2 { get; set; }

        public string AttrTypeName3 { get; set; }

        /// <summary>
        /// 购物数量
        /// </summary>
        [Required(ErrorMessage = "购物数量必填")]     
        public int Qty { get; set; }

        /// <summary>
        /// 增量
        /// </summary>
        [DefaultValue(1)]
        public int AddQty { get; set; } = 0;

        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid KolId { get; set; } = Guid.Empty;
    }
}
