
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

                var tokenInfo = AutoMapperExt.MapTo<TokenInfo>(userInfo);
                tokenInfo.Id = userInfo.Id;
                tokenInfo.IsLogin = true;
                tokenInfo.IspType = input.IspType ?? "DG";
                userInfo.Token = jwtToken.CreateToken(tokenInfo);

                //把登录信息：token,权限，菜单，角色放进redis中
                string key = $"{CacheKey.CurrentUser}";
                await RedisHelper.HSetAsync(key, userInfo.Id.ToString(), userInfo);

                result.Succeeded = true;
                result.ReturnValue = userInfo.Token;
                HttpContext.Response.Cookies.Append("access_token", userInfo.Token);              
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
            CurrentContext.HttpContext.Response.Cookies.Delete("access_token");
            CurrentContext.HttpContext.Response.Cookies.Append("access_token", result.ReturnValue.ToString());
            return result;
        }

    }
}
