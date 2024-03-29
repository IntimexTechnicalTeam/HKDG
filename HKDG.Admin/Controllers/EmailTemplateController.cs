﻿namespace HKDG.Admin.Controllers
{

    /// <summary>
    /// 產品Controller
    /// </summary>
    [LanguageResource]
    public class EmailTemplateController : WebController
    {
        public EmailTemplateController(IComponentContext service) : base(service)
        {
        }

        /// <summary>
        /// 主頁
        /// </summary>
        /// <returns></returns>
        // GET: Admin/EmailTemplate
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增和修改頁面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TemplateEdit(string id)
        {
            ViewBag.TId = id;
            return View();
        }


        public ActionResult TempItem()
        {
            return View();
        }

        [HttpGet]
        public ActionResult EditTempItem(Guid? id)
        {
            ViewBag.TId = id ?? Guid.Empty;
            return View();

        }

        public ActionResult EmailTypeItems()
        {
            return View();
        }


        public ActionResult EditEmailTypeItems()
        {
            return View();
        }

    }
}