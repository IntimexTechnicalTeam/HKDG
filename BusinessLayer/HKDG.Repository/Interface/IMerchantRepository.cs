namespace HKDG.Repository
{
    public interface IMerchantRepository:IDependency
    {
        PageData<MerchantView> SearchMerchByCond(MerchantPageInfo condition);
    }
}
