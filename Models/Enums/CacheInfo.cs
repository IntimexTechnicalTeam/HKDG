﻿namespace Enums
{
    public enum CacheKey
    {
        Ticket=100,
        CurrentUser,
        InvtActualQty,
        SalesQty,
        InvtReservedQty,
        InvtHoldQty,

        CodeMaster,

        RefuseCountries,

        SupportCountries,

        Translations,

        PriceLimit,

        ShoppingCart,

        System,
        MenuBars,
        MenuCatalog,

        ProdAttr,

        Favorite,
        OnLine,
        ProductTrack,
        ProductSku,
        Token,
        PageSizeSetting,

        Member,

        ProductIcon,
    }

    public enum CacheField
    {
        /// <summary>
        /// 头部和尾部菜单
        /// </summary>
        Menu,
        /// <summary>
        /// 多语言，币种，商城信息
        /// </summary>
        Info,
        Catalogs,
        Atts,
        Search,
        /// <summary>
        /// 系统公告，推广公告
        /// </summary>
        Notice,

        defaultCatalog,

        SubCatalog,

        Mobile,
        Desktop,
        StoreLogo,
        ProductPageSize,
        MerchantPageSize,
        MerchantProductSize,
    }
}
