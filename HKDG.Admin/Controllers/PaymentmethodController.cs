namespace HKDG.Admin.Controllers
{

    /// <summary>
    /// 產品Controller
    /// </summary>
    [LanguageResource]
    public class PaymentmethodController : BaseMvcController
    {
        public PaymentmethodController(IComponentContext service) : base(service)
        {
        }

        // GET: PaymentMgmt
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 新增或修改支付方式内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditPayMethod(string id, string para2)
        {
            ViewBag.Type = para2;
            ViewBag.Id = new Guid(id);
            return View();
        }

    }
}