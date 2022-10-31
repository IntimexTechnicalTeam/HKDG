namespace Domain
{
    /// <summary>
    /// 系統公告
    /// </summary>
    public class SysAnnounceFrontView
    {
        /// <summary>
        /// 記錄ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 標題
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; }


        /// <summary>
        /// 創建人
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 創建時間
        /// </summary>
        public string CreateDate { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdateBy { get; set; }
        /// <summary>
        /// 更新時間
        /// </summary>
        public string UpdateDate { get; set; }
        /// <summary>
        /// 是否彈窗式內容
        /// </summary>
        public bool IsPopUp { get; set; }
    }
}
