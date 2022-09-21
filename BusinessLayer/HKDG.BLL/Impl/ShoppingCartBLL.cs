using HKDG.Domain;
using HKDG.Domain.ShoppingCart;
using HKDG.Repository;
using WebCache;

namespace HKDG.BLL
{
    public class ShoppingCartBLL : BaseBLL, IShoppingCartBLL
    {
        public IDealProductQtyCacheBLL dealProductQtyCacheBLL;
        public IInventoryBLL InventoryBLL;
        public IProductBLL ProductBLL;
        public IShoppingCartRepository ShoppingCartRepository;
        public IAttributeBLL AttributeBLL;
        public ICurrencyBLL CurrencyBLL;
        public ITranslationRepository TranslationRepository;
        public IPromotionRuleRepository PromotionRuleRepository;
        public IMerchantBLL MerchantBLL;
        public ICodeMasterBLL CodeMasterBLL;
        

        public ShoppingCartBLL(IServiceProvider services) : base(services)
        {
            dealProductQtyCacheBLL = Services.Resolve<IDealProductQtyCacheBLL>();
            InventoryBLL = Services.Resolve<IInventoryBLL>(); 
            ProductBLL = Services.Resolve<IProductBLL>();
            ShoppingCartRepository = Services.Resolve<IShoppingCartRepository>();
            AttributeBLL= Services.Resolve<IAttributeBLL>();
            CurrencyBLL = Services.Resolve<ICurrencyBLL>();
            TranslationRepository = Services.Resolve<ITranslationRepository>();
            PromotionRuleRepository = Services.Resolve<IPromotionRuleRepository>();
            MerchantBLL = Services.Resolve<IMerchantBLL>();
            CodeMasterBLL = Services.Resolve<ICodeMasterBLL>();
        }

        public async Task<ShopCartInfo> GetShoppingCart()
        {
            var shoppingCart = new ShopCartInfo();
            shoppingCart.Items = await GetShoppingCartItem();

            double taxRate = 0;
            foreach (var item in shoppingCart.Items)
            {
                decimal totalPrice = (item.Qty - item.FreeQty) * (item.Product.SalePrice + item.AttrValue1.AddPrice + item.AttrValue2.AddPrice + item.AttrValue3.AddPrice);
                shoppingCart.ItemsAmount += totalPrice;
                shoppingCart.ItemsTaxAmount += totalPrice * (decimal)taxRate;
                shoppingCart.TotalWeight += item.GrossWeight * item.Qty;
            }

            shoppingCart.ItemsTaxAmount = PriceUtil.SystemPrice(shoppingCart.ItemsTaxAmount);
            shoppingCart.TotalAmount = shoppingCart.ItemsAmount + shoppingCart.DeliveryCharge;
            shoppingCart.Currency = CurrencyBLL.GetSimpleCurrency(CurrentUser.CurrencyCode);
            shoppingCart.Qty = shoppingCart.Items.Sum(d => d.Qty);
            shoppingCart.IsTemp = !CurrentUser.IsLogin;
            
            return shoppingCart;
        }

        public async Task<SystemResult> AddtoCartAsync(CartItem cartItem)
        {
            SystemResult result = new SystemResult();

            if (string.IsNullOrEmpty(cartItem.ProdCode)) throw new BLException("缺少ProdCode参数");

            string key = $"{PreHotType.Hot_Products}_{CurrentUser.Lang}";
            var product = await RedisHelper.HGetAsync<HotProduct>(key, cartItem.ProdCode);
            if (product == null)
            {
                result.Succeeded = false;
                result.Message = Resources.Message.ProductNotExist;
                return result;
            }

            #region 处理sku
            key = $"{CacheKey.ProductSku}";
            var skuList = await RedisHelper.HGetAllAsync<ProductSku>(key);
            if (skuList == null || !skuList.Any())
            {
                skuList = (await baseRepository.GetListAsync<ProductSku>(x => x.IsActive && !x.IsDeleted)).ToDictionary(x => x.Id.ToString());
                foreach (var m in skuList.Values)
                {
                    await RedisHelper.HSetAsync(key, m.Id.ToString(), m);
                }
            }
            var sku = skuList.Values.FirstOrDefault(d => d.ProductCode == cartItem.ProdCode && d.AttrValue1 == cartItem.Attr1 && d.AttrValue2 == cartItem.Attr2 && d.AttrValue3 == cartItem.Attr3 && d.IsActive && !d.IsDeleted);
            if (sku == null)
            {             
                throw new BLException("SKU is not exsit.");
            }
            cartItem.Sku = sku.Id;

            #endregion

            cartItem.AddQty = cartItem.Qty;
            var item = await baseRepository.GetModelAsync<ShoppingCartItem>(d => d.ProductId == cartItem.ProductId && d.SkuId == sku.Id && d.MemberId ==Guid.Parse(CurrentUser.UserId) && d.IsActive && !d.IsDeleted && d.KolId == cartItem.KolId);

            int holdQty = cartItem.Qty + (int)CalculateFreeQty(product.MchId, product.Code, cartItem.Qty);
            if (item != null) holdQty = cartItem.Qty;

            result = await CheckPurchasePermissionAsyncV2(cartItem, holdQty);
            if (!result.Succeeded) return result;

            if (item == null)
            {
                item = new ShoppingCartItem
                {
                    Id = Guid.NewGuid(),
                    SkuId = cartItem.Sku,
                    MemberId = Guid.Parse(CurrentUser.UserId),
                    Qty = cartItem.Qty,
                    ProductId = cartItem.ProductId,
                    KolId = cartItem.KolId,
                    CreateBy =Guid.Parse( CurrentUser.UserId),
                    UpdateBy = Guid.Parse(CurrentUser.UserId),
                };

                var details = await GenShoppingCartItemDetail(item);
                UnitOfWork.IsUnitSubmit = true;
                await baseRepository.InsertAsync(item);
                await baseRepository.DeleteAsync(details.Item1);
                await baseRepository.InsertAsync(details.Item2);
                UnitOfWork.Submit();
            }
            else
            {

                item.Qty += cartItem.Qty;
                item.UpdateDate = DateTime.Now;

                var details = await GenShoppingCartItemDetail(item);
                UnitOfWork.IsUnitSubmit = true;
                await baseRepository.UpdateAsync(item);
                await baseRepository.DeleteAsync(details.Item1);
                await baseRepository.InsertAsync(details.Item2);
                UnitOfWork.Submit();
            }

            result.Succeeded = true;
            result.Message = Resources.Message.AddtoCartSuccess;
            //await dealProductQtyCacheBll.UpdateQtyWhenAddToCart(sku.Id, holdQty, item.Id);
            return result;
           
        }

        public async Task<SystemResult> UpdateCartItemAsync(Guid itemId, int qty)
        {
            SystemResult result = new SystemResult();
            var item = await baseRepository.GetModelAsync<ShoppingCartItemDetail>(x => x.ShoppingCartItemId == itemId);
            if (item == null) throw new BLException("wrong shoppingcartdata");

            string key = $"{PreHotType.Hot_Products}_{CurrentUser.Lang}";
            var product = await RedisHelper.HGetAsync<HotProduct>(key, item.ProductCode);
            if (product == null)
            {
                result.Succeeded = false;
                result.Message = Resources.Message.ProductNotExist;
                return result;
            }

            int oldQty = item.Qty;
            CartItem cartItem = new CartItem();
            cartItem.ProductId = item.ProductId;
            cartItem.Sku = item.SkuId;
            cartItem.Qty = qty;
            cartItem.AddQty = qty - item.Qty;//計算增量
            item.Qty = qty;
            item.UpdateDate = DateTime.Now;

            int holdQty = item.Qty + (int)CalculateFreeQty(product.MchId, product.Code, cartItem.Qty);
            result = await CheckPurchasePermissionAsyncV2(cartItem, holdQty);
            if (!result.Succeeded) return result;

            var dbCart = await baseRepository.GetModelAsync<ShoppingCartItem>(x => x.Id == item.Id);
            var details = await GenShoppingCartItemDetail(dbCart);

            UnitOfWork.IsUnitSubmit = true;
            await baseRepository.UpdateAsync(item);
            await baseRepository.DeleteAsync(details.Item1);
            await baseRepository.InsertAsync(details.Item2);
            UnitOfWork.Submit();
            result.Succeeded = true;
            result.Message = Resources.Message.UpdateSuccess;
            // await this.dealProductQtyCacheBll.UpdateQtyWhenModifyCart(item.SkuId, qty, oldQty, item.Id);

            return result;
        }

        public async Task<SystemResult> RemoveFromCart(MallItem cartItem)
        {
            SystemResult result = new SystemResult();

            var checkList = (await baseRepository.GetListAsync<ShoppingCartItem>(x => x.MemberId == Guid.Parse(CurrentUser.UserId) && cartItem.Id.Contains(x.Id))).ToList();

            if (checkList == null || !checkList.Any()) throw new BLException("emptyData");

            foreach (var item in checkList.ToList())
            {
                item.IsDeleted = true;
                item.UpdateDate = DateTime.Now;
            }

            var recordDetail = await baseRepository.GetListAsync<ShoppingCartItemDetail>(x => x.MemberId == Guid.Parse(CurrentUser.UserId) && cartItem.Id.Contains(x.ShoppingCartItemId));

            UnitOfWork.IsUnitSubmit = true;
            await baseRepository.UpdateAsync(checkList);
            await baseRepository.DeleteAsync(recordDetail);
            UnitOfWork.Submit();
            result.Succeeded = true;
            result.Message = Resources.Message.RemoveCartItemSuccess;
            //await dealProductQtyCacheBll.UpdateQtyWhenDeleteCart(item.SkuId, item.Qty, item.Id);
            return result;
        }

        private async Task<List<ShopcartItem>> GetShoppingCartItem()
        {
            var query = (await baseRepository.GetListAsync<ShoppingCartItemDetail>(x => x.MemberId == Guid.Parse(CurrentUser.UserId) && x.IsActive && !x.IsDeleted)).ToList()
                            .Select(item => new ShopcartItem
                            {
                                Id = item.Id,
                                SkuId = item.SkuId,
                                CartItemType = ShoppingCartItemType.BUYDONG,
                                AttrValue1 = new ProdAttValue { Id = item.AttrValue1, AddPrice = item.AttrValue1Price },
                                AttrValue2 = new ProdAttValue { Id = item.AttrValue2, AddPrice = item.AttrValue2Price },
                                AttrValue3 = new ProdAttValue { Id = item.AttrValue3, AddPrice = item.AttrValue3Price },
                                Attr1 = new ProdAtt { Id = item.AttrId1 },
                                Attr2 = new ProdAtt { Id = item.AttrId2 },
                                Attr3 = new ProdAtt { Id = item.AttrId3 },
                                Qty = item.Qty,
                                Product = new ProductSummary()
                                {
                                    ProductId = item.ProductId,
                                    Code = item.ProductCode,
                                    MerchantId = item.MerchantId,
                                }
                            });

            foreach (var item in query)
            {
                var product = baseRepository.GetModelById<Product>(item.Product.ProductId);

                item.AttrList = AttributeBLL.GetInvAttributeByProductWithMapForFront(item.Product.ProductId);
                item.Product.MerchantName = MerchantBLL.GetMerchById(item.Product.MerchantId).Name ?? "";
                item.Product.Name = TranslationRepository.GetDescForLang(product.NameTransId, CurrentUser.Lang);
                item.Product.Currency = CurrencyBLL.GetSimpleCurrency(product.CurrencyCode);
                item.Product.Imgs = ProductBLL.GetProductImages(item.Product.ProductId);
                item.Product.MarkupPrice = product.MarkUpPrice;
                item.Product.SalePrice = product.SalePrice + product.MarkUpPrice;
                item.Product.OriginalPrice = product.OriginalPrice + product.MarkUpPrice;
                item.Product.CatalogId = product.CatalogId;
                item.Product.ApproveType = product.Status;
                item.Product.CreateDate = product.CreateDate;
                item.Product.UpdateDate = product.UpdateDate;

                GenPromotionRule(item);
            }
            return query.ToList();
        }

        private void GenPromotionRule(ShopcartItem cartItem)
        {
            //判斷該產品是否有promotion rule并計算rule
            var rule = PromotionRuleRepository.GetProductPromotionRule(cartItem.Product.MerchantId, cartItem.Product.Code);

            //如果規則是買一送一，則在購物車上添加贈送的數量
            if (rule != null)
            {
                if (rule.PromotionRule == PromotionRuleType.BuySend)
                {
                    var freeItem = GetFreeProduct(rule, cartItem);
                    if (freeItem != null)
                    {
                        cartItem.Qty += freeItem.Qty;
                        cartItem.FreeQty += freeItem.Qty;
                    }
                }
                else if (rule.PromotionRule == PromotionRuleType.GroupSale)
                {
                    SetGroupSalePrice(rule, cartItem);
                }
            }
        }

        public ShopcartItem GetFreeProduct(PromotionRuleView rule, ShopcartItem cartItem)
        {
            ShopcartItem freeItem = new ShopcartItem();
            decimal setQty = Math.Floor(cartItem.Qty / rule.X);

            if (setQty >= 1)
            {
                ClassUtility.CopyValue(freeItem, cartItem);
                freeItem.Qty = (int)setQty * (int)rule.Y;
                freeItem.IsFree = true;
                freeItem.RuleId = Guid.Empty;
                ProductSummary product = new ProductSummary();
                ClassUtility.CopyValue(product, cartItem.Product);
                product.SalePrice = 0;
                product.SalePrice2 = 0;
                //product.IconRType = ProductType.FreeR;
                product.IconRUrl = PathUtil.GetProductIconUrl(product.IconRType,CurrentUser.Lang);
                //product.IconLType = ProductType.FreeL;
                product.IconLUrl = PathUtil.GetProductIconUrl(product.IconLType,CurrentUser.Lang);
                freeItem.Product = product;
                return freeItem;
            }
            else
            {
                return null;
            }
        }

        public void SetGroupSalePrice(PromotionRuleView rule, ShopcartItem cartItem)
        {

            decimal setQty = Math.Floor(cartItem.Qty / rule.X);

            if (setQty >= 1)
            {
                cartItem.RuleId = rule.Id;
                cartItem.RuleType = PromotionRuleType.GroupSale;
                cartItem.GroupSaleDiscountPrice = Math.Round((setQty * rule.X * cartItem.Product.SalePrice) - (setQty * rule.Y), 2);

                //var a = (setQty * rule.Y) + ((cartItem.Qty - (setQty * rule.X)) * cartItem.Product.SalePrice);
                cartItem.SingleDiscountPrice = Math.Round(cartItem.GroupSaleDiscountPrice / cartItem.Qty, 2);
            }

        }
    
        async Task<Tuple<List<ShoppingCartItemDetail>, ShoppingCartItemDetail>> GenShoppingCartItemDetail(ShoppingCartItem item)
        {
            var detail =  ShoppingCartRepository.GetItemDetail(item);
            var deleteDetails = (await baseRepository.GetListAsync<ShoppingCartItemDetail>(x => x.ShoppingCartItemId == item.Id)).ToList();

            var dbDetails = AutoMapperExt.MapTo<ShoppingCartItemDetailDto, ShoppingCartItemDetail>(detail);
            dbDetails.Id = Guid.NewGuid();
            dbDetails.ShoppingCartItemId = item.Id;
            dbDetails.Qty = item.Qty;
            dbDetails.MemberId = item.MemberId;
            dbDetails.SkuId = item.SkuId;
            dbDetails.ProductId = item.ProductId;
            dbDetails.KolId = item.KolId;
            dbDetails.CreateBy = Guid.Parse( CurrentUser.UserId);
            dbDetails.UpdateBy = Guid.Parse(CurrentUser.UserId);
            dbDetails.CreateDate = DateTime.Now;

            return Tuple.Create(deleteDetails, dbDetails);
        }

        public decimal CalculateFreeQty(Guid MchId, string ProductCode, int CartItemQty)
        {
            decimal freeQty = 0;
            var rule = PromotionRuleRepository.GetProductPromotionRule(MchId, ProductCode);//贈品都要Hold住
            if (rule != null)//計算贈品價格
            {
                if (rule.PromotionRule == PromotionRuleType.BuySend)
                {
                    decimal setQty = Math.Floor(CartItemQty / rule.X);

                    freeQty = setQty * rule.Y;
                }
            }
            return freeQty;
        }

        public async Task<SystemResult> CheckPurchasePermissionAsyncV2(CartItem item, int HoldQty)
        {
            var result = new SystemResult { Succeeded = true };
            var prodExt = await baseRepository.GetModelAsync<ProductExtension>(x => x.Id == item.ProductId);

            #region 判斷購買數量限制
            var qtyLimit = new { Min = prodExt.MinPurQty, Max = prodExt.MaxPurQty };
            if (qtyLimit.Min == 0)
            {
                //不限制

            }
            else
            {
                //限制購買 
                if (item.Qty < qtyLimit.Min)
                {
                    result.Succeeded = false;
                    var error = new SystemError()
                    {
                        Code = (int)OrderErrorEnum.LessThenMin,
                        Description = GetErrorDesription((int)OrderErrorEnum.LessThenMin, CurrentUser.Lang)
                    };

                    result.Message = error.Message;
                    result.ReturnValue = error;
                    return result;
                }
            }
            if (qtyLimit.Max == 0)
            {
                //不限制

            }
            else
            {
                //限制購買   
                if (item.Qty > qtyLimit.Max)
                {
                    result.Succeeded = false;
                    var error = new SystemError()
                    {
                        Code = (int)OrderErrorEnum.MoreThenMax,
                        Description = string.Format(GetErrorDesription((int)OrderErrorEnum.MoreThenMax, CurrentUser.Lang), qtyLimit.Max)
                    };

                    result.Message = error.Message;
                    result.ReturnValue = error;
                    return result;
                }
            }
            if (item.Qty > Runtime.Setting.PurchaseLimit) //最大購買量
            {
                result.Succeeded = false;
                var error = new SystemError()
                {
                    Code = (int)OrderErrorEnum.MoreThenMax,
                    //Description = string.Format(GetErrorDesription((int)OrderErrorEnum.MoreThenMax, ReturnDataLanguage), HKDG.Runtime.Setting.PurchaseLimit)
                };

                result.Message = error.Message;
                result.ReturnValue = error;
                return result;
            }
            #endregion

            #region 判斷是否有庫存

            result = await InventoryBLL.CheckSalesQtyAsync(item.Sku, item.AddQty, HoldQty);
            #endregion

            return result;
        }

        /// <summary>
        /// 购物车中的产品已下架，则把购物车数据设置为已失效,会员登录后，加载购物车清单时，定时任务执行
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public async Task DisableMallCartItem(Guid memberId)
        {
            var details = (await baseRepository.GetListAsync<ShoppingCartItemDetail>(x => x.IsActive && !x.IsDeleted && x.MemberId ==Guid.Parse(CurrentUser.UserId))).ToList();
            if (memberId != Guid.Empty) details = details.Where(x => x.MemberId == Guid.Parse(CurrentUser.UserId)).ToList();

            string key = $"{PreHotType.Hot_Products}_{CurrentUser.Lang}";
            foreach (var item in details)
            {
                var p = await RedisHelper.HGetAsync<HotProduct>(key, item.ProductCode);
                if (p.Status != ProductStatus.OnSale)
                {
                    item.IsActive = false;
                    item.IsDeleted = true;
                    item.UpdateDate = DateTime.Now;
                }
            }

            var Ids = details.Where(x => !x.IsActive && x.IsDeleted).Select(s => s.ShoppingCartItemId).Distinct().ToList();
            var result = (await baseRepository.GetListAsync<ShoppingCartItem>(x => Ids.Contains(x.Id))).ToList();
            foreach (var item in result)
            {
                item.IsActive = false;
                item.IsDeleted = true;
                item.UpdateDate = DateTime.Now;
            }

            await baseRepository.UpdateAsync(details);
            await baseRepository.UpdateAsync(result);

            key = $"{CacheKey.ShoppingCart}_{CurrentUser.UserId}";
            await RedisHelper.DelAsync(key);
        }

        
    }
}
