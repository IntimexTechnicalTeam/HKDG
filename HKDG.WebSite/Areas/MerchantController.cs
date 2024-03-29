﻿using Autofac;
using HKDG.BLL;
using HKDG.BLL.Impl;
using Model;
using Web.Mvc.Filters;

namespace HKDG.WebSite.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantController : WebController
    {
         IProductBLL productBLL;

        public MerchantController(IComponentContext service) : base(service)
        {
            productBLL = Services.Resolve<IProductBLL>();
        }

        /// <summary>
        /// 获取商家下的产品分页数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("GetProducts")]
        [ProducesResponseType(typeof(SystemResult<PageData<ProductSummary>>), 200)]
        public async Task<SystemResult<PageData<ProductSummary>>> GetProducts([FromForm] ProductSearchCond condition)
        {
            var result = new SystemResult<PageData<ProductSummary>>();

            var cond = new ProdSearchCond { Key = "", PageInfo = condition.PageInfo, 
                ProductSearchType = ProductSearchType.OnSaleProduct,
                MerchantId = condition.MerchantId.IsEmpty() ? Guid.Empty :Guid.Parse(condition.MerchantId),
            };

            result.ReturnValue = await productBLL.SearchFrontProductSummaryAsync(cond);
            //result.ReturnValue  = productBLL.SearchFrontProductSummary(cond);
            result.Succeeded = true;
            return result;
        }
    }
}
