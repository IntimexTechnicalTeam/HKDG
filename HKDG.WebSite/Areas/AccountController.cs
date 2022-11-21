using Intimex.Utility;
using System.Security.Cryptography;
using Web.Jwt;

namespace HKDG.WebSite.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        IMemberBLL memberBLL;
        IJwtToken jwtToken;

        public AccountController(IComponentContext service) : base(service)
        {
            jwtToken = this.Services.Resolve<IJwtToken>();
            memberBLL = this.Services.Resolve<IMemberBLL>();
        }

        /// <summary>
        /// 会员登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> Login([FromForm]LoginInput input)
        {           
            var result = await loginBLL.Login(input);

            if (result.Succeeded)
            {
                var mUser = result.ReturnValue as MemberDto;

                var userInfo = AutoMapperExt.MapTo<CurrentUser>(mUser);
                userInfo.Id = mUser.Id;
                userInfo.IsLogin = true;
                userInfo.LoginType = LoginType.Member;
                userInfo.Currency = currencyBLL.GetSimpleCurrency(mUser.CurrencyCode);
                userInfo.LoginSerialNO = HashUtil.Md5Encrypt(Guid.NewGuid().ToString());

                string key = $"{CacheKey.CurrentUser}";
                await RedisHelper.HSetAsync(key, userInfo.LoginSerialNO, userInfo);

                result.Succeeded = true;
                result.ReturnValue = userInfo.LoginSerialNO;
            }

            return result;
        }
    }
}
