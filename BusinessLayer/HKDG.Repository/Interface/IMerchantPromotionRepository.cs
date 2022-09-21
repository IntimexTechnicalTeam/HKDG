namespace HKDG.Repository
{
    public interface IMerchantPromotionRepository:IDependency
    {
        MerchantPromotion GetApprovePromotion(Guid merchID);

        MerchantPromotion GetNotApprovePromotion(Guid merchID);
    }
}
