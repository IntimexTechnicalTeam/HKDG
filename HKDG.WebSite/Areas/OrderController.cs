using Web.Mvc.Filters;
using WebCache;
using WS.ECShip.Model.MailTracking;

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
        /// 我的订单 
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

        /// <summary>
        /// 订单明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [LoginAuthorize]
        [HttpGet("GetOrder")]
        [ProducesResponseType(typeof(SystemResult<PageData<OrderSummaryView>>), 200)]
        public async Task <SystemResult<OrderInfoView>> GetOrder(Guid id)
        {
           var result = new SystemResult<OrderInfoView>();
            result.ReturnValue = orderBLL.GetOrder(id);
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 創建退換單
        /// </summary>
        [LoginAuthorize]
        [HttpPost("CreateReturnOrder")]
        [ProducesResponseType(typeof(SystemResult<NewReturnOrder>), 200)]
        public async Task<SystemResult<NewReturnOrder>> CreateReturnOrder([FromForm] NewReturnOrder rOrder)
        {
             var result = await orderBLL.CreateReturnOrder(rOrder);
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 獲取快遞單的物流情況
        /// </summary>
        /// <param name="trackingNo"></param>
        /// <returns></returns>
        [HttpGet("GetOrderMailTrackingInfo")]
        [ProducesResponseType(typeof(SystemResult<MailTrackingInfo>), 200)]
        public async Task<SystemResult<MailTrackingInfo>> GetOrderMailTrackingInfo(string trackingNo)
        {
            var result = new SystemResult<MailTrackingInfo>();
            var data = orderBLL.GetOrderMailTrackingInfo(trackingNo);
            result.ReturnValue = data;
            result.Succeeded = true;
            return result;
        }
    }
}
