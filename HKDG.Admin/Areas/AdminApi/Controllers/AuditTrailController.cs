namespace HKDG.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [ApiController]
    /// <summary>
    /// 計劃任務
    /// </summary>
    public class AuditTrailController : BaseApiController
    {

        public IAuditTrailBLL AuditTrailBLL;
        public AuditTrailController(IComponentContext services) : base(services)
        {
            AuditTrailBLL = services.Resolve<IAuditTrailBLL>();
        }
        
        [HttpPost]
        public PageData<MemberLoginRecordDto> GetMemAuditTrail(MemLoginRecPager pageInfo)
        {
            return AuditTrailBLL.GetMemAuditTrail(pageInfo);
        }
    }

}
