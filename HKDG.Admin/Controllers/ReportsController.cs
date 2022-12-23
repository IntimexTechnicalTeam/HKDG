namespace HKDG.Admin.Controllers
{
    public class ReportsController : WebController
    {
        public ReportsController(IComponentContext service) : base(service)
        {
        }

        IOrderBLL OrderBLL { get; set; }
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Orders()
        {
            return View();
        }
        public ActionResult STPAccountAllocation()
        {
            return View();
        }
        /// <summary>
        /// report 4-結算報表
        /// </summary>
        /// <returns></returns>
        public ActionResult FSDSettlement()
        {
            return View();
        }
        public ActionResult FSDTransactionSummary()
        {
            return View();
        }
        /// <summary>
        /// report 6-merchant月結報表
        /// </summary>
        /// <returns></returns>
        public ActionResult MonthlyStatementOutwardDetail()
        {
            ViewBag.Lang = CurrentUser.Lang;
            ViewBag.IsMerchant = CurrentUser.IsMerchant ? 1 : 0;
            return View();
        }
        /// <summary>
        /// report 2-所有訂單
        /// </summary>
        /// <returns></returns>
        public ActionResult FSDTransactionDetaileds()
        {
            return View();
        }
        /// <summary>
        /// report 7-未完成訂單
        /// </summary>
        /// <returns></returns>
        public ActionResult FSDOutstandingOrder()
        {
            return View();
        }
        /// <summary>
        /// report 5-退貨
        /// </summary>
        /// <returns></returns>
        public ActionResult FSDRefuntRequestReport()
        {
            return View();
        }
        /// <summary>
        /// report 3-已完成訂單
        /// </summary>
        /// <returns></returns>
        public ActionResult FSDRevenueReport()
        {
            return View();
        }
        public ActionResult TransactionDetailedsMainOrder()
        {
            ViewBag.IsMerchant = CurrentUser.IsMerchant ? 1 : 0;
            return View();
        }
        public ActionResult SummaryGroupByDCAItem()
        {
            ViewBag.Lang = CurrentUser.Lang;
            ViewBag.IsMerchant = CurrentUser.IsMerchant ? 1 : 0;
            return View();
        }
        public ActionResult SummaryGroupByWarehouse()
        {
            ViewBag.Lang = CurrentUser.Lang;
            ViewBag.IsMerchant = CurrentUser.IsMerchant ? 1 : 0;
            return View();
        }
        public ActionResult OrderDetails()
        {
            ViewBag.Lang = CurrentUser.Lang;
            return View();
        }
        public ActionResult OrderNetAmounts()
        {
            return View();
        }
        public ActionResult OrderDetailsByMerchant()
        {
            return View();
        }
        public ActionResult MallFunCouponsReport()
        {
            return View();
        }
        public ActionResult WishedItemsReport()
        {
            return View();
        }

        public ActionResult MallFunHistory()
        {
            return View();
        }

        public ActionResult ClickRateReport()
        {
            return View();
        }

        public ActionResult PBSettlementSummary()
        {
            return View();
        }
    }
}