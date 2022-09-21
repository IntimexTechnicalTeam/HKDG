using Autofac;
using HKDG.BLL;
using HKDG.Domain;
using HKDG.Enums;
using Intimex.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;

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
