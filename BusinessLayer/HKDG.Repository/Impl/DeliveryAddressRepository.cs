namespace HKDG.Repository
{
    public class DeliveryAddressRepository : PublicBaseRepository, IDeliveryAddressRepository
    {
        public DeliveryAddressRepository(IServiceProvider service) : base(service)
        {

        }

        public void Insert(DeliveryAddressDto model)
        {
            string sql = $@" 
                             insert  DeliveryAddresses(id,FirstName,LastName,Mobile,Phone,Address,Address1,Address2,Address3,
                              createBy,createDate,updateBy,UpdateDate,IsActive,IsDeleted,Remark,[Default],MemberId,Email,Gender,PostalCode,CountryId,ProvinceId,clientId,city) values( 
                             @id
                             ,@firstName
                             ,@lastName
                             ,@mobile
                             ,@Phone
                             ,@Address
							 ,@Address1
							 ,@Address2 ,@Address3
                             ,@createBy,getDate(),@updateBy,getDate(),@IsActive,@IsDeleted,@Remark,@Default
							 ,@MemberId,@Email,@Gender,@PostalCode,@CountryId,@ProvinceId,@clientId,@City     
                            );";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@id", model.Id));
            paramList.Add(new SqlParameter("@firstName", model.FirstName ?? ""));
            paramList.Add(new SqlParameter("@lastName", model.LastName ?? ""));
            paramList.Add(new SqlParameter("@mobile", model.Mobile ?? ""));
            paramList.Add(new SqlParameter("@Phone", model.Phone ?? ""));
            paramList.Add(new SqlParameter("@Address", model.Address ?? ""));
            paramList.Add(new SqlParameter("@Address1", model.Address1 ?? ""));
            paramList.Add(new SqlParameter("@Address2", model.Address2 ?? ""));
            paramList.Add(new SqlParameter("@Address3", model.Address3 ?? ""));
            paramList.Add(new SqlParameter("@Gender", model.Gender ?? false));
            paramList.Add(new SqlParameter("@CountryId", model.CountryId));
            paramList.Add(new SqlParameter("@ProvinceId", model.ProvinceId));
            paramList.Add(new SqlParameter("@City", model.City ?? ""));
            paramList.Add(new SqlParameter("@PostalCode", model.PostalCode ?? ""));
            paramList.Add(new SqlParameter("@Email", model.Email));
            paramList.Add(new SqlParameter("@MemberId", model.MemberId));
            paramList.Add(new SqlParameter("@IsActive", model.IsActive));
            paramList.Add(new SqlParameter("@IsDeleted", model.IsDeleted));
            paramList.Add(new SqlParameter("@Remark", model.Remark ?? ""));
            paramList.Add(new SqlParameter("@Default", model.Default));
            paramList.Add(new SqlParameter("@createBy", CurrentUser.UserId));
            paramList.Add(new SqlParameter("@updateBy", CurrentUser.UserId));
            paramList.Add(new SqlParameter("@clientId", Guid.Empty));
            baseRepository.ExecuteSqlCommand(sql, paramList.ToArray());


        }
        public void Update(DeliveryAddressDto model)
        {


            string sql = @" 
                             update DeliveryAddresses set 
                             FirstName=@firstName,
                             LastName=@lastName,
                             Mobile=@mobile,
                             Phone=@Phone,
                             Address=@Address,
							 Address1=@Address1,
							 Address2=@Address2,
							 Address3=@Address3,
                            updateBy=@updateBy,UpdateDate=getDate(),IsActive=@IsActive,IsDeleted=@IsDeleted,Remark=@Remark,[Default]=@Default,
							MemberId=@MemberId,Email=@Email,Gender=@Gender,PostalCode=@PostalCode,CountryId=@CountryId,ProvinceId=@ProvinceId,City=@City     
                            where id=@id;";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@id", model.Id));
            paramList.Add(new SqlParameter("@firstName", model.FirstName ?? ""));
            paramList.Add(new SqlParameter("@lastName", model.LastName ?? ""));
            paramList.Add(new SqlParameter("@mobile", model.Mobile ?? ""));
            paramList.Add(new SqlParameter("@Phone", model.Phone ?? ""));
            paramList.Add(new SqlParameter("@Address", model.Address ?? ""));
            paramList.Add(new SqlParameter("@Address1", model.Address1 ?? ""));
            paramList.Add(new SqlParameter("@Address2", model.Address2 ?? ""));
            paramList.Add(new SqlParameter("@Address3", model.Address3 ?? ""));
            paramList.Add(new SqlParameter("@updateBy", CurrentUser.UserId));
            paramList.Add(new SqlParameter("@Gender", model.Gender ?? false));
            paramList.Add(new SqlParameter("@CountryId", model.CountryId));
            paramList.Add(new SqlParameter("@ProvinceId", model.ProvinceId));
            paramList.Add(new SqlParameter("@City", model.City ?? ""));
            paramList.Add(new SqlParameter("@PostalCode", model.PostalCode ?? ""));
            paramList.Add(new SqlParameter("@Email", model.Email));
            paramList.Add(new SqlParameter("@MemberId", model.MemberId));

            paramList.Add(new SqlParameter("@IsActive", model.IsActive));
            paramList.Add(new SqlParameter("@IsDeleted", model.IsDeleted));
            paramList.Add(new SqlParameter("@Remark", model.Remark ?? ""));
            paramList.Add(new SqlParameter("@Default", model.Default));
            baseRepository.ExecuteSqlCommand(sql, paramList.ToArray());
        }

        public DeliveryAddressDto GetByKey(Guid key)
        {
            var m = baseRepository.GetModel<DeliveryAddress>(d => d.Id == key);
            //DecryptField(m);
            var dto = AutoMapperExt.MapTo<DeliveryAddressDto>(m);
            return dto;
        }
        
        public List<DeliveryAddressDto> SearchAddress(Guid memberId, bool isActive, bool isDeleted)
        {
            try
            {
                var sql = $@"
                       select  FirstName, 
					    LastName,					   
					  Mobile,
					   Phone,
                        Address, 
						 Address1,
						 Address2,
						 Address3,
						id,ClientId,IsActive,IsDeleted,CreateDate,CreateBy,UpdateDate,UpdateBy,Remark,[Default],MemberId,City,PostalCode,Gender,Email,ProvinceId,CountryId 
					    from DeliveryAddresses  where MemberId= @memberId and IsActive=@isActive and IsDeleted=@isDeleted;";

                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter("@memberId", memberId));
                sqlParameters.Add(new SqlParameter("@isActive", isActive));
                sqlParameters.Add(new SqlParameter("@isDeleted", isDeleted));

                var result = baseRepository.SqlQuery<DeliveryAddress>(sql, sqlParameters.ToArray());

                var finalResult = new List<DeliveryAddress>();
                if (result?.Count > 0)
                {
                    var defaultAddrList = result.Where(x => x.Default).OrderByDescending(x => x.CreateDate).ToList();
                    if (defaultAddrList?.Count > 0)
                    {
                        finalResult.AddRange(defaultAddrList);
                    }
                    var notDefaultAddrList = result.Where(x => !x.Default).OrderByDescending(x => x.CreateDate).ToList();
                    if (notDefaultAddrList?.Count > 0)
                    {
                        finalResult.AddRange(notDefaultAddrList);
                    }
                }
                foreach (var item in finalResult)
                {
                    int tmpId;
                    if (int.TryParse(item.City, out tmpId))
                    {
                        item.CityId = tmpId;
                    }
                    else
                    {
                        item.CityId = 0;
                    }

                }
                var dtos = AutoMapperExt.MapToList<DeliveryAddress, DeliveryAddressDto>(finalResult);
                return dtos;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<DeliveryAddressDto> SearchLocalAddress(Guid memberId, bool isActive, bool isDeleted)
        {
            try
            {
                var sql = @"
                       select   FirstName, 
					   LastName,					   
					   Mobile,
					   Phone,
                          Address, 
					     Address1,
					     Address2,
					     Address3,
						d.id,d.ClientId,d.IsActive,d.IsDeleted,d.CreateDate,d.CreateBy,d.UpdateDate,d.UpdateBy,d.Remark,d.[Default],d.MemberId,d.City,d.PostalCode,d.Gender,d.Email,d.ProvinceId,d.CountryId 
					    from DeliveryAddresses d  
                        join Countries c on c.Id=d.CountryId                        
                        where d.MemberId={0} and d.IsActive={1} and d.IsDeleted={2} and c.[Code]='HKG'";

                var result = baseRepository.SqlQuery<DeliveryAddress>(sql, new object[] { memberId, isActive, isDeleted }).ToList();

                var finalResult = new List<DeliveryAddress>();
                if (result?.Count > 0)
                {
                    var defaultAddrList = result.Where(x => x.Default).OrderByDescending(x => x.CreateDate).ToList();
                    if (defaultAddrList?.Count > 0)
                    {
                        finalResult.AddRange(defaultAddrList);
                    }
                    var notDefaultAddrList = result.Where(x => !x.Default).OrderByDescending(x => x.CreateDate).ToList();
                    if (notDefaultAddrList?.Count > 0)
                    {
                        finalResult.AddRange(notDefaultAddrList);
                    }
                }
                var dtos = AutoMapperExt.MapToList<DeliveryAddress, DeliveryAddressDto>(finalResult);
                return dtos;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<DeliveryAddressDto> SearchOverseasAddress(Guid memberId, bool isActive, bool isDeleted)
        {
            try
            {
                var sql = @"
                       select   FirstName, 
					   LastName,					   
					    Mobile,
					   Phone,
                        Address, 
						Address1,
						Address2,
						Address3,
						d.id,d.ClientId,d.IsActive,d.IsDeleted,d.CreateDate,d.CreateBy,d.UpdateDate,d.UpdateBy,d.Remark,d.[Default],d.MemberId,d.City,d.PostalCode,d.Gender,d.Email,d.ProvinceId,d.CountryId 
					    from DeliveryAddresses d  
                        join Countries c on c.Id=d.CountryId                        
                        where d.MemberId={0} and d.IsActive={1} and d.IsDeleted={2} and c.[Code]<>'HKG';";

                var result = baseRepository.SqlQuery<DeliveryAddress>(sql, new object[] { memberId, isActive, isDeleted }).ToList();

                var finalResult = new List<DeliveryAddress>();
                if (result?.Count > 0)
                {
                    var defaultAddrList = result.Where(x => x.Default).OrderByDescending(x => x.CreateDate).ToList();
                    if (defaultAddrList?.Count > 0)
                    {
                        finalResult.AddRange(defaultAddrList);
                    }
                    var notDefaultAddrList = result.Where(x => !x.Default).OrderByDescending(x => x.CreateDate).ToList();
                    if (notDefaultAddrList?.Count > 0)
                    {
                        finalResult.AddRange(notDefaultAddrList);
                    }
                }
                var dtos = AutoMapperExt.MapToList<DeliveryAddress, DeliveryAddressDto>(finalResult);
                return dtos;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateOtherAddressNotDefault(Guid memberId)
        {
            try
            {

                string sql = @"
                             update DeliveryAddresses set 
                            updateBy=@updateBy,UpdateDate=getDate(),[Default]=0     
                            where MemberId=@id; ";
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@id", memberId));
                paramList.Add(new SqlParameter("@updateBy", CurrentUser.UserId));
                baseRepository.ExecuteSqlCommand(sql, paramList.ToArray());

            }
            catch (Exception ex)
            {

                throw new Exception("更新默認地址失败", ex);
            }
        }


    }
}
