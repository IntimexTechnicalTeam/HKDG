namespace HKDG.Repository
{
    public interface IEmailTempItemRepository : IDependency
    {
        List<EmailTempItemDto> Search(EmailTempItemCondition cond);
    }
}
