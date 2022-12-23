namespace HKDG.Admin.Controllers
{
    [LanguageResource]
    public class DesktopController : WebController
    {
        public DesktopController(IComponentContext service) : base(service)
        {
        }

        // GET: Desktop
        public async Task<ActionResult> Index()
        {
            if (CurrentUser == null)
                ViewBag.IsMerchant = 0;
            else
                ViewBag.IsMerchant = CurrentUser.IsMerchant.ToInt();

            return View();
        }
    }
}