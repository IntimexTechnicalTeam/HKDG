namespace Model
{
    public class ProductComment : BaseEntity<Guid>
    {
        /// <summary>
        /// 商家ID
        /// </summary>
        [Required]
        public Guid MerchantId { get; set; }
        /// <summary>
        /// 訂單ID
        /// </summary>
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public Guid SubOrderId { get; set; }
        /// <summary>
        /// 產品Id
        /// </summary>
        [Required]
        public Guid ProductId { get; set; }
        /// <summary>
        /// 評分
        /// </summary>
        [Required]
        public decimal Score { get; set; }
        /// <summary>
        /// 內容
        /// </summary>

        [StringLength(1000)]
        public string Content { get; set; }
        ///// <summary>
        ///// 回復人ID
        ///// </summary>
        //public Guid ReplyId { get; set; }

        /// <summary>
        /// 回復內容
        /// </summary>

        [StringLength(1000)]
        public string ReplyContent { get; set; }
        /// <summary>
        /// 是否顯示
        /// </summary>
        public bool IsShow { get; set; }
      
        /// <summary>
        /// 產品編號
        /// </summary>
        [Column(TypeName = "varchar")]
        [StringLength(100)]
        public string ProductCode { get; set; }
        
    }
}
