namespace Model
{
    /// <summary>
    /// 會員註冊校驗
    /// </summary>
    public class EmailCodeVerify : BaseEntity<Guid>
    {

        [Column(Order = 3)]
        public Guid MemberId { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "varchar", Order = 4)]
        public string VerifyCode { get; set; }


        [Column(Order = 5)]
        public DateTime ExpireDate { get; set; }

        [MaxLength(20)]
        [Column(TypeName = "varchar", Order = 6)]
        public string Type { get; set; }
        [NotMapped]
        /// <summary>
        /// 校驗地址
        /// </summary>
        public string VerifyUrl { get; set; }
    }
}
