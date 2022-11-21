
namespace Domain
{
    /// <summary>
    /// 当前用户信息实体
    /// </summary>
    public class CurrentUser: TokenInfo
    {
        public bool Linkuped { get; set; }

        public string HKPID { get; set; }

        public string Token { get; set; } = "";

        public List<RoleDto> Roles { get; set; } = new List<RoleDto>();

        public Guid MerchantId { get; set; }

        // public bool IsMerchant =>LoginType <= LoginType.ThirdMerchantLink ? true : false;

        private bool _IsMerchant;
        public bool IsMerchant {

            get { return LoginType <= LoginType.ThirdMerchantLink ? true : false; }
            set
            {
                _IsMerchant = value;
                
            }
        }
    }


    public class CurrentUser<T> : CurrentUser
    {
        public T UserData { get; set; }
    }
}
