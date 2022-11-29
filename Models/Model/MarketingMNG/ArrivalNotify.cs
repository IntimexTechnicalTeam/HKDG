namespace Model
{
    public class ArrivalNotify : BaseEntity<Guid>
    {
        /// <summary>
        /// 會員ID
        /// </summary>
        [Column(Order = 3)]
        public Guid? MemberId { get; set; }
        /// <summary>
        /// Sku
        /// </summary>
        [Required]
        [Column(Order = 4)]
        public Guid SkuId { get; set; }
        /// <summary>
        /// 是否已通知會員
        /// </summary>
        [Required]
        [Column(Order = 5)]
        public bool IsNotified { get; set; }
        /// <summary>
        /// 通知會員時間
        /// </summary>
        [Column(Order = 6)]
        public DateTime? NotifyDate { get; set; }
        /// <summary>
        /// 通知郵箱
        /// </summary>
        [MaxLength(50)]
        [Column(Order = 7, TypeName = "varchar")]
        public string Email { get; set; }
        /// <summary>
        /// 是否已通知商家
        /// </summary>
        [Required]
        [Column(Order = 8)]
        public bool IsMerchNotified { get; set; }
        /// <summary>
        /// 通知商家時間
        /// </summary>
        [Column(Order = 9)]
        public DateTime? MerchNotifyDate { get; set; }
        /// <summary>
        /// 商家Id
        /// </summary>
        [Required]
        [Column(Order = 10)]
        public Guid MerchantId { get; set; }
    }
}
