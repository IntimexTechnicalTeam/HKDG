namespace HKDG.Admin.Controllers
{
    public class DefaultController : Controller
    {
       
        public IActionResult Index()
        {
            ViewBag.Language = Language.C;


            return View();
        }
    }
}
