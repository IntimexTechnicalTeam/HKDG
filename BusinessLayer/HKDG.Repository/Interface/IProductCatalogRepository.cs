﻿namespace HKDG.Repository
{
    public interface IProductCatalogRepository:IDependency
    {
        IQueryable<ProductCatalogDto> baseQuery();

        List<ProductCatalogDto> GetAllActiveCatalog();

        List<ProductCatalogDto> GetAllCatalog();

        ProductCatalogDto GetById(Guid id);

        List<ProductCatalog> GetAllCatalogChilds(Guid id);

        List<Product> GetAllCatalogChildProducts(Guid id);

        List<ProductCatalogDto> GetCatalogUrlByCatalogId(Guid Id);

        List<ProductCatalogDto> GetCatalogChilds(Guid id);
    }
}
