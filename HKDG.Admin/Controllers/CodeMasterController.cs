namespace HKDG.Admin.Controllers
{

    /// <summary>
    /// 產品Controller
    /// </summary>
    // [ActionAuthorize(Module = ModuleConst.ProductModule)]
    [LanguageResource]
    public class CodeMasterController : WebController
    {
        public CodeMasterController(IComponentContext service) : base(service)
        {
        }

        /// <summary>
        /// 字碼主檔主頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 字碼主檔修改頁面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            ViewBag.cId = id;
            return View();
        }

    }
}