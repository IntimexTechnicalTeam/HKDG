using Autofac;
using HKDG.BLL.Impl;
using Model;
using Web.Mvc.Filters;

namespace HKDG.WebSite.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : BaseApiController
    {
        IEmailerBLL emailerBLL;
        IArrivalNotifyBLL arrivalNotifyBLL;

        public MessageController(IComponentContext service) : base(service)
        {            
            emailerBLL = Services.Resolve<IEmailerBLL>();
            arrivalNotifyBLL = Services.Resolve<IArrivalNotifyBLL>();
        }

        /// <summary>
        /// 獲取當前會員的消息列表
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        [HttpPost("GetMessage")]
        [ProducesResponseType(typeof(SystemResult<PageData<MessageFrontView>>), 200)]
        public async Task<SystemResult<PageData<MessageFrontView>>> GetMessage([FromForm]PageInfo pager)
        {
            var result = new SystemResult<PageData<MessageFrontView>>();
            result.ReturnValue = emailerBLL.GetUserMessage(pager);
            result.Succeeded = true;
            return result;
        }

        [LoginAuthorize]
        [HttpPost("CheckExsitArrivalNotify")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> CheckExsitArrivalNotify([FromForm]ArrivalNotifyCond cond)
        {
            var result =await arrivalNotifyBLL.CheckExsitArrivalNotify(cond);
            return result;
        }

        /// <summary>
        /// 取消到货通知
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        [LoginAuthorize]
        [HttpPost("CancelArrivalNotify")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task <SystemResult> CancelArrivalNotify([FromForm]ArrivalNotifyCond cond)
        {
            var result = await arrivalNotifyBLL.CancelArrivalNotify(cond);            
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        [LoginAuthorize]
        [HttpPost("AddArrivalNotify")]
        public async Task<SystemResult> AddArrivalNotify([FromForm] ArrivalNotifyCond cond)
        {
            var result =  await arrivalNotifyBLL.AddArrivalNotify(cond);
            return result;
        }

    }
}
