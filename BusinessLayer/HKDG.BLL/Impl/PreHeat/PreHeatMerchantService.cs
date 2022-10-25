using WebCache;

namespace HKDG.BLL
{
    public class PreHeatMerchantService : AbstractPreHeatService
    {
        public ITranslationRepository translationRepository;

        public PreHeatMerchantService(IServiceProvider services) : base(services)
        {
            translationRepository = Services.Resolve<ITranslationRepository>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">MchId</param>
        /// <returns></returns>
        public override async Task<SystemResult> CreatePreHeat(Guid Id)
        {
            var result = new SystemResult();

            var query =await GetDataSourceAsync(Id);

            await SetDataToHashCache(query, Language.E);
            await SetDataToHashCache(query, Language.C);
            await SetDataToHashCache(query, Language.S);
          
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">Merchant.Id</param>
        /// <returns></returns>
        public async Task<IQueryable<HotMerchant>> GetDataSourceAsync(Guid Id)
        {
            var query = from m in baseRepository.GetList<Merchant>()
                        join t1 in baseRepository.GetList<Translation>() on m.NameTransId equals t1.TransId into m1
                        from mt1 in m1.DefaultIfEmpty()
                        join mp in baseRepository.GetList<MerchantPromotion>() on m.Id equals mp.MerchantId into m2
                        from mmp in m2.DefaultIfEmpty()
                        join t2 in baseRepository.GetList<Translation>() on mmp.BigLogoId equals t2.TransId into m3
                        from mmt in m3.DefaultIfEmpty()
                        join c in baseRepository.GetList<CustomUrl>().Where(x => x.IsActive && !x.IsDeleted) on m.Id.ToString() equals c.KeyId into m5
                        from cm in m5.DefaultIfEmpty()
                        where m.IsActive && !m.IsDeleted && mmp.ApproveStatus == ApproveType.Pass && mmp.IsActive && mt1.Lang == mmt.Lang
                        select new HotMerchant
                        {
                            MchId = m.Id,
                            Code = m.MerchNo,
                            Name = mt1.Value,
                            LangType = mt1.Lang,
                            LogoB = mmt.Value,     //BigLogo
                            ContactEmail = m.ContactEmail,
                            CustomUrl = cm.Url,
                            MerchantType = m.MerchantType,
                        };

            if (Id != Guid.Empty)
                query = query.Where(x => x.MchId == Id);

        
            return query;
        }

        public async Task<SystemResult> SetDataToHashCache(IQueryable<HotMerchant> list, Language language)
        {
            var result = new SystemResult();
            string key = $"{PreHotType.Hot_Merchants}_{language}";

            var hotList = list.Where(x => x.LangType == language).ToList();
            if (hotList != null && hotList.Any())
            {
                foreach (var item in hotList)
                {
                    var mp = await baseRepository.GetModelAsync<MerchantPromotion>(x => x.MerchantId == item.MchId && x.IsActive && !x.IsDeleted && x.ApproveStatus == ApproveType.Pass);
                    var mRecord = await DicCollection(mp,language);
                    var banners = await GetMerchantBannerList(mp);

                    //item.Notice = mRecord["NoticeTranId"]?.FirstOrDefault(x => x.Lang == language)?.Value ?? "";
                    //item.Cover = mRecord["CoverId"]?.FirstOrDefault(x => x.Lang == language)?.Value ?? "";
                    //item.ExpCompleteDays = mRecord["OrderTransId"]?.FirstOrDefault(x => x.Lang == language)?.Value ?? "2~4";
                    //item.Logo = mRecord["SmallLogoId"]?.FirstOrDefault(x => x.Lang == language)?.Value ?? "";
                    //item.PromIntroduction = mRecord["CoverId"]?.FirstOrDefault(x => x.Lang == language)?.Value ?? "";
                    //item.PromName = mRecord["NameTranId"]?.FirstOrDefault(x => x.Lang == language)?.Value ?? "";
                    //item.TandC = mRecord["TAndCTranId"]?.FirstOrDefault(x => x.Lang == language)?.Value ?? "";
                    //item.ReturnTerms = mRecord["ReturnTermsTranId"]?.FirstOrDefault(x => x.Lang == language)?.Value ?? "";
                    //item.Description = mRecord["DescTransId"]?.FirstOrDefault(x => x.Lang == language)?.Value ?? "";

                    item.Notice = mRecord["NoticeTranId"]?.Value ?? "";
                    item.Cover = mRecord["CoverId"]?.Value ?? "";
                    item.ExpCompleteDays = mRecord["OrderTransId"]?.Value ?? "2~4";
                    item.Logo = mRecord["SmallLogoId"]?.Value ?? "";                   
                    item.PromIntroduction = mRecord["IntorductionTranId"]?.Value ?? "";
                    item.PromName = mRecord["NameTranId"]?.Value ?? "";
                    item.TandC = mRecord["TAndCTranId"]?.Value ?? "";
                    item.ReturnTerms = mRecord["ReturnTermsTranId"]?.Value ?? "";
                    item.Description = mRecord["DescTransId"]?.Value ?? "";

                    item.Banners = banners.Where(x => x.Lang == language).ToList();

                    await RedisHelper.HSetAsync(key, item.MchId.ToString(), item);                   
                }           
            }
            return result;
        }

        public async Task<Dictionary<string, Translation>> DicCollection(MerchantPromotion mp,Language language)
        {
            Dictionary<string, Translation> dic = new Dictionary<string, Translation> ();
            if (mp != null)
            {
                //var noticeList = await baseRepository.GetListAsync<Translation>(x => x.TransId == mp.NoticeTranId);
                var noticeList = await translationRepository.GetTranslationAsync(mp.NoticeTranId, language);
                if (noticeList != null)
                    dic.Add("NoticeTranId", noticeList);
                else
                    dic.Add("NoticeTranId", new Translation());

                //var logoList = await baseRepository.GetListAsync<Translation>(x => x.TransId == mp.SmallLogoId);
                var logoList = await translationRepository.GetTranslationAsync(mp.SmallLogoId, language);
                if (logoList != null )
                    dic.Add("SmallLogoId", logoList);
                else
                    dic.Add("SmallLogoId", new Translation());

                //var compleleDayList = await baseRepository.GetListAsync<Translation>(x => x.TransId == mp.OrderTransId);
                var compleleDayList = await translationRepository.GetTranslationAsync(mp.OrderTransId, language);
                if (compleleDayList != null)
                    dic.Add("OrderTransId", compleleDayList);
                else
                    dic.Add("OrderTransId", new Translation{ });

                //var coverList = await baseRepository.GetListAsync<Translation>(x => x.TransId == mp.CoverId);
                var coverList =  await translationRepository.GetTranslationAsync(mp.CoverId, language);
                if (coverList != null)
                    dic.Add("CoverId", coverList);
                else
                    dic.Add("CoverId", new Translation { });

                //var promIntroductionList = await baseRepository.GetListAsync<Translation>(x => x.TransId == mp.IntorductionTranId);
                var promIntroductionList = await translationRepository.GetTranslationAsync(mp.IntorductionTranId, language);
                if (promIntroductionList != null)
                    dic.Add("IntorductionTranId", promIntroductionList);
                else
                    dic.Add("IntorductionTranId", new Translation());

                //var promNameList = await baseRepository.GetListAsync<Translation>(x => x.TransId == mp.NameTranId);
                var promNameList = await translationRepository.GetTranslationAsync(mp.NameTranId, language);
                if (promNameList != null )
                    dic.Add("NameTranId", promNameList);
                else
                    dic.Add("NameTranId", new Translation { });

                //ReturnTerms
                //var ReturnTermsList = await baseRepository.GetListAsync<Translation>(x => x.TransId == mp.ReturnTermsTranId);
                var ReturnTermsList = await translationRepository.GetTranslationAsync(mp.ReturnTermsTranId, language);
                if (ReturnTermsList != null)
                    dic.Add("ReturnTermsTranId", ReturnTermsList);
                else
                    dic.Add("ReturnTermsTranId", new Translation { });

                //TandC
                //var TandCList = await baseRepository.GetListAsync<Translation>(x => x.TransId == mp.TAndCTranId);
                var TandCList = await translationRepository.GetTranslationAsync(mp.TAndCTranId, language);
                if (TandCList != null)
                    dic.Add("TAndCTranId", TandCList);
                else
                    dic.Add("TAndCTranId", new Translation { });

                //Description
                //var DescriptionList = await baseRepository.GetListAsync<Translation>(x => x.TransId == mp.DescTransId);
                var DescriptionList = await translationRepository.GetTranslationAsync(mp.DescTransId, language);
                if (DescriptionList != null)
                    dic.Add("DescTransId", DescriptionList);
                else
                    dic.Add("DescTransId", new Translation { });
            }
            return dic;
        }

        public async Task<List<MerchantPromotionBannerView>> GetMerchantBannerList(MerchantPromotion mp)
        {
            var result = new List<MerchantPromotionBannerView>();

            if (mp != null)
            {
                var banners = await baseRepository.GetListAsync<MerchantPromotionBanner>(x => x.PromotionId == mp.Id && x.IsActive && !x.IsDeleted);
                if (banners != null && banners.Any())
                    result = banners.Select(s => new MerchantPromotionBannerView
                    {
                        Id = s.Id,
                        Image = s.Image,
                        IsDelete = false,
                        PromotionId = s.PromotionId,
                        Lang = s.Language,
                        Seq = s.Seq,
                        IsOpenWindow = s.IsOpenWindow,
                        BannerLink = s.BannerLink
                    }).OrderBy(o => o.Seq).ToList();
            }
            return result;
        }

        public async Task<SystemResult> SetStorePickUpAddressDataToHashCache(IQueryable<StorePickUpAddress> list)
        {
            var result = new SystemResult();
            string key = $"{PreHotType.StorePickUpAddress}_{Language.C}";

            var hotList = list.Where(x => x.Lang == Language.C).ToList();
            if (hotList != null && hotList.Any())
            {
                foreach (var item in hotList)
                {

                    await RedisHelper.HSetAsync(key, item.Id.ToString(), item);
                }
            }

            key = $"{PreHotType.StorePickUpAddress}_{Language.S}";

            hotList = list.Where(x => x.Lang == Language.S).ToList();
            if (hotList != null && hotList.Any())
            {
                foreach (var item in hotList)
                {

                    await RedisHelper.HSetAsync(key, item.Id.ToString(), item);
                }
            }

            key = $"{PreHotType.StorePickUpAddress}_{Language.E}";

            hotList = list.Where(x => x.Lang == Language.E).ToList();
            if (hotList != null && hotList.Any())
            {
                foreach (var item in hotList)
                {

                    await RedisHelper.HSetAsync(key, item.Id.ToString(), item);
                }
            }


            return result;
        }

        public async Task<SystemResult> DeleteStorePickUpAddressToHashCache(string[] ids)
        {


            var result = new SystemResult();
            string key = $"{PreHotType.StorePickUpAddress}_{Language.C}";
            await RedisHelper.HDelAsync(key, ids);

            key = $"{PreHotType.StorePickUpAddress}_{Language.S}";
            await RedisHelper.HDelAsync(key, ids);

            key = $"{PreHotType.StorePickUpAddress}_{Language.E}";
            await RedisHelper.HDelAsync(key, ids);


            return result;
        }

    }

}
