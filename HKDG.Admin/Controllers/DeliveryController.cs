﻿namespace HKDG.Admin.Controllers
{

    /// <summary>
    /// 產品Controller
    /// </summary>
    [LanguageResource]
    public class DeliveryController : WebController
    {
        public DeliveryController(IComponentContext service) : base(service)
        {
        }
        ///<summary>
        /// 國家設定頁
        /// </summary>

        /// <returns></returns>
        public ActionResult Country()
        {
            return View();
        }
        ///<summary>
        /// 省份設定頁
        /// </summary>
        /// <param name="id">国家ID</param>
        /// <returns></returns>
        public ActionResult Province(int id)
        {
            ViewBag.CountryID = id;
            return View();
        }
        ///<summary>
        /// 快遞設定頁
        /// </summary>
        /// <param name="id">规则ID</param>
        /// <param name="para2">快递ID</param>
        /// <param name="para3">商家Id</param>
        /// <returns></returns>
        public ActionResult TransRuleEdit(string id, string para2, string para3)
        {
            ViewBag.RuleId = new Guid(id);
            ViewBag.exId = new Guid(para2);
            ViewBag.MerchId = new Guid(para3);
            return View();
        }
        ///<summary>
        /// 地區設定頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Zone()
        {
            return View();
        }
        ///<summary>
        /// 運費設定頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Charge()
        {
            return View();
        }

        ///<summary>
        /// 快递
        /// </summary>
        /// <returns></returns>
        public ActionResult Expressage()
        {
            return View();
        }
        ///<summary>
        /// 快递
        /// </summary>
        /// <returns></returns>
        public ActionResult Discount()
        {
            return View();
        }
        ///<summary>
        /// 地区設定頁
        /// </summary>
        /// <param name="id">地区ID</param>
        /// <returns></returns>
        public ActionResult ZoneEdit(string id)
        {
            ViewBag.Id = new Guid(id);
            return View();
        }

        /// <summary>
        /// 搜索快遞公司
        /// </summary>
        /// <param name="id"></param>
        /// <param name="para2">商家ID</param>
        /// <param name="para3"></param>
        /// <returns></returns>
        public ActionResult SelectExpressCompany(int id, string para2, string para3)
        {          
            if (para2.IsEmpty() ||  para2 == "0") 
                ViewBag.MerchantId = Guid.Empty.ToString();
            else
                ViewBag.MerchantId = para2;

            return View();
        }

        public ActionResult StoreAddress()
        {
            ViewBag.IsMerchant = CurrentUser.IsMerchant ? 1 : 0;
            ViewBag.MerchantId = CurrentUser.IsMerchant ? CurrentUser.MerchantId : Guid.Empty;

            string key = $"{PreHotType.Hot_Merchants}_{CurrentUser.Lang}";
            var mchInfo = RedisHelper.HGet<HotMerchant>(key, CurrentUser.MerchantId.ToString());
            ViewBag.MerchantName = mchInfo?.Name ?? Resources.Value.Mall;
            return View();
        }

        public ActionResult StoreAddressEdit(string id)
        {
            ViewBag.IsMerchant = CurrentUser.IsMerchant ? 1 : 0;
            ViewBag.MerchantId = CurrentUser.IsMerchant ? CurrentUser.MerchantId : Guid.Empty;
            string key = $"{PreHotType.Hot_Merchants}_{CurrentUser.Lang}";
            var mchInfo = RedisHelper.HGet<HotMerchant>(key, CurrentUser.MerchantId.ToString());
            ViewBag.MerchantName = mchInfo?.Name ?? Resources.Value.Mall;
            ViewBag.RelevanceId = new Guid(id);
            return View();
        }

    }
}