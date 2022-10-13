namespace Domain
{
    /// <summary>
    /// 系统返回给用户的错误类型
    /// 1，返回到UI
    /// 2，api返回结果
    /// </summary>
    public class SystemError
    {
        /// <summary>
        /// 错误编码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 错误描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 帮助链接
        /// </summary>
        public string HelpLink { get; set; }

        /// <summary>
        /// 语言类型
        /// </summary>
        public SystemLang Language { get; set; }

        public string Message
        {
            get
            {
                return string.Format("code={0},description={1}", this.Code, this.Description);
            }
        }



    }
}
