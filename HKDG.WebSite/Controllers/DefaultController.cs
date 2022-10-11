using Autofac.Core;
using HKDG.WebSite.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HKDG.WebSite.Controllers
{
    [AllowAnonymous]
    public class DefaultController : BaseMvcController
    {   
        public DefaultController(IComponentContext service) : base(service)
        {
            
        }

       
        public IActionResult Index()
        {          
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