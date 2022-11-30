using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebCache;

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

            if (cond.ShopperId != Guid.Empty) query = query.Where(x => x.CreateBy == cond.ShopperId);

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

        //public async Task<SystemResult> SaveComments(List<ProductCommentDto> models)
        //{
        //    SystemResult result = new SystemResult();
        //    UnitOfWork.IsUnitSubmit = true;//统一提交
        //    foreach (var model in models)
        //    {
        //        SaveComment(model);
        //        //SaveRating(model.ProductCode, model.Score);
        //    }
        //    UnitOfWork.Submit();

        //    result.Succeeded = true;
        //    return result;
        //}

        //private void SaveComment(ProductCommentDto model)
        //{
        //    if (model?.Id != Guid.Empty)
        //    {
        //        var entity = ProductCommentRepo.Find(model.Id);
        //        if (entity != null)
        //        {

        //            if (entity.IsShow)
        //            {
        //                DedutRating(entity.ProductCode, entity.Score);
        //            }
        //            entity.IsShow = false;
        //            entity.Content = model.Content;
        //            if (model.Score < 1)
        //            {
        //                model.Score = 1;
        //            }
        //            entity.Score = model.Score;

        //            ProductCommentRepo.Update(entity);
        //            var merchInfo = MerchantRepo.Find(entity.MerchantId);
        //            CommentAlertModel comm = new CommentAlertModel();
        //            comm.OrderNO = OrderRepo.Find(entity.OrderId)?.OrderNO ?? "";
        //            comm.OrderDate = OrderRepo.Find(entity.OrderId)?.CreateDate ?? new DateTime();
        //            merchInfo.NameTransId = merchInfo?.NameTransId ?? new Guid();
        //            comm.MerchantCode = merchInfo.MerchNo;
        //            comm.MerchantName = TranslationRepo.Entities.FirstOrDefault(d => d.TransId == merchInfo.NameTransId && d.Lang == UnitOfWork.Operator.Language).Value;
        //            comm.CommentDate = DateTime.Now;
        //            var transId = ProductRepo.Find(entity.ProductId)?.NameTransId ?? new Guid();
        //            comm.ProductName = TranslationRepo.Entities.FirstOrDefault(d => d.TransId == transId && d.Lang == UnitOfWork.Operator.Language).Value;
        //            comm.Rating = (int)entity.Score;
        //            comm.Comment = entity.Content;

        //            MessageBLL.SendProdCommentInsertToAdmin(new RecipientInfo(), comm);
        //        }
        //    }
        //    else
        //    {
        //        model.Id = Guid.NewGuid();
        //        ProductCommentRepo.Insert(model);
        //        var merchInfo = MerchantRepo.Find(model.MerchantId);
        //        CommentAlertModel comm = new CommentAlertModel();
        //        comm.OrderNO = OrderRepo.Find(model.OrderId)?.OrderNO ?? "";
        //        comm.OrderDate = OrderRepo.Find(model.OrderId)?.CreateDate ?? new DateTime();
        //        merchInfo.NameTransId = merchInfo?.NameTransId ?? new Guid();
        //        comm.MerchantCode = merchInfo.MerchNo;
        //        comm.MerchantName = TranslationRepo.Entities.FirstOrDefault(d => d.TransId == merchInfo.NameTransId && d.Lang == Language.C).Value;
        //        comm.CommentDate = DateTime.Now;
        //        var transId = ProductRepo.Find(model.ProductId)?.NameTransId ?? new Guid();
        //        comm.ProductName = TranslationRepo.Entities.FirstOrDefault(d => d.TransId == transId && d.Lang == Language.C).Value;
        //        comm.Rating = (int)model.Score;
        //        comm.Comment = model.Content;
        //        MessageBLL.SendProdCommentInsertToAdmin(new RecipientInfo(), comm);
        //    }

        //    AddGenHtmlFlag(model);
        //    NoticeUpdateForAppProdComment(model.ProductId);

           
        //    #region 不需要同时保存文件的情况

        //    if (model.CommentImages?.Count > 0)
        //    {

        //        var targetPath = PathUtil.GetPhysicalPath(UnitOfWork.Operator.ClientId, model.MerchantId, FileFolderEnum.ProductComment) + "\\" + model.ProductId;
        //        var relPath = PathUtil.GetRelativePath(UnitOfWork.Operator.ClientId, model.MerchantId, FileFolderEnum.ProductComment) + "/" + model.ProductId;

        //        var insertImages = model.CommentImages.Where(p => p.Id == Guid.Empty && !p.IsDeleted).ToList(); // new List<ProductCommentImage>();
        //        var deleteImages = model.CommentImages.Where(p => p.IsDeleted == true).ToList();
        //        var smallSize = SettingBLL.GetSmallProductImageSize();
        //        var largeSize = SettingBLL.GetBigProductImageSize();

        //        var dbInsertImages = new List<ProductCommentImage>();
        //        var dbdelImages = new List<ProductCommentImage>();
        //        foreach (var item in insertImages)
        //        {


        //            var dbImage = new ProductCommentImage();
        //            dbImage.Id = Guid.NewGuid();
        //            dbImage.ClientId = UnitOfWork.Operator.ClientId;
        //            dbImage.CommentId = model.Id;
        //            dbImage.ImageS = relPath + "/" + item.ImageS;
        //            dbImage.ImageB = relPath + "/" + item.ImageB;
        //            dbImage.IsActive = true;
        //            dbInsertImages.Add(dbImage);
        //        }
        //        foreach (var item in deleteImages)
        //        {
        //            var dbImage = ProdCommentImgRepo.GetByKey(item.Id);
        //            if (dbImage != null)
        //            {
        //                dbImage.IsDeleted = true;
        //                dbdelImages.Add(dbImage);
        //            }
        //        }
        //        if (dbInsertImages.Count > 0)
        //        {
        //            ProdCommentImgRepo.Insert(dbInsertImages);
        //        }
        //        if (dbdelImages.Count > 0)
        //        {
        //            ProdCommentImgRepo.Update(dbdelImages);
        //        }


        //    }
        //    #endregion
        //}

        //private void DedutRating(string code, decimal score)
        //{
        //    string key = $"{PreHotType.Hot_PreProductStatistics}";
        //    if (score < 1)
        //    {
        //        score = 1;
        //    }
        //    var oldscore = ProductStatisticsRepo.Entities.FirstOrDefault(d => d.Code == code && d.IsActive && !d.IsDeleted);
        //    if (oldscore != null)
        //    {

        //        var total = oldscore.TotalScore - score;
        //        var newNum = oldscore.ScoreNum - 1;
        //        decimal calScore = 0;
        //        if (newNum > 0)
        //        {
        //            calScore = Math.Round(total / newNum) <= 0 ? 0 : Math.Round(total / newNum);
        //        }
        //        oldscore.Score = calScore;
        //        oldscore.ScoreNum = newNum;
        //        oldscore.TotalScore = total;
        //        ProductStatisticsRepo.Update(oldscore);

        //        //計算商家評分
        //        CalculateMerchScore(oldscore.MerchantId, oldscore);

        //        CacheClient.RedisHelper.HSet(key, code, oldscore);
        //    }
        //    else
        //    {
        //        Guid merchId = Guid.Empty;
        //        var mQuery = ProductRepo.Entities.Where(x => x.Code == code).FirstOrDefault();
        //        if (mQuery != null)
        //        {
        //            merchId = mQuery.MerchantId;
        //        }

        //        ProductStatistics model = new ProductStatistics
        //        {
        //            Id = Guid.NewGuid(),
        //            Code = code,
        //            ScoreNum = 0,
        //            Score = 0,
        //            MerchantId = merchId
        //        };
        //        ProductStatisticsRepo.Insert(model);

        //        //計算商家評分
        //        CalculateMerchScore(merchId, model);
        //        RedisHelper.HSet(key, code, model);
        //    }



        //}
    }
}
