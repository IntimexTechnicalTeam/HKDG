namespace HKDG.Admin.Areas.AdminAPI.Controllers
{

    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [ApiController]
    [AdminApiAuthorize(Module = ModuleConst.SystemModule)]
    public class EmailController : BaseApiController
    {
        public ILogBLL logBLL;

        public EmailController(IComponentContext services) : base(services)
        {
            logBLL = Services.Resolve<ILogBLL>();
        }
        [HttpPost]
        public async Task<PageData<SystemEmailsView>> GetEmails([FromForm]SystemEmailsCond cond)
        {
            return  logBLL.GetEmails(cond);
        }

    }
}
