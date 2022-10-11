namespace HKDG.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : BaseApiController
    {
        public IShoppingCartBLL shoppingCartBLL;
 
        public ShoppingCartController(IComponentContext services) : base(services)
        {
            shoppingCartBLL = Services.Resolve<IShoppingCartBLL>();
        }

        ///// <summary>
        ///// 获取购物车
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost("GetShopCartAsync")]
        //[ProducesResponseType(typeof(SystemResult<ShopCartInfo>), 200)]
        //public async Task<SystemResult<ShopCartInfo>> GetShopCartAsync()
        //{
        //    var result = new SystemResult<ShopCartInfo> { Succeeded = true };
        //    result.Succeeded = CurrentUser?.IsLogin ?? false;
        //    if (!result.Succeeded) throw new BLException("请登录");
             
        //    string key = $"{CacheKey.ShoppingCart}_{CurrentUser.UserId}";
        //    //读缓存
        //    var cacheData = await RedisHelper.GetAsync<ShopCartInfo>(key);
        //    if (cacheData == null)  cacheData =await RedisHelper.CacheShellAsync(key, 600, async () =>await shoppingCartBLL.GetShoppingCart());

        //    result.ReturnValue = cacheData;
        //    return result;
        //}

        /// <summary>
        /// 加入购物车
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost("AddItem")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> AddItem([FromBody] CartItem item)
        {
            SystemResult result = new SystemResult { Succeeded = true };
            result.Succeeded = CurrentUser?.IsLogin ?? false;
            if (!result.Succeeded) throw new BLException("请登录");

            result = await shoppingCartBLL.AddtoCartAsync(item);
            string key = $"{CacheKey.ShoppingCart}_{CurrentUser.UserId}";
            await RedisHelper.DelAsync(key);
            return result;
        }

        ///// <summary>
        ///// 更新购物车项目数量
        ///// </summary>
        ///// <param name="id">购物车项目Id</param>
        ///// <param name="qty">更新后的数量</param>
        ///// <returns></returns>
        //[HttpGet("UpdateItemQty")]
        //[ProducesResponseType(typeof(SystemResult), 200)]
        //public async Task<SystemResult> UpdateItemQty(Guid id, int qty)
        //{
        //    SystemResult result = new SystemResult();

        //    result.Succeeded = CurrentUser?.IsLogin ?? false;
        //    if (!result.Succeeded) throw new BLException("请登录");
        //    result = await shoppingCartBLL.UpdateCartItemAsync(id, qty);

        //    //直接删除
        //    string key = $"{CacheKey.ShoppingCart}_{CurrentUser.UserId}";
        //    await RedisHelper.DelAsync(key);
        //    return result;
        //}

        ///// <summary>
        ///// 删除购物车项目
        ///// </summary>
        ///// <param name="mallItem"></param>
        ///// <returns></returns>
        ///// <exception cref="BLException"></exception>
        //[HttpGet("RemoveItem")]
        //[ProducesResponseType(typeof(SystemResult), 200)]
        //public async Task<SystemResult> RemoveItem(MallItem mallItem)
        //{
        //    SystemResult result = new SystemResult();

        //    result.Succeeded = CurrentUser?.IsLogin ?? false;
        //    if (!result.Succeeded) throw new BLException("请登录");
        //    result = await shoppingCartBLL.RemoveFromCart(mallItem);

        //    //直接删除
        //    string key = $"{CacheKey.ShoppingCart}_{CurrentUser.UserId}";
        //    await RedisHelper.DelAsync(key);
        //    return result;
        //}


        /// <summary>
        /// 获取购物车
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetShopCartAsync")]
        [ProducesResponseType(typeof(SystemResult<MallCartInfo>), 200)]
        public async Task<SystemResult<MallCartInfo>> GetShopCartAsync()
        {
            var result = new SystemResult<MallCartInfo> { Succeeded = true };
            result.Succeeded = CurrentUser?.IsLogin ?? false;
            if (!result.Succeeded) throw new BLException("请登录");

            string key = $"{CacheKey.ShoppingCart}_{CurrentUser.UserId}";
            //读缓存
            var cacheData = await RedisHelper.GetAsync<MallCartInfo>(key);
            if (cacheData == null) cacheData = await RedisHelper.CacheShellAsync(key, 600, async () => await shoppingCartBLL.GetShoppingCartAsync());

            result.ReturnValue = cacheData;
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 更新购物车项目数量
        /// </summary>
        /// <param name="id">购物车项目Id</param>
        /// <param name="qty">更新后的数量</param>
        /// <returns></returns>
        [HttpGet("UpdateItemQty")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> UpdateItemQty(Guid id, int qty)
        {
            SystemResult result = new SystemResult();
            result.Succeeded = CurrentUser?.IsLogin ?? false;
            if (!result.Succeeded) throw new BLException("请登录");
            result = await shoppingCartBLL.UpdateCartItemAsync(id, qty);

            //直接删除
            string key = $"{CacheKey.ShoppingCart}_{CurrentUser.UserId}";
            await RedisHelper.DelAsync(key);
            return result;
        }

        /// <summary>
        /// 删除购物车项目
        /// </summary>
        /// <param name="mallItem"></param>
        /// <returns></returns>
        /// <exception cref="BLException"></exception>
        [HttpPost("RemoveItem")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> RemoveItem(MallItem mallItem)
        {
            SystemResult result = new SystemResult();

            result.Succeeded = CurrentUser?.IsLogin ?? false;
            if (!result.Succeeded) throw new BLException("请登录");
            result = await shoppingCartBLL.RemoveFromCartAsyncV2(mallItem);

            //直接删除
            string key = $"{CacheKey.ShoppingCart}_{CurrentUser.UserId}";
            await RedisHelper.DelAsync(key);
            return result;
        }

        /// <summary>
        /// 选中购物车数据
        /// </summary>
        /// <param name="memberAddressId"></param>
        /// <returns></returns>
        [HttpGet("SelectedShopCartAsync")]
        [ProducesResponseType(typeof(SystemResult<SelectedMallCart>), 200)]
        public async Task<SystemResult<SelectedMallCart>> SelectedShopCartAsync(string memberAddressId)
        {
            var Id = Guid.Empty;
            var result = new SystemResult<SelectedMallCart>();
            if (!string.IsNullOrEmpty(memberAddressId))
                Guid.TryParse(memberAddressId, out Id);
            result = await shoppingCartBLL.SelectedShopCartAsync(Id);
            return result;
        }

        /// <summary>
        /// 选中购物车数据
        /// </summary>
        /// <param name="cartItem"></param>
        /// <returns></returns>
        [HttpPost("SaveSelectedAsync")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> SaveSelectedAsync(MallItem cartItem)
        {
            var result = new SystemResult();
            result = await shoppingCartBLL.SaveSelectedAsync(cartItem);
            return result;
        }
    }
}
