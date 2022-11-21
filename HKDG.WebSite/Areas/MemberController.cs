using Web.Jwt;

namespace HKDG.WebSite.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : BaseApiController
    {
        IMemberBLL memberBLL;
        IJwtToken jwtToken;

        public MemberController(IComponentContext service) : base(service)
        {
            jwtToken = this.Services.Resolve<IJwtToken>();
            memberBLL = this.Services.Resolve<IMemberBLL>();
        }

        /// <summary>
        /// 会员登出
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Logout")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> Logout()
        {
            string ticket = jwtToken.CreateDefautToken();
            var result = new SystemResult() { Succeeded = true };
            result.ReturnValue = ticket;
            return result;
        }

        /// <summary>
        /// 修改语言
        /// </summary>
        /// <param name="Lang"></param>
        /// <returns></returns>  
        [AllowAnonymous]
        [HttpGet("ChangeLang")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> ChangeLang(Language Lang)
        {
            var result = new SystemResult() { Succeeded = true };
            result = await memberBLL.ChangeLang(CurrentUser, Lang);
            return result;
        }

        /// <summary>
        /// 修改币种
        /// </summary>
        /// <param name="CurrencyCode"></param>
        /// <returns></returns>       
        [AllowAnonymous]
        [HttpGet("ChangeCurrencyCode")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> ChangeCurrencyCode(string CurrencyCode)
        {
            var result = new SystemResult() { Succeeded = true };
            result.ReturnValue = await memberBLL.ChangeCurrencyCode(CurrentUser, CurrencyCode);
            return result;
        }

        /// <summary>
        /// 获取登录后的会员数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetMemberInfo")]
        [ProducesResponseType(typeof(SystemResult<MemberItem>), 200)]
        public async Task<SystemResult<MemberItem>> GetMemberInfo()
        {
            var mUser = await RedisHelper.HGetAsync<MemberItem>($"{CacheKey.Member}", CurrentUser.UserId);
            if (mUser == null)
            {
                mUser = await memberBLL.GetMemberInfo();
                await RedisHelper.HSetAsync($"{CacheKey.Member}", CurrentUser.UserId, mUser);
            }

            var result = new SystemResult<MemberItem> { ReturnValue = mUser ,Succeeded =true };
            return result;
        }


    }
}
