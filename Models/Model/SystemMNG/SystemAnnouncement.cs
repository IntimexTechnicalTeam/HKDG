namespace Model
{
    public class SystemAnnouncement : BaseEntity<Guid>
    {     
        [Required]
        [Column(Order = 3)]
        public Guid SubjectTransId { get; set; }
    
        [Required]
        [Column(Order = 4)]
        public Guid ContentTransId { get; set; }
        /// <summary>
        /// 最後發送成功時間
        /// </summary>
        [Column(Order = 5)]
        public DateTime? LastSendDate { get; set; }
        /// <summary>
        /// 是否已發佈
        /// </summary>
        [Column(Order = 6)]
        public bool IsPublished { get; set; }
        /// <summary>
        /// 公告類別
        /// </summary>
        [Required]
        [Column(Order = 7)]
        public AnnouncementType Type { get; set; }
        /// <summary>
        /// 是否彈出式內容
        /// </summary>
        [Column(Order = 8)]
        public bool IsPopUp { get; set; }
    }
}
