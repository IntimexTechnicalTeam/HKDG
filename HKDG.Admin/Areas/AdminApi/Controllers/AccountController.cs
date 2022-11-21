using System.Net;

namespace HKDG.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        IUserBLL userBLL;
        public AccountController(IComponentContext services) : base(services)
        {
            userBLL = Services.Resolve<IUserBLL>();
        }

        /// <summary>
        /// 后台用户登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<SystemResult> LoginByAPI([FromForm] LoginInput input)
        {
            var user = await loginBLL.CheckAdminLogin(input);
            var result = await loginBLL.AdminLogin(user);
            if (result.Succeeded)
            {
                var userInfo = result.ReturnValue as UserDto;

                var tokenInfo = AutoMapperExt.MapTo<CurrentUser>(userInfo);
                tokenInfo.Id = userInfo.Id;
                tokenInfo.IsLogin = true;
                tokenInfo.Type = AccountType.User;                
                tokenInfo.IspType = input.IspType ?? "DG";
                tokenInfo.ExpireDate = DateTime.Now.AddSeconds(Setting.UserAccessTokenExpire);
                tokenInfo.LoginSerialNO = HashUtil.Md5Encrypt(Guid.NewGuid().ToString());

                //把登录信息：token,权限，菜单，角色放进redis中
                string key = $"{CacheKey.CurrentUser}";
                await RedisHelper.HSetAsync(key, tokenInfo.LoginSerialNO, tokenInfo);

                result.Succeeded = true;
                result.ReturnValue = tokenInfo.LoginSerialNO;
               
                var option = new CookieOptions { HttpOnly = true };
                HttpContext.Response.Cookies.Append("access_token", tokenInfo.LoginSerialNO, option);

            }

            return result;
        }

        /// <summary>
        /// 修改语言
        /// </summary>
        /// <param name="Lang"></param>
        /// <returns></returns>       
        [HttpGet]
        public async Task<SystemResult> ChangeLang(Language Lang)
        {
            var result = new SystemResult() { Succeeded = true };
            result = await userBLL.ChangeLang(CurrentUser, Lang);           
            return result;
        }

    }
}
