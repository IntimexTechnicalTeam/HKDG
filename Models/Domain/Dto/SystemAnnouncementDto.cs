
using System.ComponentModel;

namespace Domain
{
    public class SystemAnnouncementDto
    {
        /// <summary>
        /// 標題
        /// </summary>

        public string Subject { get; set; }
       
        public Guid SubjectTransId { get; set; }
        /// <summary>
        /// 內容
        /// </summary>

        public string Content { get; set; }
        
        public Guid ContentTransId { get; set; }
        /// <summary>
        /// 最後發送成功時間
        /// </summary>
       
        public DateTime? LastSendDate { get; set; }
        /// <summary>
        /// 是否已發佈
        /// </summary>
       
        public bool IsPublished { get; set; }
        /// <summary>
        /// 公告類別
        /// </summary>

        public AnnouncementType Type { get; set; }
        /// <summary>
        /// 是否彈出式內容
        /// </summary>
       
        public bool IsPopUp { get; set; }

        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public Guid CreateBy { get; set; }

        public Guid? UpdateBy { get; set; }
    }
}
