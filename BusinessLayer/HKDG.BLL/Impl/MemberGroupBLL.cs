namespace HKDG.BLL
{
    public class MemberGroupBLL : BaseBLL, IMemberGroupBLL
    {

        ITranslationRepository translationRepo;
        ISettingBLL settingBLL;
        public MemberGroupBLL(IServiceProvider services) : base(services)
        {

            translationRepo = Services.Resolve<ITranslationRepository>();
            settingBLL = Services.Resolve<ISettingBLL>();
        }

        /// <summary>
        /// 獲取會員組別列表
        /// </summary>
        /// <param name="cond">搜尋條件</param>
        /// <returns>會員組別信息列表</returns>
        public List<MemberGroupDto> Search(string name)
        {
            var langs = settingBLL.GetSupportLanguages();
          
            var query = baseRepository.GetList<MemberGroup>(p => p.IsDeleted == false && p.ParentId == Guid.Empty);
            if (!string.IsNullOrEmpty(name))
            {
                var tranIds = baseRepository.GetList<Translation>(p => p.Value.Contains(name)).Select(p => p.TransId).ToList();
                query = query.Where(x => tranIds.Contains(x.NameTransId));
            }
            var memberDtoList = AutoMapperExt.MapToList<MemberGroup, MemberGroupDto>(query.ToList());
            foreach (var item in memberDtoList)
            {
                item.SubGroup = GetSubGroup(item.Id);

                item.Names = translationRepo.GetMutiLanguage(item.NameTransId);
                item.Name = langs == null ? "" : item.Names.FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc;
            }

            return memberDtoList;
        }
        /// <summary>
        /// 獲取會員組別詳細信息
        /// </summary>
        /// <param name="id">會員組別ID</param>
        /// <returns>會員組別</returns>
        public MemberGroupDto GetById(Guid id)
        {
            var langs = settingBLL.GetSupportLanguages();
            MemberGroupDto mgd = new MemberGroupDto();

            if (id != Guid.Empty)
            {
                var mg = baseRepository.GetModel<MemberGroup>(x => x.Id == id);
                mgd = AutoMapperExt.MapToList<MemberGroup, MemberGroupDto>(mg);
                mgd.Names = translationRepo.GetMutiLanguage(mgd.NameTransId);
                List<MemberGroupDto> list = GetSubGroup(id);
                if (list?.Count > 0)
                {
                    mgd.SubGroup = list;
                }
            }
            else
            {
                mgd.Remark = "";
                mgd.Names = LangUtil.GetMutiLangFromTranslation(null, langs);
                mgd.SubGroup = new List<MemberGroupDto>();
            }
            return mgd;
        }
        /// <summary>
        /// 獲取所有組別
        /// </summary>  
        /// <returns></returns>
        public List<MemberGroupDto> GetAll()
        {
            var langs = settingBLL.GetSupportLanguages();
            List<MemberGroupDto> mgdList = new List<MemberGroupDto>();
            var list = baseRepository.GetList<MemberGroup>(p => p.IsDeleted == false && p.ParentId == Guid.Empty).ToList();
            mgdList = AutoMapperExt.MapToList<MemberGroup, MemberGroupDto>(list);

            foreach (var item in mgdList)
            {
                item.Names = translationRepo.GetMutiLanguage(item.NameTransId);
                item.Name = langs == null ? "" : item.Names.FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc;
            }
            return mgdList;
        }
        public List<MbrGroupSelect> GetBuyerGroupsSelect()
        {
            List<MbrGroupSelect> tree = new List<MbrGroupSelect>();
            var list = GetAll();
            tree = list.Select(d => new MbrGroupSelect
            {
                Text = d.Name,
                Id = d.Id,
                Children = GetSubGroupForTree(d.Id),
            }).ToList();
            return tree;
        }
        private List<MbrGroupSelect> GetSubGroupForTree(Guid id)
        {
            List<MbrGroupSelect> tree = new List<MbrGroupSelect>();
            var langs = settingBLL.GetSupportLanguages();
            var list = GetSubGroup(id);
            tree = list.Select(d => new MbrGroupSelect
            {
                Text = d.Name,
                Id = d.Id,
            }).ToList();
            return tree;
        }
        /// <summary>
        /// 保存會員組別信息
        /// </summary>
        /// <param name="model">待保存會員組別信息</param>
        /// <returns>操作結果信息</returns>
        public SystemResult Save(MemberGroupDto model)
        {
            SystemResult result = new SystemResult();
            UnitOfWork.IsUnitSubmit = true;//统一提交

            try
            {
                if (StringHtmlContentCheck(model))
                {
                    result.Succeeded = false;
                    result.Message = Resources.Message.ExistHTMLLabel;
                    return result;
                }

                Update(model);

                if (model.SubGroup != null && model.SubGroup.Count > 0)
                {
                    List<Guid> deleteSubGroupIds = new List<Guid>();
                    foreach (var item in model.SubGroup)
                    {
                        if (item.IsDeleted)
                        {
                            deleteSubGroupIds.Add(item.Id);
                        }
                    }
                    Delete(deleteSubGroupIds);
                }

                if (model.SubGroup?.Count > 0)
                {
                    foreach (var item in model.SubGroup)
                    {
                        if (!item.IsDeleted)
                        {
                            item.ParentId = model.Id;

                            Update(item);
                        }
                    }
                }
                UnitOfWork.Submit();
                result.Succeeded = true;
                result.Message = Resources.Message.DeleteSucceeded;
            }
            catch (BLException e)
            {
                result.Message = e.Message;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                //throw;
            }
            return result;
        }

        private bool StringHtmlContentCheck(MemberGroupDto model)
        {
            if (CheckMultLangListHasHTMLTag(model.Names))
            {
                return true;
            }

            if (StringUtil.CheckHasHTMLTag(model.Remark))
            {
                return true;
            }

            return false;
        }

        private bool CheckMultLangListHasHTMLTag(List<MutiLanguage> multList)
        {
            if (multList?.Count > 0)
            {
                foreach (var item in multList)
                {
                    if (StringUtil.CheckHasHTMLTag(item.Desc))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void Update(MemberGroupDto model)
        {
            if (model.Id != Guid.Empty)
            {
                var entity = baseRepository.GetModel<MemberGroup>(x => x.Id == model.Id);
                if (entity.IsDeleted)
                {
                    var trans = baseRepository.GetList<Translation>(p => p.TransId == entity.NameTransId).ToList();
                    foreach (var item in trans)
                    {
                        item.IsDeleted = false;
                        item.UpdateDate = DateTime.Now;
                        item.UpdateBy = Guid.Parse(CurrentUser.UserId);

                    }
                    baseRepository.Update(trans);
                }

                entity.Remark = model.Remark;
                translationRepo.UpdateMutiLanguage(entity.NameTransId, model.Names, TranslationType.Member);
                baseRepository.Update(entity);
            }
            else
            {
                if (model.Names != null)
                {
                    foreach (var name in model.Names)
                    {
                        name.Desc = name.Desc == null ? model.Names.FirstOrDefault(d => d.Desc != null && d.Desc != "")?.Desc ?? "--" : name.Desc;
                    }
                    model.Remark = model.Remark == null ? "--" : model.Remark;
                    model.Id = Guid.NewGuid();
                    var NameTransId = translationRepo.InsertMutiLanguage(model.Names, TranslationType.Member);
                    model.NameTransId = NameTransId;

                    var mg = AutoMapperExt.MapToList<MemberGroupDto, MemberGroup>(model);
                    baseRepository.Insert(mg);

                }
            }
        }

        /// <summary>
        /// 刪除指定id集合的组别
        /// </summary>
        /// <param name="ids">待刪除會員組別ID</param>
        /// <returns>操作結果信息</returns>
        public SystemResult Delete(List<Guid> ids)
        {
            SystemResult result = new SystemResult();
            UnitOfWork.IsUnitSubmit = true;
            try
            {
                List<Translation> updatetrans = new List<Translation>();
                var item = baseRepository.GetList<MemberGroup>(p => ids.Contains(p.Id) && p.IsDeleted == false).ToList();
                foreach (var entity in item)
                {
                    entity.IsDeleted = true;
                    entity.UpdateDate = DateTime.Now;
                    entity.UpdateBy = Guid.Parse(CurrentUser.UserId);

                    var trans = baseRepository.GetList<Translation>(p => p.TransId == entity.NameTransId).ToList();

                    foreach (var t in trans)
                    {
                        t.IsDeleted = false;
                        t.UpdateDate = DateTime.Now;
                        t.UpdateBy = Guid.Parse(CurrentUser.UserId);
                        updatetrans.Add(t);
                    }
                }

                baseRepository.Update(item);
                baseRepository.Update(updatetrans);
                UnitOfWork.Submit();
                result.Succeeded = true;
                result.Message = Resources.Message.DeleteSucceeded;
            }
            catch (BLException e)
            {
                result.Message = e.Message;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                //throw;
            }
            return result;
        }

        /// <summary>
        /// 删除指定id的组别
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SystemResult DeleteMainGroup(List<Guid> ids)
        {
            SystemResult result = new SystemResult();
            UnitOfWork.IsUnitSubmit = true;
            try
            {
                var items = baseRepository.GetList<MemberGroup>(p => (ids.Contains(p.Id) || ids.Contains(p.ParentId)) && p.IsDeleted == false).ToList();
                foreach (var entity in items)
                {
                    entity.IsDeleted = true;
                    entity.UpdateDate = DateTime.Now;
                    entity.UpdateBy = Guid.Parse(CurrentUser.UserId);
                    //translationRepo.Delete(entity.NameTransId);
                    //_memberGroupRepository.Update(entity);
                }
                var trans = baseRepository.GetList<Translation>(p => items.Select(s => s.NameTransId).Contains(p.TransId)).ToList();
                baseRepository.Delete(trans);
                baseRepository.Update(items);
                UnitOfWork.Submit();
                result.Succeeded = true;
                result.Message = Resources.Message.DeleteSucceeded;
            }
            catch (BLException e)
            {
                result.Message = e.Message;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                //throw;
            }
            return result;

        }

        /// <summary>
        /// 删除组别
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public SystemResult Delete(MemberGroup model)
        {
            return null;
        }



        /// <summary>
        /// 保存會員組別的折扣
        /// </summary>
        /// <param name="id"></param>
        /// <param name="discount"></param>
        public SystemResult SaveDiscount(Guid id, decimal? discount)
        {
            SystemResult result = new SystemResult();
            UnitOfWork.IsUnitSubmit = true;
            try
            {
                var entity = baseRepository.GetModel<MemberGroup>(p => p.Id == id);

                entity.Discount = discount;
                entity.UpdateBy = Guid.Parse(CurrentUser.UserId);
                entity.UpdateDate = DateTime.Now;
                baseRepository.Update(entity);
                UnitOfWork.Submit();
                result.Succeeded = true;
                result.Message = Resources.Message.DeleteSucceeded;
            }
            catch (BLException e)
            {
                result.Message = e.Message;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                //throw;
            }
            return result;
        }


        public List<MemberGroupDto> GetSubGroup(Guid id)
        {
            var langs = settingBLL.GetSupportLanguages();
            var list = baseRepository.GetList<MemberGroup>(g => g.ParentId == id && g.IsDeleted == false && g.IsActive).ToList();
            var memberDtoList = AutoMapperExt.MapToList<MemberGroup, MemberGroupDto>(list);

            foreach (var item in memberDtoList)
            {
                item.Names = translationRepo.GetMutiLanguage(item.NameTransId);
                item.Name = langs == null ? "" : item.Names.FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc;
            }

            return memberDtoList;
        }

    }
}
