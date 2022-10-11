using Microsoft.AspNetCore.Mvc;

namespace HKDG.WebSite.Controllers
{
    [AllowAnonymous]
    public class ProductController : BaseMvcController
    {
        public ProductController(IComponentContext service) : base(service)
        {
        }

       
        public IActionResult Index()
        {
            return GetActionResult("Index");
        }
    }
}
