using HKDG.BLL;

namespace HKDG.WebSite.Controllers
{

    public class OrderController : BaseMvcController
    {
        public OrderController(IComponentContext service) : base(service)
        {

        }

        [HttpGet("Order/List/{IspType?}")]
        public async Task<IActionResult> List(string IspType)
        {
            await InitViewPage(IspType);
            return GetActionResult("List");
        }

        [HttpGet("Order/Detail/{Id}/{IspType?}")]
        public async Task<IActionResult> Detail(string Id, string IspType)
        {
            await InitViewPage(IspType);
            return GetActionResult("Detail");
        }
    }
}
