using System.Data;

namespace HKDG.BLL
{
    public class InteractMessageBLL : BaseBLL, IInteractMessageBLL
    {
        ITranslationRepository translationRepository;

        public InteractMessageBLL(IServiceProvider services) : base(services)
        {
            translationRepository = Services.Resolve <ITranslationRepository>();
        }

        public async Task<NoticeListDto> GetLatesNoticeAsync(int top = 5)
        {
            string key = CacheKey.System.ToString();
            string field = $"{CacheField.Notice}_{CurrentUser.Lang}";

            var data = await RedisHelper.HGetAsync<NoticeListDto>(key, field);
            if (data == null)
            {
                data = new NoticeListDto();

                var DbPromotionNotice = await baseRepository.GetListAsync<SystemAnnouncement>(x => x.IsPublished && x.IsActive && !x.IsDeleted && x.Type == AnnouncementType.Promotion);
                var DbSystemNotice = await baseRepository.GetListAsync<SystemAnnouncement>(x => x.IsPublished && x.IsActive && !x.IsDeleted && x.Type == AnnouncementType.System);

                var promotionList = DbPromotionNotice.OrderByDescending(x => x.UpdateDate).Take(top).ToList();
                var noticeList = DbSystemNotice.OrderByDescending(x => x.UpdateDate).Take(top).ToList();

                var PromotionNotice = AutoMapperExt.MapToList<SystemAnnouncement, SystemAnnouncementDto>(promotionList);
                var SystemNotice = AutoMapperExt.MapToList<SystemAnnouncement, SystemAnnouncementDto>(noticeList);

                foreach (var item in PromotionNotice)
                {
                    item.Subject = (await translationRepository.GetTranslationAsync(item.SubjectTransId, CurrentUser.Lang))?.Value ?? "";
                    item.Content = (await translationRepository.GetTranslationAsync(item.ContentTransId, CurrentUser.Lang))?.Value ?? "";
                }

                foreach (var item in SystemNotice)
                {
                    item.Subject = (await translationRepository.GetTranslationAsync(item.SubjectTransId, CurrentUser.Lang))?.Value ?? "";
                    item.Content = (await translationRepository.GetTranslationAsync(item.ContentTransId, CurrentUser.Lang))?.Value ?? "";
                }

                data.PromotionNotice = AutoMapperExt.MapToList<SystemAnnouncementDto, SysAnnounceFrontView>(PromotionNotice);
                data.SystemNotice = AutoMapperExt.MapToList<SystemAnnouncementDto, SysAnnounceFrontView>(SystemNotice);

                await RedisHelper.HSetAsync(key,field,data);
            }

            return data;
        }
    }
}
