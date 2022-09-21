using Autofac;
using HKDG.BLL;

namespace HKDG.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayController : BaseApiController
    {
        public IPaymentBLL paymentBLL;
        public IOrderBLL orderBLL;
        public IDealProductQtyCacheBLL dealProductQtyCacheBLL;
        public PayController(IComponentContext services) : base(services)
        {
            paymentBLL = Services.Resolve<IPaymentBLL>();
            orderBLL = Services.Resolve<IOrderBLL>();
            dealProductQtyCacheBLL = Services.Resolve<IDealProductQtyCacheBLL>();
        }

        /// <summary>
        /// 获取支付方式
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [ProducesResponseType(typeof(SystemResult<List<PaymentMethodView>>), 200)]
        [HttpGet("GetPaymentMethod")]
        public SystemResult<List<PaymentMethodView>> GetPaymentMethod()
        {
            var result = new SystemResult<List<PaymentMethodView>>();
            result.ReturnValue = paymentBLL.GetPaymentTypes();            
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 更新訂單的支付狀態
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(SystemResult), 200)]
        [HttpGet("UpdateOrderPaymentStatus")]
        public async Task<SystemResult> UpdateOrderPaymentStatus(Guid orderId, string type)
        {
            SystemResult result = new SystemResult();

            if (type == "S")    //支付成功
            {
                await orderBLL.UpdateOrderPayStatus(orderId);
                result.Succeeded = true;
            }
            else                    //支付失败
            {
                await orderBLL.UpdateOrderCancelStatus(orderId);
                await dealProductQtyCacheBLL.UpdateQtyWhenPayTimeOut(orderId);
                result.Succeeded = true;
            }
            return result;

        }

    }
}
