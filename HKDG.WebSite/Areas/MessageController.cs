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
        [HttpPost("GetMessage")]
        public async Task<SystemResult<PageData<MessageFrontView>>> GetMessage([FromForm]PageInfo pager)
        {
            var result = new SystemResult<PageData<MessageFrontView>>();
            result.ReturnValue = emailerBLL.GetUserMessage(pager);
            result.Succeeded = true;
            return result;
        }
    }
}
