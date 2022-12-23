namespace HKDG.Admin.Controllers
{
    [LanguageResource]
    public class MemberController : WebController
    {
        public MemberController(IComponentContext service) : base(service)
        {

        }

        /// <summary>
        /// 會員搜尋主頁
        /// </summary>
        /// <returns></returns>
        // [ActionAuthorize(Module = ModuleConst.MemberModule)]
        public ActionResult Index(string id, string para2)
        {
            ViewBag.BDate = id;
            ViewBag.EDate = para2;
            ViewBag.PromoId = 0;
            ViewBag.PromoMember = "";
            return View();
        }

        public ActionResult LastMthMbr()
        {

            DateTime now = DateTime.Now;
            DateTime mon = new DateTime(now.Year, now.Month, 1);
            DateTime d1 = new DateTime(now.Year, now.Month - 1, 1);
            string date1 = d1.ToString("yyyy-MM-dd");
            DateTime d2 = d1.AddMonths(1).AddDays(-1);
            string date2 = d2.ToString("yyyy-MM-dd");
            return RedirectToAction("Index", new { id = date1, para2 = date2 });
        }

        public ActionResult ThisMthMbr()
        {

            DateTime now = DateTime.Now;
            DateTime mon = new DateTime(now.Year, now.Month, 1);
            string data1 = mon.ToString("yyyy-MM-dd");
            string date2 = now.ToString("yyyy-MM-dd");
            return RedirectToAction("Index", new { id = data1, para2 = date2 });
        }

        /// <summary>
        /// 會員選擇
        /// </summary>
        /// <param name="id">
        /// 0-AllMember
        /// 1-Buyer
        /// </param>
        /// <returns></returns>
        public ActionResult SelectMember(int id)
        {
            ViewBag.Type = id;
            return View();
        }


        /// <summary>
        /// 修改會員資料
        /// </summary>
        /// <param name="id">會員ID</param>
        /// <returns></returns>
        // [ActionAuthorize(Module = ModuleConst.MemberModule)]
        public ActionResult EditMember(string id)
        {
            ViewBag.Id = id;
            return View();
        }


        /// <summary>
        /// 會員組別
        /// </summary>
        /// <returns></returns>
        //  [ActionAuthorize(Module = ModuleConst.MemberModule)]
        public ActionResult MemberGroupIndex(/*string id*/)
        {
            //ViewBag.Id = id;
            return View();
        }

        /// <summary>
        /// 修改會員組別頁面
        /// </summary>
        /// <param name="id">組別ID</param>
        /// <returns></returns>
        //    [ActionAuthorize(Module = ModuleConst.MemberModule)]
        public ActionResult EditMemberGroup(string id)
        {
            if (id == "0")
            {
                ViewBag.Id = new Guid();
            }
            else
            {
                ViewBag.Id = id;
            }
            return View();
        }

        /// <summary>
        /// 會員組別管理頁面
        /// </summary>
        /// <returns></returns>
        //   [ActionAuthorize(Module = ModuleConst.MemberModule)]
        public ActionResult ManageMemberGroup()
        {
            return View();
        }
    }
}
