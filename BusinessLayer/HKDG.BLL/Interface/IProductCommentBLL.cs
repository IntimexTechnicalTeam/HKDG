namespace HKDG.BLL
{
    public interface IProductCommentBLL : IDependency
    {
        Task<List<ProductCommentDto>> GetProductComments(SearchCommentsInfo cond);

        // Task<SystemResult> SaveComments(List<ProductCommentDto> models);

        Task<List<ProductCommentDto>> GetSubOrderComments(Guid subOrderid);
    }
}
