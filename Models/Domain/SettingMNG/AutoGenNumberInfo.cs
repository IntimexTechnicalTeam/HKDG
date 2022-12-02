namespace Domain
{
    public class AutoGenNumberInfo
    {
        /// <summary>
        /// 前綴
        /// </summary>
        public string Perfix { get; set; }
        /// <summary>
        /// 格式
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// 後綴
        /// </summary>
        public int Postfix { get; set; }

        /// <summary>
        /// 後綴長度
        /// </summary>
        public int PostfixLength { get; set; }
    }
}
