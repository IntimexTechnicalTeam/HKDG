namespace HKDG.Repository
{
    public interface IProductAttrValueRepository:IDependency
    {
        bool CheckHasInvRecordByAttrValueId(Guid attrValueId);
    }
}
