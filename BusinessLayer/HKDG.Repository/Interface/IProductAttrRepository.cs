namespace HKDG.Repository
{
    public interface IProductAttrRepository:IDependency
    {
        List<ProductAttrDto> GetAttributeItemsMappByProductId(Guid prodID);
    }
}
