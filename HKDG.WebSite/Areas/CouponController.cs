using Web.Mvc.Filters;

namespace HKDG.WebSite.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : WebController
    {
        ICouponBLL  couponBLL;
        public CouponController(IComponentContext service) : base(service)
        {
            couponBLL = Services.Resolve<ICouponBLL>();
        }

        /// <summary>
        /// 獲取mallfun
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        [HttpPost("GetMemberCoupon")]
        [ProducesResponseType(typeof(SystemResult<PageData<CouponInfo>>), 200)]
        public async Task<SystemResult<PageData<CouponInfo>>> GetMemberCoupon([FromForm] CouponPager pager)
        {
            var result = new SystemResult<PageData<CouponInfo>>();
            result.ReturnValue = couponBLL.GetMemberCoupon(pager);
            result.Succeeded = true;
            return result;
        }
    }
}
