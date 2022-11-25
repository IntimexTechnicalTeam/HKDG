namespace Domain
{
    public class RegInfoView : SimpleMember
    {

        public string Password { get; set; }
        public bool Gender { get; set; }

        public Language Language { get; set; }

        public SimpleCurrency Currency { get; set; }

        public bool OptOut { get; set; }

        public AppTypeEnum ComeFrom { get; set; }

        public bool IsLogin { get; set; }



        /// <summary>
        /// LoginSerialNO
        /// </summary>
        public string SNO { get; set; }

        /// <summary>
        /// 最后一次访问时间
        /// </summary> 
        public DateTime LastAccessTime { get; set; }

        public bool Linkuped { get; set; }

        public string HKPID { get; set; }

        /// <summary>
        /// 如果已經過左SSO的過渡期，就為false
        /// </summary>
        public bool PwdEnable { get; set; }

        /// <summary>
        /// 第三方帳戶ID
        /// </summary>
        public string ThirdPartyUserId { get; set; }
        /// <summary>
        /// 第三方帳戶類型
        /// </summary>
        public int? ThirdPartyType { get; set; } = 0;
        public List<string> Preference { get; set; }

        /// <summary>
        /// 同步注册到TranSin
        /// </summary>
        public bool SyncTranSin { get; set; } = false;

        /// <summary>
        /// 是否Transin传递
        /// </summary>
        public bool IsTransin { get; set; } = false;

        /// <summary>
        /// 是否已綁定Transin帳戶
        /// </summary>
        public bool TransinLinkuped { get; set; }

        public bool IsTempUser { get; set; }

        /// <summary>
        /// Transin帳戶詳細
        /// </summary>
        public TransinMbrDetail TransinAcctDetail { get; set; } = new TransinMbrDetail();

    }
}
