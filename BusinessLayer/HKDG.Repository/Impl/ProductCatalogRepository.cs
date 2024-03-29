﻿namespace HKDG.Repository
{
    public class ProductCatalogRepository : PublicBaseRepository, IProductCatalogRepository
    {
        public ProductCatalogRepository(IServiceProvider service) : base(service)
        {

        }

        public IQueryable<ProductCatalogDto> baseQuery()
        {
            var query = (from c in baseRepository.GetList<ProductCatalog>()
                         join t in baseRepository.GetList<Translation>() on new { a1 = c.NameTransId, a2 = CurrentUser.Lang }
                         equals new { a1 = t.TransId, a2 = t.Lang } into tc
                         from tt in tc.DefaultIfEmpty()
                         select new ProductCatalogDto
                         {
                             Id = c.Id,
                             Code = c.Code,
                             NameTransId = c.NameTransId,
                             BigIcon = c.BigIcon,
                             Level = c.Level,
                             MBigIcon = c.MBigIcon,
                             MOriginalIcon = c.MOriginalIcon,
                             MSmallIcon = c.MSmallIcon,
                             OriginalIcon = c.OriginalIcon,
                             SmallIcon = c.SmallIcon,
                             Seq = c.Seq,
                             ParentId = c.ParentId,
                             Desc = tt.Value ?? "",
                             IsActive = c.IsActive,
                             IsDeleted = c.IsDeleted,
                             CreateBy = c.CreateBy,
                             CreateDate = c.CreateDate,
                             UpdateBy = c.UpdateBy,
                             UpdateDate = c.UpdateDate,
                             IspType = c.IspType,
                             Name = tt.Value ?? "",
                         });
            return query;
        }

        public List<ProductCatalogDto> GetAllActiveCatalog()
        {
            var query = baseQuery().Where(c => c.IsActive && !c.IsDeleted).OrderBy(o => o.Seq).ToList();
            return query;
        }

        public List<ProductCatalogDto> GetAllCatalog()
        {
            List<ProductCatalog> result = new List<ProductCatalog>();
            var query = baseQuery().Where(c => !c.IsDeleted).OrderBy(o => o.Seq).ToList();
            return query;
        }

        public ProductCatalogDto GetById(Guid id)
        {
            var result = new ProductCatalogDto();

            var query = (from c in baseRepository.GetList<ProductCatalog>().ToList()
                         join t in baseRepository.GetList<Translation>().ToList() on c.NameTransId equals t.TransId into tc
                         from tt in tc.DefaultIfEmpty()
                         where c.IsActive && !c.IsDeleted && c.Id == id
                         select new { catalog = c, name = tt }
                         );

            var groupInfo = query.GroupBy(g => g.catalog).Select(d => new
            {
                Catalog = d.Key,
                Names = d.Select(a => a.name).ToList()
            }).FirstOrDefault();

            result = AutoMapperExt.MapTo<ProductCatalogDto>(groupInfo.Catalog);

            var supportLang = GetSupportLanguage();

            result.Descs = LangUtil.GetMutiLangFromTranslation(groupInfo.Names, supportLang);
            result.Desc = result.Descs.FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc.Trim() ?? "";

            return result;
        }

        public List<ProductCatalog> GetAllCatalogChilds(Guid id)
        {
            List<ProductCatalog> list = new List<ProductCatalog>();

            var childList = (from p in baseRepository.GetList<ProductCatalogParent>()
                             join c in baseRepository.GetList<ProductCatalog>() on p.CatalogId equals c.Id
                             where !c.IsDeleted && p.ParentCatalogId == id
                             select c).ToList();

            var rootCatalog = this.baseRepository.GetModel<ProductCatalog>(x => x.Id == id);

            list.Add(rootCatalog);
            list.AddRange(childList);

            return list;
        }

        public List<Product> GetAllCatalogChildProducts(Guid id)
        {
            List<Product> list = new List<Product>();

            var products = (from p in baseRepository.GetList<ProductCatalogParent>()
                            join c in baseRepository.GetList<ProductCatalog>() on p.CatalogId equals c.Id
                            join m in baseRepository.GetList<Product>() on c.Id equals m.CatalogId
                            where !c.IsDeleted && p.ParentCatalogId == id && m.Status != ProductStatus.AutoOffSale
                            select m).ToList();

            var rootCatalogProducts = baseRepository.GetList<Product>(p => p.CatalogId == id).ToList();

            list.AddRange(rootCatalogProducts);
            list.AddRange(products);
            return list;
        }

        public List<ProductCatalogDto> GetCatalogUrlByCatalogId(Guid Id)
        {
            var list = (from u in baseRepository.GetList<ProductCatalogParent>()
                        join c in baseRepository.GetList<ProductCatalog>() on u.ParentCatalogId equals c.Id
                        join t in baseRepository.GetList<Translation>() on new { a1 = c.NameTransId, a2 = CurrentUser.Lang }
                         equals new { a1 = t.TransId, a2 = t.Lang } into tc
                        from tt in tc.DefaultIfEmpty()
                        where u.CatalogId == Id && u.IsDeleted == false && u.IsActive
                        select new ProductCatalogDto
                        {
                            Desc = tt.Value ?? "",
                            Id = c.Id,
                            Level = c.Level,
                            Code = c.Code,
                            MBigIcon = c.MBigIcon,
                            MOriginalIcon = c.MOriginalIcon,
                            MSmallIcon = c.MSmallIcon,
                            OriginalIcon = c.OriginalIcon,
                            ParentId = c.ParentId,
                            Seq = c.Seq,
                            SmallIcon = c.SmallIcon,
                            NameTransId = c.NameTransId,
                            IsActive = c.IsActive,
                            IsDeleted = c.IsDeleted,
                            CreateDate = c.CreateDate,
                            CreateBy = c.CreateBy,
                            UpdateDate = c.UpdateDate,
                            UpdateBy = c.UpdateBy,
                            BigIcon = c.BigIcon,
                        }
                     ).OrderBy(o => o.Seq).ToList();

            return list;
        }

        public List<ProductCatalogDto> GetCatalogChilds(Guid id)
        {
            var query = baseQuery().Where(x => x.ParentId == id && x.IsActive && !x.IsDeleted).OrderBy(o => o.Seq).ToList();
            return query;
        }

    }
}
