namespace HKDG.Model
{
    public class StorePickUpAddress : BaseEntity<Guid>
    {
        public Guid MerchantId { get; set; }

        public Language Lang { get; set; }

        [StringLength(400)]
        [Column(TypeName = "nvarchar")]
        public string Address { get; set; }

        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        public string Remark { get; set; } = "";

        public string Name { get; set; }

        /// <summary>
        /// 关联Id(相同地址不同语言的关联ID)
        /// </summary>
        public Guid RelevanceId { get; set; }
    }
}
