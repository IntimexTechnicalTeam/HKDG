namespace HKDG.Repository
{
    public class MemberRepository : PublicBaseRepository, IMemberRepository
    {
        public MemberRepository(IServiceProvider service) : base(service)
        {
        }

        public Member GetAccountByPwd(string account, string password)
        {
            var m = baseRepository.GetModel<Member>(x => x.Email == account && x.Password == password);

            return m;
        }

        public Member GetByAccount(string account)
        {
            var m = baseRepository.GetModel<Member>(x => x.Account == account && x.IsDeleted == false);

            return m;
        }
        public Member GetByAccountNoDecrypt(string account)
        {
            var m = baseRepository.GetModel<Member>(x => x.Account == account && x.IsDeleted == false);
            return m;
        }

        /// <summary>
        /// 移除member加密，现在不用注释掉
        /// </summary>
        /// <param name="m"></param>
        protected void DecryptField(Member m)
        {
            if (m != null)
            {
                //m.FirstName = base.Decrypt(m.FirstName);
                //m.LastName = base.Decrypt(m.LastName);
                //m.Mobile = base.Decrypt(m.Mobile);
                //m.Password = base.Decrypt(m.Password);
                try
                {
                    //2022-09-02 移除member加密，数据初始化 Roman
                    //            string sql = @" OPEN SYMMETRIC KEY AES256key_Do1Mall DECRYPTION BY CERTIFICATE [CERTDO1MAll]; 
                    //               select  CONVERT(nvarchar(1000),DecryptByKey(CONVERT(varbinary(1000), FirstName))) as  FirstName, 
                    //CONVERT(nvarchar(1000),DecryptByKey(CONVERT(varbinary(1000), LastName))) as LastName,					   
                    //CONVERT(nvarchar(1000),DecryptByKey(CONVERT(varbinary(1000), Mobile))) as Mobile,
                    //               [password]
                    // from Members   where id=  @mid ;         
                    //                CLOSE SYMMETRIC KEY AES256key_Do1Mall;";
                    //            var result = _unitWork.DataContext.Database.SqlQuery<MemberInfo>(sql, new SqlParameter("@mid", m.Id)).FirstOrDefault();

                    //m.FirstName = result.FirstName;
                    //m.LastName = result.LastName;
                    //m.Mobile = result.Mobile;
                    //m.Password = result.Password;

                }
                catch (Exception ex)
                {

                    //throw new EncryptException("执行加密操作失败", ex);
                }

            }
        }



        public Member GetByAccountOrEmail(string loginAccount)
        {

            var result = baseRepository.GetModel<Member>(d => d.Email == loginAccount && d.IsActive && d.IsApprove && !d.IsDeleted);

            return result;

        }

        public Member GetByEmail(string email)
        {
            var m = baseRepository.GetModel<Member>(d => d.Email == email);
            if (m != null)
            {
                DecryptField(m);
            }
            return m;
        }
        public Member GetByPhone(string phone)
        {
            var m = baseRepository.GetModel<Member>(d => d.Mobile == phone);
            if (m != null)
            {
                DecryptField(m);
            }
            return m;
        }
        public PageData<MemberDto> SearchMember(MbrSearchCond cond)
        {

            //char Active = new char();
            //char Suspended = new char();
            DateTime? RegDateFrom = null;
            DateTime? RegDateTo = null;


            if (cond.RegDateFrom != null && cond.RegDateTo != null)
            {
                RegDateFrom = cond.RegDateFrom;
                RegDateTo = cond.RegDateTo;
            }

            try
            {

                //var queryEncript = "OPEN SYMMETRIC KEY AES256key_Do1Mall DECRYPTION BY CERTIFICATE [CERTDO1MAll];";
                var query = @"select password,updateBy,UpdateDate,gender,Language,currencyCode,LastLogin,OptOutPromotion,id,[Email],[Account],createBy,createDate,[IsActive],[IsDeleted],[GroupId],[Code],[BirthDate],[Remark],[ClientId],IsApprove,
FirstName,LastName,Mobile  from (";

                if (!string.IsNullOrEmpty(cond.SortName))
                {
                    string descStr = "DESC";
                    if (cond.SortName == "Email")
                    {
                        if (cond.SortOrder.ToUpper() == descStr)
                        {
                            query += @"select ROW_NUMBER()over(order by Email Desc) as rowNum";
                        }
                        else
                        {
                            query += @"select ROW_NUMBER()over(order by Email) as rowNum";
                        }
                    }
                    if (cond.SortName == "LastName")
                    {
                        if (cond.SortOrder.ToUpper() == descStr)
                        {
                            query += @"select ROW_NUMBER()over(order by FirstName Desc) as rowNum";
                        }
                        else
                        {
                            query += @"select ROW_NUMBER()over(order by FirstName) as rowNum";
                        }
                    }
                    if (cond.SortName == "Code")
                    {
                        if (cond.SortOrder.ToUpper() == descStr)
                        {
                            query += @"select ROW_NUMBER()over(order by Code Desc) as rowNum";
                        }
                        else
                        {
                            query += @"select ROW_NUMBER()over(order by Code) as rowNum";
                        }
                    }
                    if (cond.SortName == "Mobile")
                    {
                        if (cond.SortOrder.ToUpper() == descStr)
                        {
                            query += @"select ROW_NUMBER()over(order by Mobile Desc) as rowNum";
                        }
                        else
                        {
                            query += @"select ROW_NUMBER()over(order by Mobile) as rowNum";
                        }
                    }
                    if (cond.SortName == "CreateDate")
                    {
                        if (cond.SortOrder.ToUpper() == descStr)
                        {
                            query += @"select ROW_NUMBER()over(order by CreateDate Desc) as rowNum";
                        }
                        else
                        {
                            query += @"select ROW_NUMBER()over(order by CreateDate) as rowNum";
                        }
                    }
                    if (cond.SortName == "LanguageName")
                    {
                        if (cond.SortOrder.ToUpper() == descStr)
                        {
                            query += @"select ROW_NUMBER()over(order by Language Desc) as rowNum";
                        }
                        else
                        {
                            query += @"select ROW_NUMBER()over(order by Language) as rowNum";
                        }
                    }
                    if (cond.SortName == "IsActive")
                    {
                        if (cond.SortOrder.ToUpper() == descStr)
                        {
                            query += @"select ROW_NUMBER()over(order by IsActive Desc) as rowNum";
                        }
                        else
                        {
                            query += @"select ROW_NUMBER()over(order by IsActive) as rowNum";
                        }
                    }
                    if (cond.SortName == "IsApprove")
                    {
                        if (cond.SortOrder.ToUpper() == descStr)
                        {
                            query += @"select ROW_NUMBER()over(order by IsApprove Desc) as rowNum";
                        }
                        else
                        {
                            query += @"select ROW_NUMBER()over(order by IsApprove) as rowNum";
                        }
                    }

                }
                else
                {
                    query += @"select ROW_NUMBER()over(order by Email) as rowNum";
                }


                query += @",* from ( ";


                if (CurrentUser.IsMerchant)
                {
                    switch (cond.Type)
                    {
                        case MemberSearchType.AllMember:
                            query += @" select password,updateBy,UpdateDate,gender,Language,currencyCode,LastLogin,OptOutPromotion,id,[Email],[Account],createBy,createDate,[IsActive],[IsDeleted],[GroupId],[Code],[BirthDate],[Remark],[ClientId],IsApprove,
                               FirstName,
                               LastName,					   
					           Mobile
                               from Members";
                            break;
                        case MemberSearchType.Buyer:
                            query += @" select distinct  c.password,c.updateBy,c.UpdateDate,c.gender,c.Language,c.currencyCode,c.LastLogin,c.OptOutPromotion,c.id,c.[Email]
		                        ,c.[Account],c.createBy,c.createDate,c.[IsActive],c.[IsDeleted],c.[GroupId],c.[Code],c.[BirthDate],c.[Remark],c.[ClientId],c.IsApprove
		                         FirstName,
                                 LastName,					   
					             Mobile  from Orders o
                                inner join OrderDeliveries od on o.id = od.OrderId
                                inner join Members c on c.id = o.MemberId
                                where od.MerchantId = @MerchantId";
                            break;
                    }
                }
                else
                {
                    query += @" select password,updateBy,UpdateDate,gender,Language,currencyCode,LastLogin,OptOutPromotion,id,[Email],[Account],createBy,createDate,[IsActive],[IsDeleted],[GroupId],[Code],[BirthDate],[Remark],[ClientId],IsApprove,
                                FirstName,
                                LastName,					   
					            Mobile
                               from Members";
                }

                query += "   ) m where 1=1";
                List<SqlParameter> param = new List<SqlParameter>();
                List<SqlParameter> param2 = new List<SqlParameter>();

                if (cond.Type == MemberSearchType.Buyer)
                {
                    param.Add(new SqlParameter("@MerchantId", CurrentUser.MerchantId));
                    param2.Add(new SqlParameter("@MerchantId", CurrentUser.MerchantId));
                }

                if (!string.IsNullOrEmpty(cond.EmailAdd))
                {
                    query += " and email like @email";
                    param.Add(new SqlParameter("@email", "%" + cond.EmailAdd + "%"));
                    param2.Add(new SqlParameter("@email", "%" + cond.EmailAdd + "%"));
                }
                if (!string.IsNullOrEmpty(cond.Code))
                {
                    query += " and Code=@Code";
                    param.Add(new SqlParameter("@Code", cond.Code));
                    param2.Add(new SqlParameter("@Code", cond.Code));
                }
                if (!string.IsNullOrEmpty(cond.FirstName))
                {
                    query += " and FirstName like @FirstName";
                    param.Add(new SqlParameter("@FirstName", "%" + cond.FirstName + "%"));
                    param2.Add(new SqlParameter("@FirstName", "%" + cond.FirstName + "%"));
                }
                if (!string.IsNullOrEmpty(cond.LastName))
                {
                    query += " and LastName like @LastName";
                    param.Add(new SqlParameter("@LastName", "%" + cond.LastName + "%"));
                    param2.Add(new SqlParameter("@LastName", "%" + cond.LastName + "%"));
                }
                if (!string.IsNullOrEmpty(cond.Mobile))
                {
                    query += " and Mobile=@Mobile";
                    param.Add(new SqlParameter("@Mobile", cond.Mobile));
                    param2.Add(new SqlParameter("@Mobile", cond.Mobile));
                }
                if (cond.RegDateFrom != null)
                {
                    query += " and CreateDate>=@RegDateFrom";
                    param.Add(new SqlParameter("@RegDateFrom", cond.RegDateFrom));
                    param2.Add(new SqlParameter("@RegDateFrom", cond.RegDateFrom));
                }
                if (cond.RegDateTo != null)
                {
                    query += " and CreateDate<=@RegDateTo";
                    param.Add(new SqlParameter("@RegDateTo", cond.RegDateTo));
                    param2.Add(new SqlParameter("@RegDateTo", cond.RegDateTo));
                }

                if (cond.Active != null)
                {
                    query += " and IsActive=@Active";
                    param.Add(new SqlParameter("@Active", cond.Active.Value));
                    param2.Add(new SqlParameter("@Active", cond.Active.Value));
                }
                if (cond.Approve != null)
                {
                    query += " and IsApprove=@Approve";
                    param.Add(new SqlParameter("@Approve", cond.Approve.Value));
                    param2.Add(new SqlParameter("@Approve", cond.Approve.Value));
                }
                if (cond.Deleted != null)
                {
                    query += " and IsDeleted=@Deleted";
                    param.Add(new SqlParameter("@Deleted", cond.Deleted.Value));
                    param2.Add(new SqlParameter("@Deleted", cond.Deleted.Value));
                }

                if (cond.OutForReceiving != null)
                {
                    query += " and OptOutPromotion=@OutForReceiving";
                    param.Add(new SqlParameter("@OutForReceiving", cond.OutForReceiving));
                    param2.Add(new SqlParameter("@OutForReceiving", cond.OutForReceiving));
                }
                if (cond.BuyerGroup != Guid.Empty)
                {
                    query += " and GroupId=@BuyerGroup";
                    param.Add(new SqlParameter("@BuyerGroup", cond.BuyerGroup));
                    param2.Add(new SqlParameter("@BuyerGroup", cond.BuyerGroup));
                }
                if (cond.Language.Count() > 0)
                {

                    var languages = cond.Language.Select(d => ((int)LangUtil.GetLang(d)).ToString());

                    query += " and [Language] in (" + string.Join(",", languages.ToArray()) + ")";
                }

                //if (cond.PageSize > 0)
                //{
                //    query += ") b where rowNum between @StartIndex and @EndIndex";
                //    var fromIndex = ((cond.Page - 1) * cond.PageSize) + 1;
                //    var toIndex = cond.Page * cond.PageSize;
                //    param.Add(new SqlParameter("@StartIndex", fromIndex));
                //    param.Add(new SqlParameter("@EndIndex", toIndex));
                //}
                //else
                //{
                //    query += ") b";
                //}
                query += ") b";
                PageData<MemberDto> data = new PageData<MemberDto>();
                data.TotalRecord = baseRepository.SqlQuery<Member>(query, param.ToArray()).Count();

                query += " where rowNum between @StartIndex and @EndIndex";
                var fromIndex = ((cond.Page - 1) * cond.PageSize) + 1;
                var toIndex = cond.Page * cond.PageSize;
                param2.Add(new SqlParameter("@StartIndex", fromIndex));
                param2.Add(new SqlParameter("@EndIndex", toIndex));

                //queryEncript += query + ";CLOSE SYMMETRIC KEY AES256key_Do1Mall;";


                var menb = baseRepository.SqlQuery<Member>(query, param2.ToArray()).ToList();
                data.Data = AutoMapperExt.MapToList<Member, MemberDto>(menb);

                //var supportLangs = GetSupportLanguage();
                //foreach (var item in data.Data)
                //{
                //item.LanguageName = supportLangs.FirstOrDefault(p => p.Code == item.Language.ToString())?.Text ?? "";
                //DecryptField(item);
                //}
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public Domain.MemberInfo GetInfoById(Guid Id)
        {
            Domain.MemberInfo result = null;
            var model = baseRepository.GetModelById<Member>(Id);

            if (model != null)
            {
                DecryptField(model);
                result = new Domain.MemberInfo
                {
                    Id = Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Mobile = model.Mobile,
                    Gender = model.Gender,
                    Language = model.Language
                };
            }
            return result;
        }

        public EmailCodeVerify GetRegisterVerifyById(Guid Id)
        {
            //IUnitOfWork _unitOfWork = new UnitOfWork();
            //IEmailCodeVerifyRepository _mbrRegisterVerifyRepository = new EmailCodeVerifyRepository(_unitOfWork);
            EmailCodeVerify verify = baseRepository.GetList<EmailCodeVerify>(d => d.MemberId == Id && d.Type == "MBRREG" && d.ExpireDate >= DateTime.Now && !d.IsDeleted && d.IsActive).OrderByDescending(d => d.CreateDate).FirstOrDefault();
            if (verify != null)
            {
                verify.VerifyUrl = @"/account/Activate/" + verify.MemberId + "/" + verify.VerifyCode;//TODO: 需動態生成
            }
            return verify;
        }
        public EmailCodeVerify GetForgetVerifyById(Guid Id)
        {
            //IUnitOfWork _unitOfWork = new UnitOfWork();
            //IEmailCodeVerifyRepository _mbrRegisterVerifyRepository = new EmailCodeVerifyRepository(_unitOfWork);
            EmailCodeVerify verify = baseRepository.GetList<EmailCodeVerify>(d => d.MemberId == Id && d.Type == "RESTPWD" && d.ExpireDate >= DateTime.Now && !d.IsDeleted && d.IsActive).OrderByDescending(d => d.CreateDate).FirstOrDefault();
            if (verify != null)
            {
                verify.VerifyUrl = @"/account/ResetPwd/" + verify.MemberId + "/" + verify.VerifyCode;//TODO: 需動態生成
            }
            return verify;
        }

        public bool UpdatePassword(Guid id, string pwd)
        {

            string sql = @"
                        update members set password= @content,updatedate=getdate()  where id=@id;";
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@content", pwd);
            sqlParameters[1] = new SqlParameter("@id", id);
            var result = baseRepository.ExecuteSqlCommand(sql, sqlParameters);
            return result == 1;
        }
        public void SetMemberApprove(Guid id)
        {
            try
            {
                string sql = @"update members set isactive=1, isapprove=1 where id=@id";
                SqlParameter[] sqlParameters = new SqlParameter[1];
                sqlParameters[0] = new SqlParameter("@id", id);
                var result = baseRepository.ExecuteSqlCommand(sql, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateSubscribe(Guid id, bool status)
        {
            try
            {
                string sql = @"update members set OptOutPromotion=@status where id=@id";
                SqlParameter[] sqlParameters = new SqlParameter[2];
                sqlParameters[0] = new SqlParameter("@id", id);
                sqlParameters[1] = new SqlParameter("@status", status);
                var result = baseRepository.ExecuteSqlCommand(sql, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteTestMember()
        {
            try
            {
                string sql = @"delete members where Code='Test'";
                var result = baseRepository.ExecuteSqlCommand(sql, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 獲取客戶消費排行榜列表
        /// </summary>
        /// <returns></returns>
        public List<MbrSpendingRank> GetMbrSpendingRank()
        {

            try
            {

                string query = $@" select CONVERT(varchar(36),q.Id)Id, LastName,
   FirstName,SUM(q.SpendingRank) SpendingRank,q.BuyerGroup from (
select s.Id,s.LastName,s.FirstName,CONVERT(varchar(36),g.NameTransId) BuyerGroup,(o.TotalAmount * o.ExchangeRate) SpendingRank from Members s
left join MemberGroups g on s.GroupId = g.Id
left join Orders o on s.Id = o.MemberId
where  o.IsPaid=@IsPaid and 
 o.Status in ({(int)OrderStatus.DeliveryArranged},{(int)OrderStatus.PaymentConfirmed},{(int)OrderStatus.Processing},{(int)OrderStatus.OrderCompleted})) q
 group by q.Id,q.LastName,q.FirstName,q.BuyerGroup
 order by SUM(q.SpendingRank) desc ";



                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@IsPaid", true));


                List<MbrSpendingRank> data = new List<MbrSpendingRank>();



                data = baseRepository.SqlQuery<MbrSpendingRank>(query, param.ToArray()).ToList();




                return data;
            }
            catch (Exception ex)
            {
                //todo add log
                //LogManager.SaveError(mWebStoreConfig, GetType().FullName, ClassUtility.GetMethodName(), "", ex.Message);
                //return null;
                throw ex;
            }
        }

        #region 數據壓力測試相關

        public SystemResult InsertPressureTsMembers(int qty)
        {
            SystemResult result = new SystemResult();
            if (qty < 1)
            {
                return result;
            }

            try
            {
                string accountBase = "pm";
                List<Member> insMbrList = new List<Member>();
                for (int i = 1; i <= qty; i++)
                {
                    string account = accountBase + i.ToString() + "_" + DateTime.Now.ToString("HHmmssfff");
                    string email = account + "@pressuretest.hk";
                    Member mbrEntity = new Member()
                    {
                        Id = Guid.NewGuid(),
                        IsApprove = true,
                        Email = email,
                        Account = email,
                        FirstName = account,
                        LastName = "Ts",
                        Mobile = "98765432",
                        Language = Language.E,
                        Gender = true,
                        Remark = string.Empty,
                        Code = "test",
                        CurrencyCode = "HKD",
                        Password = "12345678"
                    };
                    insMbrList.Add(mbrEntity);
                }

                UnitOfWork.IsUnitSubmit = true;

                baseRepository.Insert(insMbrList);


                UnitOfWork.Submit();

                result.ReturnValue = insMbrList;
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public void DeleteAllPressureTsMembers()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("declare @skuId uniqueidentifier");
            sqlBuilder.AppendLine("select top 1 @skuId=SkuId from ShoppingCartItems where MemberId in (select Id from Members where Account like @account)");
            sqlBuilder.AppendLine("delete from ShoppingCartItems where MemberId in (select Id from Members where Account like @account) ");
            sqlBuilder.AppendLine("delete from InventoryHolds where MemberId in (select Id from Members where Account like @account) ");
            sqlBuilder.AppendLine("update InventoryReserveds set ProcessState=3 where OrderId in (select Id from Orders where MemberId in (select Id from Members where Account like @account)) ");
            sqlBuilder.AppendLine("delete from InventoryReserveds where OrderId in (select Id from Orders where MemberId in (select Id from Members where Account like @account)) ");
            sqlBuilder.AppendLine("delete from DeliveryAddresses where MemberId in (select Id from Members where Account like @account) ");
            sqlBuilder.AppendLine("delete from Orders where MemberId in (select Id from Members where Account like @account) ");
            sqlBuilder.AppendLine("declare @insInvtQty int");
            sqlBuilder.AppendLine("select @insInvtQty=COUNT(*) from Members where Account like '%pressuretest.hk'");
            sqlBuilder.AppendLine("update ProductQties set InvtActualQty=InvtActualQty-@insInvtQty,SalesQty=SalesQty-@insInvtQty where SkuId = @skuId");
            sqlBuilder.AppendLine("delete from Members where Account like @account ");

            UnitOfWork.IsUnitSubmit = true;
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@account", "%pressuretest.hk");
            var result = baseRepository.ExecuteSqlCommand(sqlBuilder.ToString(), sqlParameters);

            UnitOfWork.Submit();
        }

        public List<Member> GetPressureTsMembers(List<Guid> midList)
        {
            List<Member> list = baseRepository.GetList<Member>(x => x.Account.Contains("pressuretest.hk") && x.IsActive && !x.IsDeleted && x.IsApprove && midList.Contains(x.Id)).ToList();
            return list;
        }

        #endregion
    }
}
