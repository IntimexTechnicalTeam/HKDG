namespace HKDG.BLL
{
    public interface IEmailerBLL:IDependency
    {
        PageData<MessageFrontView> GetUserMessage(PageInfo pager);
    }
}
