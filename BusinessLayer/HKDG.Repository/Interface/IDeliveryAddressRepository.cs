namespace HKDG.Repository
{
    public interface IDeliveryAddressRepository : IDependency
    {
        /// <summary>
        /// 搜寻送货地址
        /// </summary>
        /// <returns></returns>
        Task< List<DeliveryAddressDto>> SearchAddress(Guid memberId, bool isActive, bool isDeleted);

        /// <summary>
        /// 搜尋會員的香港本地地址
        /// </summary>
        List<DeliveryAddressDto> SearchLocalAddress(Guid memberId, bool isActive, bool isDeleted);

        /// <summary>
        /// 搜尋會員的海外地址清單
        /// </summary>
        List<DeliveryAddressDto> SearchOverseasAddress(Guid memberId, bool isActive, bool isDeleted);

        Task UpdateOtherAddressNotDefault(Guid memberId);

        Task<DeliveryAddressDto> GetByKey(Guid id);

        Task Update(DeliveryAddressDto model);
        void Insert(DeliveryAddressDto model);
    }
}
