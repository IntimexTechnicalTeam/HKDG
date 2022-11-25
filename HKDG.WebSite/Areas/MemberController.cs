using HKDG.BLL;
using Model;
using Web.Jwt;
using WebCache;

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
            HttpContext.Response.Cookies.Delete("access_token");

            var result = new SystemResult() { Succeeded = true };           
            await RedisHelper.HDelAsync($"{CacheKey.CurrentUser}", CurrentUser.LoginSerialNO);

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

        /// <summary>
        /// 获取登录后的会员数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("getProfile")]
        [ProducesResponseType(typeof(SystemResult<RegInfoView>), 200)]
        public async Task<SystemResult<RegInfoView>> GetProfile()
        {             
            var user = await GetMemberInfo();
            var memberInfo = user.ReturnValue;
            var regInfo = AutoMapperExt.MapTo<RegInfoView>(memberInfo);
            regInfo.SNO = CurrentUser.LoginSerialNO;
            regInfo.Language = memberInfo.Language;
            regInfo.Currency = memberInfo.Currency;
            regInfo.ComeFrom = CurrentUser?.ComeFrom ?? AppTypeEnum.WebSite;
            regInfo.IsLogin = true;
            regInfo.LastAccessTime = DateTime.Now;
            regInfo.Linkuped = CurrentUser.Linkuped;
            regInfo.HKPID = CurrentUser.HKPID;
           // regInfo.Preference = memberBLL.GeMemberPreference();

            var result = new SystemResult<RegInfoView> { ReturnValue = regInfo, Succeeded = true };
            return result;
        }

        /// <summary>
        /// 添加商家到收藏列表
        /// </summary>
        /// <param name="merchCode">商家編號</param>
        [HttpGet("AddFavMerchant")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> AddFavMerchant(string merchCode)
        {
            var result = await memberBLL.AddFavMerchant(merchCode);
            return result;
        }

        /// <summary>
        /// 從收藏列表移除商家
        /// </summary>
        /// <param name="merchCode">商家編號</param>
        [HttpGet("RemoveFavMerchant")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> RemoveFavMerchant(string merchCode)
        {
            var result = await memberBLL.RemoveFavMerchant(merchCode);
            return result;
        }

        /// <summary>
        /// 添加产品到收藏列表
        /// </summary>
        /// <param name="productId">產品ID</param>
        [HttpGet("AddFavorite")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> AddFavProduct(Guid productId)
        {
            var result = await memberBLL.AddFavProduct(productId);
            return result;
        }

        /// <summary>
        /// 從收藏列表移除产品
        /// </summary>
        /// <param name="productId">產品ID</param>
        [HttpGet("RemoveFavorite")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> RemoveFavProduct(Guid productId)
        {
            var result = await memberBLL.RemoveFavProduct(productId);
            return result;
        }

        ///// <summary>
        ///// 我收藏的商家列表
        ///// </summary>
        ///// <param name="cond"></param>
        ///// <returns></returns>
        //[HttpPost("MyFavMerchant")]
        //[ProducesResponseType(typeof(SystemResult<PageData<MicroMerchant>>), 200)]
        //public async Task<SystemResult<PageData<MicroMerchant>>> MyFavMerchant([FromBody] FavoriteCond cond)
        //{
        //    var result = new SystemResult<PageData<MicroMerchant>>() { Succeeded = true };
        //    result.ReturnValue = await memberBLL.MyFavMerchant(cond);
        //    return result;
        //}

        ///// <summary>
        ///// 我收藏的商品列表
        ///// </summary>
        ///// <param name="cond"></param>
        ///// <returns></returns>
        //[HttpPost("MyFavProduct")]
        //[ProducesResponseType(typeof(SystemResult<PageData<MicroProduct>>), 200)]
        //public async Task<SystemResult<PageData<MicroProduct>>> MyFavProduct([FromBody] FavoriteCond cond)
        //{
        //    var result = new SystemResult<PageData<MicroProduct>>() { Succeeded = true };
        //    result.ReturnValue = await memberBLL.MyFavProduct(cond);
        //    return result;
        //}

    }
}
