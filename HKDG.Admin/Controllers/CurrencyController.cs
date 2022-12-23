namespace HKDG.Admin.Controllers
{

    /// <summary>
    /// 產品Controller
    /// </summary>
    // [ActionAuthorize(Module = ModuleConst.ProductModule)]
    [LanguageResource]
    public class CurrencyController : WebController
    {
        public CurrencyController(IComponentContext service) : base(service)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CurrencySetting()
        {
            return View();
        }

        public ActionResult CurrencyEdit(string id, string para2)
        {
            ViewBag.Code = para2;
            ViewBag.EditType = id;
            return View();
        }

    }
}