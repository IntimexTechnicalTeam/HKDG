﻿namespace HKDG.BLL
{
    public interface IProductCommentBLL:IDependency
    {
        Task<List<ProductCommentDto>> GetProductComments(SearchCommentsInfo cond);
    }
}