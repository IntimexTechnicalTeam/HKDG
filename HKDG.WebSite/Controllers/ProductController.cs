﻿using Domain;
using HKDG.BLL;
using System.Security.Cryptography;

namespace HKDG.WebSite.Controllers
{
   [Hidden]
    public class ProductController : WebController
    {
        IProductCatalogBLL productCatalogBLL;
        IProductBLL productBLL;
        IMerchantBLL merchantBLL;
        public ProductController(IComponentContext service) : base(service)
        {
            productCatalogBLL = Services.Resolve<IProductCatalogBLL>();
            productBLL = Services.Resolve<IProductBLL>();
            merchantBLL = Services.Resolve<IMerchantBLL>();
        }

        /// <summary>
        /// 类目
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Product/Category/{IspType?}")]  
        public async Task<IActionResult> Category(string IspType)
        {
            await InitViewPage(IspType);
            return GetActionResult("Category");
        }

        /// <summary>
        /// 商品详情
        /// </summary>
        /// <param name="Id">ProductCode</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Product/Detail/{Id}/{IspType?}")]
        //[ProductViewTrackFilter]
        public async Task<IActionResult> Detail(string Id,string IspType)
        {
            await InitViewPage(IspType);

            var details =await productBLL.GetProductDetailAsync(Id);
            SetViewData("ProudctDetail", details);

            var mchDetail = await merchantBLL.GetMerchantInfoAsync(details.MerchantId);            
            SetViewData("MerchantDetail", mchDetail);

            var relateProdList = productBLL.GetRelatedProduct(details.Id);
            SetViewData("RelateProd", relateProdList);

            SetViewData("Title", details.Name);

            return GetActionResult("Detail");
        }

        [AllowAnonymous]
        [HttpGet("Product/Search/{key?}/{IspType?}")]
        public async Task<ActionResult> Search(string key,string IspType)
        {
            await InitViewPage(IspType);
            if (key.IsEmpty())
                return RedirectToAction("Index", "Default");

            ViewBag.Key = key;         
            return GetActionResult("Search");

        }

        [AllowAnonymous]
        [HttpGet("Product/CatProduct/{Id}/{IspType?}")]
        public async Task<ActionResult> CatProduct(string Id, string IspType)
        {
            if (!Guid.TryParse(Id, out var catId)) catId = Guid.Empty;

            ViewBag.CatId = catId;
            await InitViewPage(IspType);
         
            var catalog =await productCatalogBLL.GetCatalogById(catId);
            SetViewData("CurrentCatalog", catalog);

            CatProdPager pager = new CatProdPager() { CatId = catId };                      
            if (!IsMobile)pager.PageSize = 9;
            var catProduct = await productBLL.GetCatProdPageData(pager);
            SetViewData("CatalogProducts", catProduct);

            SetViewData("Title", catalog.Name);

            return GetActionResult("CatProduct");

        }

        [AllowAnonymous]
        [HttpGet("Product/Comment/{Id}")]
        public async Task<ActionResult> Comment(string Id)
        {
            await InitViewPage();
            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.oId = Id;              
            }
            return GetActionResult("Comment");
        }
    }
}
