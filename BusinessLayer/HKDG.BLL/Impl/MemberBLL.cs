using MemberInfo = Domain.MemberInfo;

namespace HKDG.BLL
{
    public class MemberBLL : BaseBLL, IMemberBLL
    {
        IProductBLL productBLL;
        PreHeatFavoriteService preHeatFavoriteService;
        PreHeatProductService preHeatProductService;
        PreHeatMerchantService preHeatMerchantService;
        ISettingBLL settingBLL;
        IMemberAccountRepository memberAccountRepo;
        IMemberRepository memberRepo;
        ITranslationRepository translationRepo;

        public MemberBLL(IServiceProvider services) : base(services)
        {
            productBLL = this.Services.Resolve<IProductBLL>();
            preHeatFavoriteService = (PreHeatFavoriteService)Services.GetService(typeof(PreHeatFavoriteService));
            preHeatProductService = (PreHeatProductService)Services.GetService(typeof(PreHeatProductService));
            preHeatMerchantService = (PreHeatMerchantService)Services.GetService(typeof(PreHeatMerchantService));
            settingBLL = this.Services.Resolve<ISettingBLL>();
            memberAccountRepo = services.Resolve<IMemberAccountRepository>();
            memberRepo = services.Resolve<IMemberRepository>();
            translationRepo = services.Resolve<ITranslationRepository>();
        }

        public PageData<MemberDto> SearchMember(MbrSearchCond cond)
        {
            var result = new PageData<MemberDto>();
            var query = baseRepository.GetList<Member>();

            #region 组装条件

            if (!cond.EmailAdd.IsEmpty())
                query = query.Where(x => x.Email.Contains(cond.EmailAdd));

            if (!cond.FirstName.IsEmpty())
                query = query.Where(x => x.FirstName.Contains(cond.FirstName));

            if (!cond.Code.IsEmpty())
                query = query.Where(x => x.Code.Contains(cond.Code));

            if (cond.RegDateFrom != null && cond.RegDateTo != null)
            {
                query = query.Where(x => x.CreateDate >= cond.RegDateFrom && x.CreateDate <= cond.RegDateTo);
            }
            #endregion

            result.TotalRecord = query.Count();
            if (cond.SortName.IsEmpty()) cond.SortName = "CreateDate";
            if (cond.SortOrder.IsEmpty()) cond.SortOrder = "Desc";

            var sortBy = (SortType)Enum.Parse(typeof(SortType), cond.SortOrder.ToUpper());
            query = query.AsQueryable().SortBy(cond.SortName, sortBy).Skip(cond.Offset).Take(cond.PageSize);

            result.Data = query.MapToList<Member, MemberDto>();

            return result;
        }

        public SystemResult Register(RegisterMember member)
        {
            var result = new SystemResult();

            member.Validate();

            string pwdBase = PwdUtil.GenPwdBase(member.Email, member.Password);
            var dbModel = new Member
            {
                Id = Guid.NewGuid(),
                Account = member.Account,
                Email = member.Email,
                Password = HashUtil.MD5(pwdBase),
                IsActive = true,
                IsApprove = true,
                IsDeleted = false,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CurrencyCode = "HKD",
                Language = Language.C,
                BirthDate = member.BirthDate,
                Code = AutoGenerateNumber("MB"),
                FirstName = member.FirstName,
                LastName = member.LastName,
                OptOutPromotion = member.OptOutPromotion,
            };

            baseRepository.Insert(dbModel);

            result.Succeeded = true;
            return result;
        }

        public async Task<SystemResult> ChangeLang(Language Lang)
        {
            var result = new SystemResult() { Succeeded = false };
            string message = string.Empty;
            if (CurrentUser.IsLogin)
            {
                var member = await baseRepository.GetModelByIdAsync<Member>(CurrentUser.Id);
                member.Language = Lang;
                await baseRepository.UpdateAsync(member);
            }

            var NewUser = await RedisHelper.HGetAsync<CurrentUser>($"{CacheKey.CurrentUser}", CurrentUser.LoginSerialNO);
            NewUser.Lang = Lang;

            await RedisHelper.HSetAsync($"{CacheKey.CurrentUser}", NewUser.LoginSerialNO, NewUser);
            
            result.Succeeded = true;
            return result;
        }

        public async Task<SystemResult> ChangeCurrencyCode(string CurrencyCode)
        {
            var result = new SystemResult() { Succeeded = false };
            string message = string.Empty;
            if (CurrentUser.IsLogin)
            {
                var member = await baseRepository.GetModelByIdAsync<Member>(CurrentUser.Id);
                member.CurrencyCode = CurrencyCode;
                await baseRepository.UpdateAsync(member);                
            }

            var NewUser = await RedisHelper.HGetAsync<CurrentUser>($"{CacheKey.CurrentUser}", CurrentUser.LoginSerialNO);
            NewUser.CurrencyCode = CurrencyCode;
            NewUser.Currency = currencyBLL.GetSimpleCurrency(CurrencyCode);

            await RedisHelper.HSetAsync($"{CacheKey.CurrentUser}", NewUser.LoginSerialNO, NewUser);

            result.Succeeded = true;
            return result;
        }

        public async Task<SystemResult> ChangeSetting(Language Lang, string CurrencyCode)
        {
            var result = new SystemResult() { Succeeded = false };
            string message = string.Empty;

            if (CurrentUser.IsLogin)
            {
                var member = await baseRepository.GetModelByIdAsync<Member>(CurrentUser.Id);
                member.CurrencyCode = CurrencyCode;
                member.Language = Lang;
                member.UpdateDate = DateTime.Now;
                await baseRepository.UpdateAsync(member);
            }

            var NewUser = await RedisHelper.HGetAsync<CurrentUser>($"{CacheKey.CurrentUser}", CurrentUser.LoginSerialNO);
            NewUser.Lang= Lang;
            NewUser.CurrencyCode = CurrencyCode;
            NewUser.Currency = currencyBLL.GetSimpleCurrency(CurrencyCode);

            await RedisHelper.HSetAsync($"{CacheKey.CurrentUser}", NewUser.LoginSerialNO, NewUser);

            result.Succeeded = true;
            return result;
        }


        public RegSummary GetRegSummary()
        {
            RegSummary summary = new RegSummary();

            DateTime now = DateTime.Now;
            DateTime preMonth = (new DateTime(now.Year, now.Month, 1)).AddMonths(-1);
            DateTime d1 = new DateTime(now.Year, now.Month, 1);

            summary.MemberTotal = baseRepository.GetList<Member>().Count(d => !d.IsDeleted);
            summary.LastMth = baseRepository.GetList<Member>().Count(d => d.CreateDate.Month == preMonth.Month && !d.IsDeleted);
            summary.ThisMth = baseRepository.GetList<Member>().Count(d => d.CreateDate > d1 && d.CreateDate < DateTime.Now && !d.IsDeleted);
            return summary;
        }

        public async Task<SystemResult> AddFavMerchant(string merchCode)
        {
            var result = new SystemResult();
            var mch = await baseRepository.GetModelAsync<Merchant>(x => x.MerchNo == merchCode);
            if (mch == null) throw new BLException(Resources.Message.AddFavoriteFail);

            var merchFavEntity = await baseRepository.GetModelAsync<MerchantFavorite>(x => x.MemberId == Guid.Parse(CurrentUser.UserId) && x.MerchId == mch.Id);
            if (merchFavEntity == null)
            {
                merchFavEntity = new MerchantFavorite
                {
                    Id = Guid.NewGuid(),
                    MemberId = Guid.Parse(CurrentUser.UserId),
                    MerchId = mch.Id
                };
                await baseRepository.InsertAsync(merchFavEntity);
            }
            else
            {
                merchFavEntity.IsActive = true;
                await baseRepository.UpdateAsync(merchFavEntity);
            }

            //更新缓存
            string key = CacheKey.Favorite.ToString();
            string field = CurrentUser.UserId;
            var cacheData = await RedisHelper.HGetAsync<Favorite>(key, field);

            if (cacheData == null) cacheData = new Favorite();

            cacheData.MchList.Add(mch.Id);
            await RedisHelper.HSetAsync(key, field, cacheData);

            result.ReturnValue = cacheData;
            result.Succeeded = true;
            result.Message = Resources.Message.AddFavoriteSuccess;
            return result;
        }

        public async Task<SystemResult> RemoveFavMerchant(string merchCode)
        {
            var result = new SystemResult();
            var mch = await baseRepository.GetModelAsync<Merchant>(x => x.MerchNo == merchCode);
            if (mch == null) throw new BLException(Resources.Message.AddFavoriteFail);

            var merchFavEntity = await baseRepository.GetModelAsync<MerchantFavorite>(x => x.MemberId == Guid.Parse(CurrentUser.UserId) && x.MerchId == mch.Id && x.IsActive);
            if (merchFavEntity != null)
            {
                merchFavEntity.IsActive = false;
                await baseRepository.UpdateAsync(merchFavEntity);
            }

            //更新缓存
            string key = CacheKey.Favorite.ToString();
            string field = CurrentUser.UserId;
            var cacheData = await RedisHelper.HGetAsync<Favorite>(key, field);
            cacheData.MchList.Remove(mch.Id);
            await RedisHelper.HSetAsync(key, field, cacheData);

            result.ReturnValue = cacheData;
            result.Succeeded = true;
            result.Message = HKDG.Resources.Message.RemoveFavoriteSuccess;
            return result;
        }

        public async Task<SystemResult> AddFavProduct(Guid productId)
        {
            var result = new SystemResult();
            var product = await baseRepository.GetModelByIdAsync<Product>(productId);
            if (product == null) throw new BLException(HKDG.Resources.Message.ProductCodeEmpty);

            var memberFavorite = await baseRepository.GetModelAsync<MemberFavorite>(d => d.ProductCode == product.Code && d.MemberId == Guid.Parse(CurrentUser.UserId));
            if (memberFavorite == null)
            {
                memberFavorite = new MemberFavorite();
                memberFavorite.Id = Guid.NewGuid();
                memberFavorite.MemberId = Guid.Parse(CurrentUser.UserId);
                memberFavorite.ProductId = product.Id;
                memberFavorite.ProductCode = product.Code;
                await baseRepository.InsertAsync(memberFavorite);
            }
            else
            {
                memberFavorite.IsActive = true;
                await baseRepository.UpdateAsync(memberFavorite);
            }

            string key = CacheKey.Favorite.ToString();
            string field = CurrentUser.UserId;
            var cacheData = await RedisHelper.HGetAsync<Favorite>(key, field);

            if (cacheData == null)  cacheData = new Favorite();

            cacheData.ProductList.Add(product.Code);
            await RedisHelper.HSetAsync(key, field, cacheData);

            result.ReturnValue = cacheData;
            result.Succeeded = true;
            result.Message = HKDG.Resources.Message.AddFavoriteSuccess;

            return result;
        }

        public async Task<SystemResult> RemoveFavProduct(Guid productId)
        {
            var result = new SystemResult();
            var product = await baseRepository.GetModelByIdAsync<Product>(productId);
            if (product == null) throw new BLException(HKDG.Resources.Message.ProductCodeEmpty);

            var memberFavorite = await baseRepository.GetModelAsync<MemberFavorite>(d => d.ProductCode == product.Code && d.MemberId == Guid.Parse(CurrentUser.UserId) && d.IsActive);
            if (memberFavorite != null)
            {
                memberFavorite.IsActive = false;
                await baseRepository.UpdateAsync(memberFavorite);
            }

            string key = CacheKey.Favorite.ToString();
            string field = CurrentUser.UserId;
            var cacheData = await RedisHelper.HGetAsync<Favorite>(key, field);
            cacheData.ProductList.Remove(product.Code);
            await RedisHelper.HSetAsync(key, field, cacheData);

            result.ReturnValue = cacheData;
            result.Succeeded = true;
            result.Message = Resources.Message.RemoveFavoriteSuccess;

            return result;
        }

        public async Task<PageData<MicroMerchant>> MyFavMerchant(FavoriteCond cond)
        {
            var result = new PageData<MicroMerchant>();
            string key = CacheKey.Favorite.ToString();
            string field = CurrentUser.UserId;

            var favData = await RedisHelper.HGetAsync<Favorite>(key, field);
            if (favData == null || (favData.MchList?.Any() ?? false))
            {
                //重新读取喜欢商家数据
                favData = await this.preHeatFavoriteService.GetDataSourceAsync(Guid.Parse(field));
                if (favData != null)
                {
                    //重新刷新缓存
                    await preHeatFavoriteService.SetDataToHashCache(Guid.Parse(CurrentUser.UserId), favData);
                }
            }

            key = $"{PreHotType.Hot_Merchants}_{CurrentUser.Lang}";
            var mchList = await RedisHelper.HGetAllAsync<HotMerchant>(key);
            //读数据库，并回写缓存           
            if ((!mchList?.Keys.Any() ?? false) || (!mchList.Values?.Any() ?? false))
            {
                var view = await this.preHeatMerchantService.GetDataSourceAsync(Guid.Empty);
                if (view != null && view.Any())
                {
                    //重新刷新缓存
                    await preHeatMerchantService.SetDataToHashCache(view, CurrentUser.Lang);
                    mchList = view.Where(x => x.LangType == CurrentUser.Lang).ToDictionary(x => x.MchId.ToString());
                }
            }

            var query = mchList.Values.AsQueryable().Where(x => favData.MchList.Contains(x.MchId));

            result.TotalRecord = query.Count();
            query = query.Skip(cond.Offset).Take(cond.PageSize);

            var list = query.Select(s => new MicroMerchant
            {
                Id = s.MchId,
                Code = s.Code,
                Name = s.Name,
                IsFavorite = true,
                Score = s.Score,
                LogoB = s.LogoB,
                LogoS = s.Logo,
            }).ToList();

            foreach (var item in list)
            {
                var mp = await baseRepository.GetModelAsync<MerchantPromotion>(x => x.MerchantId == item.Id && x.IsActive && !x.IsDeleted && x.ApproveStatus == ApproveType.Pass);
                var mRecord = await preHeatMerchantService.DicCollection(mp, CurrentUser.Lang);
                item.ImagePath = mRecord["SmallLogoId"]?.Value ?? "";
            }
            result.Data = list;
            return result;
        }

        public async Task<PageData<ProductSummary>> MyFavProduct(FavoriteCond cond)
        {
            var result = new PageData<ProductSummary>();
            string key = CacheKey.Favorite.ToString();
            string field = CurrentUser.UserId;

            var favData = await RedisHelper.HGetAsync<Favorite>(key, field);
            if (favData == null || (favData.ProductList?.Any() ?? false))
            {
                //重新读取喜欢商家数据
                favData = await this.preHeatFavoriteService.GetDataSourceAsync(Guid.Parse(field));
                if (favData != null)
                {
                    //重新刷新缓存
                    await preHeatFavoriteService.SetDataToHashCache(Guid.Parse(CurrentUser.UserId), favData);
                }
            }

            key = $"{PreHotType.Hot_Products}_{CurrentUser.Lang}";
            var productList = await RedisHelper.HGetAllAsync<HotProduct>(key);
            //读数据库，并回写缓存           
            if ((!productList?.Keys.Any() ?? false) || (!productList.Values?.Any() ?? false))
            {
                var view = await this.preHeatProductService.GetDataSourceAsync(Guid.Empty);
                if (view != null && view.Any())
                {
                    //重新刷新缓存
                    await preHeatProductService.SetDataToHashCache(view, CurrentUser.Lang);
                    productList = view.Where(x => x.LangType == CurrentUser.Lang).ToDictionary(x => x.ProductId.ToString());
                }
            }

            var query = productList.Values.AsQueryable().Where(x => favData.ProductList.Contains(x.ProductCode));

            result.TotalRecord = query.Count();
            query = query.Skip(cond.Offset).Take(cond.PageSize);

            var list = AutoMapperExt.MapToList<HotProduct, Product>(query.ToList());
            list.ForEach(x => {
                x.MerchantId = query.ToList().FirstOrDefault(y => y.Code == x.Code).MchId;           
            });

            result.Data = list.Select(s => productBLL.GenProductSummary(s)).ToList();
            return result;
        }

        /// <summary>
        /// 获取会员信息
        /// </summary>
        /// <returns></returns>
        public async Task<MemberItem> GetMemberInfo()
        {
            var dbMember = await baseRepository.GetModelAsync<Member>(x => x.Id == CurrentUser.Id);
            if (dbMember == null) return new MemberItem { };

            var mItem = AutoMapperExt.MapTo<MemberItem>(dbMember);          
            mItem.Currency = currencyBLL.GetSimpleCurrency(dbMember.CurrencyCode);
            return mItem;
        }

        public async Task<PageData<MicroProduct>> MyProductTrack(TrackCond cond)
        {
            var result = new PageData<MicroProduct>();

            string key = CacheKey.Favorite.ToString();
            string field = CurrentUser.UserId;

            var favData = await RedisHelper.HGetAsync<Favorite>(key, field);
            if (favData == null || (favData.ProductList?.Any() ?? false))
            {
                //重新读取喜欢商家数据
                favData = await this.preHeatFavoriteService.GetDataSourceAsync(Guid.Parse(field));
                if (favData != null)
                {
                    //重新刷新缓存
                    await preHeatFavoriteService.SetDataToHashCache(Guid.Parse(CurrentUser.UserId), favData);
                }
            }

            var query = (await baseRepository.GetListAsync<ProductTrack>(x => x.MemberId == Guid.Parse(CurrentUser.UserId) && x.IsActive && !x.IsDeleted));

            key = $"{PreHotType.Hot_Products}_{CurrentUser.Lang}";
            var productList = (await RedisHelper.HMGetAsync<HotProduct>(key, query.Select(s => s.ProductCode).ToArray()))
                .Select(s => new MicroProduct
                {
                    ProductId = s.ProductId,
                    Code = s.Code,
                    Name = s.Name,
                    SalePrice = s.SalePrice,
                    CurrencyCode = s.CurrencyCode,
                    Score = s.Score,
                    IsFavorite = favData.ProductList.Any(x=>x == s.Code),
                    CreateDate = query.FirstOrDefault(x => x.ProductCode == s.Code).CreateDate,
                    UpdateDate = s.UpdateDate,

                }).OrderByDescending(o => o.CreateDate).ToList();

            if (productList?.Any() ?? false)
            { 
                //从数据库读
            }

            result.TotalRecord = productList.Count();
            result.Data = productList.Skip(cond.Offset).Take(cond.PageSize).ToList();

            foreach (var item in result.Data)
            {
                item.ImagePath = (await productBLL.GetProductImages(item.ProductId, item.Code)).FirstOrDefault();
            }

            return result;
        }

        public List<MbrSpendingRank> GetMbrSpendingRank()
        {


            List<MbrSpendingRank> data = memberRepo.GetMbrSpendingRank();

            foreach (var item in data)
            {
                if (!string.IsNullOrEmpty(item.BuyerGroup))
                    item.BuyerGroup = translationRepo.GetMutiLanguage(new Guid(item.BuyerGroup)).FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc;

            }

            return data;

        }

        /// <summary>
        /// 獲取用戶信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MemberDto> GetMember(Guid id)
        {

            MemberDto memlist = new MemberDto();
            try
            {
                var result = await baseRepository.GetModelAsync<Member>(x => x.Id == id);
                if (result != null)
                {
                    memlist = AutoMapperExt.MapToList<Member, MemberDto>(result);
                    if (settingBLL != null)
                    {
                        var langs = settingBLL.GetSupportLanguages();
                        if (langs != null)
                        {
                            memlist.LanguageName = langs.FirstOrDefault(p => p.Code == result.Language.ToString()).Text ?? "";
                        }
                    }

                    result.MallFun = memberAccountRepo.GetMemberFun(result.Id);

                }
               

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return memlist;
        }

        public bool IsOptOutPromotion(Guid id)
        {
            Member result = new Member();
            try
            {
                var member = baseRepository.GetModelById<Member>(id);
                return member.OptOutPromotion;
            }
            catch (BLException blex)
            {
                throw blex;
            }
            catch (Exception ex)
            {
                //SaveError(GetType().FullName, ClassUtility.GetMethodName(), Resources.Message.CreateAccountFaile, ex.Message);
                throw ex;
            }

        }

        /// <summary>
        /// 修改用戶資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateMember(MemberDto model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }
            try
            {
                UnitOfWork.IsUnitSubmit = true;//统一提交

                var dbModel = baseRepository.GetModelById<Member>(model.Id);
                if (dbModel != null)
                {
                    if (StringHtmlContentCheck(model))//檢查HTML 內容
                    {
                        throw new BLException(Resources.Message.ExistHTMLLabel);
                    }

                    dbModel.Code = model.Code;
                    dbModel.GroupId = model.GroupId;
                    dbModel.Remark = model.Remark;
                    dbModel.OptOutPromotion = model.OptOutPromotion;

                    baseRepository.Update(dbModel);

                    UnitOfWork.Submit();

                    //LogManager.SaveInfoAsync(LogModuleEnum.BLL.ToString(), LogSysFuncEnum.MemberAcc.ToString(), "UpdateAccount", "更新賬戶,Account=" + dbModel.Email.ToString(), CurrentUser.Id);
                    return true;
                }
                else
                {
                    throw new BLException(Resources.Message.AccountNotExist);
                }

            }
            catch (Exception ex)
            {
                throw new BLException(ex.Message);
            }
        }

        /// <summary>
        /// 檢查資料中字符串欄位是否帶有HTML標籤內容
        /// </summary>
        /// <param name="mInfo"></param>
        public bool StringHtmlContentCheck(MemberDto mInfo)
        {
            if (mInfo != null)
            {
                if (StringUtil.CheckHasHTMLTag(mInfo.Code))
                {
                    return true;
                }
                if (StringUtil.CheckHasHTMLTag(mInfo.Remark))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 启用用戶
        /// </summary>
        /// <param name="ids"></param>
        public async Task ActiveMember(Guid[] ids)
        {
            //UnitOfWork.IsUnitSubmit = true;//统一提交

            var dbModel = (await baseRepository.GetListAsync<Member>(x => ids.Contains(x.Id))).ToList();
            foreach (var model in dbModel)
            {

                model.IsApprove = true;
                model.UpdateDate = DateTime.Now;
                model.UpdateBy = Guid.Parse(CurrentUser.UserId);
                //LogManager.SaveInfoAsync(LogModuleEnum.BLL.ToString(), LogSysFuncEnum.MemberAcc.ToString(), "ActiveAccount", "激活賬戶,Account=" + dbModel.Email.ToString(), CurrentUser.Id);
            }
            await baseRepository.UpdateAsync(dbModel);
            //UnitOfWork.Submit();
        }


        /// <summary>
        /// 恢復用戶
        /// </summary>
        /// <param name="ids"></param>
        public async Task Restore(Guid[] ids)
        {
            var dbModel = (await baseRepository.GetListAsync<Member>(x => ids.Contains(x.Id))).ToList();
            foreach (var model in dbModel)
            {

                model.IsDeleted = false;
                model.UpdateDate = DateTime.Now;
                model.UpdateBy = Guid.Parse(CurrentUser.UserId);
                //LogManager.SaveInfoAsync(LogModuleEnum.BLL.ToString(), LogSysFuncEnum.MemberAcc.ToString(), "ActiveAccount", "激活賬戶,Account=" + dbModel.Email.ToString(), CurrentUser.Id);
            }
            await baseRepository.UpdateAsync(dbModel);
        }

        /// <summary>
        /// 激活用戶
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task EnableMember(Guid[] ids)
        {
            if (ids == null)
            {
                throw new ArgumentNullException();
            }

            try
            {
                var dbModel = (await baseRepository.GetListAsync<Member>(x => ids.Contains(x.Id))).ToList();


                foreach (var model in dbModel)
                {
                    model.IsActive = true;
                    model.UpdateDate = DateTime.Now;
                    model.UpdateBy = Guid.Parse(CurrentUser.UserId);

                }
                await baseRepository.UpdateAsync(dbModel);

            }
            catch (Exception ex)
            {

                throw new BLException(ex.Message);
            }
        }

        /// <summary>
        /// 中止用戶
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task SuspendMember(Guid[] ids)
        {
            if (ids == null)
            {
                throw new ArgumentNullException();
            }

            try
            {
                var dbModel = (await baseRepository.GetListAsync<Member>(x => ids.Contains(x.Id))).ToList();

                foreach (var model in dbModel)
                {
                    model.IsActive = false;
                    model.UpdateDate = DateTime.Now;
                    model.UpdateBy = Guid.Parse(CurrentUser.UserId);

                }
                await baseRepository.UpdateAsync(dbModel);

            }
            catch (Exception ex)
            {

                throw new BLException(ex.Message);
            }
        }

        /// <summary>
        /// 刪除選中用戶（邏輯刪除）
        /// </summary>
        /// <param name="ids"></param>
        public async Task DeleteMember(Guid[] ids)
        {
            var dbModel = (await baseRepository.GetListAsync<Member>(x => ids.Contains(x.Id))).ToList();

            foreach (var model in dbModel)
            {
                model.IsDeleted = true;
                model.UpdateDate = DateTime.Now;
                model.UpdateBy = Guid.Parse(CurrentUser.UserId);

            }
            await baseRepository.UpdateAsync(dbModel);
        }

        /// <summary>
        /// 批量修改用戶分組
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="targetGroup"></param>
        public async Task MoveMember(Guid[] ids, Guid targetGroup)
        {
            var dbModel = (await baseRepository.GetListAsync<Member>(x => ids.Contains(x.Id))).ToList();

            foreach (var model in dbModel)
            {
                model.GroupId = targetGroup;
                model.UpdateDate = DateTime.Now;
                model.UpdateBy = Guid.Parse(CurrentUser.UserId);

            }
            await baseRepository.UpdateAsync(dbModel);
        }

        public List<MallFunHistoryView> GetMallFunHistory(FunCond cond)
        {
            try
            {
                return memberAccountRepo.GetMallFunHistory(cond);

            }
            catch (Exception ex)
            {
                throw new BLException(ex.Message);
            }
        }

        public PageData<MemberDto> Search(MbrSearchCond condition)
        {
            var a = memberRepo.SearchMember(condition);
            return a;
        }

        public async Task<SystemResult> UpdatePassword(string oldpwd, string newpwd)
        {
            SystemResult result = new SystemResult();

            var member = await baseRepository.GetModelByIdAsync<Member>(CurrentUser.Id);

            if (member == null)  throw new BLException(Resources.Message.NoMember);
            
            string pwdBase = PwdUtil.GenPwdBase(member.Email, newpwd);
            string oldpwdBase = PwdUtil.GenPwdBase(member.Email, oldpwd);
            oldpwd = HashUtil.MD5(oldpwdBase);

            if (member.Password != oldpwd)  throw new BLException(Resources.Message.OldPwdIncorrect);
            
            member.Password = HashUtil.MD5(pwdBase);
            //member.Password = _memberRepository.Encrypt(newpwd);
            await baseRepository.UpdateAsync(member);

            result.Succeeded = true;
            result.Message = Resources.Message.UpdatePwdSuccess;

            return result;
        }

        public async Task<bool> Subscribe(string email)
        { 
            var member = await baseRepository.GetModelAsync<Member>(x=>x.Email== email); 
            member.OptOutPromotion = true;
            await baseRepository.UpdateAsync(member);
            return true;
        }

        public async Task<bool> Unsubscribe(string email)
        {
            var member = await baseRepository.GetModelAsync<Member>(x => x.Email == email);
            member.OptOutPromotion = false;
            await baseRepository.UpdateAsync(member);
            return true;
        }

        public async Task<SystemResult<Member>> ThirdpartyLogin(ThirdpartyActView extAccount)
        {
            var result = new SystemResult<Member>();
           var linkupList =  await baseRepository.GetListAsync<ThirdpartyLinkup>(x => x.ExternalAccId == extAccount.ExternalAccId && (int)x.Type == extAccount.ExternalType && x.IsActive && !x.IsDeleted);
            Member user = null;
            var mappingEntity = linkupList .OrderByDescending(x => x.CreateDate).FirstOrDefault();
            if (mappingEntity != null)
            {
                user = await baseRepository.GetModelAsync<Member>(x => x.Id == mappingEntity.MemberId);
            }
            else
            {
                Member memberEntity = null;//有机会传入的邮箱为空，就用FBId判断
                if (!string.IsNullOrEmpty(extAccount.UserName))
                {
                    memberEntity = await baseRepository.GetModelAsync<Member>(x => x.Account == extAccount.UserName && !x.IsDeleted);
                }
                if (!string.IsNullOrEmpty(extAccount.ExternalAccId) && string.IsNullOrEmpty(extAccount.UserName))
                {                  
                    memberEntity = await baseRepository.GetModelAsync<Member>(x => x.Account == extAccount.ExternalAccId && !x.IsDeleted);
                }

                if (memberEntity != null)
                {
                    extAccount.UserName = memberEntity.Account;
                    ThirdpartyLinkup linkupEntity = new ThirdpartyLinkup()
                    {
                        Id = Guid.NewGuid(),
                        MemberId = memberEntity.Id,
                        Type = (ThridpartyType)extAccount.ExternalType,
                        ExternalAccId = extAccount.ExternalAccId
                    };
                    await baseRepository.InsertAsync(linkupEntity);
                    user = await baseRepository.GetModelAsync<Member>(x => x.Id == memberEntity.Id);
                }
            }

            result.Succeeded = true;
            result.ReturnValue = user;
            return result;
        }
    }
}
