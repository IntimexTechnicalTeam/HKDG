using Microsoft.AspNetCore.Authorization;

namespace HKDG.WebSite.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        IMemberBLL memberBLL;
        
        public AccountController(IComponentContext service) : base(service)
        {            
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
                userInfo.ExpireDate = DateTime.Now.AddSeconds(Setting.MemberAccessTokenExpire);

                string key = $"{CacheKey.CurrentUser}";
                await RedisHelper.HSetAsync(key, userInfo.LoginSerialNO, userInfo);

                HttpContext.SetCookie("access_token", userInfo.LoginSerialNO);

                result.Succeeded = true;
                result.ReturnValue = userInfo.LoginSerialNO;
            }

            return result;
        }

        /// <summary>
        /// FbLogin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> FBLogin([FromForm]ThirdpartyActView model)
        {
            var thirdLinkUp = await memberBLL.ThirdpartyLogin(model);
            var result = new SystemResult();

            //if (string.IsNullOrEmpty(model.UserName))//用于处理Email为空的情况
            //{
            //    model.UserName = model.ExternalAccId;
            //}

            var input = new LoginInput { Account = thirdLinkUp.ReturnValue.Account, Password = model.Password };
            result = await loginBLL.FBLogin(input);
            if (result.Succeeded)
            {
                var mUser = result.ReturnValue as MemberDto;

                var userInfo = AutoMapperExt.MapTo<CurrentUser>(mUser);
                userInfo.Id = mUser.Id;
                userInfo.IsLogin = true;
                userInfo.LoginType = LoginType.Member;
                userInfo.Currency = currencyBLL.GetSimpleCurrency(mUser.CurrencyCode);
                userInfo.LoginSerialNO = HashUtil.Md5Encrypt(Guid.NewGuid().ToString());
                userInfo.ExpireDate = DateTime.Now.AddSeconds(Setting.MemberAccessTokenExpire);

                string key = $"{CacheKey.CurrentUser}";
                await RedisHelper.HSetAsync(key, userInfo.LoginSerialNO, userInfo);

                HttpContext.SetCookie("access_token", userInfo.LoginSerialNO);
                HttpContext.SetCookie("logined","1");

                result.Succeeded = true;
                result.ReturnValue = userInfo.LoginSerialNO;

            }

            return result;
        }
    }
}
