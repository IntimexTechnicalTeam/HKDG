﻿using HKDG.BLL;
using Intimex.Utility;
using Model;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Web.Jwt;
using Web.Mvc.Filters;

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
        [HttpPost("GetProductComments")]
        [ProducesResponseType(typeof(SystemResult<List<ProductCommentDto>>), 200)]
        public async Task<SystemResult<List<ProductCommentDto>>> GetProductComments ([FromForm] SearchCommentsInfo cond)
        {
            var result = new SystemResult<List<ProductCommentDto>>();
            result.ReturnValue = await productCommentBLL.GetProductComments(cond);
            result.Succeeded = true;
            return result;
        }

        //[LoginAuthorize]
        //[HttpPost("SaveComments")]       
        //public async Task<SystemResult> SaveComments([FromForm] List<ProductComment> comments)
        //{
        //    SystemResult result =await productCommentBLL.SaveComments(comments);
        //    return result;
        //}

        [HttpGet("GetSubOrderComments")]
        [LoginAuthorize]
        [ProducesResponseType(typeof(SystemResult<List<ProductCommentDto>>), 200)]
        public async Task<SystemResult<List<ProductCommentDto>>> GetSubOrderComments(Guid subOrderId)
        {
            var result = new SystemResult<List<ProductCommentDto>>();
            result.ReturnValue =await productCommentBLL.GetSubOrderComments(subOrderId);
            result.Succeeded = true;
            return result;
        }
    }
}
