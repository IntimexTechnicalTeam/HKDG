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
        public ISettingBLL SettingBLL;
        public IDeliveryBLL DeliveryBLL;


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
            SettingBLL = Services.Resolve<ISettingBLL>();
            DeliveryBLL = Services.Resolve<IDeliveryBLL>();
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

            string key = $"{PreHotType.Hot_Products}_{CurrentUser.Language}";
            var product = await RedisHelper.HGetAsync<HotProduct>(key, cartItem.ProdCode);
            if (product == null)
            {
                result.Succeeded = false;
                result.Message = Resources.Message.ProductNotExist;
                return result;
            }

            #region 处理sku
            key = $"{CacheKey.ProductSku}";
            var skuList = await RedisHelper.HGetAsync<List<ProductSku>>(key, cartItem.ProdCode);
            if (skuList == null || !skuList.Any())
            {
                var productSkuList = (await baseRepository.GetListAsync<ProductSku>(x => x.IsActive && !x.IsDeleted && x.ProductCode == cartItem.ProdCode)).ToList();
                if (productSkuList.Any())
                    await RedisHelper.HSetAsync(key, cartItem.ProdCode, productSkuList);

                //skuList = AutoMapperExt.MapToList<ProductSku, ProductSkuDto>(productSkuList);
                skuList = productSkuList;
            }
            var sku = skuList.FirstOrDefault(d => d.ProductCode == cartItem.ProdCode && d.AttrValue1 == cartItem.Attr1 && d.AttrValue2 == cartItem.Attr2 && d.AttrValue3 == cartItem.Attr3 && d.IsActive && !d.IsDeleted);
            if (sku == null)
            {
                
                throw new BLException("SKU is not exsit.");
            }
            cartItem.Sku = sku.Id;

            #endregion

            cartItem.AddQty = cartItem.Qty;
            var item = await baseRepository.GetModelAsync<ShoppingCartItem>(d => d.ProductId == cartItem.ProductId && d.SkuId == sku.Id && d.MemberId == CurrentUser.Id && d.IsActive && !d.IsDeleted && d.KolId == cartItem.KolId);

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
                    MemberId = CurrentUser.Id,
                    Qty = cartItem.Qty,
                    ProductId = cartItem.ProductId,
                    KolId = cartItem.KolId,
                    CreateBy = CurrentUser.Id,
                    UpdateBy = CurrentUser.Id,
                };

                var details = await GenShoppingCartItemDetail(item, cartItem.Attr1, cartItem.Attr2, cartItem.Attr3);
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

                var dbDetail = await baseRepository.GetModelAsync<ShoppingCartItemDetail>(x => x.ShoppingCartItemId == item.Id);
                var details = await GenShoppingCartItemDetail(item, dbDetail.AttrValue1, dbDetail.AttrValue2, dbDetail.AttrValue3);
                UnitOfWork.IsUnitSubmit = true;
                await baseRepository.UpdateAsync(item);
                await baseRepository.DeleteAsync(details.Item1);
                await baseRepository.InsertAsync(details.Item2);
                UnitOfWork.Submit();
            }

            result.Succeeded = true;
            result.Message = HKDG.Resources.Message.AddtoCartSuccess;
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
            //item.Qty = qty;
            //item.UpdateDate = DateTime.Now;

            int holdQty = qty + (int)CalculateFreeQty(product.MchId, product.Code, cartItem.Qty);
            result = await CheckPurchasePermissionAsyncV2(cartItem, holdQty);
            if (!result.Succeeded) return result;

            var dbCart = await baseRepository.GetModelAsync<ShoppingCartItem>(x => x.Id == itemId);
            dbCart.Qty = qty;
            dbCart.UpdateDate = DateTime.Now;
            var details = await GenShoppingCartItemDetail(dbCart);

            UnitOfWork.IsUnitSubmit = true;

            await baseRepository.UpdateAsync(dbCart);
            await baseRepository.DeleteAsync(item);
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
            if (item.Qty > Setting.PurchaseLimit) //最大購買量
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

            result = InventoryBLL.CheckQty(item.Sku, HoldQty);
            #endregion

            return result;
        }

        /// <summary>
        /// 购物车中的产品已下架，则把购物车数据设置为已失效,会员登录后，加载购物车清单时，定时任务执行
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public async Task DisableMallCartItem(Guid memberId, int top = 10)
        {
            var query = await baseRepository.GetListAsync<ShoppingCartItemDetail>(x => x.IsActive && !x.IsDeleted);
            if (memberId != Guid.Empty) query = query.Where(x => x.MemberId == CurrentUser.Id);

            query = query.Take(top);
            var details = query.ToList();
            string key = string.Empty;

            HotProduct p = null;
            foreach (var item in details)
            {
                p = await RedisHelper.HGetAsync<HotProduct>($"{PreHotType.Hot_Products}_C", item.ProductCode);
                if (p == null) p = await RedisHelper.HGetAsync<HotProduct>($"{PreHotType.Hot_Products}_E", item.ProductCode);
                if (p == null) p = await RedisHelper.HGetAsync<HotProduct>($"{PreHotType.Hot_Products}_S", item.ProductCode);
                if (p == null)
                {
                    var product = await baseRepository.GetModelAsync<Product>(x => x.Code == item.ProductCode && x.IsActive && !x.IsDeleted);
                    p = new HotProduct { Status = product.Status };
                }

                if (p == null) continue;
                if (p.Status != ProductStatus.OnSale)
                {
                    item.IsActive = false;
                    //item.IsDeleted = true;
                    item.UpdateDate = DateTime.Now;
                }
            }

            var Ids = details.Where(x => !x.IsActive && x.IsDeleted).Select(s => s.ShoppingCartItemId).Distinct().ToList();
            var result = (await baseRepository.GetListAsync<ShoppingCartItem>(x => Ids.Contains(x.Id))).ToList();
            foreach (var item in result)
            {
                item.IsActive = false;
                //item.IsDeleted = true;
                item.UpdateDate = DateTime.Now;

                key = $"{CacheKey.ShoppingCart}_{item.MemberId}";
                await RedisHelper.DelAsync(key);
            }

            await baseRepository.UpdateAsync(details);
            await baseRepository.UpdateAsync(result);

        }

        public async Task<MallCartInfo> GetShoppingCartAsync()
        {
            var info = new MallCartInfo();

            if (!CurrentUser.IsLogin) throw new BLException(Message.PleaseLogin);

            await DisableMallCartItem(CurrentUser.Id);
            double taxRate = SettingBLL.GetSalePriceTaxRate();

            var cartDetails = await baseRepository.GetListAsync<ShoppingCartItemDetail>(x => x.IsActive && !x.IsDeleted && x.MemberId == CurrentUser.Id);
            info.EnableMallCart = await GenMallCartItem(cartDetails.ToList());

            info.IsShowAdult = info.EnableMallCart.Any(x => x.ProductList.Any(v => v.IsShowAdult));
            info.HasPromotionRule = info.EnableMallCart.Any(x => x.ProductList.Any(v => v.RuleType != PromotionRuleType.None));

            info.Currency = CurrencyBLL.GetDefaultCurrency();
            info.TotalQty = info.EnableMallCart.Sum(s => s.ProductList.Sum(v => v.BuyQty));
            info.TotalAmount = info.EnableMallCart.Sum(s => s.ProductList.Sum(v => v.ItemAmount)) + info.DeliveryCharge;

            info.TotalTax = info.TotalAmount * (decimal)taxRate;
            info.TotalTax = PriceUtil.SystemPrice(info.TotalTax);

            //info.TotalDicount
            info.EventProductPrice = info.TotalAmount;

            #region 处理失效购物车

            var diableList = await baseRepository.GetListAsync<ShoppingCartItemDetail>(x => !x.IsActive && !x.IsDeleted && x.MemberId == CurrentUser.Id);
            if (diableList != null && diableList.Any())
            {
                info.DisableProductList = diableList.OrderByDescending(o => o.CreateDate)
                                                       .Select(item => new DisableProduct { Id = item.ShoppingCartItemId, ProductCode = item.ProductCode, SkuId = item.SkuId }).ToList();

                foreach (var item in info.DisableProductList)
                {
                    var imgList = await RedisHelper.HGetAsync<List<HotProductImage>>($"{PreHotType.Hot_ProductImage}", item.ProductCode);
                    var product = await RedisHelper.HGetAsync<HotProduct>($"{PreHotType.Hot_Products}_{CurrentUser.Language}", item.ProductCode);
                    item.ProductName = product.Name ?? "";
                    item.ProductImage = imgList?.FirstOrDefault(x => x.Type == ImageSizeType.S3)?.ImagePath ?? "";
                }
            }
            #endregion

            return info;
        }


        async Task<List<MallCartItem>> GenMallCartItem(List<ShoppingCartItemDetail> cartDetails)
        {
            List<MallCartItem> info = new List<MallCartItem>();
            if (cartDetails != null && cartDetails.Any())
            {
                var mchList = cartDetails.GroupBy(g => g.MerchantId).Select(m => m).ToList();
                foreach (var item in mchList)
                {
                    var mItem = new MallCartItem { MchId = item.Key };
                    foreach (var vv in item.ToList())
                    {
                        var prodItem = await GenMallCartProduct(vv);

                        #region promotion rule

                        //判斷該產品是否有promotion rule并計算rule
                        var rule = PromotionRuleRepository.GetProductPromotionRule(mItem.MchId, vv.ProductCode);
                        if (rule != null)
                        {
                            if (rule.PromotionRule == PromotionRuleType.GroupSale)            //多件优惠
                            {
                                SetGroupSalePrice(rule, prodItem);
                            }
                            else if (rule.PromotionRule == PromotionRuleType.BuySend)       //如果規則是買一送一，則添加贈品
                            {
                                var freeItem = GetFreeProduct(rule, prodItem);
                                if (freeItem != null)
                                {
                                    //追加赠品
                                    prodItem.RuleType = PromotionRuleType.BuySend;
                                    mItem.ProductList.Add(freeItem);
                                }
                            }
                            prodItem.RuleTitle = rule.Title;
                            prodItem.RuleId = rule.Id;
                        }

                        #endregion

                        mItem.ProductList.Add(prodItem);
                    }

                    var mch = await RedisHelper.HGetAsync<HotMerchant>($"{PreHotType.Hot_Merchants}_{CurrentUser.Language}", mItem.MchId.ToString());
                    mItem.MchName = mch?.Name ?? "";   //商家名称
                    mItem.ItemQty = mItem.ProductList.Sum(s => s.BuyQty);
                    mItem.ItemAmount = mItem.ProductList.Sum(s => s.ItemAmount);

                    info.Add(mItem);
                }
            }

            return info;
        }

        async Task<MallCartProduct> GenMallCartProduct(ShoppingCartItemDetail item)
        {
            var prodItem = item.DeepClone<MallCartProduct>();
            prodItem.Id = item.ShoppingCartItemId;
            prodItem.BuyQty = item.Qty;
            prodItem.RealQty = prodItem.BuyQty;

            var attr1 = await ShoppingCartRepository.GenMallCartProductAttr(item.AttrId1, item.AttrValue1);
            prodItem.AttrList.Add(attr1);
            var attr2 = await ShoppingCartRepository.GenMallCartProductAttr(item.AttrId2, item.AttrValue2);
            prodItem.AttrList.Add(attr2);
            var attr3 = await ShoppingCartRepository.GenMallCartProductAttr(item.AttrId3, item.AttrValue3);
            prodItem.AttrList.Add(attr3);

            var product = await RedisHelper.HGetAsync<HotProduct>($"{PreHotType.Hot_Products}_{CurrentUser.Language}", item.ProductCode);
            var imgList = await RedisHelper.HGetAsync<List<HotProductImage>>($"{PreHotType.Hot_ProductImage}", item.ProductCode);

            prodItem.CatalogId = product?.CatalogId ?? Guid.Empty;
            prodItem.ProductName = product?.Name ?? "";
            prodItem.RealPrice = product?.SalePrice ?? 0;
            prodItem.SalePrice = prodItem.RealPrice + item.AttrValue1Price + item.AttrValue2Price + item.AttrValue3Price;
            prodItem.ProductImage = imgList?.Where(x => x.Type == ImageSizeType.S1 || x.Type == ImageSizeType.S3).Select(i => i.ImagePath)?.ToList();

            prodItem.CartItemStatus = GenCartItemStatus(product, item.SkuId, prodItem.BuyQty);
            prodItem.Currency = CurrencyBLL.GetSimpleCurrency(product.CurrencyCode);

            //产品扩展属性
            var productExt = await baseRepository.GetModelByIdAsync<ProductExtension>(item.ProductId);
            prodItem.IsShowAdult = productExt?.IsShowAdult ?? false;
            prodItem.SaleQuota = productExt?.MaxPurQty ?? 0;

            var productSp = await baseRepository.GetModelByIdAsync<ProductSpecification>(item.ProductId);
            prodItem.IsSelected = item?.IsSelected ?? false;
            prodItem.Weight = productSp?.GrossWeight ?? 0;
            return prodItem;
        }

        CartItemStatus GenCartItemStatus(HotProduct product, Guid SkuId, int Qty)
        {
            CartItemStatus status = CartItemStatus.Normal;

            var ReservedQty = RedisHelper.ZScore($"{CacheKey.InvtReservedQty}", SkuId) ?? 0;
            var SalesQty = RedisHelper.ZScore($"{CacheKey.SalesQty}", SkuId) ?? 0;

            ///只要不是OnSale为已下架或已删除
            if (product.Status != ProductStatus.OnSale)
            {
                status = CartItemStatus.OffSale; return status;
            }

            ///冻结数大于SalesQty为库存不足
            if (ReservedQty >= SalesQty)
            {
                status = CartItemStatus.OutOfStock; return status;
            }

            ///低于安全库存时为库存紧张
            var ableQty = SalesQty - ReservedQty;  //真正可用的库存
            if (ableQty <= product.SafetyStock)
            {
                status = CartItemStatus.StressOfStock; return status;
            }

            return status;
        }

        /// <summary>
        /// 计算多件购买优惠
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="cartItem"></param>
        void SetGroupSalePrice(PromotionRuleView rule, MallCartProduct cartItem)
        {
            decimal setQty = Math.Floor(cartItem.BuyQty / rule.X);
            if (setQty >= 1)
            {
                cartItem.RuleType = PromotionRuleType.GroupSale;
                cartItem.GroupSaleDiscountPrice = Math.Round((setQty * rule.X * cartItem.SalePrice) - (setQty * rule.Y), 2);
                cartItem.SingleDiscountPrice = Math.Round(cartItem.GroupSaleDiscountPrice / cartItem.BuyQty, 2);
            }

        }
        MallCartProduct GetFreeProduct(PromotionRuleView rule, MallCartProduct cartItem)
        {
            MallCartProduct freeItem = null;
            decimal setQty = Math.Floor(cartItem.BuyQty / rule.X);

            if (setQty >= 1)
            {
                freeItem = new MallCartProduct();
                freeItem = cartItem.DeepClone<MallCartProduct>();
                freeItem.BuyQty = (int)setQty * (int)rule.Y;
                freeItem.IsFree = true;
                return freeItem;
            }
            return freeItem;
        }

        async Task<Tuple<List<ShoppingCartItemDetail>, ShoppingCartItemDetail>> GenShoppingCartItemDetail(ShoppingCartItem item, Guid AttrId1, Guid AttrId2, Guid AttrId3)
        {
            var detail = await ShoppingCartRepository.GetItemDetailAsync(item);
            var deleteDetails = (await baseRepository.GetListAsync<ShoppingCartItemDetail>(x => x.ShoppingCartItemId == item.Id)).ToList();

            var dbDetails = detail.DeepClone<ShoppingCartItemDetail>();
            dbDetails.Id = Guid.NewGuid();
            dbDetails.ShoppingCartItemId = item.Id;
            dbDetails.Qty = item.Qty;
            dbDetails.MemberId = item.MemberId;
            dbDetails.SkuId = item.SkuId;
            dbDetails.ProductId = item.ProductId;
            dbDetails.KolId = item.KolId;
            dbDetails.CreateBy = CurrentUser.Id;
            dbDetails.UpdateBy = CurrentUser.Id;
            dbDetails.CreateDate = DateTime.Now;

            dbDetails.AttrValue1Price = await GenAddPrice(item.ProductId, AttrId1);
            dbDetails.AttrValue2Price = await GenAddPrice(item.ProductId, AttrId2);
            dbDetails.AttrValue3Price = await GenAddPrice(item.ProductId, AttrId3);

            return Tuple.Create(deleteDetails, dbDetails);
        }

        async Task<decimal> GenAddPrice(Guid ProductId, Guid AttrId)
        {
            decimal price = decimal.Zero;

            var m = await ShoppingCartRepository.GetAddtionPrice(ProductId, AttrId);

            if (m == null) return price;
            price = m.AdditionalPrice;
            return price;
        }

        public async Task<SystemResult> RemoveFromCartAsyncV2(MallItem cartItem)
        {
            SystemResult result = new SystemResult();

            var checkList = (await baseRepository.GetListAsync<ShoppingCartItem>(x => x.MemberId == Guid.Parse(CurrentUser.UserId) && cartItem.Id.Contains(x.Id))).ToList();

            if (checkList == null || !checkList.Any()) throw new BLException("emptyData");

            foreach (var item in checkList.ToList())
            {
                item.IsDeleted = true;
                item.UpdateDate = DateTime.Now;
            }

            var recordDetail = (await baseRepository.GetListAsync<ShoppingCartItemDetail>(x => x.MemberId == Guid.Parse(CurrentUser.UserId) && cartItem.Id.Contains(x.ShoppingCartItemId))).ToList();

            UnitOfWork.IsUnitSubmit = true;
            await baseRepository.UpdateAsync(checkList);
            await baseRepository.DeleteAsync(recordDetail);
            UnitOfWork.Submit();
            result.Succeeded = true;
            result.Message = Resources.Message.RemoveCartItemSuccess;
            //await dealProductQtyCacheBll.UpdateQtyWhenDeleteCart(item.SkuId, item.Qty, item.Id);
            return result;
        }

        public async Task<SystemResult<SelectedMallCart>> SelectedShopCartAsync(Guid memberAddressId)
        {
            var result = new SystemResult<SelectedMallCart>();
            var info = new SelectedMallCart();

            if (!CurrentUser.IsLogin) throw new BLException(Resources.Message.PleaseLogin);

            double taxRate = SettingBLL.GetSalePriceTaxRate();

            string key = $"{CacheKey.ShoppingCart}_{CurrentUser.UserId}";
            var cacheData = await RedisHelper.GetAsync<MallCartInfo>(key);

            //优先读缓存
            if (cacheData == null)
            {
                //如果缓存没有，则重新设置
                cacheData = await GetShoppingCartAsync();
                await RedisHelper.SetAsync(key, cacheData);
            }

            //var cartDetails = await baseRepository.GetListAsync<ShoppingCartItemDetail>(x => x.MemberId == CurrentUser.Id && x.IsActive && !x.IsDeleted && cartItem.Id.Contains(x.ShoppingCartItemId) );
            var cartList = new List<MallCartItem>();
            foreach (var item in cacheData.EnableMallCart)
            {
                var pList = item.ProductList.Where(v => v.IsSelected).ToList();//增加购物车选中产品判断条件
                if (pList != null && pList.Any())
                {
                    MallCartItem cMall = item;
                    cMall.ProductList = pList.ToList();
                    cMall.ItemQty = cMall.ProductList.Sum(s => s.BuyQty);
                    cMall.ItemAmount = cMall.ProductList.Sum(s => s.ItemAmount);
                    //设置店铺地址
                    cMall.StoreAddress = await DeliveryBLL.GetStorePickUpAddress(item.MchId);
                    info.EnableMallCart.Add(cMall);
                }
            }

            if (info.EnableMallCart.Any())
            {
                info.HasPromotionRule = info.EnableMallCart.Any(x => x.ProductList.Any(v => v.RuleType != PromotionRuleType.None && v.IsSelected));//增加购物车选中产品判断条件
                info.TotalQty = info.EnableMallCart.Sum(s => s.ProductList.Where(v => v.IsSelected).Sum(v => v.BuyQty));//增加购物车选中产品判断条件
                info.TotalAmount = info.EnableMallCart.Sum(s => s.ProductList.Where(v => v.IsSelected).Sum(v => v.ItemAmount)) + info.DeliveryCharge;//增加购物车选中产品判断条件
                info.TotalTax = info.TotalAmount * (decimal)taxRate;
                info.TotalTax = PriceUtil.SystemPrice(info.TotalTax);

                await GetExpressCharge(info.EnableMallCart, memberAddressId);
            }


            //info.TotalDicount
            info.EventProductPrice = info.TotalAmount;
            info.Currency = CurrencyBLL.GetDefaultCurrency();
            result.ReturnValue = info;
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 获取商家付运方式
        /// </summary>
        /// <param name="cartItem"></param>
        /// <returns></returns>
        public async Task GetExpressCharge(List<MallCartItem> cartItem, Guid memberAddressId)
        {
            var deliveryAddressList = new List<DeliveryAddress>();
            #region 获取各个商家付运方式
            var memberAddress = (await baseRepository.GetListAsync<DeliveryAddress>(x => x.IsActive && !x.IsDeleted && x.MemberId == Guid.Parse(CurrentUser.UserId))).ToList();

            if (memberAddressId != Guid.Empty)
                memberAddress = memberAddress.Where(x => x.Id == memberAddressId).ToList();

            if (memberAddress.Any())
            {
                var DefaultId = Guid.Empty;
                //因为旧数据增加新地址很多都没有默认，所以如果存在有默认直接用默认没有就获取最新修改地址当默认
                if (memberAddress.Any(x => x.Default))
                    DefaultId = memberAddress.FirstOrDefault(x => x.Default).Id;
                else
                    DefaultId = memberAddress.OrderByDescending(o => o.CreateDate).FirstOrDefault().Id;

                foreach (var item in cartItem)
                {
                    var cond = new ExpressCondition();
                    cond.CCode = "Other";
                    cond.DeliveryAddrId = DefaultId;
                    cond.TCode = "D";
                    cond.MerchantId = item.MchId;
                    cond.TotalWeight = item.ProductList.Sum(s => s.Weight) * 0.001m;
                    cond.ItemAmount = item.ItemAmount;
                    cond.ProductWeightInfo = item.ProductList.Select(x => new ProductWeightInfo { Code = x.ProductCode, Qty = x.BuyQty }).ToList();

                    var result = DeliveryBLL.GetExpressChargeListByCode(cond);
                    if (result.Succeeded)
                        item.ExpressChargeList = JsonUtil.JsonToObject<List<ExpressChargeInfo>>(result.ReturnValue.ToString());
                    //result.ReturnValue as List<ExpressChargeInfo>;
                }
            }
            #endregion
        }


        public async Task<SystemResult> SaveSelectedAsync(MallItem cartItem)
        {
            var result = new SystemResult();




            try
            {
                var cartItems = (await baseRepository.GetListAsync<ShoppingCartItem>(x => x.IsActive && !x.IsDeleted && x.MemberId == Guid.Parse(CurrentUser.UserId))).ToList();
                var detailLists = (await baseRepository.GetListAsync<ShoppingCartItemDetail>(x => x.IsActive && !x.IsDeleted && x.MemberId == Guid.Parse(CurrentUser.UserId))).ToList();
                //重新选中要先把已选中的全部改为未选中
                foreach (var item in detailLists.Where(x => x.IsSelected && !cartItem.Id.Contains(x.ShoppingCartItemId)).ToList())
                {
                    item.IsSelected = false;
                    item.UpdateDate = DateTime.Now;
                    item.UpdateBy = Guid.Parse(CurrentUser.UserId);
                }

                foreach (var item in cartItems.Where(x => x.IsSelected && !cartItem.Id.Contains(x.Id)).ToList())
                {
                    item.IsSelected = false;
                    item.UpdateDate = DateTime.Now;
                    item.UpdateBy = Guid.Parse(CurrentUser.UserId);
                }


                foreach (var item in cartItems.Where(x => cartItem.Id.Contains(x.Id)).ToList())
                {
                    item.IsSelected = true;
                    item.UpdateDate = DateTime.Now;
                    item.UpdateBy = Guid.Parse(CurrentUser.UserId);
                }

                foreach (var item in detailLists.Where(x => cartItem.Id.Contains(x.ShoppingCartItemId)).ToList())
                {
                    item.IsSelected = true;
                    item.UpdateDate = DateTime.Now;
                    item.UpdateBy = Guid.Parse(CurrentUser.UserId);
                }
                // using (var tran = baseRepository.CreateTransation())

                UnitOfWork.IsUnitSubmit = true;

                await baseRepository.UpdateAsync(cartItems);
                await baseRepository.UpdateAsync(detailLists);
                // tran.Commit();

                UnitOfWork.Submit();

                result.Succeeded = true;
                result.ReturnValue = cartItem.Id;
                // }

                string key = $"{CacheKey.ShoppingCart}_{CurrentUser.UserId}";
                var cacheData = await RedisHelper.DelAsync(key);//清空用户ShoppingCart缓存
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                throw new BLException(ex.Message);
            }

            return result;
        }
    }
}
