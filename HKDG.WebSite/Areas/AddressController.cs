using Autofac;
using HKDG.BLL.Impl;
using Model;
using Web.Mvc.Filters;

namespace HKDG.WebSite.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : BaseApiController
    {
         IDeliveryAddressBLL deliveryAddressBLL;

        public AddressController(IComponentContext service) : base(service)
        {
            deliveryAddressBLL = Services.Resolve<IDeliveryAddressBLL>();
        }

        /// <summary>
        /// 获取会员地址
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        [HttpGet("GetAddresses")]
        [ProducesResponseType(typeof(SystemResult<List<DeliveryAddressDto>>), 200)]
        public async Task<SystemResult<List<DeliveryAddressDto>>> GetAddresses()
        {
            var result = new SystemResult<List<DeliveryAddressDto>>();
            result.ReturnValue =await deliveryAddressBLL.GetMemberAddress(CurrentUser.Id);
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 获取国家数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("Country")]
        [ProducesResponseType(typeof(SystemResult<List<CountryDto>>), 200)]
        public async Task<SystemResult<List<CountryDto>>> Country()
        {
            var result = new SystemResult<List<CountryDto>>();
            result.ReturnValue = await deliveryAddressBLL.GetCountries();
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 获取省份数据
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        [HttpGet("Province")]
        [ProducesResponseType(typeof(SystemResult<List<ProvinceDto>>), 200)]
        public async Task<SystemResult<List<ProvinceDto>>> Province(int countryId)
        {
            var result = new SystemResult<List<ProvinceDto>>();
            result.ReturnValue = await deliveryAddressBLL.GetProvinces(countryId);
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 新增送货地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [LoginAuthorize]
        [HttpPost("Add")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> Add([FromForm] AddressInfo address)
        {
            SystemResult result = new SystemResult();
            var model = AutoMapperExt.MapTo<DeliveryAddressDto>(address);
            model.Address = address.ContractAddress;
            result =await deliveryAddressBLL.CreateAddress(model);
            return result;
        }

        /// <summary>
        /// 修改送货地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [LoginAuthorize]
        [HttpPost("Update")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> Update([FromForm] AddressInfo address)
        {
            SystemResult result = new SystemResult();
            var model = AutoMapperExt.MapTo<DeliveryAddressDto>(address);
            model.Address = address.ContractAddress;
            result =await deliveryAddressBLL.UpdateAddress(model);
            return result;
        }

        /// <summary>
        /// 删除送货地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [LoginAuthorize]
        [HttpGet("Remove")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> Remove(Guid id)
        {
            SystemResult result = new SystemResult();
            result = await deliveryAddressBLL.RemoveAddress(id);
            return result;
        }

    }
}
