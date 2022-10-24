

namespace HKDG.Admin.Controllers
{
   
    public class AccountController : Controller
    {
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
        public async Task<IActionResult> Login(string IspType)
        {
            ViewBag.CopyRight = "2341234";
            ViewBag.IspType = IspType ?? "DG";
            return View();
        }

        public async Task<IActionResult> LogOff()
        {           
            return RedirectToAction("Login", "Account");
        }

    }
}
