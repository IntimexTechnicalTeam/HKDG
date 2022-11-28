﻿using HKDG.BLL;
using Intimex.Utility;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Web.Jwt;

namespace HKDG.WebSite.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCommentController : BaseApiController
    {
        IProductCommentBLL  productCommentBLL;

        public ProductCommentController(IComponentContext service) : base(service)
        {
            productCommentBLL = Services.Resolve<IProductCommentBLL>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("GetProductComments")]
        public async Task<SystemResult<List<ProductCommentDto>>> GetProductComments ([FromForm] SearchCommentsInfo cond)
        {
            var result = new SystemResult<List<ProductCommentDto>>();
            result.ReturnValue = await productCommentBLL.GetProductComments(cond);
            result.Succeeded = true;
            return result;
        }
    }
}