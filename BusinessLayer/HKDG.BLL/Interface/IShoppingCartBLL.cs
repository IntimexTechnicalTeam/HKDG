using HKDG.Domain.ShoppingCart;

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
    }

}
