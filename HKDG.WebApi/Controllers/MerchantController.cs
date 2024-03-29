﻿namespace HKDG.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantController : BaseApiController
    {
        IMerchantBLL merchantBLL;

        public MerchantController(IComponentContext services) : base(services)
        {
            merchantBLL = Services.Resolve<IMerchantBLL>();
        }

        /// <summary>
        /// 获取商家列表
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("GetMerchantListAsync")]
        [ProducesResponseType(typeof(SystemResult<PageData<MicroMerchant>>), 200)]
        public async Task<SystemResult<PageData<MicroMerchant>>> GetMerchantListAsync(MerchantCond cond)
        {
            var result = new SystemResult<PageData<MicroMerchant>>() { Succeeded = true };
            result.ReturnValue = await merchantBLL.GetMerchantListAsync(cond);
            return result;
        }

        /// <summary>
        /// 获取商家明细信息
        /// </summary>
        /// <param name="merchID"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetMerchantInfo")]
        [ProducesResponseType(typeof(SystemResult<MerchantInfoView>), 200)]
        public async Task<SystemResult<MerchantInfoView>> GetMerchantInfo(Guid merchID)
        {
            var result = new SystemResult<MerchantInfoView>() { Succeeded = true };

            if (merchID==  Guid.Empty) throw new BLException();

            result.ReturnValue=await merchantBLL.GetMerchantInfoAsync(merchID);
            return result;
        }

       
    }
}
