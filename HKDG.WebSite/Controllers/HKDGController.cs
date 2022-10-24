using Autofac.Core;
using HKDG.WebSite.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HKDG.WebSite.Controllers
{
    public class HKDGController : BaseMvcController
    {
        IIspProviderBLL ispProviderBLL;

        public HKDGController(IComponentContext service) : base(service)
        {
            ispProviderBLL = Services.Resolve<IIspProviderBLL>();
        }

        //[AllowAnonymous]
       
        public async Task<IActionResult> Index(string IspType)
        {
            if (IspType.IsEmpty()) IspType = "DG";
            var flag = await ispProviderBLL.CheckIspType(IspType);
            if (!flag) throw new BLException($"wrong IspType: {IspType}");

            ViewBag.IspType = IspType;
            await InitViewPage(IspType);
            return GetActionResult("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}