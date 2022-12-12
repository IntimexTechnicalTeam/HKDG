namespace HKDG.BLL
{
    //public delegate void CreatedMember(Member member);
    //public delegate void PasswordChanged(MemberInfo member);

    public interface IMemberBLL : IDependency
    {
        PageData<MemberDto> SearchMember(MbrSearchCond cond);

        SystemResult Register(RegisterMember member);

        Task<SystemResult> ChangeLang( Language Lang);

        Task<SystemResult> ChangeCurrencyCode( string CurrencyCode);

        Task<SystemResult> ChangeSetting(Language Lang, string CurrencyCode);
        RegSummary GetRegSummary();

        Task<SystemResult> AddFavMerchant(string merchCode);

        Task<SystemResult> RemoveFavMerchant(string merchCode);

        Task<SystemResult> AddFavProduct(Guid productId);

        Task<SystemResult> RemoveFavProduct(Guid productId);

        Task<PageData<MicroMerchant>> MyFavMerchant(FavoriteCond cond);

        Task<PageData<ProductSummary>> MyFavProduct(FavoriteCond cond);

        Task<MemberItem> GetMemberInfo();

        Task<PageData<MicroProduct>> MyProductTrack(TrackCond cond);

        /// <summary>
        /// 獲取會員消費排行榜
        /// </summary>
        /// <returns></returns>
        List<MbrSpendingRank> GetMbrSpendingRank();

        /// <summary>
        /// 獲取指定會員的具體信息列表
        /// </summary>
        /// <param name="id">會員ID</param>
        /// <returns>會員詳細信息</returns>
        Task<MemberDto> GetMember(Guid id);

        /// <summary>
        /// 判断會員是否接收推廣郵件信息
        /// </summary>
        /// <param name="id">會員ID</param>
        /// <returns></returns>
        bool IsOptOutPromotion(Guid id);

        /// <summary>
        /// 更新会员信息，不包括密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateMember(MemberDto model);

        /// <summary>
        /// 激活會員
        /// </summary>
        /// <param name="ids">待激活的會員ID列表</param>
        /// <returns>操作結果信息</returns>
        Task ActiveMember(Guid[] ids);

        Task Restore(Guid[] ids);

        Task EnableMember(Guid[] ids);

        /// <summary>
        /// 停用會員
        /// </summary>
        /// <param name="ids">待停用的會員ID列表</param>
        /// <returns>操作結果信息</returns>
        Task SuspendMember(Guid[] ids);

        /// <summary>
        /// 批量刪除會員記錄
        /// </summary>
        /// <param name="ids">待刪除的記錄ID列表</param>
        /// <returns>操作結果信息</returns>
        Task DeleteMember(Guid[] ids);

        /// <summary>
        /// 批量移動會員到指定分組
        /// </summary>
        /// <param name="ids">待移動的會員ID列表</param>
        /// <param name="targetGroup">組別ID</param>
        /// <returns>操作結果信息</returns>
        Task MoveMember(Guid[] ids, Guid targetGroup);

        List<MallFunHistoryView> GetMallFunHistory(FunCond cond);

        /// <summary>
        /// 獲取指定條件的會員列表
        /// </summary>
        /// <param name="condition">搜尋條件</param>
        /// <returns>會員信息列表</returns>
        PageData<MemberDto> Search(MbrSearchCond condition);

        Task<SystemResult> UpdatePassword(string oldpwd, string newpwd);

        Task<bool> Subscribe(string email);

        Task<bool> Unsubscribe(string email);

        Task<SystemResult<Member>> ThirdpartyLogin(ThirdpartyActView extAccount);
    }
}
