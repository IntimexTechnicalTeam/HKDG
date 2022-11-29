using Web.Mvc.Filters;

namespace HKDG.WebSite.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : BaseApiController
    {
        IEmailerBLL emailerBLL;
        public MessageController(IComponentContext service) : base(service)
        {            
            emailerBLL = Services.Resolve<IEmailerBLL>();
        }

        /// <summary>
        /// 獲取當前會員的消息列表
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        [HttpPost("GetMessage")]
        public async Task<SystemResult<PageData<MessageFrontView>>> GetMessage([FromForm]PageInfo pager)
        {
            var result = new SystemResult<PageData<MessageFrontView>>();
            result.ReturnValue = emailerBLL.GetUserMessage(pager);
            result.Succeeded = true;
            return result;
        }

        //[LoginAuthorize]
        //[HttpPost("CheckExsitArrivalNotify")]
        //public SystemResult CheckExsitArrivalNotify(ArrivalNotifyCond cond)
        //{
        //    var sysRslt = new SystemResult();
        //    try
        //    {
        //        if (CurrentUser != null && CurrentUser.IsLogin)
        //        {
        //            Guid skuId = GetSkuId(cond);
        //            if (skuId != Guid.Empty)
        //            {
        //                sysRslt = ArrivalNotifyBLL.CheckExsitArrivalNotify(new ArrivalNotify()
        //                {
        //                    MemberId = CurrentUser.Id,
        //                    SkuId = skuId,
        //                });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        sysRslt.Succeeded = false;
        //        sysRslt.Message = ex.Message;
        //    }
        //    return sysRslt;
        //}
    }
}
