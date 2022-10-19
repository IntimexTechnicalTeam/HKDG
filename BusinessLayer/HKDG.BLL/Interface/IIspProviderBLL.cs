namespace HKDG.BLL
{
    public interface IIspProviderBLL : IDependency
    {
        Task<bool> CheckIspType(string type);
    }
}
