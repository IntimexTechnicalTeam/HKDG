using Enums;

namespace HKDG.BLL
{
    public class IspProviderBLL : BaseBLL, IIspProviderBLL
    {
        public IspProviderBLL(IServiceProvider services) : base(services)
        {
        }

        public async Task<bool> CheckIspType(string type)
        {           
            var flag = await baseRepository.AnyAsync<IspProvider>(x => x.IsActive && !x.IsDeleted && x.IspType.ToUpper().Equals(type.ToUpper()));
            return flag;
        }
    }
}
