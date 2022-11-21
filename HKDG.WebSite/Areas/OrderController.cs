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
        [HttpPost("Create")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> Create([FromForm] NewOrder checkout)
        {
            SystemResult result = new SystemResult();

            if (checkout == null) throw new BLException(HKDG.Resources.Message.SaveFail);

            result = await orderBLL.BuildOrder(checkout);            
            return result;
        }
    }
}
