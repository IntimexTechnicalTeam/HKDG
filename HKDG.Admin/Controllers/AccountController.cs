

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
        public async Task<IActionResult> Login(string id)
        {
            ViewBag.CopyRight = "2341234";
            ViewBag.IspType = id ?? "DG";
            return View();
        }

        public async Task<IActionResult> LogOff()
        {
            var access_token = HttpContext.Request?.Cookies["access_token"];

            HttpContext.Response.Cookies.Delete("access_token");
            await RedisHelper.HDelAsync($"{CacheKey.CurrentUser}", access_token);

            return RedirectToAction("Login", "Account");
        }

    }
}
