namespace Domain
{
    public class NoticeListDto
    {
        /// <summary>
        /// 系统公告
        /// </summary>
        public List<SysAnnounceFrontView> SystemNotice { get; set; } = new List<SysAnnounceFrontView>();

        /// <summary>
        /// 推广公告
        /// </summary>
        public List<SysAnnounceFrontView> PromotionNotice { get; set; } = new List<SysAnnounceFrontView>();
    }
}
