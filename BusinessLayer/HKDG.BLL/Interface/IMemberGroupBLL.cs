namespace HKDG.BLL
{
    public interface IMemberGroupBLL : IDependency
    {

        /// <summary>
        /// 獲取會員組別列表
        /// </summary>
        /// <param name="name">搜尋條件</param>
        /// <returns>會員組別信息列表</returns>
        List<MemberGroupDto> Search(string name);

        /// <summary>
        /// 獲取會員組別詳細信息
        /// </summary>
        /// <param name="id">會員組別ID</param>
        /// <returns>會員組別</returns>
        MemberGroupDto GetById(Guid id);

        /// <summary>
        /// 獲取所有組別
        /// </summary>  
        /// <returns></returns>
        List<MemberGroupDto> GetAll();

        /// <summary>
        /// 保存會員組別信息
        /// </summary>
        /// <param name="model">待保存會員組別信息</param>
        /// <returns>操作結果信息</returns>
        SystemResult Save(MemberGroupDto model);

        /// <summary>
        /// 刪除指定id集合的组别
        /// </summary>
        /// <param name="ids">待刪除會員組別ID</param>
        /// <returns>操作結果信息</returns>
        SystemResult Delete(List<Guid> ids);

        /// <summary>
        /// 删除指定id的组别
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SystemResult DeleteMainGroup(List<Guid> ids);

        /// <summary>
        /// 删除组别
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        SystemResult Delete(MemberGroup model);

        /// <summary>
        /// 获取下级组别
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<MemberGroupDto> GetSubGroup(Guid id);

        /// <summary>
        /// 保存會員組別的節扣
        /// </summary>
        /// <param name="id"></param>
        /// <param name="discount"></param>
        SystemResult SaveDiscount(Guid id, decimal? discount);

        List<MbrGroupSelect> GetBuyerGroupsSelect();
    }
}
