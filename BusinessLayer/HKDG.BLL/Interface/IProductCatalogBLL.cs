﻿namespace HKDG.BLL
{
    public interface IProductCatalogBLL:IDependency
    {
        List<ProductCatalogSummaryView> GetAllCatalog();

        List<ProductCatalogEditModel> GetCatalogTree(bool isActive);

        ProductCatalogEditModel GetCatalog(Guid catId);

        void DeleteCatalog(Guid id);

        Task<SystemResult> SaveCatalog(ProductCatalogEditModel productCatalog);

        Task UpdateCatalogSeqAsync(List<ProductCatalogEditModel> list);

        Task<SystemResult> DisActiveCatalogAsync(Guid catId);

        Task<SystemResult> ActiveCatalogAsync(Guid catId);

        string GetCatalogPath(Guid catID);

        Task<List<ProdCatatogInfo>> GetCatalogAsync();

        List<ProdCatatogInfo> GetCatalogListById(Guid catID);

        Task<ProdCatatogInfo> GetCatalogById(Guid catID);
    }
}
