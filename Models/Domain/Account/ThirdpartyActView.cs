using System.ComponentModel;

namespace Domain
{
    public class ThirdpartyActView
    {
        /// <summary>
        /// 第三方帳戶ID
        /// </summary>
        [Required]
        public string ExternalAccId { get; set; }
        /// <summary>
        /// 第三方帳戶類型   1为facebook,2为transin
        /// </summary>
        [Required]
        public int ExternalType { get; set; }
        /// <summary>
        /// 帳戶名稱
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 帳戶密碼
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 本地會員ID
        /// </summary>
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid LocalMbrId { get; set; } = Guid.Empty;
    }
}
