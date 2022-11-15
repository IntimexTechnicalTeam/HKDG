using HKDG.BLL;

namespace HKDG.WebSite.Controllers
{

    public class OrderController : BaseMvcController
    {
        public OrderController(IComponentContext service) : base(service)
        {

        }

        [AllowAnonymous]
        public async Task<IActionResult> List()
        {            
            return GetActionResult("List");
        }
    }
}
