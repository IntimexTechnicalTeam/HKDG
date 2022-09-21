using Autofac;
using HKDG.BLL;
using HKDG.Domain;
using Intimex.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Intimex.Common;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using HKDG.Domain;
using HKDG.Enums;
using System.Threading;
using System.Globalization;
using HKDG.BLL;

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
