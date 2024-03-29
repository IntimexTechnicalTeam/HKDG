﻿namespace HKDG.BLL
{
    public interface IAttributeBLL:IDependency
    {

        List<KeyValue> GetInveAttribute();

        List<KeyValue> GetNonInveAttribute();

        List<KeyValue> GetAttrLayout();

        List<ProductAttributeValueDto> GetInveAttributeValueSummary();

        PageData<ProductAttributeDto> SearchAttribute(ProductAttributeCond attrCond);

        ProductAttributeDto GetAttribute(Guid id);

        ProductAttributeValueDto GetAttributeValue(Guid id);

        bool CheckAttrIsUsed(string ids);

        SystemResult DeleteAttribute(Guid[] ids);

        SystemResult Save(ProductAttributeDto attributeObj);

        List<AttributeObjectView> GetInvAttributeByCatId(Guid catId);

        List<AttributeObjectView> GetNonInvAttributeByCatId(Guid catId);

        List<AttributeObjectView> GetNonInvAttributeByProduct(Guid prodId);

        List<AttributeObjectView> GetInvAttributeByProduct(Guid prodId);

        List<AttributeObjectView> GetInvAttributeByProductWithMap(Guid prodId);

        ProductAttributeDto GenProductAttribute(ProductAttributeDto attribute);

        ProductAttributeDto GetProductAttribute(Guid attrId, Language lang);

        InvAttributeLst GetInvAttributeItemsByCatID(Guid catID);

        List<ProdAtt> GetInvAttributeByProductWithMapForFront(Guid prodId);

        List<ProdAtt> GetFrontAttribute(ProdAttCond cond);

        Task<List<ProdAtt>> GetFrontAttributeAsync(ProdAttCond cond);
    }
}
