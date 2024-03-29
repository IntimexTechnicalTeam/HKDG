﻿namespace Model
{
    /// <summary>
    /// 购物车
    /// </summary>
    public class ShoppingCartItem : BaseEntity<Guid>
    {
        /// <summary>
        ///  会员ID/临时ID
        /// </summary>
        [Column(Order = 3)]
        public Guid MemberId { get; set; }


        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        [Column(Order = 4)]
        public Guid ProductId { get; set; }

        /// <summary>
        /// 单品ID
        /// </summary>
        [Column(Order = 5)]
        public Guid SkuId { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        [Column(Order = 6)]
        public int Qty { get; set; }

        [Column(Order =9)]
        public Guid KolId { get; set; }

        [ForeignKey("SkuId")]
        public virtual ProductSku Sku { get; set; }


        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        /// <summary>
        /// IsSelected
        /// </summary>
        [Column(Order = 9)]
        public bool IsSelected { get; set; } = false;
    }
}
