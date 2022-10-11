namespace HKDG.Repository
{
    public class MemberAccountRepository : PublicBaseRepository, IMemberAccountRepository
    {
        public MemberAccountRepository(IServiceProvider service) : base(service)
        {
        }
        public decimal GetMemberFun()
        {
            return baseRepository.GetModel<MemberAccount>(p => p.IsActive && !p.IsDeleted && p.MemberId == Guid.Parse(CurrentUser.UserId))?.Fun ?? 0;
        }

        public decimal GetMemberFun(Guid memberId)
        {
            return baseRepository.GetModel<MemberAccount>(p => p.IsActive && !p.IsDeleted  && p.MemberId == memberId)?.Fun ?? 0;
        }

        public MemberAccount GetByMemberId()
        {
            return baseRepository.GetModel<MemberAccount>(p => p.IsActive && !p.IsDeleted && p.MemberId == Guid.Parse(CurrentUser.UserId));
        }

        public MemberAccount GetByMemberId(Guid memberId)
        {
            return baseRepository.GetModel<MemberAccount>(p => p.IsActive && !p.IsDeleted && p.MemberId == memberId);
        }

        public List<MallFunHistoryView> GetMallFunHistory(FunCond cond)
        {
            //var fromIndex = ((cond.Page - 1) * cond.PageSize) + 1;
            //var toIndex = cond.Page * cond.PageSize;
            List<MallFunHistoryView> data = new List<MallFunHistoryView>();

            StringBuilder sb = new StringBuilder();

            //sb.AppendLine("OPEN SYMMETRIC KEY AES256key_Do1Mall DECRYPTION BY CERTIFICATE [CERTDO1MAll]");
            //sb.AppendLine("select * from (select f.Id,CONVERT(nvarchar(1000),DecryptByKey(CONVERT(varbinary(1000), FirstName))) as  FirstName,");
            //sb.AppendLine("CONVERT(nvarchar(1000), DecryptByKey(CONVERT(varbinary(1000), LastName))) as LastName,");
            sb.AppendLine("select * from (select f.Id,  FirstName,");
            sb.AppendLine(" LastName,");
            sb.AppendLine("m.Email,f.Amount as Fun,f.Type,f.CreateDate");
            sb.AppendLine("from Members m");
            sb.AppendLine("inner join MemberAccounts ma on m.Id = ma.MemberId");
            sb.AppendLine("inner join FunHistories f on f.AccountId = ma.Id");
            sb.AppendLine("where m.IsActive=1 and m.IsDeleted=0 ) a");
            sb.AppendLine("where 1=1 ");

            if (!string.IsNullOrEmpty(cond.FirstName))
            {
                sb.AppendFormat(" and a.FirstName like '%{0}%'", cond.FirstName);
            }
            if (!string.IsNullOrEmpty(cond.LastName))
            {
                sb.AppendFormat(" and a.LastName like '%{0}%'", cond.LastName);
            }
            if (!string.IsNullOrEmpty(cond.Email))
            {
                sb.AppendFormat(" and a.Email like '%{0}%'", cond.Email);
            }

            if (cond.Type != -1)
            {
                sb.AppendFormat(" and a.Type={0}", cond.Type);
            }
            //sb.AppendLine(";CLOSE SYMMETRIC KEY AES256key_Do1Mall; ");

            List<SqlParameter> paramList = new List<SqlParameter>();
           

            data = baseRepository.SqlQuery<MallFunHistoryView>(sb.ToString(), paramList.ToArray()).ToList();

            foreach (var item in data)
            {
                item.CreateDateString = DateUtil.DateTimeToString(item.CreateDate, Runtime.Setting.DefaultDateTimeFormat);
            }
            return data;

        }
    }
}
