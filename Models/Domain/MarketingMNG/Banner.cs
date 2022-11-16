namespace Domain
{
    public class Banner
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 橫幅圖片
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 序號
        /// </summary>
        public int Seq { get; set; }

        /// <summary>
        /// 跳转url
        /// </summary>
        public string Url { get; set; }


        public string Content { get; set; }
    }
}
