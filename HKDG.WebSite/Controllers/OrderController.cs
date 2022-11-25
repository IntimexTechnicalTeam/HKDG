using HKDG.BLL;

namespace HKDG.WebSite.Controllers
{

    public class OrderController : BaseMvcController
    {
        public OrderController(IComponentContext service) : base(service)
        {

        }

        public async Task<IActionResult> List(string IspType)
        {
            await InitViewPage(IspType);
            return GetActionResult("List");
        }
    }
}
