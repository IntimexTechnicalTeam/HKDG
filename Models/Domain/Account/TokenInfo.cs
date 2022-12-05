using System.ComponentModel;
using System.Resources;
using System.Security.Principal;

namespace Domain
{
    /// <summary>
    /// token参数实体
    /// </summary>
    public class TokenInfo
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string UserId  => Id.ToString();

        public string Account { get; set; }

        public string Email { get; set; } = "";

        public string Password { get; set; }


        public Language Lang { get; set; } = Language.C;

        public Language Language => Lang;

        public LoginType LoginType { get; set; } = LoginType.TempUser;

        public string CurrencyCode { get; set; } = "HKD";

        public bool IsLogin { get; set; } = false;

        /// <summary>
        /// 取配置文件的
        /// </summary>
        public string IspType { get; set; } = "DG";

        public DateTime ExpireDate { get; set; }

        public SimpleCurrency Currency { get; set; } = new SimpleCurrency();

        /// <summary>
        /// 
        /// </summary>
        public string LoginSerialNO { get; set; }

        
        public string Name { get; set; }

      
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsTempUser { get; set; }

        public string Mobile { get; set; }
     
        public AccountType Type { get; set; }

     
        public AppTypeEnum ComeFrom { get; set; }

        public string ComeFromDisplay
        {
            get
            {

                return ComeFrom.ToString();
            }
        }

        public bool OptOut { get; set; }

        public Language? LanguageArg { get; set; }

        public string DateTimeFormat { get; set; }

        public Guid MerchantId { get; set; }

       
        public DateTime LastLogin { get; set; }

        public UserType UserType { get; set; }

        [NotMapped]
        public bool IsMerchant
        {
            get
            {
                return MerchantId != Guid.Empty;
            }
        }
        /// <summary>
        /// 商家名称
        /// </summary>
       
        public string MerchantName { get; set; }

        public string IPAddress { get; set; }

        public string AppNO { get; set; }


        public DateTime LastAccessTime { get; set; }

    }
}
