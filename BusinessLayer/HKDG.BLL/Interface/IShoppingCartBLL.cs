namespace HKDG.BLL
{
    public interface IShoppingCartBLL:IDependency
    {
        Task<ShopCartInfo> GetShoppingCart();

        Task<SystemResult> AddtoCartAsync(CartItem cartItem);

        Task<SystemResult> UpdateCartItemAsync(Guid itemId, int qty);

        Task<SystemResult> RemoveFromCart(MallItem cartItem);

        Task<SystemResult> CheckPurchasePermissionAsyncV2(CartItem item, int HoldQty);

        Task DisableMallCartItem(Guid memberId);

        decimal CalculateFreeQty(Guid MchId, string ProductCode, int CartItemQty);

        Task<MallCartInfo> GetShoppingCartAsync();

        //Task<SystemResult> UpdateCartItemAsyncV2(Guid itemId, int qty);

        Task<SystemResult> RemoveFromCartAsyncV2(MallItem cartItem);

        /// <summary>
        /// 获取已选中商品
        /// </summary>
        /// <param name="memberAddressId">会员地址</param>
        /// <returns></returns>
        Task<SystemResult<SelectedMallCart>> SelectedShopCartAsync(Guid memberAddressId);


        /// <summary>
        /// 保存购物车已选中商品
        /// </summary>
        /// <param name="cartIds"></param>
        /// <returns></returns>
        Task<SystemResult> SaveSelectedAsync(MallItem cartItem);

    }

}
