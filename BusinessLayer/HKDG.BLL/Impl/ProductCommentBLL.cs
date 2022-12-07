using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Xml.Linq;
using WebCache;

namespace HKDG.BLL
{
    public class ProductCommentBLL : BaseBLL, IProductCommentBLL
    {
        IProductBLL proudctBLL;
        IMemberBLL memberBLL;
        ITranslationRepository translationRepository;
        ISettingBLL settingBLL;

        public ProductCommentBLL(IServiceProvider services) : base(services)
        {
            proudctBLL = Services.Resolve<IProductBLL>();
            memberBLL= Services.Resolve<IMemberBLL>();
            settingBLL = Services.Resolve<ISettingBLL>();
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

            //if (cond.ShopperId != Guid.Empty) query = query.Where(x => x.CreateBy == cond.ShopperId);

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

        public async Task<SystemResult> SaveComments(List<ProductCommentDto> models)
        {
            SystemResult result = new SystemResult();
            UnitOfWork.IsUnitSubmit = true;//统一提交
            foreach (var model in models)
            {
                // 预先处理文件名称
                var insertImages = model.CommentImages.Where(p => p.Id == Guid.Empty).ToList();
                foreach (var item in insertImages)
                {
                    var smallImg = Guid.NewGuid() + Path.GetExtension(item.ImageName);
                    var bigImg = Guid.NewGuid() + Path.GetExtension(item.ImageName);
                    item.ImageS = smallImg;
                    item.ImageB = bigImg;
                }
                await SaveComment(model);              
            }
            UnitOfWork.Submit();

            #region 上传文件

            //List<string> files = new List<string>();
            //List<string> oldFiles = new List<string>();
            foreach (var model in models)
            {
                var tempPath = PathUtil.GetPhysicalPath(Configuration["UploadPath"], CurrentUser.MerchantId.ToString(), FileFolderEnum.TempPath);
                var targetPath = PathUtil.GetPhysicalPath(Configuration["UploadPath"], model.MerchantId.ToString(), FileFolderEnum.ProductComment) + "\\" + model.ProductId;
                var relPath = PathUtil.GetRelativePath( model.MerchantId.ToString(), FileFolderEnum.ProductComment) + "/" + model.ProductId;

                var insertImages = model.CommentImages.Where(p => p.Id == Guid.Empty).ToList();
                var deleteImages = model.CommentImages.Where(p => p.IsDeleted == true).ToList();
              
                var dbInsertImages = new List<ProductCommentImage>();
                var dbdelImages = new List<ProductCommentImage>();
                foreach (var item in insertImages)
                {
                    var tmpFilePath = tempPath + "\\" + item.ImageName;
                    ImageUtil.CreateImg(tmpFilePath, targetPath, item.ImageS, 100, 100);//生成100*100的缩略图
                                                                                        //ImageUtil.CreateImg(tmpFilePath, targetPath, item.ImageB, 400, 400);//生成400*400的缩略图
                                                                                        //ImageUtil(tmpFilePath, targetPath, item.ImageB, 400, 400);//生成400*400的缩略图
                    Intimex.Utility.FileUtil.MoveFile(tmpFilePath, targetPath, item.ImageB);
                    //files.Add(relPath + "/" + item.ImageS);
                    //files.Add(relPath + "/" + item.ImageB);
                }

                foreach (var item in deleteImages)
                {
                    Intimex.Utility.FileUtil.DeleteFile(targetPath + "\\" + Path.GetFileName(item.ImageS));
                    Intimex.Utility.FileUtil.DeleteFile(targetPath + "\\" + Path.GetFileName(item.ImageB));

                    //oldFiles.Add(item.ImageS);
                    //oldFiles.Add(item.ImageB);
                }

                //#region 分发图片文件                
                //if (oldFiles != null)
                //{
                //    ResourceDistribute.Delete(oldFiles, AppTypeEnum.WebSite);
                //}
                //ResourceDistribute.Distribute(files, AppTypeEnum.WebSite);
                //#endregion

            }

            #endregion

            result.Succeeded = true;
            return result;
        }

        public async Task<List<ProductCommentDto>> GetSubOrderComments(Guid subOrderid)
        {
            var comments = new List<ProductCommentDto>();

            var query = await baseRepository.GetListAsync<OrderDeliveryDetail>(d => d.DeliveryId == subOrderid && d.IsFree == false);
            var list = query.OrderByDescending(o => o.CreateDate).ToList();

            //if (list != null && list.Any())
            //{
            foreach (var item in list)
            {
                var comment = await GetItem(subOrderid, item.ProductId);
                var subOrder = await baseRepository.GetModelAsync<OrderDelivery>(x => x.Id == subOrderid);
                var member = await baseRepository.GetModelAsync<Member>(x => x.Id == comment.CreateBy);
                comment.OrderId = subOrder.OrderId;
                comment.SubOrderId = subOrderid;
                comment.ProductId = item.ProductId;
                comment.CommentImages = await GetCommentImage(comment.Id, Guid.Empty);
                comment.MemberName = member?.FirstName ?? "";
                comment.MerchantId = subOrder.MerchantId;
                comments.Add(comment);
            }
            // }
            return comments;
        }

        async Task<ProductCommentDto> GetItem(Guid subOrderId, Guid prodId)
        {
            //ProductComment item = ProductCommentRepo.GetItem(subOrderId, prodId);
            ProductCommentDto item = new ProductCommentDto();
            var dbList = await baseRepository.GetListAsync<ProductComment>(d => !d.IsDeleted && d.SubOrderId == subOrderId && d.ProductId == prodId);
            var dbItem = dbList.OrderBy(o=>o.CreateDate).FirstOrDefault();

            if (dbItem != null)
            {
                item = AutoMapperExt.MapTo<ProductCommentDto>(dbItem);
                var imageList = await baseRepository.GetListAsync<ProductCommentImage>(d => d.CommentId == item.Id && d.IsDeleted == false);
                item.CommentImages = AutoMapperExt.MapToList<ProductCommentImage, ProductCommentImageDto>(imageList.ToList());
            }

            var productInfo = proudctBLL.GetProductInfo(prodId);
            item.OrderNO = (await baseRepository.GetModelByIdAsync<Order>(item.OrderId))?.OrderNO ?? "";
            item.ProductImg = productInfo.Images.Count() > 2 ? productInfo.Images[2] : "";
            item.ProductName = productInfo?.Name ?? "";
            item.ProductCode = productInfo?.Code ?? "";
           
            return item;
        }

        async Task SaveComment(ProductCommentDto model)
        {
            var entity = await baseRepository.GetModelByIdAsync<ProductComment>(model.Id);
            if (entity != null)
            {
                if (entity.IsShow) await  DedutRating(entity.ProductCode, entity.Score);               
                entity.IsShow = false;
                entity.Content = model.Content;
                if (model.Score < 1) model.Score = 1;
                entity.Score = model.Score;
                await baseRepository.UpdateAsync(entity);
            }
            else
            {
                model.Id = Guid.NewGuid();
                await baseRepository.InsertAsync(model);
            }

            var merchInfo = await baseRepository.GetModelByIdAsync<Merchant>(entity.MerchantId);
            CommentAlertModel comm = new CommentAlertModel();

            var orderInfo = await baseRepository.GetModelByIdAsync<Order>(entity.OrderId);
            comm.OrderNO = orderInfo?.OrderNO ?? "";
            comm.OrderDate = orderInfo?.CreateDate ?? new DateTime();

            merchInfo.NameTransId = merchInfo?.NameTransId ?? Guid.Empty;
            comm.MerchantCode = merchInfo.MerchNo;
            comm.MerchantName = (await baseRepository.GetModelAsync<Translation>(x => x.TransId == merchInfo.NameTransId && x.Lang == CurrentUser.Language))?.Value ?? "";
            comm.CommentDate = DateTime.Now;

            var transId = (await baseRepository.GetModelByIdAsync<Product>(entity.ProductId))?.NameTransId ?? Guid.Empty;           
            comm.ProductName = (await baseRepository.GetModelAsync<Translation>(x => x.TransId == transId && x.Lang == CurrentUser.Language))?.Value ?? "";
            comm.Rating = (int)entity.Score;
            comm.Comment = entity.Content;

            //MessageBLL.SendProdCommentInsertToAdmin(new RecipientInfo(), comm);

            #region 不需要同时保存文件的情况

            if (model.CommentImages?.Count > 0)
            {
                var relPath = PathUtil.GetRelativePath(model.MerchantId.ToString(), FileFolderEnum.ProductComment) + "/" + model.ProductId;
                var insertImages = model.CommentImages.Where(p => p.Id == Guid.Empty && !p.IsDeleted).ToList(); // new List<ProductCommentImage>();
                var deleteImages = model.CommentImages.Where(p => p.IsDeleted == true).ToList();
                var smallSize = settingBLL.GetSmallProductImageSize();
                var largeSize = settingBLL.GetBigProductImageSize();

                var dbInsertImages = new List<ProductCommentImage>();
                var dbdelImages = new List<ProductCommentImage>();
                foreach (var item in insertImages)
                {
                    var dbImage = new ProductCommentImage();
                    dbImage.Id = Guid.NewGuid();
                    
                    dbImage.CommentId = model.Id;
                    dbImage.ImageS = relPath + "/" + item.ImageS;
                    dbImage.ImageB = relPath + "/" + item.ImageB;
                    dbImage.IsActive = true;
                    dbInsertImages.Add(dbImage);
                }
                foreach (var item in deleteImages)
                {                   
                    var dbImage = await baseRepository.GetModelByIdAsync<ProductCommentImage>(item.Id);
                    if (dbImage != null)
                    {
                        dbImage.IsDeleted = true;
                        dbdelImages.Add(dbImage);
                    }
                }

                await baseRepository.InsertAsync(dbInsertImages);
                await baseRepository.UpdateAsync(dbdelImages);
            }
            #endregion
        }

        async Task DedutRating(string code, decimal score)
        {
            string key = $"{PreHotType.Hot_PreProductStatistics}";
            if (score < 1) score = 1;

            var oldscore = await baseRepository.GetModelAsync<ProductStatistics>(d => d.Code == code && d.IsActive && !d.IsDeleted);
            if (oldscore != null)
            {
                var total = oldscore.TotalScore - score;
                var newNum = oldscore.ScoreNum - 1;
                decimal calScore = 0;
                if (newNum > 0)
                {
                    calScore = Math.Round(total / newNum) <= 0 ? 0 : Math.Round(total / newNum);
                }
                oldscore.Score = calScore;
                oldscore.ScoreNum = newNum;
                oldscore.TotalScore = total;
                await baseRepository.UpdateAsync(oldscore);
                //計算商家評分
                await CalculateMerchScore(oldscore.MerchantId, oldscore);
                await RedisHelper.HSetAsync(key, code, oldscore);
            }
            else
            {
                Guid merchId = Guid.Empty;
                var mQuery = await baseRepository.GetModelAsync<Product>(x => x.Code == code);
                if (mQuery != null) merchId = mQuery.MerchantId;

                ProductStatistics model = new ProductStatistics
                {
                    Id = Guid.NewGuid(),
                    Code = code,
                    ScoreNum = 0,
                    Score = 0,
                    MerchantId = merchId
                };
                await baseRepository.InsertAsync(model);
                //計算商家評分
                await CalculateMerchScore(merchId, model);
                await RedisHelper.HSetAsync(key, code, model);
            }

        }

        async Task CalculateMerchScore(Guid merchId, ProductStatistics prdStatisticLatest)
        {
            if (merchId != Guid.Empty)
            {
                var oldScore = await baseRepository.GetModelAsync<MerchantStatistic>(x => x.MerchId == merchId && x.IsActive && !x.IsDeleted);
                decimal scoreQty = 1;//被評價產品的數量，因為要作為分母，所以不能為零，默認為1
                decimal totalScore = 0;//被評價產品的總評分
                int totalScoreQty = 0;//被評價產品的總被評價次數
                decimal merchScore = 5;//商家被評價分數

                //獲取到全部有效的產品評價分數數據
                var query = (await baseRepository.GetListAsync<ProductStatistics>(x => x.IsActive && !x.IsDeleted && x.MerchantId == merchId)).ToList();
                if (query!= null && query.Any())
                {
                    //prdStatisticLatest可以為空，但不為空時代表在事務內存在新數據，需要手動加入到總數據中計算
                    if (prdStatisticLatest != null)
                    {
                        var oldPrdScore = query.FirstOrDefault(x => x.Id == prdStatisticLatest.Id);
                        if (oldPrdScore != null)
                        {
                            oldPrdScore.Score = prdStatisticLatest.Score;
                            oldPrdScore.ScoreNum = prdStatisticLatest.ScoreNum;
                        }
                        else
                        {
                            query.Add(prdStatisticLatest);
                        }
                    }

                    //計算數據
                    scoreQty = query.Count;
                    totalScore = query.Sum(x => x.Score);
                    totalScoreQty = query.Sum(x => x.ScoreNum);
                    merchScore = (int)(totalScore / scoreQty);
                }

                if (oldScore != null)
                {
                    //更新已有數據
                    oldScore.Score = merchScore;
                    oldScore.ScoreQty = totalScoreQty;
                   await baseRepository.UpdateAsync(oldScore);
                }
                else
                {
                    //新增數據
                    MerchantStatistic newScore = new MerchantStatistic()
                    {
                        Id = Guid.NewGuid(),
                        Score = merchScore,
                        ScoreQty = totalScoreQty
                    };
                    await baseRepository.InsertAsync(newScore);
                }
            }
        }
    }
}
