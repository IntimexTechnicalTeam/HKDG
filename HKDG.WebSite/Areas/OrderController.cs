using Web.Mvc.Filters;
using WebCache;

namespace HKDG.WebSite.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseApiController
    {
        IOrderBLL orderBLL;
        
        public OrderController(IComponentContext service) : base(service)
        {
            orderBLL = this.Services.Resolve<IOrderBLL>();
        }

        /// <summary>
        /// 创建订单 
        /// </summary>
        /// <param name="checkout"></param>
        /// <returns></returns>
        /// <exception cref="BLException"></exception>
        [LoginAuthorize]
        [HttpPost("Create")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> Create([FromForm] NewOrder checkout)
        {
            SystemResult result = new SystemResult();

            if (checkout == null) throw new BLException(HKDG.Resources.Message.SaveFail);

            result = await orderBLL.BuildOrder(checkout);            
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        [LoginAuthorize]
        [HttpPost("SearchOrder")]
        [ProducesResponseType(typeof(SystemResult<PageData<OrderSummaryView>>), 200)]
        public async Task<SystemResult<PageData<OrderSummaryView>>> SearchOrder([FromForm]OrderPager pager)
        {
            var result = new SystemResult<PageData<OrderSummaryView>>();
            var cond = new OrderCondition
            {
                PageInfo = pager,
                MemberId = CurrentUser.Id,
                StatusCode = pager.Status,
                OrderBy = pager.OrderBy,
                IsFront = true,
            };

            result.ReturnValue = await orderBLL.GetOrders(cond);
            result.Succeeded = true;
            return result;
        }

    }
}
