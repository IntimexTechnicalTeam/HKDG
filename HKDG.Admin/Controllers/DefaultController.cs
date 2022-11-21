namespace HKDG.Admin.Controllers
{
    [LanguageResource]
    public class DefaultController : BaseMvcController
    {
        public DefaultController(IComponentContext service) : base(service)
        {
        }

        public IActionResult Index()
        {
            ViewBag.Lang = Language.C;
            ViewBag.UserName = CurrentUser?.Email;
            ViewBag.IspType = CurrentUser.IspType;
            
            return View("Index");
        }
    }
}
