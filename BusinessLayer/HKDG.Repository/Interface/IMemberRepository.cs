namespace HKDG.Repository
{
    public interface  IMemberRepository :IDependency
    {


        /// <summary>
        /// 根据搜寻条件查询会员
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        PageData<MemberDto> SearchMember(MbrSearchCond condition);

        /// <summary>
        /// 通过账号获取会员信息 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Member GetByAccount(string account);

        /// <summary>
        /// 通过账号获取会员信息 (没有解码)
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Member GetByAccountNoDecrypt(string account);



        /// <summary>
        /// 通过邮箱获取会员信息 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Member GetByEmail(string email);

        Member GetByPhone(string phone);



        /// <summary>
        /// 通过登入账号获取会员信息
        /// </summary>
        /// <param name="loginAccount"></param>
        /// <returns></returns>
        Member GetByAccountOrEmail(string loginAccount);
        /// <summary>
        /// 通过账号，密码获取会员账号信息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Member GetAccountByPwd(string account, string password);

        /// <summary>
        /// 獲取指定會員信息
        /// </summary>
        /// <param name="Id">會員ID</param>
        Domain.MemberInfo GetInfoById(Guid Id);

        /// <summary>
        /// 獲取指定會員的註冊校驗信息
        /// </summary>
        /// <param name="Id">會員ID</param>
        //EmailCodeVerify GetRegisterVerifyById(Guid Id);
        //EmailCodeVerify GetForgetVerifyById(Guid Id);

        /// <summary>
        /// 用db encrypt password ，使用时注意，这个方法现在不适用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        bool UpdatePassword(Guid id, string pwd);


        void SetMemberApprove(Guid id);

        void DeleteTestMember();

        SystemResult InsertPressureTsMembers(int qty);

        void DeleteAllPressureTsMembers();

        void UpdateSubscribe(Guid id, bool status);

        List<Member> GetPressureTsMembers(List<Guid> midList);

        /// <summary>
        /// 獲取會員消費排行榜
        /// </summary>
        /// <returns></returns>
        List<MbrSpendingRank> GetMbrSpendingRank();
    }
}
