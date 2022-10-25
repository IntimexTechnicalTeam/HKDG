using Microsoft.AspNetCore.Mvc;

namespace HKDG.WebSite.Controllers
{
   
    public class ProductController : BaseMvcController
    {
        public ProductController(IComponentContext service) : base(service)
        {
        }

        [AllowAnonymous]
        public IActionResult Category()
        {            
            return GetActionResult("Category");
        }

        [AllowAnonymous]
        public IActionResult Detail(Guid Id)
        {
            return GetActionResult("Detail",Id);
        }
    }
}
