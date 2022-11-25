using Member = Model.Member;

namespace HKDG.BLL
{
    public class EmailerBLL : BaseBLL, IEmailerBLL
    {
        public EmailerBLL(IServiceProvider services) : base(services)
        {
        }

        public PageData<MessageFrontView> GetUserMessage(PageInfo pager)
        {
            PageData<MessageFrontView> data = new PageData<MessageFrontView>();
            var query = (from a in baseRepository.GetList<Emailer>()
                         join m in baseRepository.GetList<EmailerMember>() on a.Id equals m.EmailerId
                         join e in baseRepository.GetList <SystemEmail>() on m.Id equals e.Id
                         join c in baseRepository.GetList <Member>() on m.MemberId equals c.Id
                         join t in baseRepository.GetList <Translation>() on new { a1 = a.SubjectTransId, a2 = c.Language } equals new { a1 = t.TransId, a2 = t.Lang } into tc
                         from tt in tc.DefaultIfEmpty()
                         join st in baseRepository.GetList <EmailContent>() on e.ContentId equals st.Id
                         where a.IsActive && !a.IsDeleted  
                         && m.MemberId == CurrentUser.Id && e.Type == MailType.EMAILER.ToString()
                         orderby a.CreateDate descending
                         select new MessageFrontView
                         {
                             Id = a.Id,
                             Subject = tt.Value,
                             Content = st.Content,
                             CreateDate = a.CreateDate
                         });

            data.TotalRecord = query.Count();
            data.Data = query.Skip(pager.Offset).Take(pager.PageSize).ToList();
            return data;
        }
    }
}
