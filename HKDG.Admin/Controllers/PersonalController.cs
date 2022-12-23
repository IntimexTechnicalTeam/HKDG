namespace HKDG.Admin.Controllers
{
    // [ActionAuthorize(Module = ModuleConst.PersonalSetting)]
    public class PersonalController : WebController
    {
        public PersonalController(IComponentContext service) : base(service)
        {
        }

        // GET: Personal
        public ActionResult ChangePassword()
        {
            ViewBag.Lang = CurrentUser.Lang;
            return View();
        }

        public ActionResult NoPermission()
        {
            ViewBag.Lang = CurrentUser.Lang;
            return View();
        }
    }
}