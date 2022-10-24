using Microsoft.AspNetCore.Mvc;

namespace HKDG.WebSite.Controllers
{
   
    public class ProductController : BaseMvcController
    {
        public ProductController(IComponentContext service) : base(service)
        {
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return GetActionResult("Index");
        }
    }
}
