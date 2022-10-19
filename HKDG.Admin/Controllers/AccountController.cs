

namespace HKDG.Admin.Controllers
{
    
    public class AccountController : BaseMvcController
    {
        public AccountController(IComponentContext service) : base(service)
        {
        }

        [LanguageResource]
        [HttpGet]
        public ActionResult User()
        {
            return View();
        }

        [LanguageResource]
        [HttpGet]
        public ActionResult Role()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Login(string id)
        {
            ViewBag.CopyRight = "2341234";
            ViewBag.ID = Guid.NewGuid().ToString();
            return View();
        }

        public async Task<IActionResult> LogOff()
        {           
            return RedirectToAction("Login", "Account");
        }

    }
}
