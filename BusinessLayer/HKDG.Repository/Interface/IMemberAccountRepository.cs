namespace HKDG.Repository
{
    public interface IMemberAccountRepository : IDependency
    {
        /// <summary>
        /// 獲取會員的積分
        /// </summary>
        /// <returns></returns>
        decimal GetMemberFun();

        decimal GetMemberFun(Guid memberId);

        List<MallFunHistoryView> GetMallFunHistory(FunCond cond);

        /// <summary>
        /// 获取memberAccount
        /// </summary>
        /// <returns></returns>
        MemberAccount GetByMemberId();

        MemberAccount GetByMemberId(Guid memberId);
    }
}
