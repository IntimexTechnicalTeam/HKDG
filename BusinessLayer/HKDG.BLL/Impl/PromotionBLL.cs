using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebCache;

namespace HKDG.BLL
{
    public class PromotionBLL : BaseBLL, IPromotionBLL
    {
        private Dictionary<PrmtLayoutType, PromotionCond> dicLayOutType = new Dictionary<PrmtLayoutType, PromotionCond>();

        ICodeMasterBLL codeMasterBLL;
        IProductBLL productBLL;

        public PromotionBLL(IServiceProvider services) : base(services)
        {
            dicLayOutType.Add(PrmtLayoutType.None, new PromotionCond { ShowBanner = false, ShowMerchant = false, ShowProduct = false });
            dicLayOutType.Add(PrmtLayoutType.BannerLayout, new PromotionCond { ShowBanner = true, ShowMerchant = false, ShowProduct = false });
            dicLayOutType.Add(PrmtLayoutType.ProductLayout, new PromotionCond { ShowBanner = false, ShowMerchant = false, ShowProduct = true });
            dicLayOutType.Add(PrmtLayoutType.MerchantLayout, new PromotionCond { ShowBanner = false, ShowMerchant = true, ShowProduct = false });
            dicLayOutType.Add(PrmtLayoutType.MerchantProLayout, new PromotionCond { ShowBanner = false, ShowMerchant = true, ShowProduct = true });

            codeMasterBLL = Services.Resolve<ICodeMasterBLL>();
            productBLL = Services.Resolve<IProductBLL>();
        }

        /// <summary>
        /// 获取首页推广数据
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        public async Task<List<PromotionPage>> ShowPromotionList(PromotionCond cond)
        {
            string key = $"{CacheKey.PageSizeSetting}";
            var pageSize = await RedisHelper.HGetAllAsync<int>(key);
            if (pageSize.Any())
            {
                cond.ProductSize = pageSize.FirstOrDefault(x => x.Key == $"{CacheField.ProductPageSize}").Value;
                cond.MerchantSize = pageSize.FirstOrDefault(x => x.Key == $"{CacheField.MerchantPageSize}").Value;
                cond.MerchantProductSize = pageSize.FirstOrDefault(x => x.Key == $"{CacheField.MerchantProductSize}").Value;
            }

            var prmtLst = await GetPromotionCacheDataByCond(cond);
            if (prmtLst != null && prmtLst.Any()) {

                foreach (var promotion in prmtLst)
                {
                    var prmtDetailId = (await GetPromotionDetailsIdByCond(cond, promotion.Id)).FirstOrDefault();
                    if (prmtDetailId != Guid.Empty)
                    {
                        cond.ShowBanner = dicLayOutType[promotion.PrmtLstType].ShowBanner;
                        cond.ShowProduct = dicLayOutType[promotion.PrmtLstType].ShowProduct;
                        cond.ShowMerchant = dicLayOutType[promotion.PrmtLstType].ShowMerchant;

                        if (cond.ShowProduct)
                        {
                            promotion.PrmtProductList = await GetPromotionProductFromCache(promotion.Id, prmtDetailId, cond);
                        }

                        if (cond.ShowBanner)
                        {
                            promotion.BannerList = await GetPromotionBannerListFromCache(promotion.Id, prmtDetailId);
                        }

                        if (cond.ShowMerchant)
                        {
                            promotion.MerchantList = await GetPromotionMerchantFromCache(promotion.Id, prmtDetailId, cond);
                        }
                    }
                }

                key = CacheKey.Favorite.ToString();
                string favField = CurrentUser.UserId;
                var cacheData = await RedisHelper.HGetAsync<Favorite>(key, favField);

                Action<PromotionPage> FavoriteAction = (m) =>
                {
                    m.PrmtProductList.ForEach(product =>
                    {
                        //会员喜欢的商品
                        var favoritePro = cacheData?.ProductList?.Any(x => x == product.Code) ?? false;
                        if (favoritePro) product.IsFavorite = true;

                    });

                    m.MerchantList.ForEach(merchant =>
                    {

                        //会员喜欢的商家
                        var favoritePro = cacheData?.MchList?.Any(x => x == merchant.Id) ?? false;
                        if (favoritePro) merchant.IsFavorite = true;
                    });
                };

                Action<PromotionPage> CurrencyAction = (m) =>
                {
                    m.PrmtProductList.ForEach(product =>
                    {
                        product.Currency2 = currencyBLL.GetSimpleCurrency(CurrentUser.CurrencyCode);
                        product.SalePrice2 = MoneyConversion(product.SalePrice);
                        product.OriginalPrice2 = MoneyConversion(product.OriginalPrice);
                    });
                };

               
                //不管有没有登录，都检查一下有没有设置过其他货币
                if (!IsMatchBaseCurrency(CurrentUser.CurrencyCode))
                    SetPersonalData(prmtLst, CurrencyAction);

                //处理用户登录后的数据
                if (CurrentUser.IsLogin)
                    SetPersonalData(prmtLst, FavoriteAction);

            }

            return prmtLst;
        }


        async Task<List<PromotionPage>> GetPromotionCacheDataByCond(PromotionCond cond)
        {
            var lang = CurrentUser.Lang;
            string key = $"{PreHotType.Hot_PromotionView}_{lang}";
            var cacheData = await RedisHelper.HGetAllAsync<HotPromotionView>(key);
            //读数据库，回写缓存
            //if (!cacheData.Any() || !cacheData.Values.Any())
            //{
            //    var view = await preHeatPromotionViewService.GetDataSourceAsync(Guid.Empty);
            //    if (view != null && view.Any())
            //    {
            //        await preHeatPromotionViewService.SetDataToHashCache(view, lang);
            //        cacheData = view.Where(x => x.LangType == lang).ToDictionary(x => x.Id.ToString());
            //    }
            //}

            var query = cacheData.Values.Where(x => x.ClientType == cond.From && x.LangType == lang && x.IspType == cond.IspType)
                                            .Select(s => new PromotionPage
                                            {
                                                Id = s.Id,
                                                Seq = s.Seq,
                                                Name = s.Name,
                                                Desc = s.Desc,
                                                Page = s.Page,
                                                PrmtLstType = s.PrmtLType,
                                                ClientType = s.ClientType,
                                                Lang = lang.ToString(),
                                                ImgPath = s.ImgPath,
                                            }).Distinct().AsQueryable();

            #region 

            if (!string.IsNullOrEmpty(cond.Name))
            {
                query = query.Where(x => x.Name.Contains(cond.Name.Trim()));
            }

            if (!string.IsNullOrEmpty(cond.Desc))
            {
                query = query.Where(x => x.Desc.Contains(cond.Desc.Trim()));
            }

            if ((int)cond.From > -1)
            {
                query = query.Where(x => x.ClientType == cond.From);
            }

            if (cond.Position != -1)
            {
                query = query.Where(x => x.Seq == cond.Position);
            }

            if (!string.IsNullOrEmpty(cond.PageStr))
            {
                query = query.Where(x => string.Equals(cond.PageStr, (x.Page ?? ""), StringComparison.CurrentCultureIgnoreCase));
            }

            #endregion

            var prmtLst = query.OrderBy(o => o.Seq).ToList();
            return prmtLst;
        }

        async Task<IQueryable<Guid>> GetPromotionDetailsIdByCond(PromotionCond cond, Guid PrmtId)
        {
            var lang = CurrentUser.Lang;
            string key = $"{PreHotType.Hot_PromotionView.ToString()}_{lang.ToString()}";
            var cacheData = await RedisHelper.HGetAllAsync<HotPromotionView>(key);
            //读数据库，回写缓存
            //if (!cacheData.Any() || !cacheData.Values.Any())
            //{
            //    var view = await preHeatPromotionViewService.GetDataSourceAsync(Guid.Empty);
            //    if (view != null && view.Any())
            //    {
            //        await preHeatPromotionViewService.SetDataToHashCache(view, CurrentUser.Language);
            //        cacheData = view.Where(x => x.LangType == CurrentUser.Language).ToDictionary(x => x.Id.ToString());
            //    }
            //}

            var query = cacheData.Values.Where(x => x.ClientType == cond.From && x.LangType == lang).ToList().AsQueryable();

            #region 

            if (!string.IsNullOrEmpty(cond.IspType))
                query = query.Where(x => x.IspType == cond.IspType);

            if (!string.IsNullOrEmpty(cond.Name))
            {
                query = query.Where(x => x.Name.Contains(cond.Name.Trim()));
            }

            if (!string.IsNullOrEmpty(cond.Desc))
            {
                query = query.Where(x => x.Desc.Contains(cond.Desc.Trim()));
            }

            if ((int)cond.From > -1)
            {
                query = query.Where(x => x.ClientType == cond.From);
            }

            if (cond.Position != -1)
            {
                query = query.Where(x => x.Seq == cond.Position);
            }

            //if (!string.IsNullOrEmpty(cond.PageStr))
            //{
            //    query = query.Where(x => x.Page.ToUpper() == cond.PageStr.ToUpper());
            //}

            #endregion

            var list = query.Where(x => x.Id == PrmtId).Select(s => s.PromotionDetailsId);
            return list;
        }

        /// <summary>
        ///  获取Banner明细数据
        /// </summary>
        /// <param name="PrmtId">Promotion.Id</param>
        /// <param name="PrmtDtlId">PromotionDetail.Id</param>
        /// <returns></returns>
        async Task<List<Banner>> GetPromotionBannerListFromCache(Guid PrmtId, Guid PrmtDtlId)
        {
            string key = $"{PreHotType.Hot_PromotionBanner}";
            var cacheData = await RedisHelper.HGetAllAsync<HotPromotionBanner>(key);
            //读数据库，回写缓存
            //if (!cacheData.Any() || !cacheData.Values.Any())
            //{
            //    var view = await preHeatPromotionBannerService.GetDataSourceAsync(Guid.Empty);
            //    if (view != null && view.Any())
            //    {
            //        await preHeatPromotionBannerService.SetDataToHashCache(view, Guid.Empty);
            //        cacheData = view.ToDictionary(x => x.Id.ToString());
            //    }
            //}

            var query = cacheData.Values.Where(x => x.PrmtID == PrmtId && x.PrmtDtlId == PrmtDtlId).OrderBy(o => o.Seq)
                                    .Select(s => new Banner
                                    {
                                        Id = s.Id,
                                        Content = s.Content,
                                        Image = s.Image,
                                        Seq = s.Seq,
                                        Url = s.Url,
                                    });
            return query.ToList();
        }

        /// <summary>
        /// 获取PromotionProduct数据
        /// </summary>
        /// <param name="PrmtId">Promotion.Id</param>
        /// <param name="PrmtDtlId">PromotionDetail.Id</param>
        /// <param name="cond"></param>
        /// <returns></returns>
         async Task<List<ProductDto>> GetPromotionProductFromCache(Guid PrmtId, Guid PrmtDtlId, PromotionCond cond)
        {
            var list = new List<ProductDto>();

            #region 初始化

            var lang = CurrentUser.Lang;
            string promtionKey = $"{PreHotType.Hot_PromotionProduct}";
            string productKey = $"{PreHotType.Hot_Products}_{lang}";
            string mchKey = $"{PreHotType.Hot_Merchants}_{lang}";
            string mchFeeChargeKey = $"{PreHotType.Hot_MerchantFreeCharge}";
            string simpleKey = $"{CacheKey.System}";
            string field = $"{CacheField.Info}_{lang}";
            string hotTopKey = $"{PreHotType.Hot_PreProductStatistics}";

            var promotionCache = await RedisHelper.HGetAllAsync<HotPromotionProduct>(promtionKey);
            var productCache = await RedisHelper.HGetAllAsync<HotProduct>(productKey);
            var currcencyCache = await RedisHelper.HGetAsync<SystemInfoDto>(simpleKey, field);
            var mchFeeChargeCache = await RedisHelper.HGetAllAsync<HotMerchantFreeCharge>(mchFeeChargeKey);
            var hotTopProductsCache = await RedisHelper.HGetAllAsync<HotPreProductStatistics>(hotTopKey);

            #region 读数据库，回写缓存

            //if (!promotionCache.Any() || !promotionCache.Values.Any())
            //{
            //    var view = await preHeatPromotionProductService.GetDataSourceAsync(Guid.Empty);
            //    if (view != null && view.Any())
            //    {
            //        await preHeatPromotionProductService.SetDataToHashCache(view, Guid.Empty);
            //        promotionCache = view.ToDictionary(x => x.Id.ToString());
            //    }
            //}

            //if (!productCache.Any() || !productCache.Values.Any())
            //{
            //    var view_pro = await preHeatProductService.GetDataSourceAsync(Guid.Empty);
            //    if (view_pro != null && view_pro.Any())
            //    {
            //        await preHeatProductService.SetDataToHashCache(view_pro, CurrentUser.Language);
            //        productCache = view_pro.ToDictionary(x => x.Code);
            //    }
            //}

            //if (!mchFeeChargeCache.Any() || !mchFeeChargeCache.Values.Any())
            //{
            //    var view_fee = await preHeatMchFeeChargeService.GetDataSourceAsync(Guid.Empty);
            //    if (view_fee != null && view_fee.Any())
            //    {
            //        await preHeatMchFeeChargeService.SetDataToHashCache(view_fee);
            //        mchFeeChargeCache = view_fee.ToDictionary(x => x.Id.ToString());
            //    }
            //}

            //if (!hotTopProductsCache.Any() || !hotTopProductsCache.Values.Any())
            //{
            //    var view_hot = await preHeatProductStaticsService.GetDataSourceAsync(Guid.Empty, "");
            //    await preHeatProductStaticsService.SetDataToHashCache(view_hot);
            //    hotTopProductsCache = view_hot.ToDictionary(x => x.Code);
            //}
            #endregion

            var code_NewProductDay = codeMasterBLL.GetCodeMaster(CodeMasterModule.Setting, CodeMasterFunction.ProductIconLogic, "NewProductDay");
            var code_HotProductTop = codeMasterBLL.GetCodeMaster(CodeMasterModule.Setting, CodeMasterFunction.ProductIconLogic, "HotProductTop");
            var newProductDay = int.Parse(code_NewProductDay?.Value ?? "7");            //新產品天數
            var hotProductTop = int.Parse(code_HotProductTop?.Value ?? "10");            //熱門前幾

            var hotTopProducts = hotTopProductsCache.Values.OrderByDescending(o => o.PurchaseCounter).Select(d => d.Code).Take(hotProductTop);
            #endregion

            var query = from m in promotionCache.Values.Where(x => x.PromotionId == PrmtId && x.PrmtDtlId == PrmtDtlId)
                        join pro in productCache.Values.Where(x => x.Status == ProductStatus.OnSale) on m.ProdCode equals pro.ProductCode
                        select new ProductDto
                        {
                            Seq = m.Seq,
                            Code = m.ProdCode,
                            ProductId = pro.ProductId,
                            SalePrice = pro.SalePrice,
                            MarkupPrice = pro.MarkUpPrice,
                            OriginalPrice = pro.OriginalPrice,
                            CreateDate = pro.CreateDate,
                            CurrencyCode = pro.CurrencyCode,
                            MerchantId = pro.MchId,
                            Name = pro.Name,                           //商品名称
                            IconType = pro.IconType,

                        };

            if (cond.ProductSize > 0)
                list = query.OrderBy(o => o.Seq).Take(cond.ProductSize).ToList();
            else
                list = query.OrderBy(o => o.Seq).ToList();

            #region 为了支持首页promotion产品分页

            if (cond.PageInfo.Page <= 0) cond.PageInfo.Page = 1;
            if (cond.PageInfo.PageSize > 0) list = query.OrderBy(o => o.Seq).Skip(cond.PageInfo.Offset).Take(cond.PageInfo.PageSize).ToList();

            #endregion

            foreach (var item in list)
            {
                bool hotProductFlag = hotTopProducts.Any(x => x == item.Code);
                bool feeProFlag = mchFeeChargeCache.Values.Any(x => x.MchId == item.MerchantId && x.ProductCode == item.Code);

                item.MerchantName = RedisHelper.HGet<HotMerchant>(mchKey, item.MerchantId.ToString())?.Name ?? "";
                item.Currency = currcencyCache?.simpleCurrencies?.FirstOrDefault(x => x.Code == item.CurrencyCode) ?? currencyBLL.GetDefaultCurrency();
                item.OriginalPrice = item.OriginalPrice + item.MarkupPrice;
                item.SalePrice = item.SalePrice + item.MarkupPrice;

                item.Imgs =await productBLL.GetProductImages(item.ProductId,item.Code);
                //await GetCornermarker(item, newProductDay, hotProductFlag, feeProFlag);
            }

            return list;
        }

        /// <summary>
        /// 获取PromotionMerchant数据
        /// </summary>
        /// <param name="PrmtId"></param>
        /// <param name="PrmtDtlId"></param>
        /// <param name="cond"></param>
        /// <returns></returns>
        async Task<List<MerchantDto>> GetPromotionMerchantFromCache(Guid PrmtId, Guid PrmtDtlId, PromotionCond cond)
        {
            List<MerchantDto> list = new List<MerchantDto>();

            var lang = CurrentUser.Lang;
            string promtionKey = $"{PreHotType.Hot_PromotionMerchant}";
            string mchKey = $"{PreHotType.Hot_Merchants}_{lang}";
            string productKey = $"{PreHotType.Hot_Products}_{lang}";

            var promotionCache = await RedisHelper.HGetAllAsync<HotPromotionMerchant>(promtionKey);
            var mchCache = await RedisHelper.HGetAllAsync<HotMerchant>(mchKey);
            var productCache = await RedisHelper.HGetAllAsync<HotProduct>(productKey);

            #region 读数据库，回写缓存

            //if (!promotionCache.Any() || !promotionCache.Values.Any())
            //{
            //    var motions = await preHeatPromotionMerchantService.GetDataSourceAsync(Guid.Empty);
            //    if (motions != null && motions.Any())
            //    {
            //        await preHeatPromotionMerchantService.SetDataToHashCache(motions, Guid.Empty);
            //        promotionCache = motions.ToDictionary(x => x.Id.ToString());
            //    }
            //}

            //if (!mchCache.Any() || !mchCache.Values.Any())
            //{
            //    var view_mch = await preHeatMerchantService.GetDataSourceAsync(Guid.Empty);
            //    if (view_mch != null && view_mch.Any())
            //    {
            //        await preHeatMerchantService.SetDataToHashCache(view_mch, CurrentUser.Language);
            //        ///mchCache = view_mch.Where(x=>x.LangType == CurrentUser.Language).ToDictionary(x => x.MchId.ToString());     
            //        mchCache = await RedisHelper.HGetAllAsync<HotMerchant>(mchKey);
            //    }
            //}

            //if (!productCache.Any() || !productCache.Values.Any())
            //{
            //    var view_product = await preHeatProductService.GetDataSourceAsync(Guid.Empty);
            //    if (view_product != null && view_product.Any())
            //    {
            //        await preHeatProductService.SetDataToHashCache(view_product, CurrentUser.Language);
            //        productCache = view_product.Where(x => x.LangType == CurrentUser.Language).ToDictionary(x => x.ProductId.ToString());
            //    }
            //}

            #endregion

            var query = from p in promotionCache.Values.Where(x => x.PrmtID == PrmtId && x.PrmtDtlId == PrmtDtlId)
                        join m in mchCache.Values on p.MerchantId equals m.MchId
                        select new MerchantDto
                        {
                            MchId = m.MchId,
                            Seq = p.Seq,
                            Code = m.Code,
                            Name = m.Name,
                            LogoB = m.LogoB
                        };

            if (cond.MerchantSize > 0)
                list = query.OrderBy(o => o.Seq).Take(cond.MerchantSize).ToList();
            else
                list = query.ToList();

            foreach (var item in list)
            {
                item.Score = baseRepository.GetModel<MerchantStatistic>(x => x.MerchId == item.MchId && x.IsActive && !x.IsDeleted)?.Score ?? 5;

                /////前端暂时没用到这部分数据
                //var mchProduct = productCache.Values.Where(x => x.MchId == item.MchId);
            }

            return list;
        }

        void SetPersonalData(List<PromotionPage> promotionList, Action<PromotionPage> action)
        {
            promotionList.ForEach(x =>
            {
                action.Invoke(x);
            });
        }
    }
}
