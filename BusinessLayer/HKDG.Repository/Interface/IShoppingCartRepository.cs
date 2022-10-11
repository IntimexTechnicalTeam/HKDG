namespace HKDG.Repository
{
    public interface IShoppingCartRepository:IDependency
    {
        /// <summary>
        /// 生成Detail
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        ShoppingCartItemDetailDto GetItemDetail(ShoppingCartItem item);

        Task<KeyValue> GenMallCartProductAttr(Guid AttrId, Guid AttrValueId);
    }
}
