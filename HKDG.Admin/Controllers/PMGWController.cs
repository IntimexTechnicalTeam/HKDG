namespace HKDG.Admin.Controllers
{
    public class PMGWController : BaseMvcController
    {
        public PMGWController(IComponentContext service) : base(service)
        {
        }

        public ActionResult Wechat()
        {
            return View();
        }

        public ActionResult AliPay()
        {
            return View();
        }
        public ActionResult AliPayHK()
        {
            return View();
        }

        /// <summary>
        /// UNION
        /// </summary>
        /// <returns></returns>
        public ActionResult MPGS2()
        {
            return View();
        }

        /// <summary>
        /// MASTER
        /// </summary>
        /// <returns></returns>
        public ActionResult MPGS1()
        {
            return View();
        }

        public ActionResult Stripe()
        {
            return View();
        }

        public ActionResult PayMe()
        {
            return View();
        }

        public ActionResult Paypal()
        {
            return View();
        }
        public ActionResult Atome()
        {
            return View();
        }
    }
}