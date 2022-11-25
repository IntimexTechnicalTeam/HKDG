using Model;

namespace HKDG.Repository
{
    public class ProductRepository : PublicBaseRepository ,IProductRepository
    {
        public ITranslationRepository TranslationRepository;

        public ProductRepository(IServiceProvider service) : base(service)
        {
            //baseRepository = this.Services.Resolve<IBaseRepository>();
            TranslationRepository = service.Resolve<ITranslationRepository>();
        }

        public Task UpdateProduct()
        {
           
            var unitwork = this.UnitOfWork;
            var dbContext = this.DbContext;

            var testUser = CurrentUser;

            return Task.CompletedTask;
        }

        public List<Product> GetProductByAttrValueId(Guid attrValueId)
        {
            List<Product> list = (from p in baseRepository.GetList<Product>()
                                  join a in baseRepository.GetList<ProductAttr>() on p.Id equals a.ProductId
                                  join b in baseRepository.GetList<ProductAttrValue>() on a.AttrId equals b.ProdAttrId
                                  where b.ProdAttrId == attrValueId
                                  select p).ToList();
            return list;
        }

        /// <summary>
        /// 根据属性ID获取与之匹配的产品
        /// </summary>
        /// <param name="attrId"></param>
        /// <returns></returns>
        public List<Product> GetProductByAttrId(Guid attrId)
        {
            List<Product> list = (from p in baseRepository.GetList<Product>()
                                  join a in baseRepository.GetList<ProductAttr>() on p.Id equals a.ProductId
                                  where a.AttrId == attrId

                                  select p).ToList();
            return list;
        }

        public PageData<ProductSummary> Search(ProdSearchCond cond)
        {
            PageData<ProductSummary> data = new PageData<ProductSummary>();
           
            StringBuilder sb = new StringBuilder();

            var baseQuery = GenBaseQuery(cond);
            data.TotalRecord = GetProductCount(baseQuery);

            var fromIndex = ((cond.PageInfo.Page - 1) * cond.PageInfo.PageSize) + 1;
            var toIndex = cond.PageInfo.Page * cond.PageInfo.PageSize;
            List<SqlParameter> paramList = new List<SqlParameter>();

            sb.AppendLine("select ProductId, Name, ApproveType, CatalogId, CatalogName, Code, CreateDate, CurrencyCode, GrossWeight, IconType, IconUrl, IsActive, IsApprove, "); 
            sb.AppendLine("IsGS1, MerchantId, MerchantName, MerchantNameId, SalePrice, OriginalPrice,MarkupPrice, Seq, UpdateDate, WeightUnit, Score, ''as PromotionRuleTitle,IsLimit,IsSalesReturn,NameTransId,PurchaseCounter,IsDeleted,GS1Status");
            sb.AppendLine(" from(");

            if (!cond.PageInfo.SortName.IsEmpty())
            {
                if (cond.PageInfo.SortName == "ApproveTypeString") cond.PageInfo.SortName = "ApproveType";

                sb.AppendLine($"select ROW_NUMBER() OVER(order by {cond.PageInfo.SortName} {cond.PageInfo.SortOrder}) as rowNum");
            }
            else
            {
                sb.AppendLine("select ROW_NUMBER() OVER(order by Code) as rowNum");
            }
            sb.AppendLine(" ,*from(");

            sb.AppendLine($"{ baseQuery.strSql }");

            sb.AppendLine(") a");
            sb.AppendLine(")b where rowNum between @StartIndex and @EndIndex");

            foreach (var item in baseQuery.ParamList)
            {
                SqlParameter p = (SqlParameter)item;
                paramList.Add(new SqlParameter { ParameterName = p.ParameterName, Value = p.Value });
            }

            paramList.Add(new SqlParameter("@StartIndex", fromIndex));
            paramList.Add(new SqlParameter("@EndIndex", toIndex));
            var result = baseRepository.SqlQuery<ProductSummary>(sb.ToString(), paramList.ToArray());

            data.Data = result;
            return data;
        }

        public async Task<PageData<ProductSummary>> SearchAsync(ProdSearchCond cond)
        {
            //PageData<ProductSummary> data = new PageData<ProductSummary>();

            StringBuilder sb = new StringBuilder();

            var baseQuery = GenBaseQuery(cond);
            //data.TotalRecord = GetProductCount(baseQuery);

            var fromIndex = ((cond.PageInfo.Page - 1) * cond.PageInfo.PageSize) + 1;
            var toIndex = cond.PageInfo.Page * cond.PageInfo.PageSize;
            List<SqlParameter> paramList = new List<SqlParameter>();

            sb.AppendLine("select ProductId, Name, ApproveType, CatalogId, CatalogName, Code, CreateDate, CurrencyCode, GrossWeight, IconType, IconUrl, IsActive, IsApprove, ");
            sb.AppendLine("IsGS1, MerchantId, MerchantName, MerchantNameId, SalePrice, OriginalPrice,MarkupPrice, Seq, UpdateDate, WeightUnit, Score, ''as PromotionRuleTitle,IsLimit,IsSalesReturn,NameTransId,PurchaseCounter,IsDeleted,GS1Status");
            sb.AppendLine(" from(");

            if (!cond.PageInfo.SortName.IsEmpty())
            {
                if (cond.PageInfo.SortName == "ApproveTypeString") cond.PageInfo.SortName = "ApproveType";

                sb.AppendLine($"select ROW_NUMBER() OVER(order by {cond.PageInfo.SortName} {cond.PageInfo.SortOrder}) as rowNum");
            }
            else
            {
                sb.AppendLine("select ROW_NUMBER() OVER(order by Code) as rowNum");
            }
            sb.AppendLine(" ,*from(");

            sb.AppendLine($"{baseQuery.strSql}");

            sb.AppendLine(") a");
            sb.AppendLine(")b where rowNum between @StartIndex and @EndIndex");

            foreach (var item in baseQuery.ParamList)
            {
                SqlParameter p = (SqlParameter)item;
                paramList.Add(new SqlParameter { ParameterName = p.ParameterName, Value = p.Value });
            }

            paramList.Add(new SqlParameter("@StartIndex", fromIndex));
            paramList.Add(new SqlParameter("@EndIndex", toIndex));

            var result = await baseRepository.GetPageListAsync<ProductSummary>(sb.ToString(), paramList.ToArray());
            return result;
        }

        public PageData<Product> SearchRelatedProduct(RelatedProductCond cond)
        {

            PageData<Product> list = new PageData<Product>();

            var query = from i in baseRepository.GetList<Product>().ToList()
                        join t in baseRepository.GetList<Translation>().ToList() on i.NameTransId equals t.TransId into tc
                        from tt in tc.DefaultIfEmpty()
                        join c in baseRepository.GetList<ProductCatalog>().ToList() on i.CatalogId equals c.Id
                        orderby i.Code
                        where i.IsDeleted == false && i.IsActive && i.Status == ProductStatus.OnSale
                        && !(from r in baseRepository.GetList<ProductRelatedItem>().ToList() where r.ProductId == cond.ProductId && r.IsActive && !r.IsDeleted select r.ItemCode).Contains(i.Code)
                        select new
                        {
                            product = i,
                            catalog = c,
                            tran = tt
                        };

            #region 查询条件

            if (CurrentUser.IsMerchant)
            {
                query = query.Where(p => p.product.MerchantId == CurrentUser.MerchantId);
            }

            if (cond.ProductId != Guid.Empty)
            {
                query = query.Where(p => p.product.Id != cond.ProductId);
            }

            if (!string.IsNullOrEmpty(cond.ProductCode))
            {
                query = query.Where(p => p.product.Code == cond.ProductCode);
            }
            if (cond.CategoryID != Guid.Empty)
            {
                query = query.Where(p => p.catalog.Id == cond.CategoryID);
            }

            var queryGroup = query.GroupBy(g => g.product).Select(d => new { product = d.Key, trans = d.Select(a => a.tran).ToList() });

            if (!string.IsNullOrEmpty(cond.ProductName))
            {
                queryGroup = queryGroup.Where(p => p.trans.Any(a => a.Value.Contains(cond.ProductName)));

            }

            #endregion

            list.TotalRecord = queryGroup.Count();

            List<Product> queryData = new List<Product>();

            if (cond.PageInfo.PageSize > 0)
            {
                queryData = queryGroup.OrderBy(o => o.product.Code).Select(d => d.product).Skip(cond.PageInfo.Offset).Take(cond.PageInfo.PageSize).ToList();
            }
            else
            {
                queryData = queryGroup.OrderBy(o => o.product.Code).Select(d => d.product).ToList();
            }
            list.TotalRecord = queryData.Count();
            list.Data = queryData;
            return list;
        }

        public List<Product> GetRelatedProduct(Guid id)
        {           
           var list = (from i in baseRepository.GetList<ProductRelatedItem>()
                    join p in baseRepository.GetList<Product>() on i.ItemCode equals p.Code
                    where  i.IsDeleted == false && i.IsActive
                    && i.ProductId == id && p.IsActive && p.IsDeleted == false && p.Status == ProductStatus.OnSale
                    select p).ToList();
            return list;
        }

        public LastVersionProductView GetLastVersionProductByCode(string prodCode)
        {
       
                LastVersionProductView lvProduct = null;

                if (!string.IsNullOrEmpty(prodCode))
                {
                    var lvProductList = GetLastVersionProductLstByCode(new List<string>() { prodCode });
                    lvProduct = lvProductList.FirstOrDefault();
                }

                return lvProduct;
           
        }

        public Guid? GetOnSaleProductId(string prodCode)
        {
            var obj = baseRepository.GetModel<Product>(d => d.Code == prodCode && d.IsActive && d.Status == ProductStatus.OnSale && d.IsDeleted == false);
            if (obj != null)
            {
                return obj.Id;
            }
            return null;
        }

        public List<LastVersionProductView> GetLastVersionProductLstByCode(List<string> prodCodeLst)
        {
            var lvProductList = new List<LastVersionProductView>();

            if (prodCodeLst != null && prodCodeLst.Any())
            {
                var query = (from p in baseRepository.GetList<Product>()
                             where p.IsActive && p.IsDeleted == false && prodCodeLst.Contains(p.Code)
                             select p).ToList();

                var queryOnSaleGroup = query.Where(x => x.Status == ProductStatus.OnSale).GroupBy(x => x.Code);
                var queryNotSaleGroup = query.Where(x => x.Status != ProductStatus.OnSale).GroupBy(x => x.Code);

                foreach (var productGroup in queryOnSaleGroup)
                {
                    var product = productGroup.OrderByDescending(x => x.UpdateDate).FirstOrDefault();
                    if (product != null)
                    {
                        var lvProduct = GenLastVerProd(product);
                        lvProductList.Add(lvProduct);
                    }
                }

                foreach (var productGroup in queryNotSaleGroup)
                {
                    var onSaleQty = lvProductList.Any(x => x.Code == productGroup.Key);
                    if (!onSaleQty)
                    {
                        var product = productGroup.OrderByDescending(x => x.UpdateDate).FirstOrDefault();
                        if (product != null)
                        {
                            var lvProduct = GenLastVerProd(product);
                            lvProductList.Add(lvProduct);
                        }
                    }
                }
            }

            return lvProductList;
        }

        /// <summary>
        /// 获取产品的附加价钱
        /// </summary>
        /// <param name="id"></param>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public async Task<List<decimal>> GetProductAddPriceBySku(Guid id, Guid skuId)
        {

            List<decimal> addPrices = new List<decimal>();

            //var attrs = (from d in baseRepository.GetList<ProductAttr>()
            //             where d.IsInv == true && d.ProductId == id
            //             orderby d.Seq
            //             select d
            //             ).ToList();

            var attrs= (await baseRepository.GetListAsync<ProductAttr>(d => d.IsInv && d.ProductId == id)).OrderBy(o => o.Seq).ToList();

            var skus = baseRepository.GetModel<ProductSku>(p => p.Id == skuId);
            for (int i = 0; i < attrs.Count(); i++)
            {
                if (i == 0)
                {
                    var attrValue1 = baseRepository.GetModel<ProductAttrValue>(p => p.ProdAttrId == attrs[0].Id && p.AttrValueId == skus.AttrValue1);            
                    if (attrValue1 != null) addPrices.Add(attrValue1.AdditionalPrice);                    
                }
                else if (i == 1)
                {
                    var attrValue2 = baseRepository.GetModel<ProductAttrValue>(p => p.ProdAttrId == attrs[1].Id && p.AttrValueId == skus.AttrValue2);
                    if (attrValue2 != null)
                    {
                        addPrices.Add(attrValue2.AdditionalPrice);
                    }

                }
                else if (i == 2)
                {
                    var attrValue3 = baseRepository.GetModel<ProductAttrValue>(p => p.ProdAttrId == attrs[2].Id && p.AttrValueId == skus.AttrValue3);
                    if (attrValue3 != null)
                    {
                        addPrices.Add(attrValue3.AdditionalPrice);
                    }

                }
            }
            return addPrices;

        }

        private QueryParam GenBaseQuery(ProdSearchCond cond)
        {
            StringBuilder sb = new StringBuilder();           
            List<SqlParameter> paramList = new List<SqlParameter>();

            var nameList = string.Empty;          
            var keywordList = string.Empty;

            if (!string.IsNullOrEmpty(cond.Key))
            {
                var tranIds = GetFrontProductTranIds(cond);
                var keywordTranIds = GetFrontProductSeoTranIds(cond);
              
                nameList = string.Join("','", tranIds);
                keywordList = string.Join("','", keywordTranIds);
            }

            sb.AppendLine(" select  p.Id as ProductId, p.Status as ApproveType, p.CatalogId as CatalogId, ISNULL(ct.Value, '') as CatalogName, p.Code,p.CreateDate, p.CurrencyCode");
            sb.AppendLine(" , ps.GrossWeight, e.ProductType as IconType, '' as IconUrl,ISNULL(t.Value,'') as Name");
            sb.AppendLine(" , p.IsActive, p.IsApprove, cast((case when m.MerchantType = 1 then 1 else 0 end) as bit) as IsGS1, m.Id as MerchantId, ISNULL(mt.Value, '') as MerchantName");
            sb.AppendLine(" , m.NameTransId as MerchantNameId, p.NameTransId,");
            sb.AppendLine(" SalePrice=p.SalePrice+p.MarkupPrice, OriginalPrice=p.OriginalPrice+p.MarkupPrice, 0 as Seq, p.UpdateDate, ps.WeightUnit, ISNULL(s.Score, 0) as Score, IsLimit,IsSalesReturn,p.MarkupPrice,PurchaseCounter=isnull(s.PurchaseCounter,0),p.IsDeleted,e.GS1Status");
            sb.AppendLine(" from products p");
            sb.AppendLine(" left join ProductStatistics s on s.Code = p.Code");
            if (cond.Attribute != Guid.Empty)
            {
                sb.AppendLine(" left join ProductAttrs a on a.ProductId = p.Id");
            }
            if (cond.AttributeValue != Guid.Empty)
            {
                sb.AppendLine(" left join ProductAttrValues v on v.ProdAttrId = a.Id");
            }
            sb.AppendLine(" left join Translations t on t.TransId = p.NameTransId and t.Lang = @lang");        
            sb.AppendLine(" join ProductCatalogs c on c.Id = p.CatalogId");
            sb.AppendLine(" left join Translations ct on ct.TransId = c.NameTransId and ct.Lang = @lang");
            sb.AppendLine(" join ProductExtensions  e on e.Id = p.Id");
            sb.AppendLine(" join ProductSpecifications ps on ps.Id = p.Id");
            sb.AppendLine(" join Merchants m on m.Id = p.MerchantId");
            sb.AppendLine(" left join Translations mt on mt.TransId = m.NameTransId and mt.Lang = @lang");
    
            sb.AppendLine(" where 1 = 1");

            #region 

            if (cond.IsActive != -1)
            {
                sb.AppendLine(" and p.IsActive=@IsActive ");
                paramList.Add(new SqlParameter("@IsActive", cond.IsActive));
            }
            if (cond.IsDeleted != -1)
            {
                sb.AppendLine(" and p.IsDeleted = @IsDeleted ");
                paramList.Add(new SqlParameter("@IsDeleted", cond.IsDeleted));
            }

            if (CurrentUser.IsMerchant)
            {
                sb.AppendLine(" and p.MerchantId = @MerchId");
                paramList.Add(new SqlParameter("@MerchId", CurrentUser.MerchantId.ToString()));
            }

            if (cond.MerchantId != Guid.Empty)
            {
                sb.AppendLine(" and p.MerchantId = @MerchId");
                paramList.Add(new SqlParameter("@MerchId", cond.MerchantId.ToString()));
            }
            if (!string.IsNullOrEmpty(cond.ProductCode))
            {
                sb.AppendLine(" and p.Code like @ProductCode ");
                paramList.Add(new SqlParameter("@ProductCode", "%" + cond.ProductCode + "%"));
            }

            if (cond.Attribute != Guid.Empty)
            {
                sb.AppendLine(" and a.AttrId = @AttrId");
                paramList.Add(new SqlParameter("@AttrId", cond.Attribute));
            }

            if (cond.AttributeValue != Guid.Empty)
            {
                sb.AppendLine(" and v.AttrValueId = @AttrValueId");
                paramList.Add(new SqlParameter("@AttrValueId", cond.AttributeValue));
            }

            if (cond.IsApprove != -1)
            {
                sb.AppendLine(" and p.IsApprove = @IsApprove");
                paramList.Add(new SqlParameter("@IsApprove", cond.IsApprove));
            }

            if (cond.Category != Guid.Empty)
            {
                sb.AppendLine(" and p.CatalogId = @CatalogId");
                paramList.Add(new SqlParameter("@CatalogId", cond.Category));
            }

            if (CurrentUser.IsMerchant)
            {
                sb.Append("and left(p.Code, 2)=@IspType");
                paramList.Add(new SqlParameter("@IspType", CurrentUser.IspType));
            }

            #endregion

            #region 关键字查

            if (!string.IsNullOrEmpty(cond.Key))
            {
                if (!string.IsNullOrEmpty(nameList) && !string.IsNullOrEmpty(keywordList))
                {
                    sb.AppendFormat(" and (p.NameTransId in ('{0}') or p.KeyWordTransId in ('{1}') or p.Code like @Key)", nameList, keywordList);
                }
                else if (!string.IsNullOrEmpty(nameList))
                {
                    sb.AppendFormat(" and (p.NameTransId in ('{0}') or p.Code like @Key)", nameList);
                }
                else if (!string.IsNullOrEmpty(keywordList))
                {
                    sb.AppendFormat(" and (p.KeyWordTransId in ('{0}') or p.Code like @Key)", keywordList);
                }
                else
                {
                    sb.AppendLine(" and p.Code like @Key");
                }
                paramList.Add(new SqlParameter("@Key", "%" + cond.Key.Trim() + "%"));

            }

            #endregion

            #region 价格范围区间
            if (cond.Prices?.Count == 2)
            {
                sb.AppendLine(" and ((p.SalePrice >=@price1 and p.SalePrice<=@price2))");
                paramList.Add(new SqlParameter("@price1", cond.Prices[0]));
                paramList.Add(new SqlParameter("@price2", cond.Prices[1]));
            }
            #endregion

            switch (cond.ProductSearchType)
            {
                case ProductSearchType.AllProduct:
                    if (cond.ApproveStatus != "-1" && !string.IsNullOrEmpty(cond.ApproveStatus))
                    {
                        var a = "(" + cond.ApproveStatus + ")";
                        sb.AppendFormat(" and p.Status in {0}", a);
                    }
                    break;
                case ProductSearchType.PromotionRuleProduct:
                    sb.AppendLine(" and not exists(select ProductCode from PromotionRuleProducts rp where rp.IsActive = 1 and rp.IsDeleted = 0 and rp.ProductCode = p.Code) ");
                    sb.AppendLine(" and p.Status = @Status");
                    paramList.Add(new SqlParameter("@Status", (int)ProductStatus.OnSale));
                    break;
                case ProductSearchType.OnSaleProduct:
                    sb.AppendLine(" and p.Status = @Status and m.IsActive=1 and m.IsDeleted=0");
                    paramList.Add(new SqlParameter("@Status", (int)ProductStatus.OnSale));
                    break;
            }

            paramList.Add(new SqlParameter("@lang", CurrentUser.Lang.ToInt()));
            var result = new QueryParam { strSql = sb, ParamList = paramList.ToArray() };
            return result;
        }

        private List<Guid> GetFrontProductTranIds(ProdSearchCond cond)
        {
            var query = (from p in baseRepository.GetList<Product>()
                         join t in baseRepository.GetList<Translation>() on p.NameTransId equals t.TransId into tc
                         //join m in baseRepository.GetList<Merchant>() on p.MerchantId equals m.Id
                         from tt in tc.DefaultIfEmpty()
                         where p.Status == ProductStatus.OnSale
                        // && m.IsActive && m.IsDeleted == false
                         select new
                         {
                             Product = p,
                             Trans = tt
                         });
            if (cond.IsActive != -1)
            {
                var isActive = cond.IsActive == 1 ? true : false;
                query = query.Where(p => p.Product.IsActive == isActive);
            }

            if (cond.IsDeleted != -1)
            {
                var isDeleted = cond.IsDeleted == 1 ? true : false;
                query = query.Where(p => p.Product.IsDeleted == isDeleted);
            }

            if (CurrentUser.IsMerchant == true)
            {
                query = query.Where(p => p.Product.MerchantId == CurrentUser.MerchantId);
            }

            if (cond.MerchantId != Guid.Empty)
            {
                query = query.Where(p => p.Product.MerchantId == cond.MerchantId);
            }
            if (!string.IsNullOrEmpty(cond.ProductCode))
            {
                query = query.Where(p => p.Product.Code.Contains(cond.ProductCode.Trim()));
            }

            if (!string.IsNullOrEmpty(cond.KeyWordType) && cond.KeyWordType != "-1")
            {
                if (cond.KeyWordType == "0")
                {
                    query = query.Where(p => p.Trans.Value == cond.Key.Trim());
                }
                else
                {
                    query = query.Where(p => p.Trans.Value.Contains(cond.Key.Trim()));

                }
            }
            else
            {
                query = query.Where(p => p.Trans.Value.Contains(cond.Key.Trim()));
            }

            return query.Select(d => d.Product.NameTransId).Distinct().ToList();
        }
        private List<Guid> GetFrontProductSeoTranIds(ProdSearchCond cond)
        {
            var query = (from p in baseRepository.GetList<Product>()
                         join t in baseRepository.GetList<Translation>() on p.KeyWordTransId equals t.TransId into tc
                         //join m in baseRepository.GetList<Merchant>() on p.MerchantId equals m.Id
                         from tt in tc.DefaultIfEmpty()
                         where p.Status == ProductStatus.OnSale
                         //&& m.IsActive && m.IsDeleted == false
                         select new
                         {
                             Product = p,
                             Trans = tt
                         });
            if (cond.IsActive != -1)
            {
                var isActive = cond.IsActive == 1 ? true : false;
                query = query.Where(p => p.Product.IsActive == isActive);
            }

            if (cond.IsDeleted != -1)
            {
                var isDeleted = cond.IsDeleted == 1 ? true : false;
                query = query.Where(p => p.Product.IsDeleted == isDeleted);
            }

            if (CurrentUser.IsMerchant == true)
            {
                query = query.Where(p => p.Product.MerchantId == CurrentUser.MerchantId);
            }

            if (cond.MerchantId != Guid.Empty)
            {
                query = query.Where(p => p.Product.MerchantId == cond.MerchantId);
            }
            if (!string.IsNullOrEmpty(cond.ProductCode))
            {
                query = query.Where(p => p.Product.Code.Contains(cond.ProductCode.Trim()));
            }

            if (!string.IsNullOrEmpty(cond.KeyWordType) && cond.KeyWordType != "-1")
            {
                if (cond.KeyWordType == "0")
                {
                    query = query.Where(p => p.Trans.Value == cond.Key.Trim());
                }
                else
                {
                    query = query.Where(p => p.Trans.Value.Contains(cond.Key.Trim()));

                }
            }
            else
            {
                query = query.Where(p => p.Trans.Value.Contains(cond.Key.Trim()));
            }

            return query.Select(d => d.Product.KeyWordTransId).Distinct().ToList();
        }

        private int GetProductCount(QueryParam baseQuery)
        {
            StringBuilder sb = new StringBuilder();
            List<SqlParameter> paramList = new List<SqlParameter>();

            sb.AppendLine($"select count(1) from ({ baseQuery.strSql } ) as b");
            var result = baseRepository.IntFromSql(sb.ToString(), baseQuery.ParamList.ToArray());

            return result;
        }

        private string GetProdDefaultImgPath(Guid prodId, Guid defaultImgId)
        {
            string path = string.Empty;
            string imgPath = baseRepository.GetList<ProductImageList>(x => x.ImageID == defaultImgId).OrderByDescending(x => x.Size).FirstOrDefault()?.Path ?? string.Empty;
            string fileServer = string.Empty;
            if (!string.IsNullOrEmpty(imgPath))
            {
                path = fileServer + imgPath;
            }
            return path;
        }

        private LastVersionProductView GenLastVerProd(Product product)
        {
            var lvProduct = new LastVersionProductView();

            Guid nameGuid = product.NameTransId;
            var prodStatics = baseRepository.GetModel<ProductStatistics>(x => x.IsActive && x.IsDeleted == false && x.Code == product.Code);
            if (prodStatics != null && prodStatics.InternalNameTransId != Guid.Empty)
            {
                nameGuid = prodStatics.InternalNameTransId;
            }

            lvProduct.Id = product.Id;
            lvProduct.Code = product.Code;
            lvProduct.Names = TranslationRepository.GetMutiLanguage(nameGuid);
            lvProduct.Name = lvProduct.Names.FirstOrDefault(x => x.Language == CurrentUser.Lang)?.Desc ?? string.Empty;

            lvProduct.SmallImage = GetProdDefaultImgPath(product.Id, product.DefaultImage);
            lvProduct.MerchantId = product.MerchantId;
            lvProduct.CatalogId = product.CatalogId;

            return lvProduct;
        }
    }
}
