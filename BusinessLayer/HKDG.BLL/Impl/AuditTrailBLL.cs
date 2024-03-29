﻿namespace HKDG.BLL
{
    public class AuditTrailBLL : BaseBLL, IAuditTrailBLL
    {
        private ICodeMasterRepository _codeMasterRepo;
        //private ITranslationRepository _translationRepo;

        public AuditTrailBLL(IServiceProvider services) : base(services)
        {
        }
        public PageData<MemberLoginRecordDto> GetMemAuditTrail(MemLoginRecPager pageInfo)
        {

            PageData<MemberLoginRecordDto> result = new PageData<MemberLoginRecordDto>(pageInfo);
            var dbctxt = UnitOfWork.DataContext;
            var query = from l in baseRepository.GetList<MemberLoginRecord>()
                        join m in baseRepository.GetList<Member>() on l.MemberId equals m.Id into lms
                        from d in lms.DefaultIfEmpty()
                        select new
                        {
                            MemberId = l.MemberId,
                            Email = d.Email,
                            LoginTime = l.LoginTime,
                            LoginFrom = l.LoginFrom,
                            Duration = l.Duration,
                            LogoutTime = l.LogoutTime,
                            LogoutType = l.LogoutType,
                            CreateDate = l.CreateDate
                        };
            
            if (!string.IsNullOrEmpty(pageInfo.Email))
            {
                query = query.Where(d => d.Email == pageInfo.Email);
            }
            result.TotalRecord = query.Count();
            if (!string.IsNullOrEmpty(pageInfo.SortName))
            {
                if (pageInfo.SortName == "LoginFromDisplay")
                {
                    pageInfo.SortName = "LoginFrom";
                }
                else
                {
                    if (pageInfo.SortName == "LogoutTypeDisplay")
                    {
                        pageInfo.SortName = "LogoutType";
                    }
                    if (pageInfo.SortName == "LogoutTime")
                    {
                        pageInfo.SortName = "LogoutTime";
                    }
                    else if (pageInfo.SortName == "MemberName")
                    {
                        pageInfo.SortName = "Email";
                    }
                }
                query = query.SortBy(pageInfo.SortName, pageInfo.SortOrder.ToUpper().ToEnum<SortType>());
            }
            else
            {

                query = query.OrderByDescending(d => d.CreateDate);
            }
            var data = query.Skip(pageInfo.Offset).Take(pageInfo.PageSize).ToList();
            foreach (var item in data)
            {
                string logoutTypeDisplay = string.Empty;
                switch (item.LogoutType)
                {
                    case LogoutType.UserLogout:
                        logoutTypeDisplay = Resources.Value.UserLogout;
                        break;
                    case LogoutType.TokenExpire:
                        logoutTypeDisplay = Resources.Value.TokenExpire;
                        break;
                    case LogoutType.TimeOut:
                        logoutTypeDisplay = Resources.Value.LoginTimeout;
                        break;
                }
                MemberLoginRecordDto rec = new MemberLoginRecordDto()
                {
                    MemberId = item.MemberId,
                    MemberName = item.Email,
                    LoginTime = item.LoginTime,
                    LoginFrom = item.LoginFrom,
                    LoginFromDisplay = item.LoginFrom.ToString(),
                    Duration = item.Duration,
                    LogoutTime = item.LogoutTime,
                    LogoutTypeDisplay = logoutTypeDisplay,
                    LogoutType = item.LogoutType,
                    CreateDate = item.CreateDate
                };
                result.Data.Add(rec);
            }

            return result;
        }

    }
}
