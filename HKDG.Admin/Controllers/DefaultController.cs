namespace HKDG.Admin.Controllers
{
    [LanguageResource]
    public class DefaultController : WebController
    {
        public DefaultController(IComponentContext service) : base(service)
        {
        }

        public IActionResult Index()
        {
            ViewBag.Lang = CurrentUser.Lang;
            ViewBag.UserName = CurrentUser?.Email;
            ViewBag.IspType = CurrentUser.IspType;
            
            return View("Index");
        }
    }
}
