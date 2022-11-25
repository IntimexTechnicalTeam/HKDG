using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HKDG.BLL
{
    public class ProductCommentBLL : BaseBLL, IProductCommentBLL
    {
        IProductBLL proudctBLL;
        IMemberBLL memberBLL;
        ITranslationRepository translationRepository;

        public ProductCommentBLL(IServiceProvider services) : base(services)
        {
            proudctBLL = Services.Resolve<IProductBLL>();
            memberBLL= Services.Resolve<IMemberBLL>();
            translationRepository =Services.Resolve<ITranslationRepository>();
        }

        public async Task<List<ProductCommentDto>> GetProductComments(SearchCommentsInfo cond)
        {
            var result = new List<ProductCommentDto>();
            List<Guid> prodIds = null;
            
            if (cond.ProductId == Guid.Empty) return result;

            var p = await baseRepository.GetModelByIdAsync<Product>(cond.ProductId);
            if (p != null) prodIds = (await baseRepository.GetListAsync<Product>(x => x.Code == p.Code)).Select(x => x.Id).ToList();

            var query = await baseRepository.GetListAsync<ProductComment>(x => x.IsShow && !x.IsDeleted);

            if (cond.ShopperId.HasValue) query = query.Where(x => x.CreateBy == cond.ShopperId.Value);

            if (prodIds !=null)  query = query.Where(d => prodIds.Contains(d.ProductId));

            var list = query.OrderByDescending(o=>o.CreateDate).ToList();
            result = AutoMapperExt.MapToList<ProductComment, ProductCommentDto>(list);

            foreach (var item in result)
            {
                var order = await baseRepository.GetModelByIdAsync<Order>(item.OrderId);
                if (order != null)
                {
                    var member = await memberBLL.GetMember(item.CreateBy);
                    item.OrderNO = order.OrderNO;
                    item.CommentImages =await GetCommentImage(item.Id,item.MerchantId);
                    item.ProductImg = proudctBLL.GetProductImages(item.ProductId).FirstOrDefault();
                    item.ProductName = translationRepository.GetDescForLang(p.NameTransId, CurrentUser.Lang);
                    item.MemberName = member?.FullName ?? "";
                }
            }

            return result;
        }

        async Task<List<ProductCommentImageDto>> GetCommentImage(Guid Id,Guid MchId)
        {
            var list =await baseRepository.GetListAsync<ProductCommentImage>(d => d.CommentId == Id && !d.IsDeleted);

            var result = AutoMapperExt.MapToList<ProductCommentImage, ProductCommentImageDto>(list.ToList());

            string fileServer = string.Empty;

            foreach (var item in result)
            {
                item.ImageS = fileServer + item.ImageS;
                item.ImageB = fileServer + item.ImageB;
                item.ImageName = fileServer + item.ImageS;
            }

            return result;
        }

    }
}
