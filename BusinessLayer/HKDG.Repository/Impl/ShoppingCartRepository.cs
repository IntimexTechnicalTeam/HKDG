﻿namespace HKDG.Repository
{
    public class ShoppingCartRepository : PublicBaseRepository, IShoppingCartRepository
    {
        public ShoppingCartRepository(IServiceProvider service) : base(service)
        {
        }

        /// <summary>
        /// 生成Detail
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public ShoppingCartItemDetailDto GetItemDetail(ShoppingCartItem item)
        {
            var strSql = @"select ps.Id as SkuId,ps.ProductCode,p.MerchantId
                            ,ps.Attr1 as AttrId1,ps.Attr2 as AttrId2,ps.Attr3 as AttrId3,ps.AttrValue1,ps.AttrValue2,ps.AttrValue3
                            ,ma.DescTransId as AttrValueName1,mb.DescTransId as AttrValueName2,mc.DescTransId as AttrValueName3
                            ,at1.DescTransId as AttrName1,at2.DescTransId as AttrName2,at3.DescTransId as AttrName3
                            ,ap1.AdditionalPrice as AttrValue1Price,ap2.AdditionalPrice as AttrValue2Price,ap3.AdditionalPrice as AttrValue3Price
                            from ProductSkus ps 
                            LEFT join Products p on  p.Code = ps.ProductCode and p.Status=4 and p.IsActive=1 and p.IsDeleted=0
                            left join ProductAttributeValues ma on ma.Id = ps.AttrValue1
                            left join ProductAttributeValues mb on mb.Id = ps.AttrValue2
                            left join ProductAttributeValues mc on mc.Id = ps.AttrValue3
                            left join ProductAttributes at1 on at1.Id = ps.Attr1
                            left join ProductAttributes at2 on at2.Id = ps.Attr2
                            left join ProductAttributes at3 on at3.Id = ps.Attr3
                            left join ProductAttrs pa on pa.ProductId = p.Id and pa.IsInv=1 and pa.IsDeleted =0 
                            left join ProductAttrValues ap1 on ap1.AttrValueId = ps.AttrValue1 and pa.Id = ap1.ProdAttrId
                            left join ProductAttrValues ap2 on ap2.AttrValueId = ps.AttrValue2 and pa.Id = ap2.ProdAttrId
                            left join ProductAttrValues ap3 on ap3.AttrValueId = ps.AttrValue3 and pa.Id = ap3.ProdAttrId 
                            where ps.ID=@Id";

            var param = new List<SqlParameter>();
            param.Add(new SqlParameter { ParameterName = "@Id", Value = item.SkuId });

            var data = baseRepository.SqlQuery<ShoppingCartItemDetailDto>(strSql,param.ToArray());

            return data?.FirstOrDefault() ?? null;
        }

        public async Task<KeyValue> GenMallCartProductAttr(Guid AttrId, Guid AttrValueId)
        {
            string sql = $@"select 
                    (select Value from Translations where TransId =(select DescTransId from ProductAttributes where Id =@AttrId) and Lang=@Lang) as Id,
                    (select Value from Translations where TransId =(select DescTransId from ProductAttributeValues where Id=@AttrValueId) and Lang=@Lang) as Text";

            var param = new List<SqlParameter>();
            param.Add(new SqlParameter { ParameterName = "@AttrId", Value = AttrId });
            param.Add(new SqlParameter { ParameterName = "@AttrValueId", Value = AttrValueId });
            param.Add(new SqlParameter { ParameterName = "@Lang", Value = CurrentUser.Lang });

            var data = baseRepository.SqlQuery<KeyValue>(sql, param.ToArray());

            return data?.FirstOrDefault() ?? null;
        }

        public async Task<ProductAttrValue> GetAddtionPrice(Guid ProductId, Guid AttrValueId)
        {
            var sb = new StringBuilder();
            sb.Append($@" select c.* from ProductAttributeValues a inner join ProductAttrs b on a.AttrId = b.AttrId
                                    inner join ProductAttrValues c on c.ProdAttrId = b.Id and c.AttrValueId = @AttrValueId
                                    where b.ProductId = @ProductId and b.IsInv = 1 and a.Id = @AttrValueId");

            var param = new List<SqlParameter>();
            param.Add(new SqlParameter { ParameterName = "@ProductId", Value = ProductId });
            param.Add(new SqlParameter { ParameterName = "@AttrValueId", Value = AttrValueId });

            var data = (await baseRepository.GetListAsync<ProductAttrValue>(sb.ToString(), param.ToArray())).FirstOrDefault();
            return data;
        }

        public async Task<ShoppingCartItemDetailView> GetItemDetailAsync(ShoppingCartItem item)
        {
            var strSql = @"select distinct ps.ClientId,ps.Id as SkuId,p.Id as ProductId ,ps.ProductCode,p.MerchantId
                            ,ps.Attr1 as AttrId1,ps.Attr2 as AttrId2,ps.Attr3 as AttrId3,ps.AttrValue1,ps.AttrValue2,ps.AttrValue3
                            ,ISNULL(ma.DescTransId,'00000000-0000-0000-0000-000000000000') as AttrValueName1
							,ISNULL(mb.DescTransId,'00000000-0000-0000-0000-000000000000') as AttrValueName2
							,ISNULL(mc.DescTransId,'00000000-0000-0000-0000-000000000000') as AttrValueName3
                            ,ISNULL(at1.DescTransId,'00000000-0000-0000-0000-000000000000') as AttrName1
							,ISNULL(at2.DescTransId,'00000000-0000-0000-0000-000000000000') as AttrName2
							,ISNULL(at3.DescTransId,'00000000-0000-0000-0000-000000000000') as AttrName3
                            --,ISNULL(ap1.AdditionalPrice,0) as AttrValue1Price
							--,ISNULL(ap2.AdditionalPrice,0) as AttrValue2Price
							--,ISNULL(ap3.AdditionalPrice,0) as AttrValue3Price
                            ,0.00 as AttrValue1Price
							,0.00 as AttrValue2Price
							,0.00 as AttrValue3Price
                            from ProductSkus ps 
                            LEFT join Products p on  p.Code = ps.ProductCode and p.Status=4 and p.IsActive=1 and p.IsDeleted=0
                            left join ProductAttributeValues ma on ma.Id = ps.AttrValue1
                            left join ProductAttributeValues mb on mb.Id = ps.AttrValue2
                            left join ProductAttributeValues mc on mc.Id = ps.AttrValue3
                            left join ProductAttributes at1 on at1.Id = ps.Attr1
                            left join ProductAttributes at2 on at2.Id = ps.Attr2
                            left join ProductAttributes at3 on at3.Id = ps.Attr3
                            left join ProductAttrs pa on pa.ProductId = p.Id and pa.IsInv=1 and pa.IsDeleted=0 
                            --left join ProductAttrValues ap1 on ap1.AttrValueId = ps.AttrValue1 and pa.Id = ap1.ProdAttrId
                            --left join ProductAttrValues ap2 on ap2.AttrValueId = ps.AttrValue2 and pa.Id = ap2.ProdAttrId
                            --left join ProductAttrValues ap3 on ap3.AttrValueId = ps.AttrValue3 and pa.Id = ap3.ProdAttrId  
                            where ps.ID=@Id";

            var param = new List<SqlParameter>();
            param.Add(new SqlParameter { ParameterName = "@Id", Value = item.SkuId });

            var data = (await baseRepository.GetListAsync<ShoppingCartItemDetailView>(strSql, param)).FirstOrDefault();

            return data;
        }

    }
}
